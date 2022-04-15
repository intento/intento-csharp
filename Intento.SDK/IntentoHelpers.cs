using System.Reflection;

namespace Intento.SDK
{
    /// <summary>
    /// Helpers for work with Intento SDK
    /// </summary>
    public class IntentoHelpers
    {
        /// <summary>
        ///  Get versions of assembly
        /// </summary>
        /// <param name="currentAssembly"></param>
        /// <returns></returns>
        public static string GetVersion(Assembly currentAssembly = null)
        {
            currentAssembly ??= typeof(IntentoHelpers).Assembly;
            var fvi = currentAssembly.GetName()?.Version;
            return fvi != null ? $"{fvi.Major}.{fvi.Minor}.{fvi.Build}" : "unknown";
        }

    }
}
