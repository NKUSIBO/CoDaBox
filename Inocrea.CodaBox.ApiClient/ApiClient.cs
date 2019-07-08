using System;
using System.Net.Http;

using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Inocrea.CodaBox.ApiClient
{
    public partial class ApiClient
    {
        private readonly HttpClient _httpClient;
        private Uri BaseEndpoint { get; set; }
        public ApiClient(Uri baseEndpoint)
        {
            if (baseEndpoint == null)
            {
                throw new ArgumentNullException("baseEndpoint");
            }
            BaseEndpoint = baseEndpoint;
            _httpClient = new HttpClient();
        }
        private async Task<T> GetAsync<T>(Uri requestUrl)
        {

            var response = await _httpClient.GetAsync(requestUrl, HttpCompletionOption.ResponseHeadersRead);
            response.EnsureSuccessStatusCode();
            var data = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(data);
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
            var uriBuilder = new UriBuilder(endpoint);
            uriBuilder.Query = queryString;
            return uriBuilder.Uri;
        }
        private void addHeaders()
        {
            
            _httpClient.DefaultRequestHeaders.Add("X-Software-Company", "641088c3-8fcb-47a3-8cef-de8197f5172c");
        }
    }
}
