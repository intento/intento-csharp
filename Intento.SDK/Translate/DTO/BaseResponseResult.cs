using Newtonsoft.Json;

namespace Intento.SDK.Translate.DTO
{
    internal class BaseResponseResult
    {
        [JsonProperty("status")]
        public string Status { get; set; }
    }
}