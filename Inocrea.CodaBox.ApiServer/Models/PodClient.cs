using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Inocrea.CodaBox.ApiServer
{
    public class PodClient
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("fetch_delay")]
        public int FetchDelay { get; set; }

        [JsonProperty("allowed_formats")]
        public AllowedFormats AllowedFormats { get; set; }

        [JsonProperty("feed_clients")]
        public List<FeedClient> FeedClients { get; set; }
    }

    public class AllowedFormats
    {
        [JsonProperty("expense")]
        public List<string> Expense { get; set; }

        [JsonProperty("soda")]
        public List<string> Soda { get; set; }

        [JsonProperty("purchase_invoice")]
        public List<string> PurchaseInvoice { get; set; }

        [JsonProperty("sales_invoice")]
        public List<string> SalesInvoice { get; set; }

        [JsonProperty("coda")]
        public List<string> Coda { get; set; }
    }

    public class FeedClient
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("client_id")]
        public Guid ClientId { get; set; }

        [JsonProperty("client_name")]
        public string ClientName { get; set; }

        [JsonProperty("client_code")]
        public string ClientCode { get; set; }

        [JsonProperty("fiduciary_id")]
        public Guid FiduciaryId { get; set; }

        [JsonProperty("target_root_state")]
        public string TargetRootState { get; set; }

        [JsonProperty("delivery_config")]
        public object DeliveryConfig { get; set; }
    }
}
