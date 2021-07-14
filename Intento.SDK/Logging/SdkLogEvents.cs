namespace Intento.SDK.Logging
{
    /// <summary>
    /// Events for log
    /// </summary>
    internal static class SdkLogEvents
    {
        /// <summary>
        /// Http request is started
        /// </summary>
        internal const int START_HTTP_REQUEST = 1000;
        
        /// <summary>
        /// After http request
        /// </summary>
        internal const int AFTER_SEND_HTTP_REQUEST = 1001;
        
        /// <summary>
        /// Http response
        /// </summary>
        internal const int HTTP_RESPONSE = 1002;
        
        /// <summary>
        /// Http error
        /// </summary>
        internal const int HTTP_ERROR = 1003;

        /// <summary>
        /// Method is invoked
        /// </summary>
        internal const int INVOKE_METHOD = 2000;
    }
}
