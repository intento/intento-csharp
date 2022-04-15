using System.Threading.Tasks;
using Intento.SDK.Client;
using Intento.SDK.Logging.DTO;

namespace Intento.SDK.Logging
{
    internal class TelemetryService: ITelemetryService
    {
        private TelemetryHttpClient Client { get; }
        
        public TelemetryService(TelemetryHttpClient client)
        {
            Client = client;
        }

        public async Task<TelemetryLogResult> Send(TelemetryLogItem logItem)
        {
            var path = "telemetry/upload_json";
            var result = await Client.PostAsync<TelemetryLogItem, TelemetryLogResult>(path, logItem);
            return result;
        }
    }
}