using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace BarcodeQrScanner
{
    public static class ApiHandler
    {
        private static readonly HttpClient _client = new HttpClient();
        public static async Task<string> PostToApi(string uri, string resultQrCode)
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
    }
}
