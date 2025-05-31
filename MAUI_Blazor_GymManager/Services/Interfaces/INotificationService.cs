namespace Services.Interfaces
{
    public interface INotificationService
    {
        Task ShowSuccess(string message);
        Task ShowError(string message);
        Task ShowInfo(string message);
    }
}
