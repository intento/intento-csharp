using Newtonsoft.Json;

namespace Intento.SDK.Translate.DTO
{
    [JsonObject("pair")]
    public class LanguagePair
    {
        [JsonProperty("from")]
        public string From { get; set; }

        [JsonProperty("to")]
        public string To { get; set; }
    }
}