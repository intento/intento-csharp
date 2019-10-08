using System;
using System.Net.Http;
using System.Net;
using System.Runtime.Serialization;

namespace IntentoSDK
{
    [Serializable]
    public class IntentoException : Exception
    {
        protected internal IntentoException(string message)
        {
        }

        protected IntentoException(SerializationInfo info, StreamingContext context)
            : base(info, context) { }

        /// <summary>
        /// Makes appropriate Exception depending on response
        /// </summary>
        /// <param name="response"></param>
        /// <param name="jsonResult"></param>
        /// <returns></returns>
        public static IntentoException Make(HttpResponseMessage response, dynamic jsonResult)
        {
            switch(response.StatusCode)
            {
                case HttpStatusCode.Forbidden:
                    return new IntentoInvalidApiKeyException(response, jsonResult);
            }

            return new IntentoApiException(response, jsonResult);
        }

    }

    [Serializable]
    public class IntentoSdkException : IntentoException
    {
        protected internal IntentoSdkException(string message)
            : base(message)
        {
        }

        protected IntentoSdkException(SerializationInfo info, StreamingContext context)
            : base(info, context) { }
    }

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

    [Serializable]
    public class IntentoApiException : IntentoException
    {
        HttpResponseMessage response;
        dynamic jsonResult;

        protected internal IntentoApiException(HttpResponseMessage response, object jsonResult)
            : base("Error in Intento API call")
        {
            this.response = response;
            this.jsonResult = jsonResult;
        }

        protected IntentoApiException(SerializationInfo info, StreamingContext context)
            : base(info, context) { }

        public HttpStatusCode StatusCode
        { get { return response.StatusCode; } }

        public int StatusCodeInt
        { get { return (int)response.StatusCode; } }

        public string ReasonPhrase
        { get { return response.ReasonPhrase; } }

        public dynamic Content
        { get { return jsonResult; } }

        public dynamic JsonResult
        { get { return jsonResult; } }

    }

    [Serializable]
    public class IntentoInvalidParameterException: IntentoException
    {
        public IntentoInvalidParameterException(string parameterName, string hint = null)
            : base(hint != null ? 
                  string.Format("Invalid {0} parameter: {1}", parameterName, hint) : 
                  string.Format("Invalid {0} parameter", parameterName))
        { }

        protected IntentoInvalidParameterException(SerializationInfo info, StreamingContext context)
            : base(info, context) { }
    }
}
