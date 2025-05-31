using Microsoft.AspNetCore.Components;
using Newtonsoft.Json;
using Services;
using System.Net;
namespace MAUI_Blazor_GymManager.Handlers
{
    public class ErrorHandlingHandler (NotificationService notificationService, IServiceProvider serviceProvider) : DelegatingHandler
    {
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var nav = serviceProvider.GetRequiredService<NavigationManager>();

            if (Connectivity.Current.NetworkAccess != NetworkAccess.Internet)
            {
                notificationService.ShowMessage("Internet konekcija nije dostupna.", NotificationType.Error);
                return new HttpResponseMessage(HttpStatusCode.ServiceUnavailable)
                {
                    Content = new StringContent("Internet nije dostupan.")
                };
            }

            HttpResponseMessage response;

            try
            {
                response = await base.SendAsync(request, cancellationToken);
            }
            catch (Exception)
            {
                // Kada API nije dostupan udje ovde
                notificationService.ShowMessage("Greška prilikom slanja zahteva.", NotificationType.Error);
                return new HttpResponseMessage(HttpStatusCode.InternalServerError)
                {
                    Content = new StringContent("Greška prilikom slanja zahteva.")
                };
            }

            if (!response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();

                try
                {
                    var errorResponse = JsonConvert.DeserializeObject<ApiResponse>(content);

                    var error = errorResponse?.Errors?.FirstOrDefault();
                    if (error != null)
                        notificationService.ShowMessage(error.Message ?? $"Greška: {error.ErrorCode}", NotificationType.Error);
                    else
                        notificationService.ShowMessage("Greška sa servera.", NotificationType.Error);
                }
                catch
                {
                    notificationService.ShowMessage("Neočekivana greška u obradi odgovora.", NotificationType.Error);
                }

                if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    nav.NavigateTo("/login", forceLoad: true);
                }

                return null;
            }

            return response;
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
}
