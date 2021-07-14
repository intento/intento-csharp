using System.Collections.Generic;
using Intento.SDK.Translate.Converters;
using Intento.SDK.Translate.Options;
using Newtonsoft.Json;

namespace Intento.SDK.Translate.DTO
{
    public class TranslateServiceDto
    {
        [JsonProperty("provider")]
        public string Provider { get; set; }

        [JsonProperty("auth")]
        [JsonConverter(typeof(AuthProviderInfoConverter))]
        public AuthProviderInfo[] Auth { get; set; }

        [JsonProperty("async")]
        public bool Async { get; set; }

        [JsonProperty("routing")]
        public string Routing { get; set; }

        [JsonProperty("processing")]
        public TranslateProcessingDto Processing { get; set; }

        [JsonProperty("failover")]
        public bool Failover { get; set; }

        [JsonProperty("failover_list")]
        public string[] FailoverList { get; set; }
    }
}