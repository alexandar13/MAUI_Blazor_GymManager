using Camera.MAUI;
using MAUI_Blazor_GymManager.Extensions;
using Microsoft.Extensions.Logging;
using Services.Interfaces;

namespace MAUI_Blazor_GymManager
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseMauiCameraView()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                });


            builder.Services.ConfigureInternalServices();
            builder.Services.InitializeHttpClients();
            builder.Services.AddMauiBlazorWebView();

#if DEBUG
    		builder.Services.AddBlazorWebViewDeveloperTools();
    		builder.Logging.AddDebug();
#endif
            var app = builder.Build();

            app.Services.ConfigureNavigation();
            return app;
        }
    }
}
