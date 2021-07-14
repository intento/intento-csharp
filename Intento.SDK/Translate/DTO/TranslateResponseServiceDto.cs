using Newtonsoft.Json;

namespace Intento.SDK.Translate.DTO
{
    [JsonObject("response_service")]
    public class TranslateResponseServiceDto
    {
        [JsonProperty("provider")]
        public TranslateResponseProviderDto Provider { get; set; }
    }
}