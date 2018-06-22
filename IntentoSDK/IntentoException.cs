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
        HttpResponseMessage response;
        dynamic jsonResult;

        protected internal IntentoException(HttpResponseMessage response, object jsonResult)
        {
            this.response = response;
            this.jsonResult = jsonResult;
        }

        // Make is used because in the future we may have more specific Exceptions with additional fields like invalid lang pair
        public static IntentoException Make(HttpResponseMessage response, dynamic jsonResult)
        {
            switch(response.StatusCode)
            {
                case HttpStatusCode.Forbidden:
                    return new IntentoInvalidApiKeyException(response, jsonResult);
            }

            return new IntentoException(response, jsonResult);
        }

        public HttpStatusCode StatusCode
        { get { return response.StatusCode; } }

        public int StatusCodeInt
        { get { return (int)response.StatusCode; } }

        public string ReasonPhrase
        { get { return response.ReasonPhrase; } }

        public dynamic Content
        { get { return jsonResult; } }

    }

    public class IntentoInvalidApiKeyException: IntentoException
    {
        protected internal IntentoInvalidApiKeyException(HttpResponseMessage response, object jsonResult)
            : base(response, jsonResult)
        { }
    }
}
