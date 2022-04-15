# Intento C# SDK

An adapter to query Intento API. Intento provides a single API to Cognitive AI services from many vendors.
To get more information, check out [the site](https://inten.to/).

[API User Manual](https://github.com/intento/intento-api)

If you don't have a key to use Intento API, please register here [console.inten.to](https://console.inten.to)

# Build
<code>dotnet build SDK.build.proj /p:Configuration=%Configuration% /p:DoSign=%DoSign% /p:Version=%Version% /fileLogger</code>

# Sign
To sign package you need to install Intento certificate with CertificateFingerprint=d79d7faf87aa9eecc1437e7da38e81f8a547dc38

# Tests
To run test set environment variable "IntentoAPIKey". Api key you can relieve from [console.inten.to](https://console.inten.to)

# Init intento client
 ```csharp
 var options = new Options
 {
   ApiKey = "ApiKey",
   ClientUserAgent = $"Intento.SDK.Test/{assemblyVersion}"
 };
 IntentoClient.Init(options);
 ```
# Init logger
You should specify ILoggerFactory instance in container to work with SDK. If you use Intento.SDK container you should create IContainerRegisterExtension implementation.
 ```csharp
 [RegisterExtension]
 internal class LoggerRegisterExtension: IContainerRegisterExtension
 {
      public void Register(IServiceCollection services)
      {
          services.AddSingleton<ILoggerFactory, NullLoggerFactory>();
      }
 }
 ```

# DI
By default, Intento SDK creates its own container for services. By the way, you can create your own container in the app and pass servicesCollection to Init function.
 ```csharp
IntentoClient.Init(options, serviceCollection);
 ```
If you don't have your own container, you can register your own services in the container of Intento SDK (For example you want to use the injection of services)
 ```csharp
[RegisterExtension]
internal sealed class ServicesRegisterExtension: IContainerRegisterExtension
{
   /// <inheritdoc />
   public void Register(IServiceCollection services)
   {
     services.AddSingleton<ITranslateService, TranslateDynamicService>();
   }
 }
 ```

# Use intento API
You can inject ITranslateService from the container (if you use your own container) or get it from Locator.
 ```csharp
var service = Locator.Resolve<ITranslateService>();
var res = await service.AgnosticGlossariesAsync();
 ```


