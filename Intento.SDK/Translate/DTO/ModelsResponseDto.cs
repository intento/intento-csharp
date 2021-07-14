using Newtonsoft.Json;

namespace Intento.SDK.Translate.DTO
{
    public class ModelsResponseDto
    {
        [JsonProperty("response")]
        public ModelDto[] Models { get; set; }
    }
}