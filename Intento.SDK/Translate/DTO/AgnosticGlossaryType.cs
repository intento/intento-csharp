using Newtonsoft.Json;

namespace Intento.SDK.Translate.DTO
{
    public class AgnosticGlossaryType
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }
    }
}