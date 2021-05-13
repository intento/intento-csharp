using Newtonsoft.Json;

namespace Intento.SDK.Translate.DTO
{
    [JsonObject("timing")]
    public class TranslateResponseTimingDto
    {
        [JsonProperty("provider")]
        public double Provider { get; set; }
    }
}