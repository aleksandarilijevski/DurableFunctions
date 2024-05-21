using Newtonsoft.Json;

namespace DurableFunctions.Models
{
    public class DummyObject
    {
        [JsonProperty("id")]
        public string UniqueID { get; set; }

        public string Name { get; set; }
    }
}
