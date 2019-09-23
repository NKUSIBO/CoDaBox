using System;
using System.Net.Http;

namespace WebCodaBox.Helper
{
    public class ApiServerCoda
    {
        public HttpClient Initial()
        {
            var client = new HttpClient {BaseAddress = new Uri("https://localhost:44330/") };
            return client;
        }
    }
}
