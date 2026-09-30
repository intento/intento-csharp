using Newtonsoft.Json;

namespace Intento.SDK.Translate.DTO
{
    public class StorageSettings
    {
        [JsonProperty("path")]
        public string Path { get; set; }

        [JsonProperty("searchkeys")]
        public string[] SearchKeys { get; set; }
    }
}