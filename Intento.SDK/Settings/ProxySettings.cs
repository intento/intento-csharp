using System;

namespace Intento.SDK.Settings
{
    /// <summary>
    /// Proxy server configuration class for service requests
    /// </summary>
    public class ProxySettings
    {
        /// <summary>
        /// Is proxy enable
        /// </summary>
        public bool ProxyEnabled { get; set; }

        /// <summary>
        /// Proxy address
        /// </summary>
        public string ProxyAddress { get; set; }

        /// <summary>
        /// Proxy port number.
        /// </summary>
        public string ProxyPort { get; set; }

        /// <summary>
        /// Create proxy Uri
        /// </summary>
        public Uri ProxyUri
        {
            get
            {
                if (string.IsNullOrWhiteSpace(ProxyAddress) || string.IsNullOrWhiteSpace(ProxyPort))
                {
                    return null;
                }
                return new Uri($"http://{ProxyAddress}:{ProxyPort}");
            }
        }

        /// <summary>
        /// Username to log in to proxy server
        /// </summary>
        public string ProxyUserName { get; set; }

        /// <summary>
        /// Password to log in to proxy server
        /// </summary>
        public string ProxyPassword { get; set; }
    }
}
