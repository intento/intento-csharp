using Intento.SDK.Settings;

namespace Intento.SDK.DI
{
    /// <summary>
    /// Container for all services
    /// </summary>
    public static class Locator
    {
        private static ILocatorImpl _locatorImpl;

        /// <summary>
        /// Set implementation for locator
        /// </summary>
        /// <param name="impl"></param>
        /// <param name="options"></param>
        public static void SetImpl(ILocatorImpl impl, Options options)
        {
            _locatorImpl = impl;
            impl?.Init(options);
        }

        /// <summary>
        /// Resolve service from locator
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T Resolve<T>()
        {
            return _locatorImpl.Resolve<T>();
        }
    }
}
