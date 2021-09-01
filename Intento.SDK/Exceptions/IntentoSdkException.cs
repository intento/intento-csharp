using System;
using System.Runtime.Serialization;

namespace Intento.SDK.Exceptions
{
    /// <summary>
    /// Exception in SDK
    /// </summary>
    [Serializable]
    public class IntentoSdkException : IntentoException
    {
        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="message"></param>
        protected internal IntentoSdkException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        protected IntentoSdkException(SerializationInfo info, StreamingContext context)
            : base(info, context) { }
    }
}
