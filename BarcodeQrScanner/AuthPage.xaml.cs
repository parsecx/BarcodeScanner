using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BarcodeQrScanner
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AuthPage : ContentPage
    {
        public AuthPage()
        {
            InitializeComponent();
        }

        private async void OnGetTokenButtonClicked(object sender, EventArgs e)
        {
            var email = mailInput.Text.Trim();
            var pass = passInput.Text.Trim();
            var token = ApiHandler.GetApiKey("https://apigeosoft.mojerp.hr/ver2/auth/gettoken", email, pass);
            if (await token != null && token is Task<string>) 
            {
                App.Tocken = await token;
                await Navigation.PushAsync(new MainPage());
            } else
            {
                await DisplayAlert("Auth error", "Your credentials were not correct or some other problem occured. Check your internet connectioin.", "OK");
            }
        }
        protected override bool OnBackButtonPressed()
        {
            return true;
        }

        private void CancelGettingTokenButtonClicked(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }
    }
}