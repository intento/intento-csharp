using Newtonsoft.Json;

namespace Intento.SDK.Translate.DTO
{
    internal class AgnosticGlossariesResult: BaseResponseResult
    {
        [JsonProperty("glossaries")]
        public GlossaryDetailed[] Glossaries { get; set; }
    }
}