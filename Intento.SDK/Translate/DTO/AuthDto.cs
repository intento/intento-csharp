using Newtonsoft.Json;

namespace Intento.SDK.Translate.DTO
{
    /// <summary>
    /// AuthInfo
    /// </summary>
    [JsonObject("auth")]
    public class AuthDto
    {
        [JsonProperty("access_token")]
        public string AccessToken { get; set; }

        [JsonProperty("project_id")]
        public string ProjectId { get; set; }
    }
}