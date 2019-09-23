using System;
using System.Threading;
using Inocrea.CodaBox.CodaApiClient;
using WebCodaBox.Helper;

namespace WebCodaBox.Factory
{
    public class ApiServerFactory
    {
       
        private static readonly Uri StatementsUri;


        private static Lazy<ApiClient> _restClient = new Lazy<ApiClient>(
            () => new ApiClient(StatementsUri),
            LazyThreadSafetyMode.ExecutionAndPublication);

        static ApiServerFactory()
        {
            StatementsUri = new Uri(ApiServerSettings.StatementsUrl);
          

        }

        public static ApiClient Instance => _restClient.Value;
    }
}
