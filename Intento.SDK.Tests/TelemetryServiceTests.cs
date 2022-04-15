using System.Threading.Tasks;
using Intento.SDK.DI;
using Intento.SDK.Logging;
using Intento.SDK.Logging.DTO;
using NUnit.Framework;

namespace Intento.SDK.Tests
{
    [TestFixture(Category = "Telemetry API")]
    [Parallelizable(ParallelScope.Self)]
    public class TelemetryServiceTests: IntentoTests
    {
        [Test(Description = "Send logs to Telemetry API")]
        [TestCase("5CB5D0B4-BF5B-43F3-B911-39DA16FCE512", "Test of telemetry")]
        public async Task SendTest(string sessionId, string logs)
        {
            var service = Locator.Resolve<ITelemetryService>();
            var result = await service.Send(new TelemetryLogItem
            {
                SessionId = sessionId,
                PluginName = "MemoQPlugin",
                Logs = logs
            });
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Ok);
        }
    }
}