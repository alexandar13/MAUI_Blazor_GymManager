using Clients;
using MAUI_Blazor_GymManager.Components;
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
        public static void ConfigureInternalServices(this IServiceCollection services)
        {
            services.AddSingleton<INavigationService, NavigationService>();
        }

        public static void InitializeHttpClients(this IServiceCollection services)
        {
            services.AddRefitClient<IUsersApi>().ConfigureHttpClient(c => //TODO: Token je hardkodiran. Treba se napraviti mehanizam za ubacivanje JWT-a u heder
            {
                c.BaseAddress = new Uri("http://192.168.1.5:5001");
                c.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1bmlxdWVfbmFtZSI6ImthdGFyaW5hLm0iLCJuYW1laWQiOiJhMWEzZTFmMC0wMDAxLTAwMDAtMDAwMC0wMDAwMDAwMDAwNzAiLCJyb2xlIjoiQWRtaW4iLCJuYmYiOjE3NDgyOTM4MTIsImV4cCI6MTc0ODI5NzQxMiwiaWF0IjoxNzQ4MjkzODEyLCJpc3MiOiJHeW1NYW5hZ2VyQVBJIiwiYXVkIjoiR3ltTWFuYWdlck1BVUkifQ.W9HXdiHzPX8lWQJHuAImbRhZMuHGCpScF6WDo8Zdmto");
                c.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json");
            });
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
