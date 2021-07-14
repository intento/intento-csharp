using Newtonsoft.Json;

namespace Intento.SDK.Translate.DTO
{
    /// <summary>
    /// Detailed provider info
    /// </summary>
    [JsonObject("provider_detailed")]
    public class ProviderDetailedDto: BaseProviderDto
    {
        [JsonProperty("logo")]
        public string Logo { get; set; }

        [JsonProperty("billing")]
        public string Billing { get; set; }

        [JsonProperty("delegated_credentials")]
        public string DelegatedCredentials { get; set; }

        [JsonProperty("auth")]
        public AuthDto Auth { get; set; }

        [JsonProperty("languages")]
        public LanguageMapDto Languages { get; set; }

        [JsonProperty("format")]
        public string[] Format { get; set; }

        [JsonProperty("lang_detect")]
        public bool LangDetect { get; set; }

        [JsonProperty("bulk")]
        public bool Bulk { get; set; }

        [JsonProperty("custom_glossary")]
        public bool CustomGlossary { get; set; }
    }
}