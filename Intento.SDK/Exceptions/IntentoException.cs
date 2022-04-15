using System;
using System.Runtime.Serialization;

namespace Intento.SDK.Exceptions
{
    /// <summary>
    /// Global Intento exception
    /// </summary>
    [Serializable]
    public class IntentoException : Exception
    {
        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="message"></param>
        protected internal IntentoException(string message):
            base(message)
        {
            
        }

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        protected IntentoException(SerializationInfo info, StreamingContext context)
            : base(info, context){ }

        
    }

}
