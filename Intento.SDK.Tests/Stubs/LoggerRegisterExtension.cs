using Intento.SDK.DI;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace Intento.SDK.Tests.Stubs
{
    [RegisterExtension]
    internal class LoggerRegisterExtension: IContainerRegisterExtension
    {
        public void Register(IServiceCollection services)
        {
            services.AddSingleton<ILoggerFactory, NullLoggerFactory>();
        }
    }
}