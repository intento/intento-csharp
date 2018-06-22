using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntentoSDK
{
    /// <summary>
    /// Root class for all Intento services. 
    /// </summary>
    public class Intento
    {
        public string apiKey;
        public Dictionary<string, object> auth;
        public bool isProd = false;
        public string serverUrl;

        private Intento(string apiKey, Dictionary<string, object> auth=null, bool isProd=false)
        {
            this.apiKey = apiKey;
            this.auth = auth != null ? new Dictionary<string, object>(auth) : null;
            this.isProd = isProd;
            this.serverUrl = "https://api.inten.to/";
        }

        public static Intento Create(string intentoKey, Dictionary<string, object> auth=null, bool isProd = false)
        {
            Intento intento = new Intento(intentoKey, auth:auth, isProd:isProd);
            return intento;
        }

        public IntentoAi Ai
        { get { return new IntentoAi(this); } }

    }
}
