using Microsoft.AspNetCore.Components;
using Newtonsoft.Json;
using Services.Interfaces;
using System;
using System.Net;
using System.Net.Sockets;
namespace MAUI_Blazor_GymManager.Handlers
{
    public class ErrorHandlingHandler (IServiceProvider serviceProvider) : DelegatingHandler
    {
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var notificationService = serviceProvider.GetRequiredService<INotificationService>();
            var nav = serviceProvider.GetRequiredService<NavigationManager>();

            if (Connectivity.Current.NetworkAccess != NetworkAccess.Internet)
            {
                await notificationService.ShowError("Internet konekcija nije dostupna.");
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

                await notificationService.ShowError("Greška prilikom slanja zahteva.");
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
                        await notificationService.ShowError(error.Message ?? $"Greška: {error.ErrorCode}");
                    else
                        await notificationService.ShowError("Greška sa servera.");

                    // You can use other status codes, such as HttpStatusCode.GatewayTimeout etc.
                    return new HttpResponseMessage(HttpStatusCode.ServiceUnavailable)
                    {
                        Content = new StringContent("exception"),
                        RequestMessage = request,
                    };
                }
                catch
                {
                    await notificationService.ShowError("Neočekivana greška u obradi odgovora.");
                }

                if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    nav.NavigateTo("/login", forceLoad: true);
                }
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
