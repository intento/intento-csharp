using Intento.SDK.Translate.Converters;
using Newtonsoft.Json;

namespace Intento.SDK.Translate.DTO
{
    [JsonObject("context")]
    public class TranslateContext
    {
        [JsonProperty("text")]
        [JsonConverter(typeof(ArrayToSingleStringConverter))]
        public string[] Text { get; set; }

        [JsonProperty("to")]
        public string To { get; set; }
        
        [JsonProperty("from")]
        public string From { get; set; }

        [JsonProperty("outputFileExtension")]
        public string OutputFileExtension { get; set; }

        [JsonProperty("size")]
        public long Size { get; set; }

        [JsonProperty("outputFormat")]
        public string OutputFormat { get; set; }

        [JsonProperty("format")]
        public string Format { get; set; }

        [JsonProperty("category")]
        public string Category { get; set; }

        [JsonIgnore]
        public string Glossary { get; set; }

        [JsonProperty("glossary")]
        public Glossary[] Glossaries { get; set; }
    }
}