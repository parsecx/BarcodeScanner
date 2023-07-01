using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly:XamlCompilation(XamlCompilationOptions.Compile)]
namespace BarcodeQrScanner
{
	public class App : Application
	{
		public static string Tocken = string.Empty; 
		public App ()
		{
			if (Tocken != string.Empty)
			{
				MainPage = new NavigationPage(new BarcodeQrScanner.MainPage());
			}
			else
			{
				MainPage = new NavigationPage(new BarcodeQrScanner.AuthPage());
			}
		}

		protected override void OnStart ()
		{
			// Handle when your app starts
		}

		protected override void OnSleep ()
		{
			// Handle when your app sleeps
		}

		protected override void OnResume ()
		{
			// Handle when your app resumes
		}
	}
}

