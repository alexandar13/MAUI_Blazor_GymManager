using Camera.MAUI;

namespace MAUI_Blazor_GymManager.PagesXaml.Pages;

public partial class Scanner : ContentPage
{

#if ANDROID || IOS || MACCATALYST
    public Scanner()
    {
        InitializeComponent();
    }

    private void cameraView_CamerasLoaded(object sender, EventArgs e)
    {
        if (cameraView.Cameras.Count > 0)
        {
            cameraView.Camera = cameraView.Cameras.First();
            MainThread.BeginInvokeOnMainThread(async () =>
            {
                await cameraView.StopCameraAsync();
                await cameraView.StartCameraAsync();
            });
        }
    }

    private void CameraView_barcodeDetected(object sender, Camera.MAUI.ZXingHelper.BarcodeEventArgs args)
    {
        MainThread.BeginInvokeOnMainThread(() =>
        {
            barcodeResult.Text = args.Result[0].Text;
        });
    }

    private async void GetDeviceLocation(object sender, EventArgs e)
    {
        var request = new GeolocationRequest(GeolocationAccuracy.High, TimeSpan.FromSeconds(10));
        var result = await Geolocation.GetLocationAsync(request);

        if (result != null)
        {
            locatoinResultLongitude.Text = result.Longitude.ToString();
            locatoinResultLatitude.Text = result.Latitude.ToString();
        }
    }

#endif
}