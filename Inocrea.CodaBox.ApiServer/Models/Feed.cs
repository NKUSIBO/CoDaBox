using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Inocrea.CodaBox.ApiServer
{
    public class Feed
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("feed_entries")]
        public List<FeedEntry> FeedEntries { get; set; }
    }

    public class FeedEntry
    {
        [JsonProperty("feed_index")]
        public Guid FeedIndex { get; set; }

        [JsonProperty("document_model")]
        public string DocumentModel { get; set; }

        [JsonProperty("metadata")]
        public Metadata Metadata { get; set; }
    }

    public class Metadata
    {
        [JsonProperty("movement_count")]
        public int MovementCount { get; set; }

        [JsonProperty("last_statement_number")]
        public int LastStatementNumber { get; set; }

        [JsonProperty("extension_zone")]
        public object ExtensionZone { get; set; }

        [JsonProperty("new_balance_date")]
        public DateTimeOffset NewBalanceDate { get; set; }

        [JsonProperty("bank_id")]
        public string BankId { get; set; }

        [JsonProperty("currency")]
        public string Currency { get; set; }

        [JsonProperty("iban")]
        public string Iban { get; set; }

        [JsonProperty("first_statement_number")]
        public int FirstStatementNumber { get; set; }
    }

}