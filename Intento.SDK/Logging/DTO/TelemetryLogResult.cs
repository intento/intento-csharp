using Newtonsoft.Json;

namespace Intento.SDK.Logging.DTO
{
    public class TelemetryLogResult
    {
        [JsonProperty("ok")]
        public bool Ok { get; set; }

        [JsonProperty("error")]
        public string Error { get; set; }
    }
}