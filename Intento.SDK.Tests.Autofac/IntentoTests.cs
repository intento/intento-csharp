using System;
using Intento.SDK.Autofac;
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
            var assembly = typeof(IntentoTests).Assembly;
            var assemblyVersion = IntentoHelpers.GetVersion(assembly);
            var options = new Options
            {
                ApiKey = Environment.GetEnvironmentVariable("IntentoAPIKey"),
                ClientUserAgent = $"Intento.SDK.Test/{assemblyVersion}",
                TmsServerUrl = "", //"https://connectors-stage.inten.to/tms",
                ServerUrl = "https://api2.inten.to",
                SyncwrapperUrl = "https://api2_syncwrapper.inten.to"
            };
            IntentoClient.Init(options);
        }
    }
}