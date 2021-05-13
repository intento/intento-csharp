using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Intento.SDK.DI;
using Microsoft.Extensions.Logging;

namespace Intento.SDK
{
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
