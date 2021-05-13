using Newtonsoft.Json;

namespace Intento.SDK.Translate.DTO
{
    [JsonObject("routing_response")]
    public class RoutingResponseDto
    {
        [JsonProperty("routing")]
        public RoutingDto[] Routing { get; set; }
    }
}