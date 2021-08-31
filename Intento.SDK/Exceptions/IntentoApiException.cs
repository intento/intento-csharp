using System;
using System.Net;
using System.Net.Http;
using System.Runtime.Serialization;

namespace Intento.SDK.Exceptions
{
    [Serializable]
    public class IntentoApiException : IntentoException
    {
        private readonly HttpResponseMessage _response;
        
        protected internal IntentoApiException(HttpResponseMessage response, object jsonResult)
            : base("Error in Intento API call")
        {
            this._response = response;
            JsonResult = jsonResult;
        }

        protected IntentoApiException(SerializationInfo info, StreamingContext context)
            : base(info, context) { }

        public HttpStatusCode StatusCode => _response?.StatusCode ?? HttpStatusCode.BadGateway;

        public string ReasonPhrase => _response?.ReasonPhrase;

        public object JsonResult { get; }
        
    }
}
