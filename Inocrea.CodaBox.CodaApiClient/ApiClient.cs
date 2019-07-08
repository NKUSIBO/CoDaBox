using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Inocrea.CodaBox.CodaApiClient
{
    public partial class ApiClient
    {
        private readonly HttpClient _httpClient;
        private Uri BaseEndpoint { get; set; }
        public ApiClient(Uri baseEndpoint)
        {
            BaseEndpoint = baseEndpoint ?? throw new ArgumentNullException("baseEndpoint");
            _httpClient = new HttpClient();
        }
        private async Task<string> GetAsync<T>(Uri requestUrl)
        {
            AddHeaders();
            var response = await _httpClient.GetAsync(requestUrl, HttpCompletionOption.ResponseHeadersRead);
            
            response.EnsureSuccessStatusCode();
            var data = await response.Content.ReadAsStringAsync();
            return data;
        }
        private static JsonSerializerSettings MicrosoftDateFormatSettings
        {
            get
            {
                return new JsonSerializerSettings
                {
                    DateFormatHandling = DateFormatHandling.MicrosoftDateFormat
                };
            }
        }
        private Uri CreateRequestUri(string relativePath, string queryString = "")
        {
            var endpoint = new Uri(BaseEndpoint, relativePath);
            var uriBuilder = new UriBuilder(endpoint) {Query = queryString};
            return uriBuilder.Uri;
        }
        private void AddHeaders()
        {
            
            
            _httpClient.DefaultRequestHeaders.Add("X-Software-Company", "641088c3-8fcb-47a3-8cef-de8197f5172c");
            var byteArray = Encoding.ASCII.GetBytes("GF-4e2cee89-e8df-4a1d-b285-f7c:XyJn6NQYrm");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));
        }

   
    }
}
