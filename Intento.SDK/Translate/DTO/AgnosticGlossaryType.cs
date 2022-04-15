using Newtonsoft.Json;

namespace Intento.SDK.Translate.DTO
{
    /// <summary>
    /// Glossary
    /// </summary>
    public class AgnosticGlossaryType
    {
        /// <summary>
        /// Unique Id
        /// </summary>
        [JsonProperty("id")]
        public string Id { get; set; }

        /// <summary>
        /// Name of glossary type
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }
    }
}