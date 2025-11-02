using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace VRChatAutoFishing
{
    public struct NotifyResult
    {
        public readonly bool success;
        public readonly string message;

        public NotifyResult(bool v1, string v2) : this()
        {
            this.success = v1;
            this.message = v2;
        }
    }

    public interface INotificationHandler
    {
        Task<NotifyResult> NotifyAsync(string message);

        public NotifyResult Notify(string message) {
            var task = Task.Run(async () =>
            {
                return (await NotifyAsync(message));

            });
            task.Wait();
            return task.Result;
        }
    }

    public class NotificationManager
    {
        private readonly List<INotificationHandler> _handlers = new();

        public void AddHandler(INotificationHandler handler)
        {
            if (handler == null) throw new ArgumentNullException(nameof(handler));
            _handlers.Add(handler);
        }

        public void Clear() { _handlers.Clear(); }

        public bool HasHandlers() { return _handlers.Count > 0; }

        public NotifyResult NotifyAll(string message)
        {
            if (_handlers.Count() == 0)
                return new(false, "没有配置任何通知处理器");

            List<NotifyResult> results = new();
            try
            {
                Task.Run(async () =>
                {
                    foreach (var handler in _handlers)
                    {
                        NotifyResult res = await handler.NotifyAsync(message);
                        results.Add(res);
                    }
                }).Wait();
            }
            catch (AggregateException ex)
            {
                Console.WriteLine();
                return new(false, $"通知发送失败: {ex.Flatten().InnerException?.Message}");
            }

            bool finalSuccess = true;
            string finalMessage = "";
            foreach (var res in results)
            {
                if (!res.success)
                {
                    finalSuccess = false;
                    finalMessage += $"Error({res.message})\n";
                }
            }

            return new(finalSuccess, finalMessage);
        }
    }
}