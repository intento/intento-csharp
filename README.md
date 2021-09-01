# Intento C# SDK

An adapter to query Intento API.


Intento provides a single API to Cognitive AI services from many vendors.
To get more information check out [the site](https://inten.to/).

[API User Manual](https://github.com/intento/intento-api)

In case you don't have a key to use Intento API, please register here [console.inten.to](https://console.inten.to)

#Build 

<code>dotnet build SDK.build.proj /p:Configuration=%Configuration% /p:DoSign=%DoSign% /p:Version=%Version% /fileLogger</code>

#Sign

To sign package you need to install Intento certificate with CertificateFingerprint=d79d7faf87aa9eecc1437e7da38e81f8a547dc38