using Newtonsoft.Json;

namespace Intento.SDK.Translate.DTO
{
    [JsonObject("response_provider")]
    public class TranslateResponseProvider: BaseProvider
    {
        [JsonProperty("timing")]
        public TranslateResponseTiming Timing { get; set; }
    }
}