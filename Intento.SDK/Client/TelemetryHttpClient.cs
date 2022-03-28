using System;
using System.Net.Http;
using Intento.SDK.Settings;
using Microsoft.Extensions.Logging;

namespace Intento.SDK.Client
{
    public class TelemetryHttpClient: BaseHttpClient
    {
        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="client"></param>
        /// <param name="options"></param>
        /// <param name="logger"></param>
        public TelemetryHttpClient(HttpClient client, Options options, ILogger<TelemetryHttpClient> logger) : base(client, options, logger)
        {
            client.DefaultRequestHeaders.Add("x-consumer-id", DateTime.UtcNow.ToString("yyyy-MM-dd_HH"));
        }
    }
}