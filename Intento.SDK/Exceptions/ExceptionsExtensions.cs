using System;
using System.Net;
using System.Net.Http;

namespace Intento.SDK.Exceptions
{
    public static class ExceptionsExtensions
    {
        /// <summary>
        /// Makes appropriate Exception depending on response
        /// </summary>
        /// <param name="response">Response</param>
        /// <param name="jsonResult">Json result from response</param>
        /// <returns></returns>
        public static IntentoException Create(this HttpResponseMessage response, object jsonResult)
        {
            switch (response.StatusCode)
            {
                case HttpStatusCode.Unauthorized:
                case HttpStatusCode.Forbidden:
                    return new IntentoInvalidApiKeyException(response, jsonResult);
                default:
                    throw new IntentoApiException(response, jsonResult);
            }
        }
    }
}
