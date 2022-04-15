using Newtonsoft.Json;

namespace Intento.SDK.Translate.DTO
{
    public class NativeGlossary
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("from")]
        public string From { get; set; }

        [JsonProperty("to")]
        public string To { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("entry_count")]
        public int EntryCount { get; set; }

        [JsonProperty("internal_id")]
        public string InternalId { get; set; }
    }
}