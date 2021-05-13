using Newtonsoft.Json;

namespace Intento.SDK.Translate.DTO
{
    [JsonObject("meta")]
    public class TranslationMetaDto
    {
        [JsonProperty("detected_source_language")]
        public string[] DetectedSourceLanguage { get; set; }

        [JsonProperty("client_key_label")]
        public string ClientKeyLabel { get; set; }
    }
}