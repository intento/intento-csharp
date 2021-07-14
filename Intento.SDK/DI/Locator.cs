using Intento.SDK.Settings;

namespace Intento.SDK.DI
{
    /// <summary>
    /// Container for all services
    /// </summary>
    public static class Locator
    {
        private static ILocatorImpl locatorImpl;

        /// <summary>
        /// Set implementation for locator
        /// </summary>
        /// <param name="impl"></param>
        /// <param name="options"></param>
        public static void SetImpl(ILocatorImpl impl, Options options)
        {
            locatorImpl = impl;
            impl?.Init(options);
        }

        /// <summary>
        /// Resolve service from locator
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T Resolve<T>()
        {
            return locatorImpl.Resolve<T>();
        }
    }
}
