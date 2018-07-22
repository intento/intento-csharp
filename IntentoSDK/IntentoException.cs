using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net;

namespace IntentoSDK
{
    public class IntentoException : Exception
    {
        string messaage;

        protected internal IntentoException(string message)
        {
        }

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

    public class IntentoSdkException : IntentoException
    {
        protected internal IntentoSdkException(string message)
            : base(message)
        {
        }
    }

    public class IntentoInvalidApiKeyException : IntentoApiException
    {
        protected internal IntentoInvalidApiKeyException(HttpResponseMessage response, object jsonResult)
            : base(response, jsonResult)
        {
        }
    }
    
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

    public class IntentoInvalidParameterException: IntentoException
    {
        public IntentoInvalidParameterException(string parameterName, string hint = null)
            : base(hint != null ? 
                  string.Format("Invalid {0} parameter: {1}", parameterName, hint) : 
                  string.Format("Invalid {0} parameter", parameterName))
        { }
    }
}
