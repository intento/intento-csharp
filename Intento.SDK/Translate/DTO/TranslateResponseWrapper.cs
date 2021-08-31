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
        public TranslateResponse[] Response { get; set; }

        [JsonProperty("meta")]
        public TranslationMeta Meta { get; set; }

        [JsonProperty("error")]
        public Error Error { get; set; }
    }
}