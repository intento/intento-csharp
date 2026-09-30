using Newtonsoft.Json;

namespace Intento.SDK.Translate.DTO
{
    public class CacheSettings
    {
        [JsonProperty("apply")]
        public bool Apply { get; set; }
        
        [JsonProperty("update")]
        public bool Update { get; set; }
    }
}