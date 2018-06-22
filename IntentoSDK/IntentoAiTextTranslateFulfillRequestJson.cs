using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace IntentoSDK
{
    [DataContract]
    internal struct IntentoAiTextTranslateFulfillRequestContentJson
    {
        [DataMember]
        public List<string> text;
        [DataMember]
        public string to;
        [DataMember]
        public string from;
    }

    [DataContract]
    internal struct IntentoAiTextTranslateFulfillRequestServiceJson
    {
        [DataMember]
        public bool async;
    }

    [DataContract]
    internal struct IntentoAiTextTranslateFulfillRequestJson
    {
        [DataMember]
        public IntentoAiTextTranslateFulfillRequestContentJson context;

        [DataMember]
        public IntentoAiTextTranslateFulfillRequestServiceJson service;
    }
}
