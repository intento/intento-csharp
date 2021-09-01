using System;
using System.Net.Http;
using System.Runtime.Serialization;

namespace Intento.SDK.Exceptions
{
    /// <summary>
    /// Invalid API key in request
    /// </summary>
    [Serializable]
    public class IntentoInvalidApiKeyException : IntentoApiException
    {
        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="response"></param>
        /// <param name="jsonResult"></param>
        protected internal IntentoInvalidApiKeyException(HttpResponseMessage response, object jsonResult)
            : base(response, jsonResult)
        {
        }

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        protected IntentoInvalidApiKeyException(SerializationInfo info, StreamingContext context)
            : base(info, context) { }
    }
}
