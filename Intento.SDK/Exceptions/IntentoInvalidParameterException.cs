using System;
using System.Runtime.Serialization;

namespace Intento.SDK.Exceptions
{
    /// <summary>
    /// Invalid parameter in request
    /// </summary>
    [Serializable]
    public class IntentoInvalidParameterException : IntentoException
    {
        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="parameterName"></param>
        /// <param name="hint"></param>
        public IntentoInvalidParameterException(string parameterName, string hint = null)
            : base(hint != null ? $"Invalid {parameterName} parameter: {hint}"
                : $"Invalid {parameterName} parameter")
        { }

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        protected IntentoInvalidParameterException(SerializationInfo info, StreamingContext context)
            : base(info, context) { }
    }
}
