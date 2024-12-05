using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Inocrea.CodaBox.CodaApiClient;
using CodaWeb.Helper;

namespace CodaWeb.Factory
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
