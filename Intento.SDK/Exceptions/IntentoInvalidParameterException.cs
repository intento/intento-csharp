using System;
using System.Runtime.Serialization;

namespace Intento.SDK.Exceptions
{
    [Serializable]
    public class IntentoInvalidParameterException : IntentoException
    {
        public IntentoInvalidParameterException(string parameterName, string hint = null)
            : base(hint != null ? $"Invalid {parameterName} parameter: {hint}"
                : $"Invalid {parameterName} parameter")
        { }

        protected IntentoInvalidParameterException(SerializationInfo info, StreamingContext context)
            : base(info, context) { }
    }
}
