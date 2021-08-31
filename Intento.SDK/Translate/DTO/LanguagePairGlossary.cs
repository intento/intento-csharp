using Newtonsoft.Json;

namespace Intento.SDK.Translate.DTO
{
    public class LanguagePairGlossary
    {
        [JsonProperty("source")]
        public string Source { get; set; }

        [JsonProperty("target")]
        public string Target { get; set; }
    }
}