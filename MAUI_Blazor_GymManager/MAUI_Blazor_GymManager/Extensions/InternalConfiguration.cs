using Blazored.Toast;
using Clients;
using MAUI_Blazor_GymManager.Authentication.AuthHelper;
using MAUI_Blazor_GymManager.Authentication.Token;
using MAUI_Blazor_GymManager.Handlers;
using Refit;
using Services;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MAUI_Blazor_GymManager.Extensions
{
    public static class InternalConfiguration
    {
        public static void ConfigureAuth(this IServiceCollection services)
        {
            services.AddSingleton<ITokenStorage, SecureTokenStorage>();
            services.AddSingleton<IAuthTokenStore, AuthTokenStore>();
            services.AddScoped<ITokenService, TokenService>();
            services.AddTransient<AuthHandler>();
        }

        public static void ConfigureInternalServices(this IServiceCollection services)
        {
            services.AddSingleton<INavigationService, NavigationService>();
            services.AddBlazoredToast();
            services.AddScoped<INotificationService, BlazorNotificationService>();
            services.AddTransient<ErrorHandlingHandler>();
        }

        public static void InitializeHttpClients(this IServiceCollection services)
        {
            services.AddRefitClient<IAuthApi>()
                .ConfigureHttpClient(c => c.BaseAddress = new Uri("http://192.168.1.5:5001"))
                .AddHttpMessageHandler<ErrorHandlingHandler>();

            services.AddRefitClient<IUsersApi>()
                .ConfigureHttpClient(c => c.BaseAddress = new Uri("http://192.168.1.5:5001"))
                .AddHttpMessageHandler<ErrorHandlingHandler>()
                .AddHttpMessageHandler<AuthHandler>();
        }

        public static void ConfigureNavigation(this IServiceProvider services)
        {
            var navService = services.GetRequiredService<INavigationService>();

            navService.NavigateToMauiPageAction = async (pageNavigation) =>
            {
                await Application.Current.MainPage.Navigation.PushAsync(MAUI_Blazor_GymManager.PagesXaml.Routes.getPageByRoute(pageNavigation));
            };
        }
    }
}
