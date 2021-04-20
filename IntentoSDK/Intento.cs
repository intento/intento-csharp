using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net;
using System.Reflection;
using System.Diagnostics;

namespace IntentoSDK
{
    /// <summary>
    /// Proxy server configuration class for service requests
    /// </summary>
    public class ProxySettings
    {
        private string proxyUserName;
        private string proxyPassword;
        private string proxyAddress;
        private string proxyPort;


        public bool ProxyEnabled { get; set; }
        /// <summary>
        /// Proxy address
        /// </summary>
        public string ProxyAddress { get => proxyAddress; set => proxyAddress = value; }
        /// <summary>
        /// Proxy port number.
        /// </summary>
        public string ProxyPort { get => proxyPort; set => proxyPort = value; }
        public Uri ProxyUri {
            get
            {
                if (string.IsNullOrWhiteSpace(proxyAddress) || string.IsNullOrWhiteSpace(proxyPort))
                {
                    return null;
                }
                return new Uri($"http://{proxyAddress}:{proxyPort}");
            }
        }
        public string ProxyUserName { get => proxyUserName; set => proxyUserName = value; }
        public string ProxyPassword { get => proxyPassword; set => proxyPassword = value; }
    }


    /// <summary>
    /// Root class for all Intento services. 
    /// </summary>
    public class Intento
    {
        internal string apiKey;
        internal Dictionary<string, object> auth;
        internal string serverUrl;
        internal string otherUserAgent;
        internal string version;
        internal Action<string, string, Exception> loggingCallback;
        internal int waitAsyncDelay = 0;
        internal ProxySettings proxy;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="apiKey"></param>
        /// <param name="auth"></param>
        /// <param name="path"></param>
        /// <param name="userAgent"></param>
        /// <param name="loggingCallback"></param>
        /// <param name="asyncDelay">Delay for waitAsync operations in sec. Default: 30 sec</param>
        private Intento(
            string apiKey, 
            Dictionary<string, object> auth=null, 
            string path=null,
            string userAgent = null,
            Action<string, string, Exception> loggingCallback = null,
            int waitAsyncDelay=0,ProxySettings proxySet = null)
        {
            this.apiKey = apiKey;
            this.auth = auth != null ? new Dictionary<string, object>(auth) : null;
            this.serverUrl = string.IsNullOrEmpty(path) ? "https://api.inten.to/" : path;
            otherUserAgent = userAgent;
            System.Net.ServicePointManager.SecurityProtocol |= SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
            this.waitAsyncDelay = waitAsyncDelay;

            var assembly = Assembly.GetExecutingAssembly();
            var fvi = assembly.GetName().Version;
            this.version = string.Format("{0}.{1}.{2}.{3}", fvi.Major, fvi.Minor, fvi.Build, IntentoHelpers.GetGitCommitHash(assembly));
            
            this.loggingCallback = loggingCallback;
            if (loggingCallback != null)
                loggingCallback("IntentoSDK: Intento.ctor", null, null);
            this.proxy = proxySet;
        }

        public static Intento Create(
            string intentoKey, 
            Dictionary<string, object> auth=null, 
            string path = "https://api.inten.to/",
            string userAgent = null,
            Action<string, string, Exception> loggingCallback = null, 
            ProxySettings proxySet = null)
            
        {
            Intento intento = new Intento(intentoKey, auth:auth, path: path, userAgent: userAgent, loggingCallback: loggingCallback, proxySet: proxySet);
            return intento;
        }

        public IntentoAi Ai
        { get { return new IntentoAi(this); } }

        public dynamic CheckAsyncJob(string asyncId)
        {
            Task<dynamic> task = Task.Run<dynamic>(async () => await this.CheckAsyncJobAsync(asyncId));
            return task.Result;
        }

        async public Task<dynamic> CheckAsyncJobAsync(string asyncId)
        {
            // Open connection to Intento API and set ApiKey
            Log(string.Format("CheckAsyncJobAsync-1: {0}ms", asyncId));
            dynamic result;
            using (HttpConnector client = new HttpConnector(this))
            {
                Log(string.Format("CheckAsyncJobAsync-2: {0}ms", asyncId));

                // async operations inside
                Log(string.Format("CheckAsyncJobAsync-3: {0}ms", asyncId));
                result = await client.GetAsync(string.Format("operations/{0}", asyncId));
            }

            Log(string.Format("CheckAsyncJobAsync-4: {0}ms", asyncId));
            return result;
        }

        public dynamic WaitAsyncJob(string asyncId, int delay = 0)
        {
            Task<dynamic> taskResult = Task.Run<dynamic>(async () => await this.WaitAsyncJobAsync(asyncId, delay: delay == 0 ? waitAsyncDelay : delay));
            return taskResult.Result;
        }

        private List<int> CalcDelays(int delay)
        {
            List<int> delays = new List<int>();
            do
            {
                delays.Add(delay);
                delay += 100;
            } while (delay < 3000);
            return delays;
        }

        public void Log(string subject, string comment=null, Exception ex=null)
        {
            if (loggingCallback == null)
                return;
            loggingCallback(subject, comment, ex);
        }

        async public Task<dynamic> WaitAsyncJobAsync(string asyncId, int delay = 0)
        {
            DateTime dt = DateTime.Now;

            Log(string.Format("WaitAsyncJobAsync-start: {0} - {1}ms", asyncId, delay));
            List<int> delays;
            int n = 0;

            if (delay == -1)
                delays = new List<int> { 0 };
            else if (delay == 0)
                delays = CalcDelays(400);
            else
                delays = CalcDelays(delay);

            delay = delays[0];
            do
            {
                Log(string.Format("WaitAsyncJobAsync-loop: {0} - {1}ms", asyncId, delay));
                Thread.Sleep(delay);
                Log(string.Format("WaitAsyncJobAsync-loop after sleep: {0} - {1}ms", asyncId, delay));

                dynamic result = await CheckAsyncJobAsync(asyncId);

                Log(string.Format("WaitAsyncJobAsync-loop1a: {0} - {1}ms", asyncId, delay));
                if (((bool)result.done))
                    return result;
                n++;
                if (n < delays.Count)
                    delay = delays[n];
                Log(string.Format("WaitAsyncJobAsync-loop2: {0} - {1}ms", asyncId, delay));
            } while (DateTime.Now < dt.AddSeconds(delay));

            // Timeout
            dynamic json = new JObject();
            json.id = asyncId;
            json.done = false;
            json.response = null;

            dynamic error = new JObject();
            error.type = "Timeout";
            error.reason = "Too long response from Intento MT plugin";
            error.data = null;
            json.error = error;

            return json;
        }

    }

    [AttributeUsage(AttributeTargets.Assembly)]
    public class AssemblyGitHash : Attribute
    {
        public string hash;
        public AssemblyGitHash() : this(string.Empty) { }
        public AssemblyGitHash(string txt) { hash = txt; }
        public string Hash() { return hash; }
    }

}
