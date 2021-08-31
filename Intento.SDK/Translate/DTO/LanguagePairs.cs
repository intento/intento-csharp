using Newtonsoft.Json;

namespace Intento.SDK.Translate.DTO
{
    /// <summary>
    /// Info about language pairs
    /// </summary>
    [JsonObject("pairs_info")]
    public class LanguagePairs
    {
        [JsonProperty("pairs")]
        public LanguagePair[] Pairs { get; set; }
    }
}