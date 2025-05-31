using Services.Interfaces;
using Blazored.Toast.Services;

namespace Services
{
    public class BlazorNotificationService : INotificationService
    {
        private readonly IToastService _toastService;

        public BlazorNotificationService(IToastService toastService)
        {
            _toastService = toastService;
        }

        public Task ShowSuccess(string message)
        {
            _toastService.ShowSuccess(message);
            return Task.CompletedTask;
        }

        public Task ShowError(string message)
        {
            _toastService.ShowError(message);
            return Task.CompletedTask;
        }

        public Task ShowInfo(string message)
        {
            _toastService.ShowInfo(message);
            return Task.CompletedTask;
        }
    }
}
