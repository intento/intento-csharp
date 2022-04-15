using System.Collections.Generic;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace Intento.SDK.Translate.DTO
{
    internal class AccountsResult
    {
        [JsonProperty("result")]
        [XmlArray]
        public Account[] Result { get; set; }
    }
}