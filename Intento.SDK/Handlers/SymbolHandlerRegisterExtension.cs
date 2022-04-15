using System.Collections.Generic;
using Intento.SDK.DI;
using Microsoft.Extensions.DependencyInjection;

namespace Intento.SDK.Handlers
{
    [RegisterExtension]
    internal sealed class SymbolHandlerRegisterExtension : IContainerRegisterExtension
    {
        /// <inheritdoc />
        public IEnumerable<ServiceDescriptor> GetServices()
        {
            yield return new ServiceDescriptor(typeof(ISymbolHandler), typeof(XmlSymbolHandler),
                ServiceLifetime.Singleton);
            yield return new ServiceDescriptor(typeof(ISymbolHandler), typeof(HtmlSymbolHandler),
                ServiceLifetime.Singleton);
            yield return new ServiceDescriptor(typeof(ISymbolHandlersFactory), typeof(SymbolHandlersFactory),
                ServiceLifetime.Singleton);
        }
    }
}
