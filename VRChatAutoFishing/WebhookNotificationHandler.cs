using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace VRChatAutoFishing
{
    public class WebhookNotificationHandler : INotificationHandler
    {
        private const string LegacyDefaultTemplate = "{\\\"msg_type\\\":\\\"text\\\",\\\"content\\\":{\\\"text\\\":\\\"{{message}}\\\"}}";
        private const string DefaultTemplate = "{\\\"msg_type\\\":\\\"text\\\",\\\"content\\\":{\\\"text\\\":\\\"{{message_json}}\\\"}}";
        private const string TelegramTemplate = "{\\\"text\\\":\\\"{{message_json}}\\\"}";

        private readonly string _webhookUrl;
        private readonly string _template;
        private readonly bool _useProxy;
        private readonly bool _isTelegramWebhook;
        private readonly IWebProxy? _customProxy;

        public WebhookNotificationHandler(string webhookUrl, string template, bool useProxy, string proxyAddress)
        {
            if (string.IsNullOrWhiteSpace(webhookUrl))
            {
                throw new ArgumentException("WebHook URL 不能为空。", nameof(webhookUrl));
            }

            _webhookUrl = webhookUrl;

            template = template?.Trim() ?? string.Empty;

            string normalizedTemplate;

            if (string.IsNullOrWhiteSpace(template))
            {
                normalizedTemplate = DefaultTemplate;
            }
            else if (string.Equals(template, LegacyDefaultTemplate, StringComparison.Ordinal))
            {
                normalizedTemplate = DefaultTemplate;
            }
            else
            {
                normalizedTemplate = template;
            }

            _isTelegramWebhook = IsTelegramWebhook(webhookUrl);

            if (_isTelegramWebhook && string.Equals(normalizedTemplate, DefaultTemplate, StringComparison.Ordinal))
            {
                normalizedTemplate = TelegramTemplate;
            }

            _template = normalizedTemplate;
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
                string payload = BuildPayload(message, out var content);
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

        private string BuildPayload(string? message, out HttpContent content)
        {
            string rawMessage = message ?? string.Empty;
            string jsonMessage = JsonEncodedText.Encode(rawMessage).ToString();

            string payload = _template
                .Replace("{{message_json}}", jsonMessage)
                .Replace("{{message}}", rawMessage);

            if (_isTelegramWebhook && TryCreateTelegramContent(payload, rawMessage, out var telegramContent))
            {
                content = telegramContent;
                return payload;
            }

            content = new StringContent(payload, Encoding.UTF8, "application/json");
            return payload;
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

        private static bool TryCreateTelegramContent(string payload, string rawMessage, out HttpContent content)
        {
            try
            {
                using var document = JsonDocument.Parse(payload);
                if (document.RootElement.ValueKind != JsonValueKind.Object)
                {
                    content = null!;
                    return false;
                }

                Dictionary<string, string> parameters = new();

                foreach (JsonProperty property in document.RootElement.EnumerateObject())
                {
                    parameters[property.Name] = property.Value.ValueKind switch
                    {
                        JsonValueKind.String => property.Value.GetString() ?? string.Empty,
                        JsonValueKind.Number => property.Value.GetRawText(),
                        JsonValueKind.True => "true",
                        JsonValueKind.False => "false",
                        JsonValueKind.Null => string.Empty,
                        _ => property.Value.GetRawText()
                    };
                }

                if (!parameters.TryGetValue("text", out var textValue) || string.IsNullOrWhiteSpace(textValue))
                {
                    parameters["text"] = rawMessage;
                }

                content = new FormUrlEncodedContent(parameters);
                return true;
            }
            catch (JsonException)
            {
                content = null!;
                return false;
            }
        }

        private static bool IsTelegramWebhook(string url)
        {
            if (!Uri.TryCreate(url, UriKind.Absolute, out var uri))
            {
                return false;
            }

            return uri.Host.Contains("api.telegram.org", StringComparison.OrdinalIgnoreCase);
        }
    }
}
