using System;
using System.Threading;
using Inocrea.CodaBox.CodaApiClient;
using WebCodaBox.Helper;

namespace WebCodaBox.Factory
{
    internal static class ApiClientFactory
    {
        private static readonly Uri ApiUri;
        private static readonly Uri StatementsUri;


        private static Lazy<ApiClient> _restClient = new Lazy<ApiClient>(
            () => new ApiClient(ApiUri),
            LazyThreadSafetyMode.ExecutionAndPublication);
        private static Lazy<ApiClient> _restClientApiserver = new Lazy<ApiClient>(
            () => new ApiClient(StatementsUri),
            LazyThreadSafetyMode.ExecutionAndPublication);
        static ApiClientFactory()
        {
            try
            {
                ApiUri = new Uri(AppSettings.ApiUrl);
               
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        public static ApiClient Instance => _restClient.Value;
        public static ApiClient InstanceApiServer => _restClientApiserver.Value;
    }
}
