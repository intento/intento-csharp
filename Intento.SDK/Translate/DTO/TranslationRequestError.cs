using Newtonsoft.Json;

namespace Intento.SDK.Translate.DTO
{
    public class TranslationRequestError
    {
        [JsonProperty("code")]
        public string Code { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }
    }
}