using Newtonsoft.Json;

namespace Intento.SDK.Translate.DTO
{
    /// <summary>
    /// Glossary info
    /// </summary>
    public class GlossaryInfo
    {
        [JsonProperty("id")]
        public int Id { get; set; }
    }
}