using Microsoft.Extensions.DependencyInjection;
using Ninject.Syntax;

namespace Intento.SDK.Ninject;

internal static class NinjectExtensions
{
    internal static IBindingWhenInNamedWithOrOnSyntax<T> ConfigureLifecycle<T>(
        this IBindingWhenInNamedWithOrOnSyntax<T> registrationBuilder,
        ServiceLifetime lifecycleKind)
    {
        switch (lifecycleKind)
        {
            case ServiceLifetime.Singleton:
                registrationBuilder.InSingletonScope();
                break;
            case ServiceLifetime.Scoped:
                registrationBuilder.InThreadScope();
                break;
            case ServiceLifetime.Transient:
                registrationBuilder.InTransientScope();
                break;
        }

        return registrationBuilder;
    }
}