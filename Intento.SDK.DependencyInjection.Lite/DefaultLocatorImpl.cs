using System;
using System.Net;
using System.Net.Http;
using System.Reflection;
using Intento.SDK.Client;
using Intento.SDK.DI;
using Intento.SDK.Settings;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Intento.SDK.DependencyInjection.Lite
{
    /// <summary>
    /// Locator implementation for default
    /// </summary>
    public sealed class DefaultLocatorImpl: ILocatorImpl
    {
        private IServiceProvider _serviceProvider;
        
        private static T CreateClient<T>(Options options, IServiceProvider c) where T : BaseHttpClient
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

            var logger = c.GetService<ILogger<T>>();
            return (T)Activator.CreateInstance(typeof(T), client, options, logger);
        }

        

        /// <inheritdoc />
        public void Init(Options options, IServiceCollection services = null)
        {
            var needBuild = services == null;
            services ??= new ServiceCollection();
            services.AddSingleton(typeof(ILogger<>), typeof(Logger<>));
            services.AddSingleton(c => CreateClient<IntentoHttpClient>(options, c));
            services.AddSingleton(c => CreateClient<TelemetryHttpClient>(options, c));
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
