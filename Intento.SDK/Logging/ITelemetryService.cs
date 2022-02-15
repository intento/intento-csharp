using System.Threading.Tasks;
using Intento.SDK.Logging.DTO;

namespace Intento.SDK.Logging
{
    public interface ITelemetryService
    {
        /// <summary>
        /// Send logs to telemetry service
        /// </summary>
        /// <param name="logItem"></param>
        /// <returns></returns>
        Task<TelemetryLogResult> Send(TelemetryLogItem logItem);
    }
}