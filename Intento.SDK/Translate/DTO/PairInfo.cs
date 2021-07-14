using Newtonsoft.Json;

namespace Intento.SDK.Translate.DTO
{
    /// <summary>
    /// Pair info
    /// </summary>
    [JsonObject("pair")]
    public class PairInfo
    {
        [JsonProperty("to")]
        public string To { get; set; }

        [JsonProperty("from")]
        public string From { get; set; }
    }
}