using Autofac.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Intento.SDK.Autofac;

internal static class AutofacExtensions
{
    internal static IRegistrationBuilder<object, TActivatorData, TRegistrationStyle> ConfigureLifecycle<TActivatorData, TRegistrationStyle>(
        this IRegistrationBuilder<object, TActivatorData, TRegistrationStyle> registrationBuilder,
        ServiceLifetime lifecycleKind,
        object lifetimeScopeTagForSingleton)
    {
        switch (lifecycleKind)
        {
            case ServiceLifetime.Singleton:
                if (lifetimeScopeTagForSingleton == null)
                {
                    registrationBuilder.SingleInstance();
                }
                else
                {
                    registrationBuilder.InstancePerMatchingLifetimeScope(lifetimeScopeTagForSingleton);
                }

                break;
            case ServiceLifetime.Scoped:
                registrationBuilder.InstancePerLifetimeScope();
                break;
            case ServiceLifetime.Transient:
                registrationBuilder.InstancePerDependency();
                break;
        }

        return registrationBuilder;
    }
}