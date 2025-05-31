using Microsoft.AspNetCore.Components;
using Newtonsoft.Json;
using Services;
using System.Net;
using Toolbelt.Blazor;

public class GlobalHttpInterceptor : IDisposable
{
    private readonly HttpClientInterceptor _interceptor;
    private readonly NotificationService _notificationService;
    private readonly NavigationManager _nav;

    public GlobalHttpInterceptor(HttpClientInterceptor interceptor, NotificationService notificationService, NavigationManager nav)
    {
        _interceptor = interceptor;
        _notificationService = notificationService;
        _nav = nav;

        _interceptor.BeforeSend += OnBeforeSend;
        _interceptor.AfterSend += OnAfterSend;
    }

    public void MonitorEvent() => _interceptor.AfterSend += InterceptResponse;

    private async void InterceptResponse(object? sender, HttpClientInterceptorEventArgs e)
    {
        var response = e.Response;
        if (!response.IsSuccessStatusCode)
        {
            try
            {
                var content = await response.Content.ReadAsStringAsync();
                var apiResponse = JsonConvert.DeserializeObject<ApiResponse>(content);

                var message = apiResponse?.Errors?.FirstOrDefault()?.Message ?? "Greška sa servera.";
                _notificationService.ShowMessage(message, NotificationType.Error);

                if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    _nav.NavigateTo("/login", forceLoad: true);
                }
            }
            catch
            {
                _notificationService.ShowMessage("Neočekivana greška u obradi odgovora.", NotificationType.Error);
            }
        }
    }


    private void OnBeforeSend(object? sender, HttpClientInterceptorEventArgs e)
    {
        // Možeš da dodaš loader
    }

    private async void OnAfterSend(object? sender, HttpClientInterceptorEventArgs e)
    {
        // Po želji: skloni loader
    }

    public void Dispose()
    {
        _interceptor.BeforeSend -= OnBeforeSend;
        _interceptor.AfterSend -= OnAfterSend;
        _interceptor.AfterSend -= InterceptResponse;
    }

    private class ApiResponse
    {
        public object? Data { get; set; }
        public List<ApiError>? Errors { get; set; }
    }

    private class ApiError
    {
        public int ErrorCode { get; set; }
        public object[]? Parameters { get; set; }
        public string? Message { get; set; }
    }
}
