using Intento.SDK.DI;
using Microsoft.Extensions.DependencyInjection;

namespace Intento.SDK.Handlers
{
    [RegisterExtension]
    internal sealed class SymbolHandlerRegisterExtension : IContainerRegisterExtension
    {
        /// <inheritdoc />
        public void Register(IServiceCollection services)
        {
            services
                .AddSingleton<ISymbolHandler, XmlSymbolHandler>()
                .AddSingleton<ISymbolHandler, HtmlSymbolHandler>()
                .AddSingleton<ISymbolHandlersFactory, SymbolHandlersFactory>();
        }
    }
}
