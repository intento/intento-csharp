using System;
using System.Runtime.Serialization;

namespace Intento.SDK.Exceptions
{
    [Serializable]
    public class IntentoException : Exception
    {
        protected internal IntentoException(string message):
            base(message)
        {
            
        }

        protected IntentoException(SerializationInfo info, StreamingContext context)
            : base(info, context){ }

        
    }

}
