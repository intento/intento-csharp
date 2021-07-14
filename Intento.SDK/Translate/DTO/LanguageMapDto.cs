using Newtonsoft.Json;

namespace Intento.SDK.Translate.DTO
{
    [JsonObject("language_map")]
    public class LanguageMapDto
    {
        [JsonProperty("symmetric")]
        public string[] Symmetric { get; set; }
        
        [JsonProperty("pairs")]
        public PairDto[] Pairs { get; set; }
    }
}