using System;
using System.Runtime.Serialization;

namespace Intento.SDK.Exceptions
{
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
}
