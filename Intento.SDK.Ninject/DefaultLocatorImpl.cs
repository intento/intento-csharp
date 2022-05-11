using System.Net;
using System.Reflection;
using Intento.SDK.Client;
using Intento.SDK.DI;
using Intento.SDK.Settings;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Ninject;
using Ninject.Activation;
using Ninject.Syntax;

namespace Intento.SDK.Ninject
{
    /// <summary>
    /// Locator implementation for default
    /// </summary>
    public sealed class DefaultLocatorImpl: ILocatorImpl
    {
        private IKernel _kernel;

        private T CreateClient<T>(Options options, IContext c) where T : BaseHttpClient
        {
            HttpClient client;
            if (options.Proxy?.ProxyUri != null && options.Proxy.ProxyEnabled)
            {
                var proxy = new WebProxy()
                {
                    Address = options.Proxy.ProxyUri,
                    UseDefaultCredentials = false
                };

                if (!string.IsNullOrWhiteSpace(options.Proxy.ProxyUserName))
                {
                    proxy.Credentials = new NetworkCredential(userName: options.Proxy.ProxyUserName,
                        password: options.Proxy.ProxyPassword);
                }

                var httpClientHandler = new HttpClientHandler()
                {
                    Proxy = proxy,
                };
                client = new HttpClient(httpClientHandler, false);
            }
            else
                client = new HttpClient();

            var logger = c.Kernel.Get<ILogger<T>>();
            return (T)Activator.CreateInstance(typeof(T), client, options, logger);
        }

        /// <inheritdoc />
        public void Init(Options options, IServiceCollection services = null)
        {
            _kernel = new StandardKernel();
            RegisterExtensions(_kernel);
            _kernel.Bind(typeof(ILogger<>))
                .To(typeof(Logger<>))
                .InSingletonScope();
            _kernel.Bind<IntentoHttpClient>().ToMethod(c => CreateClient<IntentoHttpClient>(options, c))
                .InSingletonScope();
            _kernel.Bind<TelemetryHttpClient>().ToMethod(c => CreateClient<TelemetryHttpClient>(options, c))
                .InSingletonScope();
            _kernel.Bind<Options>().ToMethod(c => options).InSingletonScope();
            if (services != null)
            {
                Register(_kernel, services);
            }
        }

        /// <inheritdoc />
        public T Resolve<T>()
        {
            return _kernel == null ? default : _kernel.Get<T>();
        }

        private static void RegisterExtensions(IBindingRoot kernel)
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            foreach (var assembly in assemblies)
            {
                if (assembly.GetCustomAttribute<IntentoComponentsAttribute>() == null)
                {
                    continue;
                }

                var types = assembly.GetTypes();
                foreach (var type in types)
                {
                    var attr = type.GetCustomAttribute<RegisterExtensionAttribute>();
                    if (attr == null)
                    {
                        continue;
                    }
                    if (Activator.CreateInstance(type) is IContainerRegisterExtension extension)
                    {
                        var services = extension.GetServices();
                        Register(kernel, services);
                    }
                }
            }
        }
        
        private static void Register(
            IBindingRoot kernel,
            IEnumerable<ServiceDescriptor> descriptors)
        {
            foreach (var descriptor in descriptors)
            {
                if (descriptor.ImplementationType != null)
                {
                    // Test if the an open generic type is being registered
                    kernel.Bind(descriptor.ServiceType)
                        .To(descriptor.ImplementationType)
                        .ConfigureLifecycle(descriptor.Lifetime);
                }
                else if (descriptor.ImplementationFactory != null)
                {
                    kernel.Bind(descriptor.ServiceType).ToMethod(context =>
                    {
                        var serviceProvider = context.Kernel.Get<IServiceProvider>();
                        return descriptor.ImplementationFactory(serviceProvider);
                    }).ConfigureLifecycle(descriptor.Lifetime);
                }
                else
                {
                    kernel.Bind(descriptor.ServiceType)
                        .ToConstant(descriptor.ImplementationInstance)
                        .ConfigureLifecycle(descriptor.Lifetime);
                }
            }
        }
    }
}
