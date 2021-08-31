using System;
using System.Net;
using System.Net.Http;
using System.Reflection;
using Intento.SDK.Client;
using Intento.SDK.Settings;
using Microsoft.Extensions.DependencyInjection;

namespace Intento.SDK.DI
{
    /// <summary>
    /// Locator implementation for default
    /// </summary>
    internal sealed class DefaultLocatorImpl: ILocatorImpl
    {
        private IServiceProvider _serviceProvider;

        /// <inheritdoc />
        public void Init(Options options, IServiceCollection services = null)
        {
            var needBuild = services == null;
            services ??= new ServiceCollection();
            var client = services
                .AddHttpClient<IntentoHttpClient>();

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
                        extension.Register(services);
                    }
                }
            }
        }
    }
}
