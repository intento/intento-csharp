using Newtonsoft.Json;

namespace Intento.SDK.Logging.DTO
{
    public class TelemetryLogItem
    {
        [JsonProperty("session_id")]
        public string SessionId { get; set; }

        [JsonProperty("plugin_name")]
        public string PluginName { get; set; }

        [JsonProperty("logs")]
        public string Logs { get; set; }
    }
}