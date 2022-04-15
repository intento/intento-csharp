using Newtonsoft.Json;

namespace Intento.SDK.Translate.DTO
{
    [JsonObject("translate_request")]
    public class TranslateRequest
    {
        [JsonProperty("context")]
        public TranslateContext Context { get; set; }

        [JsonProperty("service")]
        public TranslateService Service { get; set; }

        [JsonProperty("userdata")]
        public TranslateUserData UserData { get; set; }

        /// <summary>
        /// Ctor
        /// </summary>
        public TranslateRequest()
        {
            Context = new TranslateContext();
        }
    }
}