using System.Collections.Generic;
using Newtonsoft.Json;

namespace Intento.SDK.Translate.DTO
{
    /// <summary>
    /// Provider info
    /// </summary>
    [JsonObject("provider")]
    public class Provider: BaseProvider
    {
        [JsonProperty("delegated_credentials")]
        public bool DelegatedCredentials { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("score")]
        public int Score { get; set; }

        [JsonProperty("price")]
        public int Price { get; set; }

        [JsonProperty("api_id")]
        public string ApiId { get; set; }

        [JsonProperty("picture")]
        public string Picture { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("pairs")]
        public LanguagePair[] Pairs { get; set; }

        [JsonProperty("symmetric")]
        public string[] Symmetric { get; set; }
        
        
        [JsonProperty("logo")]
        public string Logo { get; set; }

        [JsonProperty("billing")]
        public string Billing { get; set; }

        [JsonProperty("auth")]
        public Auth Auth { get; set; }

        [JsonProperty("languages")]
        public LanguageMap Languages { get; set; }

        [JsonProperty("format")]
        public string[] Format { get; set; }

        [JsonProperty("lang_detect")]
        public bool LangDetect { get; set; }

        [JsonProperty("bulk")]
        public bool Bulk { get; set; }

        [JsonProperty("custom_glossary")]
        public bool CustomGlossary { get; set; }

        [JsonProperty("intento_glossary")]
        public bool IntentoGlossary { get; set; }
    }
}