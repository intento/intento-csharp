using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace Intento.SDK.Translate.DTO
{
    public class Account
    {
        [JsonProperty("credential_id")]
        public string CredentialId { get; set; }

        [JsonProperty("created_at")]
        public DateTime? CreatedAt { get; set; }

        [JsonProperty("provider_id")]
        public string ProviderId { get; set; }

        [JsonProperty("credentials")]
        public AccountCredentials Credentials { get; set; }

        [JsonProperty("expiry_at")]
        public DateTime? ExpiryAt { get; set; }

        [JsonProperty("default")]
        public bool Default { get; set; }

        [JsonProperty("account")]
        public string AccountName { get; set; }

        [JsonProperty("comment")]
        public string Comment { get; set; }

        [JsonProperty("error_message")]
        public string ErrorMessage { get; set; }
    }
}