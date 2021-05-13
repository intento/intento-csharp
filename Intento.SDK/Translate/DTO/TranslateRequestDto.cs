using Newtonsoft.Json;

namespace Intento.SDK.Translate.DTO
{
    [JsonObject("translate_request")]
    internal class TranslateRequestDto
    {
        [JsonProperty("context")]
        public TranslateContextDto Context { get; set; }

        [JsonProperty("service")]
        public TranslateServiceDto Service { get; set; }

        /// <summary>
        /// Ctor
        /// </summary>
        public TranslateRequestDto()
        {
            Context = new TranslateContextDto();
        }
    }
}