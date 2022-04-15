using Newtonsoft.Json;

namespace Intento.SDK.Translate.DTO
{
    [JsonObject("response_service")]
    public class TranslateResponseService
    {
        [JsonProperty("provider")]
        public TranslateResponseProvider Provider { get; set; }
    }
}