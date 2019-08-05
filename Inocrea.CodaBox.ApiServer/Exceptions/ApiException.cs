using System;
using System.Net.Http;

namespace Inocrea
{
    public class ApiException : Exception
    {
        public HttpClient httpClient;
        public string httpMethod;
        public Uri uri;
        public HttpContent httpContent;
        public HttpResponseMessage httpResponse;
        public string message;

        public ApiException(HttpClient httpClient, string httpMethod, Uri uri, HttpContent httpContent = null, HttpResponseMessage httpResponse = null, string message = null)
        {
            this.httpClient = httpClient;
            this.httpMethod = httpMethod;
            this.uri = uri;
            this.httpContent = httpContent;
            this.httpResponse = httpResponse;
            this.message = message;
        }

        public ApiException(HttpClient httpClient, string httpMethod, Uri uri, HttpResponseMessage httpResponse, string message = null) : this(httpClient, httpMethod, uri, null, httpResponse, message) { }
    }
}