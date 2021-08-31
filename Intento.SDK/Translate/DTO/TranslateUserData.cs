using Newtonsoft.Json;

namespace Intento.SDK.Translate.DTO
{
    public class TranslateUserData
    {
        [JsonProperty("fileName")]
        public string FileName { get; set; }
    }
}