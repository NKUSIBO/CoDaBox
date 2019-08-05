using System;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
namespace Inocrea.CodaBox.ApiServer.Services
{
    public class ApiZoho:ApiBase
    {

        private string url = "https://accounts.zoho.com/oauth/v2/token?refresh_token=1000.79e58712713e023b5d22837b16f67491.f740bbff11e0a42826c9783cea6c4e72&client_id=1000.H6F3FI4R4KNM30739F6DC6V3PRVIPH&client_secret=984d92f7749e2f232d4c5583e3c3eab67fb6c225ab&redirect_uri=http://inocea.be/callback&grant_type=refresh_token";
        private string _token;
        public string Token {
            get
            {
                if (_token == null) _token = RefreshToken();
                return _token;
            }
        }

        private string RefreshToken()
        {
            var uri = new Uri(url);
            var rep = PostAsync(uri).Result;
            var json = JToken.Parse(rep);
            var token = json.Value<string>("access_token");
            return token;
        }
    }
}
