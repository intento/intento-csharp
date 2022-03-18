using System;
using Intento.SDK.Settings;
using NUnit.Framework;

namespace Intento.SDK.Tests
{
    /// <summary>
    /// Base class for tests
    /// </summary>
    public abstract class IntentoTests
    {
        [OneTimeSetUp]
        public virtual void Setup()
        {
            var assembly = typeof(TranslationServiceTests).Assembly;
            var assemblyVersion = IntentoHelpers.GetVersion(assembly);
            var options = new Options
            {
                ApiKey = Environment.GetEnvironmentVariable("IntentoAPIKey"),
                ClientUserAgent = $"Intento.SDK.Test/{assemblyVersion}",
                TmsServerUrl = "https://connectors-stage.inten.to/tms",
                ServerUrl = "https://api2.inten.to"
            };
            IntentoClient.Init(options);
        }
    }
}