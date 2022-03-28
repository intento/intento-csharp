using System.Net;
using System.Reflection;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Intento.SDK.Client;
using Intento.SDK.DI;
using Intento.SDK.Settings;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Intento.SDK.Autofac
{
    /// <summary>
    /// Locator implementation for default
    /// </summary>
    internal sealed class DefaultLocatorImpl: ILocatorImpl
    {
        private IContainer container;

        private T CreateClient<T>(Options options, IComponentContext c) where T : BaseHttpClient
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

            var logger = c.Resolve<ILogger<T>>();
            return (T)Activator.CreateInstance(typeof(T), client, options, logger);
        }

        /// <inheritdoc />
        public void Init(Options options, IServiceCollection services = null)
        {
            var builder = new ContainerBuilder();
            RegisterExtensions(builder);
            builder.RegisterGeneric(typeof(Logger<>))
                .As(typeof(ILogger<>))
                .SingleInstance();
            builder.Register(c => CreateClient<IntentoHttpClient>(options, c)).SingleInstance().AsSelf();
            builder.Register(c => CreateClient<TelemetryHttpClient>(options, c)).SingleInstance().AsSelf();
            builder.Register(c => options).SingleInstance();
            if (services != null)
            {
                builder.Populate(services);
            }
            container = builder.Build();
        }

        /// <inheritdoc />
        public T Resolve<T>()
        {
            return container == null ? default : container.Resolve<T>();
        }

        private static void RegisterExtensions(ContainerBuilder builder)
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            foreach (var assembly in assemblies)
            {
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
                        builder.Populate(services);
                    }
                }
            }
        }
    }
}
