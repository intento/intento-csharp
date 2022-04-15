using Newtonsoft.Json;

namespace Intento.SDK.Translate.DTO
{
    [JsonObject("processing")]
    public class TranslateProcessing
    {
        [JsonProperty("pre")]
        public string[] Pre { get; set; }

        [JsonProperty("post")]
        public string[] Post { get; set; }
    }
}