# Intento C# SDK

An adapter to query Intento API. Intento provides a single API to Cognitive AI services from many vendors.
To get more information, check out [the site](https://inten.to/).

[API User Manual](https://github.com/intento/intento-api)

If you don't have a key to use Intento API, please register here [console.inten.to](https://console.inten.to)

# Build
<code>dotnet build SDK.build.proj /p:Configuration=%Configuration% /p:DoSign=%DoSign% /p:Version=%Version% /fileLogger</code>

# Sign
Packages are signed with the Intento, Inc. code signing certificate stored in DigiCert KeyLocker
(SHA-256 fingerprint `cfe2b6c33f7e79805a3e611c4aab1a67a13c6ce8f068b711a7b49bac69086481`, timestamp server `http://timestamp.digicert.com`).

To sign locally you need the DigiCert ONE Signing Manager tools (`smctl`) installed and configured:

1. `smctl healthcheck` must report `Status: Connected` with a valid client certificate.
2. Register the KSP once (admin shell): `smctl windows ksp register`.
3. Sync the certificate into the user store: `smctl windows certsync`.
4. Build with signing: `dotnet build SDK.build.proj /p:Configuration=Release /p:DoSign=1 /p:Version=<version>`.

Fingerprint and timestamp server can be overridden with `/p:CertificateFingerprint=...` and `/p:Timestamper=...`.

Notes:

- NuGet 6.12+ (.NET SDK 9 and newer) accepts only SHA-256/384/512 fingerprints and rejects SHA-1 with NU3043. NuGet 6.11 and older (.NET SDK 8 and older) only match SHA-1 thumbprints and fail with NU3001 when given a SHA-256 fingerprint. `SDK.build.proj` picks the right one automatically based on the MSBuild version; the SHA-1 thumbprint of the same certificate is `0d1f66efbfc3f97c281800cbc3a91ab883fb1663`.
- `NU3001: No certificates were found` also happens when the certificate has not been synced into `CurrentUser\My` yet; run `smctl windows certsync` and check with `Get-ChildItem Cert:\CurrentUser\My`.
- The DigiCert KSP (`smksp.dll`) is x64-only, so signing must run in an x64 process. On Windows on ARM the default `dotnet` is native ARM64 and fails with `Provider DLL failed to initialize correctly`; sign on an x64 machine (or CI) instead.

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


