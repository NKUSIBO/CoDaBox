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
            }
            catch (Exception)
            {
                throw new ApiException(_httpClient, "PUT", uri, content, response);
            }
            return true;
        }

        protected async Task<bool> PostFileAsync(Uri uri, byte[] data, string fileName)
        {
            var formContent = new MultipartFormDataContent();
            formContent.Add(new StreamContent(new MemoryStream(data)), "content", fileName);
            var rep = await PostAsync(uri, formContent);
            return true;
        }

        protected async Task<string> PostAsync(Uri uri, HttpContent content=null)
        {
            HttpResponseMessage response = null;
            try
            {
                response = await _httpClient.PostAsync(uri, content);
                response.EnsureSuccessStatusCode();
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
