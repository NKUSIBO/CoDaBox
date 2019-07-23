using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Inocrea.CodaBox.CodaApiClient.Helper
{
    public class ApiServerCoda
    {
        public HttpClient Initial()
        {
            var client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44330/");
            return client;
        }
    }
}
