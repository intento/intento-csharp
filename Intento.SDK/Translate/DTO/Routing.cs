using Newtonsoft.Json;

namespace Intento.SDK.Translate.DTO
{
    [JsonObject("routing")]
    public class Routing
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("pairs")]
        public LanguagePair[] Pairs { get; set; }
    }
}