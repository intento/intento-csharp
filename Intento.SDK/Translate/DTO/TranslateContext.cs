using System;
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
        public string Glossary
        {
            get
            {
                return JsonGlossary switch
                {
                    null => null,
                    string glossary => glossary,
                    _ => null
                };
            }
            set => JsonGlossary = value;
        }

        [JsonIgnore]
        public Glossary[] Glossaries
        {
            get
            {
                return JsonGlossary switch
                {
                    null => null,
                    Glossary[] glossaries => glossaries,
                    _ => null
                };
            }
            set => JsonGlossary = value;
        }

        /// <summary>
        /// Typeof property should be string or Glossary[]
        /// </summary>
        [JsonProperty("glossary")]
        [JsonConverter(typeof(GlossaryConverter))]
        public object JsonGlossary
        {
            get => _jsonGlossary;
            set
            {
                if (value is not string && value is not Glossary[])
                {
                    throw new Exception("Typeof property should be string or Glossary[]");
                }
                _jsonGlossary = value;
            }
        }
        
        private object _jsonGlossary;
    }
}