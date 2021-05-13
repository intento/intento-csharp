using Newtonsoft.Json;

namespace Intento.SDK.Translate.DTO
{
    [JsonObject("routing")]
    public class RoutingDto
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("pairs")]
        public PairDto[] Pairs { get; set; }
    }
}