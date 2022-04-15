using System;
using Newtonsoft.Json;

namespace Intento.SDK.Translate.DTO
{
    public class GlossaryDetailed
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("license_id")]
        public int LicenseId { get; set; }

        [JsonProperty("count")]
        public int Count { get; set; }

        [JsonProperty("has_draft")]
        public bool HasDraft { get; set; }

        [JsonProperty("type")]
        public int Type { get; set; }

        [JsonProperty("cs_type")]
        public int CsType { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("created_at")]
        public DateTime? Created { get; set; }

        [JsonProperty("created_at_epoch_secs")]
        public double CreatedAtEpochSecs { get; set; }

        [JsonProperty("created_client_key_id")]
        public Guid CreatedClientKeyId { get; set; }

        [JsonProperty("updated_at")]
        public DateTime? Updated { get; set; }

        [JsonProperty("updated_at_epoch_secs")]
        public double? UpdatedAtEpochSecs { get; set; }

        [JsonProperty("updated_client_key_id")]
        public Guid? UpdatedClientKeyId { get; set; }
        
        [JsonProperty("published_at")]
        public DateTime? Published { get; set; }

        [JsonProperty("published_at_epoch_secs")]
        public double? PublishedAtEpochSecs { get; set; }

        [JsonProperty("published_client_key_id")]
        public Guid? PublishedClientKeyId { get; set; }

        [JsonProperty("draft_updated_at")]
        public DateTime? DraftUpdated { get; set; }

        [JsonProperty("draft_updated_at_epoch_secs")]
        public double? DraftUpdatedAtEpochSecs { get; set; }

        [JsonProperty("language_pairs")]
        public LanguagePairGlossary[] LanguagePairs { get; set; }
    }
    
}