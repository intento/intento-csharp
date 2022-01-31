using System.Xml.Serialization;
using Newtonsoft.Json;

namespace Intento.SDK.Translate.DTO
{
    internal class NativeGlossaryResult
    {
        [JsonProperty("response")]
        [XmlArray]
        public NativeGlossary[] Response { get; set; }
    }
}