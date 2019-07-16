using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Inocrea.CodaBox.CodaApiClient;
using Inocrea.CodaBox.Web.Helper;

namespace Inocrea.CodaBox.Web.Factory
{
    internal static class ApiClientFactory
    {
        private static readonly Uri ApiUri;

        private static Lazy<ApiClient> _restClient = new Lazy<ApiClient>(
            () => new ApiClient(ApiUri),
            LazyThreadSafetyMode.ExecutionAndPublication);

        static ApiClientFactory()
        {
            ApiUri = new Uri(AppSettings.ApiUrl);
        }

        public static ApiClient Instance => _restClient.Value;
    }
}
