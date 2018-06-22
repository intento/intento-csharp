using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace IntentoSDK
{
    [DataContract]
    internal class IntentoAiTextTranslateFulfillResultJson
    {
        [DataMember]
        public List<string> results;

        [DataMember]
        public string id;
    }
}
