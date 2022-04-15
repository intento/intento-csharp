using System.Collections.Generic;
using Newtonsoft.Json;

namespace Intento.SDK.Translate.DTO
{
    internal class AgnosticGlossariesTypesResult: BaseResponseResult
    {
        [JsonProperty("types")]
        public IList<AgnosticGlossaryType> Types { get; set; }
    }
}