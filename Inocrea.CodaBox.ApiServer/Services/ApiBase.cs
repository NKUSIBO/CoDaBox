using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Inocrea.CodaBox.ApiServer.Services
{
    public class ApiBase
    {
        private HttpClient _httpClient;


        protected ApiBase()
        {
            _httpClient = new HttpClient();
        }

        protected void SetRequestHeaders(string key, string value)
        {
            _httpClient.DefaultRequestHeaders.Add(key, value);
        }

        protected void SetAuthorization(AuthenticationHeaderValue authenticationHeaderValue)
        {
            _httpClient.DefaultRequestHeaders.Authorization = authenticationHeaderValue;
        }

        protected async Task<string> GetAsync(Uri uri)
        {
            HttpResponseMessage response = null;
            try
            {
                response = await _httpClient.GetAsync(uri, HttpCompletionOption.ResponseHeadersRead);
                response.EnsureSuccessStatusCode();
                Console.WriteLine("GET\t" + uri);
            }
            catch (Exception)
            {
                throw new ApiException(_httpClient, "GET", uri, response);
            }
            var data = await response.Content.ReadAsStringAsync();
            return data;
        }

        protected async Task<bool> PutAsync(Uri uri, HttpContent content)
        {
            HttpResponseMessage response = null;
            try
            {
                //todo put feed
                //response = await _httpClient.PutAsync(uri, content);
                //response.EnsureSuccessStatusCode();
                Console.WriteLine("PUT\t" + uri);
            }
            catch (Exception)
            {
                throw new ApiException(_httpClient, "PUT", uri, content, response);
            }
            return true;
        }

        protected async Task PostAsync(Uri uri, byte[] data, string fileName)
        {
            var formContent = new MultipartFormDataContent();
            formContent.Add(new StreamContent(new MemoryStream(data)), "content", fileName);
            var response = await _httpClient.PostAsync(uri, formContent);
        }

        protected async Task<string> PostAsync(Uri uri)
        {
            HttpResponseMessage response = null;
            try
            {
                response = await _httpClient.PostAsync(uri, null);
                response.EnsureSuccessStatusCode();
                Console.WriteLine("POST\t" + uri);
            }
            catch (Exception)
            {
                throw new ApiException(_httpClient, "POST", uri, response);
            }
            var data = await response.Content.ReadAsStringAsync();
            return data;
        }
    }
}
