using Newtonsoft.Json;

namespace Intento.SDK.Translate.DTO
{
    [JsonObject("response_provider")]
    public class TranslateResponseProviderDto: BaseProviderDto
    {
        [JsonProperty("timing")]
        public TranslateResponseTimingDto Timing { get; set; }
    }
}