using System;
using System.Net;
using System.Net.Http;
using System.Text;
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
            {
                throw new ArgumentException("WebHook URL 不能为空。", nameof(webhookUrl));
            }

            _webhookUrl = webhookUrl;
            _template = string.IsNullOrWhiteSpace(template)
                ? "{\"msg_type\":\"text\",\"content\":{\"text\":\"{{message}}\"}}"
                : template;
            _useProxy = useProxy;

            if (_useProxy && !string.IsNullOrWhiteSpace(proxyAddress))
            {
                try
                {
                    _customProxy = new WebProxy(proxyAddress);
                }
                catch (Exception ex) when (ex is UriFormatException || ex is ArgumentException)
                {
                    throw new ArgumentException($"代理地址无效: {ex.Message}", nameof(proxyAddress));
                }
            }
        }


        public async Task<NotifyResult> NotifyAsync(string message)
        {
            try
            {
                using var client = CreateHttpClient();
                string payload = _template.Replace("{{message}}", message);
                var content = new StringContent(payload, Encoding.UTF8, "application/json");
                var response = await client.PostAsync(_webhookUrl, content);
                if (!response.IsSuccessStatusCode)
                {
                    string responseBody = await response.Content.ReadAsStringAsync();
                    return new(false, $"Webhook 通知失败: {response.StatusCode}, Body: {responseBody}");
                }
                string successMessage = await response.Content.ReadAsStringAsync();
                return new(true, successMessage);
            }
            catch (Exception ex)
            {
                return new(false, $"Webhook 通知错误: {ex.Message}");
            }
        }

        private HttpClient CreateHttpClient()
        {
            var handler = new HttpClientHandler();

            if (_customProxy != null)
            {
                handler.Proxy = _customProxy;
            }

            handler.UseProxy = _useProxy;

            return new HttpClient(handler, disposeHandler: true);
        }
    }
}