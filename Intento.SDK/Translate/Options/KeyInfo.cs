using Newtonsoft.Json;

namespace Intento.SDK.Translate.Options
{
    /// <summary>
    /// Key info for provider
    /// </summary>
    public class KeyInfo
    {
        /// <summary>
        /// Key of provider
        /// </summary>
        [JsonProperty("credential_id")]
        public string Key { get; set; }
    }
}