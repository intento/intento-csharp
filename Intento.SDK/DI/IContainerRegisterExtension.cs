using Microsoft.Extensions.DependencyInjection;

namespace Intento.SDK.DI
{
    /// <summary>
    /// Register additional services to DI
    /// </summary>
    public interface IContainerRegisterExtension
    {
        void Register(IServiceCollection services);
    }
}
