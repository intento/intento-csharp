using Newtonsoft.Json;

namespace Intento.SDK.Translate.DTO
{
    /// <summary>
    /// Glossary info
    /// </summary>
    public class Glossary
    {
        [JsonProperty("id")]
        public int Id { get; set; }
    }
}