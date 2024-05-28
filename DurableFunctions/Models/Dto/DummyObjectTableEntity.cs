using Azure;
using Azure.Data.Tables;
using Newtonsoft.Json;
using System;

namespace DurableFunctions.Models.Dto
{
    public class DummyObjectTableEntity : ITableEntity
    {
        [JsonProperty("id")]
        public string RowKey { get; set; }

        [JsonProperty("partitionKey")]
        public string PartitionKey { get; set; } = "InputKey";

        public DateTimeOffset? Timestamp { get; set; }

        public ETag ETag { get; set; }

        public string Name { get; set; }
    }
}
