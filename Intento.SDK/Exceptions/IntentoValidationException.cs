using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Intento.SDK.Exceptions
{
    /// <summary>
    /// Validate SDK exception
    /// </summary>
    [Serializable]
    public class IntentoValidationException: IntentoException
    {
        /// <summary>
        /// Members with errors
        /// </summary>
        public IEnumerable<string> Members { get; }

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="message"></param>
        /// <param name="members"></param>
        protected internal IntentoValidationException(string message, IEnumerable<string> members) : base(message)
        {
            Members = members;
        }

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        protected IntentoValidationException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}