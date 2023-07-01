using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace BarcodeQrScanner
{
    public static class ApiHandler
    {
        private static string _apiKey = "39521ef5-b6e5-4f02-abf5-de9c8003a697";
        private static readonly HttpClient _client = new HttpClient();
        public static async Task<string> PostToApi(string uri, string resultQrCode, string token)
        {
            string[] args = resultQrCode.Split('?');
            var content = args[1].Split('&');
            var lastArgs = new Dictionary<string, string>();
            try
            {
                foreach ( var arg in content )
                {
                    var headerObject = arg.Split('=');
                    lastArgs.Add(headerObject[0], headerObject[1]);
                }
                lastArgs.Add("Authorization", token);
                var finalContent = new FormUrlEncodedContent(lastArgs);
                var response = await _client.PostAsync(uri, finalContent);
                if (response != null && response.IsSuccessStatusCode)
                {
                    return response.StatusCode + ", post request was sent";
                } else
                {
                    return "Response wasn't successful!";
                }
                    
            }
            catch
            {
                return "false";
            }

        }
        public static async Task<string> GetApiKey(string authUri, string login, string password) 
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Authorization =
                        new AuthenticationHeaderValue(
                        "Basic", Convert.ToBase64String(
                        System.Text.ASCIIEncoding.ASCII.GetBytes(
                        $"{login}:{password}")));
                    client.DefaultRequestHeaders.Add("APIKEY", _apiKey); 
                    var response = await client.PostAsync(authUri, null);
                    if (response != null && response.IsSuccessStatusCode)
                    {
                        HttpHeaders headers = response.Headers;
                        IEnumerable<string> values;
                        if(headers.TryGetValues("Token", out values))
                        {
                            return values.First();
                        }
                        return null;
                    }
                    else
                    {
                        return null;
                    }
                }
            } catch
            {
                return null;
            }
        }
    }
}
