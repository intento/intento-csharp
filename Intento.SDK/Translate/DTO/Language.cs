using Newtonsoft.Json;

namespace Intento.SDK.Translate.DTO
{
    /// <summary>
    /// API info for languages
    /// </summary>
    [JsonObject("language")]
    public class Language
    {
        [JsonProperty("iso_name")]
        public string IsoName { get; set; }

        [JsonProperty("intento_code")]
        public string IntentoCode { get; set; }

        [JsonProperty("direction")]
        public string Direction { get; set; }
        
        public bool Equals(Language other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return IntentoCode == other.IntentoCode;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Language) obj);
        }

        public override int GetHashCode()
        {
            return IntentoCode != null ? IntentoCode.GetHashCode() : 0;
        }
        
        public static Language Autodetect = new()
        {
            IntentoCode = null,
            IsoName = "Autodetect"
        };
    }
}