using Microsoft.Extensions.DependencyInjection;

namespace Intento.SDK.DI
{
    /// <summary>
    /// Register additional services to DI
    /// </summary>
    public interface IContainerRegisterExtension
    {
        /// <summary>
        /// Register new instances in container
        /// </summary>
        /// <param name="services"></param>
        void Register(IServiceCollection services);
    }
}
