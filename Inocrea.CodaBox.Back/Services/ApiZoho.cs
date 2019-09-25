using System;
using System.Net.Http;
using Newtonsoft.Json.Linq;
namespace Inocrea.CodaBox.Back.Services
{
    public class ApiZoho
    {

        private static string url = "https://accounts.zoho.com/oauth/v2/token?refresh_token=1000.79e58712713e023b5d22837b16f67491.f740bbff11e0a42826c9783cea6c4e72&client_id=1000.H6F3FI4R4KNM30739F6DC6V3PRVIPH&client_secret=984d92f7749e2f232d4c5583e3c3eab67fb6c225ab&redirect_uri=http://inocea.be/callback&grant_type=refresh_token";
        private static string _token;
        public static string Token
        {
            get
            {
                if (_token == null)
                    _token = RefreshToken();
                return _token;
            }
        }

        private static string RefreshToken()
        {
            var uri = new Uri(url);
            var client = new HttpClient();
            var rep = client.PostAsync(uri, null).Result;
            var content = rep.Content.ReadAsStringAsync().Result;
            var json = JToken.Parse(content);
            var token = json.Value<string>("access_token");
            return token;
        }
    }
}
