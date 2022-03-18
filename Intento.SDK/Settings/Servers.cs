using System.IO;
using Newtonsoft.Json;

namespace Intento.SDK.Settings
{
    /// <summary>
    /// Urls settings
    /// </summary>
    public class Servers
    {
        /// <summary>
        /// Url for main server for API
        /// </summary>
        [JsonProperty("ServerUrl")]
        public string ServerUrl { get; set; }

        /// <summary>
        /// Url of tms-backend
        /// </summary>
        [JsonProperty("TmsServerUrl")]
        public string TmsServerUrl { get; set; }

        /// <summary>
        /// Url for sync wrapper (for requests with options SyncWrapper = true)
        /// </summary>
        [JsonProperty("SyncwrapperUrl")]
        public string SyncwrapperUrl { get; set; }

        private static Servers LoadServers()
        {
            using var stream = typeof(Servers).Assembly.GetManifestResourceStream("Intento.SDK.Servers.json");
            if (stream != null)
            {
                using var reader = new StreamReader(stream);
                var content = reader.ReadToEnd();
                return JsonConvert.DeserializeObject<Servers>(content);
            }

            return null;
        }

        /// <summary>
        /// Servers config
        /// </summary>
        public static Servers Config { get; } = LoadServers();
    }
}