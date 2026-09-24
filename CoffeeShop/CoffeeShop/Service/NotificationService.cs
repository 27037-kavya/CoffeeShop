using System.Collections.Concurrent;


namespace CoffeeShop.Service
{
    internal class NotificationService
    {
        private ConcurrentQueue<string> _notification;

        public NotificationService()
        {
            this._notification = new ConcurrentQueue<string>();
        }

        public void AddNotification(string notification)
        {
            this._notification.Enqueue(notification);
        }

        public bool TryGet(out string? notification)
        {
            return this._notification.TryDequeue(out notification);
        }
    }
}
