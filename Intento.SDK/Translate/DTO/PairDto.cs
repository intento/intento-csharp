using Newtonsoft.Json;

namespace Intento.SDK.Translate.DTO
{
    [JsonObject("pair")]
    public class PairDto
    {
        [JsonProperty("from")]
        public string From { get; set; }

        [JsonProperty("to")]
        public string To { get; set; }
    }
}