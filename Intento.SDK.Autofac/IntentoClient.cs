using Intento.SDK.DI;
using Intento.SDK.Settings;
using Microsoft.Extensions.DependencyInjection;

namespace Intento.SDK.Autofac
{
    /// <summary>
    /// Initializer for Intento.SDK
    /// </summary>
    public static class IntentoClient
    {
        /// <summary>
        /// Prepare Locator and services
        /// </summary>
        /// <param name="options"></param>
        ///  /// <param name="services"></param>
        /// <param name="ipml"></param>
        public static void Init(Options options, IServiceCollection services = null, ILocatorImpl ipml = null)
        {
            ipml ??= new DefaultLocatorImpl();
            Locator.SetImpl(ipml, options, services);
        }
    }
}