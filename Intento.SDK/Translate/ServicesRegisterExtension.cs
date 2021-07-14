using Intento.SDK.DI;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;

namespace Intento.SDK.Translate
{
    [RegisterExtension]
    internal sealed class ServicesRegisterExtension: IContainerRegisterExtension
    {
        /// <inheritdoc />
        public void Register(IServiceCollection services)
        {
            services.AddSingleton<ITranslateService, TranslateDynamicService>();
        }
    }
}