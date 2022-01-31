using Newtonsoft.Json;

namespace Intento.SDK.Translate.DTO
{
    public class AccountCredentials
    {
        [JsonProperty("access_token")]
        public string AccessToken { get; set; }

        [JsonProperty("project_id")]
        public string ProjectId { get; set; }
    }
}