using System;
using System.Net;
using System.Net.Http;
using System.Runtime.Serialization;

namespace Intento.SDK.Exceptions
{
    /// <summary>
    /// Global API exception
    /// </summary>
    [Serializable]
    public class IntentoApiException : IntentoException
    {
        private readonly HttpResponseMessage _response;
        
        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="response"></param>
        /// <param name="jsonResult"></param>
        protected internal IntentoApiException(HttpResponseMessage response, object jsonResult)
            : base("Error in Intento API call")
        {
            this._response = response;
            JsonResult = jsonResult;
        }

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        protected IntentoApiException(SerializationInfo info, StreamingContext context)
            : base(info, context) { }

        /// <summary>
        /// Status code of response
        /// </summary>
        public HttpStatusCode StatusCode => _response?.StatusCode ?? HttpStatusCode.BadGateway;

        /// <summary>
        /// Reason of response
        /// </summary>
        public string ReasonPhrase => _response?.ReasonPhrase;

        /// <summary>
        /// Result in response
        /// </summary>
        public object JsonResult { get; }
        
    }
}
