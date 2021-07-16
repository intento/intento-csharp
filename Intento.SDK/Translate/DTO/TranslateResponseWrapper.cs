using Newtonsoft.Json;

namespace Intento.SDK.Translate.DTO
{
    public class TranslateResponseWrapper
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("done")]
        public bool Done { get; set; }

        [JsonProperty("response")]
        public TranslateResponseDto[] Response { get; set; }

        [JsonProperty("meta")]
        public TranslationMetaDto Meta { get; set; }

        [JsonProperty("error")]
        public Error Error { get; set; }
    }
}