using System.Collections.Concurrent;


namespace CoffeeShop.Service
{
    internal class NotificationService
    {
        public event Action<string>? ShowNotification;

        public NotificationService()
        {
        }

        public void Notify(string message)
        {
            ShowNotification?.Invoke(message);
        }
    }
}
