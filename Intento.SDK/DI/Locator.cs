using Intento.SDK.Settings;
using Microsoft.Extensions.DependencyInjection;

namespace Intento.SDK.DI
{
    /// <summary>
    /// Container for all services
    /// </summary>
    public static class Locator
    {
        private static ILocatorImpl _locatorImpl;

        /// <summary>
        /// Set implementation for locator
        /// </summary>
        /// <param name="impl"></param>
        /// <param name="options"></param>
        /// <param name="services"></param>
        public static void SetImpl(ILocatorImpl impl, Options options, IServiceCollection services = null)
        {
            _locatorImpl = impl;
            impl?.Init(options, services);
        }

        /// <summary>
        /// Resolve service from locator
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T Resolve<T>()
        {
            return _locatorImpl.Resolve<T>();
        }
    }
}
