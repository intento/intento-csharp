using Intento.SDK.DI;
using Intento.SDK.Logging;
using Microsoft.Extensions.DependencyInjection;

namespace Intento.SDK.Translate
{
    [RegisterExtension]
    internal sealed class ServicesRegisterExtension: IContainerRegisterExtension
    {
        /// <inheritdoc />
        public void Register(IServiceCollection services)
        {
            services.AddSingleton<ITranslateService, TranslateDynamicService>();
            services.AddSingleton<ITelemetryService, TelemetryService>();
        }
    }
}