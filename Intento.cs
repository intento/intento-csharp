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


// Revision history 
// 1.1.0: Public version
// 1.1.1: 2019-01-06
//   - Corrected behaviour for async-wait-async request with sandbox apikey or error in validation of prarmeters. 
//     In this case no operation id returned from IntentoAPI, but result (in case of sandbox key) or error information. 
// 1.1.2: 2019-02-02
//   - More correct processing of text parameter of Fulfill operation
// 1.1.3: 2019-03-31
//   - Added methods for obtaining models and glossaries from the provider
// 1.2.1: 2019-05-28
//   - Smart delays in wait-async mode
// 1.2.2: 2019-06-08
//   - Logging callback.
//   - Call logging callback for successfull and non-susscessfull Intento API calls. 
//   - try-catch around calls to http to write log. 
// 1.2.3: 2019-06-25
// - Bug with extracting version from dll        
// 1.2.4: 2019-07-02
// - waitAsyncDelay
// 1.2.5: 2019-07-03
// - The version in useragent now has a commit hash in git
// - Auxiliary public methods for extracting information from the assembly
// 1.2.6: 2019-07-14
// - Minor changes in processing of Modes and Glossaries operations. 
// 1.2.7: 2019-08-14
// - Change version of Newtonsoft dll to be compatible with other products (downgrade to 10.0.3)


namespace IntentoSDK
{
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
            int waitAsyncDelay=0)
        {
            this.apiKey = apiKey;
            this.auth = auth != null ? new Dictionary<string, object>(auth) : null;
            this.serverUrl = string.IsNullOrEmpty(path) ? "https://api.inten.to/" : path;
            otherUserAgent = userAgent;
            System.Net.ServicePointManager.SecurityProtocol |= SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
            this.waitAsyncDelay = waitAsyncDelay;

            var assembly = Assembly.GetExecutingAssembly();
            version = string.Format("{0}-{1}", IntentoHelpers.GetVersion(assembly), IntentoHelpers.GetGitCommitHash(assembly));

            
this.loggingCallback = loggingCallback;
            if (loggingCallback != null)
                loggingCallback("IntentoSDK: Intento.ctor", null, null);
        }

        public static Intento Create(
            string intentoKey, 
            Dictionary<string, object> auth=null, 
            string path = "https://api.inten.to/",
            string userAgent = null,
            Action<string, string, Exception> loggingCallback = null)
        {
            Intento intento = new Intento(intentoKey, auth:auth, path: path, userAgent: userAgent, loggingCallback: loggingCallback);
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
            HttpConnector client = new HttpConnector(this);
            Log(string.Format("CheckAsyncJobAsync-2: {0}ms", asyncId));

            // async operations inside
            Log(string.Format("CheckAsyncJobAsync-3: {0}ms", asyncId));
            dynamic result = await client.GetAsync(string.Format("operations/{0}", asyncId));
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

            if (delay == 0)
                delays = CalcDelays(400);
            else
                delays = CalcDelays(delay);

            delay = delays[0];
            while (DateTime.Now < dt.AddSeconds(delay))
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
            }

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
