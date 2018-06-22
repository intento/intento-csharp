using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntentoSDK
{
    /// <summary>
    /// Result of AiTextTranslate.AiTextTranslateResult operation
    /// </summary>
    public struct IntentoAiTextTranslateResult
    {
        /// <summary>
        /// id of async operation. Make sence only if async=true. In this case all other fields are not used
        /// </summary>
        public string asyncId;

        public List<string> results;
    }
}
