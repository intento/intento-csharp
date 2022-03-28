using System.Net;
using System.Reflection;
using Intento.SDK.Client;
using Intento.SDK.DI;
using Intento.SDK.Settings;
using Microsoft.Extensions.DependencyInjection;

namespace Intento.SDK.DependencyInjection
{
    /// <summary>
    /// Locator implementation for default
    /// </summary>
    internal sealed class DefaultLocatorImpl: ILocatorImpl
    {
        private IServiceProvider _serviceProvider;

        private void AddHttpClient<T>(Options options,  IServiceCollection services) where T : class
        {
            var client = services
                .AddHttpClient<T>();

            if (options.Proxy?.ProxyUri != null && options.Proxy.ProxyEnabled)
            {
                client
                    .ConfigurePrimaryHttpMessageHandler(() =>
                    {
                        var proxy = new WebProxy()
                        {
                            Address = options.Proxy.ProxyUri,
                            UseDefaultCredentials = false
                        };

                        if (!string.IsNullOrWhiteSpace(options.Proxy.ProxyUserName))
                        {
                            proxy.Credentials = new NetworkCredential(options.Proxy.ProxyUserName,
                                options.Proxy.ProxyPassword);
                        }

                        var httpClientHandler = new HttpClientHandler()
                        {
                            Proxy = proxy,
                        };

                        return httpClientHandler;
                    });
            }
        }

        /// <inheritdoc />
        public void Init(Options options, IServiceCollection services = null)
        {
            var needBuild = services == null;
            services ??= new ServiceCollection();
            AddHttpClient<IntentoHttpClient>(options, services);
            AddHttpClient<TelemetryHttpClient>(options, services);

            services.AddSingleton(options);
            RegisterExtensions(services);
            if (needBuild)
            {
                _serviceProvider = services.BuildServiceProvider();
            }
        }

        /// <inheritdoc />
        public T Resolve<T>()
        {
            return _serviceProvider == null ? default : _serviceProvider.GetService<T>();
        }

        private static void RegisterExtensions(IServiceCollection services)
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
                        var customServices = extension.GetServices();
                        foreach (var serviceDescriptor in customServices)
                        {
                            services.Add(serviceDescriptor);
                        }
                    }
                }
            }
        }
    }
}
