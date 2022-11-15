using System.Net.Http;
using Intento.SDK.Settings;
using Microsoft.Extensions.Logging;

namespace Intento.SDK.Client
{
    public class TmsBackendClient : BaseHttpClient
    {
        public TmsBackendClient(HttpClient client, Options options, ILogger<TmsBackendClient> logger) : base(client,
            options, logger)
        {
        }
    }
}