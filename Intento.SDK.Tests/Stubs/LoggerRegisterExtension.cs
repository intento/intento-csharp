using System.Collections.Generic;
using Intento.SDK.DI;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace Intento.SDK.Tests.Stubs
{
    [RegisterExtension]
    internal class LoggerRegisterExtension: IContainerRegisterExtension
    {
        public IEnumerable<ServiceDescriptor> GetServices()
        {
            yield return new ServiceDescriptor(typeof(ILoggerFactory), typeof(NullLoggerFactory),
                ServiceLifetime.Singleton);
        }
    }
}