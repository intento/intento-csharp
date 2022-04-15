using Intento.SDK.Settings;
using Microsoft.Extensions.DependencyInjection;

namespace Intento.SDK.DI
{
    /// <summary>
    /// Locator implementation
    /// </summary>
    public interface ILocatorImpl
    {
        /// <summary>
        /// Init implementation
        /// </summary>
        /// <param name="options">Options for connections</param>
        /// <param name="services">Services for register (if null -> new ServiceCollection())</param>
        void Init(Options options, IServiceCollection services = null);

        /// <summary>
        /// Resolve service from Locator
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        T Resolve<T>();
    }
}
