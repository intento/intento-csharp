using System;
using System.Net.Http;
using System.Runtime.Serialization;

namespace Intento.SDK.Exceptions
{
    [Serializable]
    public class IntentoInvalidApiKeyException : IntentoApiException
    {
        protected internal IntentoInvalidApiKeyException(HttpResponseMessage response, object jsonResult)
            : base(response, jsonResult)
        {
        }

        protected IntentoInvalidApiKeyException(SerializationInfo info, StreamingContext context)
            : base(info, context) { }
    }
}
