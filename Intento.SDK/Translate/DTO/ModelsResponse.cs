using Newtonsoft.Json;

namespace Intento.SDK.Translate.DTO
{
    public class ModelsResponse
    {
        [JsonProperty("response")]
        public Model[] Models { get; set; }
    }
}