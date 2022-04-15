using Newtonsoft.Json;

namespace Intento.SDK.Translate.DTO
{
    [JsonObject("translate_response")]
    public class TranslateResponse
    {
        [JsonProperty("results")]
        public string[] Results { get; set; }

        [JsonProperty("meta")]
        public TranslationMeta Meta { get; set; }

        [JsonProperty("service")]
        public TranslateResponseService Service { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("request_id")]
        public string RequestId { get; set; }
        
        [JsonProperty("error")]
        public TranslationRequestError Error { get; set; }
    }
}