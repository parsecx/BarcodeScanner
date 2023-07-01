using System;
using Xamarin.Forms;
using Xamarin.Essentials;
using System.IO;
using System.Threading.Tasks;
using BarcodeQrScanner.Services;
using SkiaSharp;
using Xamarin.Forms.Shapes;

namespace BarcodeQrScanner
{
	public partial class MainPage : ContentPage
	{
        string PhotoPath = "";
        IBarcodeQRCodeService _barcodeQRCodeService;
        string _tocken = string.Empty;

        public MainPage()
		{
            InitializeComponent();
            InitService();
            _tocken = App.Tocken;
            

        }

        //https://docs.microsoft.com/en-us/xamarin/essentials/media-picker?context=xamarin%2Fxamarin-forms&tabs=android
		async void OnTakePhotoButtonClicked (object sender, EventArgs e)
		{
#pragma warning disable CS0168 // Variable is declared but never used
#pragma warning disable CS0168 // Variable is declared but never used
            try
            {
                var photo = await MediaPicker.CapturePhotoAsync();
                await LoadPhotoAsync(photo);
                Console.WriteLine($"CapturePhotoAsync COMPLETED: {PhotoPath}");
            }
            catch (FeatureNotSupportedException fnsEx)
            {
                // Feature is not supported on the device
            }
            catch (PermissionException pEx)
            {
                // Permissions not granted
            }
            catch (Exception ex)
            {
                Console.WriteLine($"CapturePhotoAsync THREW: {ex.Message}");
            }
#pragma warning restore CS0168 // Variable is declared but never used
#pragma warning restore CS0168 // Variable is declared but never used
        }

        async Task LoadPhotoAsync(FileResult photo)
        {
            // canceled
            if (photo == null)
            {
                PhotoPath = null;
                return;
            }
            // save the file into local storage
            var newFile = System.IO.Path.Combine(FileSystem.CacheDirectory, photo.FileName);
            using (var stream = await photo.OpenReadAsync())
            using (var newStream = File.OpenWrite(newFile))
                await stream.CopyToAsync(newStream);

            PhotoPath = newFile;

            await GetResponse(PhotoPath);
        }

        private async void InitService()
        {        
            _barcodeQRCodeService = DependencyService.Get<IBarcodeQRCodeService>();
            await Task.Run(() =>
            {
                try
{
                    _barcodeQRCodeService.InitSDK("DLS2eyJoYW5kc2hha2VDb2RlIjoiMjAwMDAxLTE2NDk4Mjk3OTI2MzUiLCJvcmdhbml6YXRpb25JRCI6IjIwMDAwMSIsInNlc3Npb25QYXNzd29yZCI6IndTcGR6Vm05WDJrcEQ5YUoifQ==");
                }
                catch (Exception ex)
                {
                    DisplayAlert("Error", ex.Message, "OK");
                }

                return Task.CompletedTask;
            });
        }

        public async Task<string> GetResponse(string PhotoPath)
        {
            var result = await _barcodeQRCodeService.DecodeFile(PhotoPath);
            if(result == null)
            {
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
                DisplayAlert("No data was found!", "Try scanning qr code again", "OK");
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
                return "No data";
            }    
            foreach (var item in result) 
            {

                var text = await ApiHandler.PostToApi("https://apigeosoft.mojerp.hr/ver2/sni/QRCODEPOZIVI", item.text, _tocken);
                if (text != "false")
                {
                    await DisplayAlert("Response from api", text, "OK");
                    return string.Empty;
                }
                else
                {
                    await DisplayAlert("Something went wrong", "Maybe there is no internet connection or you scaned wrong qrcode?", "Okay");
                    return string.Empty;
                }
            }
            return "Okay";
        }
    }
}

