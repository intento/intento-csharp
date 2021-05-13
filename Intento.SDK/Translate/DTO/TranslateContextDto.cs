using Newtonsoft.Json;

namespace Intento.SDK.Translate.DTO
{
    [JsonObject("context")]
    public class TranslateContextDto
    {
        [JsonProperty("text")]
        public string[] Text { get; set; }

        [JsonProperty("to")]
        public string To { get; set; }
        
        [JsonProperty("from")]
        public string From { get; set; }

        [JsonProperty("format")]
        public string Format { get; set; }

        [JsonProperty("category")]
        public string Category { get; set; }

        [JsonProperty("glossary")]
        public string Glossary { get; set; }
    }
}