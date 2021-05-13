using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Intento.SDK.Exceptions
{
    [Serializable]
    public class IntentoValidationException: IntentoException
    {
        public IEnumerable<string> Members { get; }

        protected internal IntentoValidationException(string message, IEnumerable<string> members) : base(message)
        {
            Members = members;
        }

        protected IntentoValidationException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}