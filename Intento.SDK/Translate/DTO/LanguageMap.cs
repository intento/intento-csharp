using Newtonsoft.Json;

namespace Intento.SDK.Translate.DTO
{
    [JsonObject("language_map")]
    public class LanguageMap
    {
        [JsonProperty("symmetric")]
        public string[] Symmetric { get; set; }
        
        [JsonProperty("pairs")]
        public LanguagePair[] Pairs { get; set; }
    }
}