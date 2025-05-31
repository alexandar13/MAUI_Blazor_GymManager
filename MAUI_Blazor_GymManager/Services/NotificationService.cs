namespace Services
{
    public class NotificationService
    {
        public event Action<NotificationMessage>? OnShowMessage;

        public void ShowMessage(string message, NotificationType type)
        {
            OnShowMessage?.Invoke(new NotificationMessage { Message = message, Type = type });
        }
    }
    public enum NotificationType
    {
        Info,
        Success,
        Error
    }

    public class NotificationMessage
    {
        public string Message { get; set; } = "";
        public NotificationType Type { get; set; }
    }
}
