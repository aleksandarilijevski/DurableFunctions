using Azure;
using Azure.Data.Tables;
using Newtonsoft.Json;
using System;

namespace DurableFunctions.Models
{
    public class DummyObject : ITableEntity
    {
        public string PartitionKey { get; set; } = "InputKey";

        public string RowKey { get; set; }

        public DateTimeOffset? Timestamp { get; set; }

        public ETag ETag { get; set; }

        [JsonProperty("id")]
        public string UniqueID { get; set; }

        public string Name { get; set; }

        public DateTime TriggerDate { get; set; }
    }
}
