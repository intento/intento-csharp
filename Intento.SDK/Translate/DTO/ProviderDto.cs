using System.Collections.Generic;
using Newtonsoft.Json;

namespace Intento.SDK.Translate.DTO
{
    /// <summary>
    /// Provider info
    /// </summary>
    [JsonObject("provider")]
    public class ProviderDto: BaseProviderDto
    {
        [JsonProperty("delegated_credentials")]
        public bool DelegatedCredentials { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("score")]
        public int Score { get; set; }

        [JsonProperty("price")]
        public int Price { get; set; }

        [JsonProperty("api_id")]
        public string ApiId { get; set; }

        [JsonProperty("picture")]
        public string Picture { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("pairs")]
        public PairDto[] Pairs { get; set; }

        [JsonProperty("symmetric")]
        public string[] Symmetric { get; set; }
    }
}