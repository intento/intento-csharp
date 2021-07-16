using Intento.SDK.Translate.Converters;
using Newtonsoft.Json;

namespace Intento.SDK.Translate.DTO
{
    [JsonObject("context")]
    public class TranslateContextDto
    {
        [JsonProperty("text")]
        [JsonConverter(typeof(ArrayToSingleStringConverter))]
        public string[] Text { get; set; }

        [JsonProperty("to")]
        public string To { get; set; }
        
        [JsonProperty("from")]
        public string From { get; set; }

        [JsonProperty("format")]
        public string Format { get; set; }

        [JsonProperty("category")]
        public string Category { get; set; }

        [JsonIgnore]
        public string Glossary { get; set; }

        [JsonProperty("glossary")]
        public GlossaryInfo[] Glossaries { get; set; }
    }
}