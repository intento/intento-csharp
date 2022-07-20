using System.ComponentModel.DataAnnotations;
using Intento.SDK.Validation;

namespace Intento.SDK.Settings
{
    /// <summary>
    /// Options for connections
    /// </summary>
    public class Options : BaseOptions
    {
        /// <summary>
        /// Proxy settings
        /// </summary>
        public ProxySettings Proxy { get; set; }

        /// <summary>
        /// Api key to Intento API
        /// </summary>
        [Required(ErrorMessage = "Api key to Intento can't be empty")]
        public string ApiKey { get; set; }

        /// <summary>
        /// User agent of client
        /// </summary>
        public string ClientUserAgent { get; set; }

        /// <summary>
        /// Url to Intento API
        /// </summary>
        public string ServerUrl { get; set; } = Servers.Config?.ServerUrl;

        /// <summary>
        /// Url of tms-backend
        /// </summary>
        public string TmsServerUrl { get; set; } = Servers.Config?.TmsServerUrl;

        /// <summary>
        /// Url to syncwrapper server
        /// </summary>
        public string SyncwrapperUrl { get; set; } = Servers.Config?.SyncwrapperUrl;
    }
}