using System;
using System.Collections.Generic;


using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

using System.Threading.Tasks;

using Inocrea.CodaBox.ApiModel;
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
        
      

       
        private async Task<List<T>> GetAsync<T> (Uri requestUrl)
        {
             //AddHeaders();
             HttpResponseMessage response;
            try
            {
                response = await _httpClient.GetAsync(requestUrl, HttpCompletionOption.ResponseHeadersRead);
                response.EnsureSuccessStatusCode();
                var data1 =  response.Content.ReadAsStringAsync().Result;

                var stat = JsonConvert.DeserializeObject<List<T>>(data1);
                response.EnsureSuccessStatusCode();
               
               
                return stat;
            }
            catch (Exception ex)
            {
                
                throw ex;
            }            
           
           
            
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
        private HttpContent CreateHttpContent<T>(T content)
        {
            var json = JsonConvert.SerializeObject(content, MicrosoftDateFormatSettings);
            return new StringContent(json, Encoding.UTF8, "application/json");
        }

        /// <summary>
        /// Common method for making POST calls
        /// </summary>
        private async Task<Message<T>> PostAsync<T>(Uri requestUrl, T content)
        {
            AddHeaders();
            var response = await _httpClient.PostAsync(requestUrl.ToString(), CreateHttpContent<T>(content));
            response.EnsureSuccessStatusCode();
            var messageRet = new Message<T>
            {
                Data= default(T),
                IsSuccess = response.IsSuccessStatusCode,
                ReturnMessage = response.ToString()


            };

            var data = await response.Content.ReadAsStringAsync();
            return messageRet;
        }
        private async Task<Message<T1>> PostAsync<T1, T2>(Uri requestUrl, T2 content)
        {
            AddHeaders();
            var response = await _httpClient.PostAsync(requestUrl.ToString(), CreateHttpContent<T2>(content));
            response.EnsureSuccessStatusCode();
            var data = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Message<T1>>(data);
        }
        private void AddHeaders()
        {
            //_httpClient.DefaultRequestHeaders.Remove("X-Software-Company");
            //_httpClient.DefaultRequestHeaders.Add("X-Software-Company", "641088c3-8fcb-47a3-8cef-de8197f5172c");
        }

    }
}
