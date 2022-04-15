using Newtonsoft.Json;

namespace Intento.SDK.Translate.DTO
{
    /// <summary>
    /// Auth
    /// </summary>
    [JsonObject("auth")]
    public class Auth
    {
        [JsonProperty("access_token")]
        public string AccessToken { get; set; }

        [JsonProperty("project_id")]
        public string ProjectId { get; set; }
    }
}