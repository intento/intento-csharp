using Newtonsoft.Json;

namespace Intento.SDK.Translate.DTO
{
    [JsonObject("pair")]
    public class LanguagePair
    {
        [JsonProperty("from")]
        public string From { get; set; }

        [JsonProperty("to")]
        public string To { get; set; }

        public bool Equals(LanguagePair other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return From == other.From && To == other.To;
        }
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((LanguagePair) obj);
        }

        public override int GetHashCode()
        {
            return 31*(From != null ? From.GetHashCode() : 0) + (To != null ? To.GetHashCode() : 0);
        }
    }
}