using Newtonsoft.Json;

namespace Intento.SDK.Translate.DTO
{
    public class Error
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("reason")]
        public string Reason { get; set; }

        [JsonProperty("data")]
        public object Data { get; set; }
    }
}