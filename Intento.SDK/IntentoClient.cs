using Intento.SDK.DI;
using Intento.SDK.Settings;

namespace Intento.SDK
{
    /// <summary>
    /// Initializer for Intento.SDK
    /// </summary>
    public static class IntentoClient
    {
        /// <summary>
        /// Prepare Locator and services
        /// </summary>
        /// <param name="options"></param>
        /// <param name="ipml"></param>
        public static void Init(Options options, ILocatorImpl ipml = null)
        {
            ipml ??= new DefaultLocatorImpl();
            Locator.SetImpl(ipml, options);
        }
    }
}