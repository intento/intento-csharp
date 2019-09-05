using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace IntentoSDK
{
    public static class IntentoHelpers
    {
        public static string GetVersion(Assembly currentAssem)
        {
            string version = "unknown";
            try
            {
                var fvi = currentAssem.GetName().Version;
                version = string.Format("{0}.{1}.{2}", fvi.Major, fvi.Minor, fvi.Build);
            }
            catch { }

            return version;
        }
        public static string GetGitCommitHash(Assembly currentAssem)
        {
            string hash = "unknown";
            try
            {
                var attr = currentAssem.GetCustomAttributes(typeof(AssemblyGitHash), false).Cast<AssemblyGitHash>().FirstOrDefault();
                if (attr != null)
                    hash = attr.Hash().Substring(0, 7);
            }
            catch { }

            return hash;
        }
    }
}
