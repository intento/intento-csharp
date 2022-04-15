using System.Net.Http;
using Intento.SDK.Settings;
using Microsoft.Extensions.Logging;

namespace Intento.SDK.Client
{
    /// <summary>
    /// Connection client to API
    /// </summary>
    public class IntentoHttpClient: BaseHttpClient
    {
        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="client">HttpClient for requests</param>
        /// <param name="options">Options of connections</param>
        /// <param name="logger">Logger implementation</param>
        public IntentoHttpClient(HttpClient client, Options options, ILogger<IntentoHttpClient> logger): base(client, options, logger)
        {
            
        }
    }
}
