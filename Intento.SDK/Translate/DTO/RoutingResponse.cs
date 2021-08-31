using Newtonsoft.Json;

namespace Intento.SDK.Translate.DTO
{
    [JsonObject("routing_response")]
    public class RoutingResponse
    {
        [JsonProperty("routing")]
        public Routing[] Routing { get; set; }
    }
}