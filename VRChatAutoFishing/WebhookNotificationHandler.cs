using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace VRChatAutoFishing
{
    public class WebhookNotificationHandler : INotificationHandler
    {
        private readonly string _webhookUrl;
        private readonly string _template;
        private readonly bool _useProxy;
        private readonly IWebProxy? _customProxy;

        public WebhookNotificationHandler(string webhookUrl, string template, bool useProxy, string proxyAddress)
        {
            if (string.IsNullOrWhiteSpace(webhookUrl))
            _useProxy = useProxy;

            if (_useProxy && !string.IsNullOrWhiteSpace(proxyAddress))
            {
                try
                {
                    _customProxy = new WebProxy(proxyAddress);
                }
                catch (Exception ex) when (ex is UriFormatException || ex is ArgumentException)
                {
                    throw new ArgumentException($"ЧĴַ: {ex.Message}", nameof(proxyAddress));
                }
            }
                var handler = new HttpClientHandler();
                if (!_useProxy)
                {
                    handler.UseProxy = false;
                }
                else if (_customProxy != null)
                {
                    handler.Proxy = _customProxy;
                }

                using var client = new HttpClient(handler);

            _webhookUrl = webhookUrl;
            _template = template;
        }


        public async Task<NotifyResult> NotifyAsync(string message)
        {
            try
            {
                using var client = new HttpClient();
                string payload = _template.Replace("{{message}}", message);
                var content = new StringContent(payload, Encoding.UTF8, "application/json");
                var response = await client.PostAsync(_webhookUrl, content);
                if (!response.IsSuccessStatusCode)
                {
                    return new(false, $"Webhook 通知失败: {response.StatusCode}, Body: {response.Content}");
                }
                return new(true, response.Content.ToString() ?? "");
            }
            catch (Exception ex)
            {
                return new(false, $"Webhook 通知错误: {ex.Message}");
            }
        }
    }
}