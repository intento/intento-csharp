using Newtonsoft.Json;

namespace Intento.SDK.Translate.DTO
{
    [JsonObject("model")]
    public class ModelDto
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("from")]
        public string From { get; set; }

        [JsonProperty("to")]
        public string To { get; set; }

        [JsonProperty("stock")]
        public bool Stock { get; set; }
    }
}