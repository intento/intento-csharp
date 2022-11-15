using System.Collections.Generic;
using Intento.SDK.DI;
using Intento.SDK.Logging;
using Intento.SDK.Tms;
using Microsoft.Extensions.DependencyInjection;

namespace Intento.SDK.Translate
{
    [RegisterExtension]
    internal sealed class ServicesRegisterExtension: IContainerRegisterExtension
    {
        /// <inheritdoc />
        public IEnumerable<ServiceDescriptor> GetServices()
        {
            yield return new ServiceDescriptor(typeof(ITranslateService), typeof(TranslateDynamicService),
                ServiceLifetime.Singleton);
            yield return new ServiceDescriptor(typeof(ITelemetryService), typeof(TelemetryService),
                ServiceLifetime.Singleton);
            yield return new ServiceDescriptor(typeof(ITmsBackendService), typeof(TmsBackendService),
                ServiceLifetime.Singleton);
        }
    }
}