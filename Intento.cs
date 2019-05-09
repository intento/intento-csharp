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
    /// Root class for all Intento services. 
    /// </summary>
    public class Intento
    {
        internal string apiKey;
        internal Dictionary<string, object> auth;
        internal string serverUrl;
        internal string otherUserAgent;
        internal string version;

        // Revision history 
        // 1.1.0: Public version
        // 1.1.1: 2019-01-06
        //   - Corrected behaviour for async-wait-async request with sandbox apikey or error in validation of prarmeters. 
        //     In this case no operation id returned from IntentoAPI, but result (in case of sandbox key) or error information. 
        // 1.1.2: 2019-02-02
        //   - More correct processing of text parameter of Fulfill operation
        // 1.1.3: 2019-03-31
        //   - Added methods for obtaining models and glossaries from the provider

        private Intento(string apiKey, Dictionary<string, object> auth=null, string path="https://api.inten.to/",
            string userAgent = null)
        {
            this.apiKey = apiKey;
            this.auth = auth != null ? new Dictionary<string, object>(auth) : null;
            this.serverUrl = path;
            otherUserAgent = userAgent;
            System.Net.ServicePointManager.SecurityProtocol |= SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
            var assembly = Assembly.GetExecutingAssembly();
            var fvi = FileVersionInfo.GetVersionInfo(assembly.Location);
            this.version = string.Format("{0}.{1}.{2}", fvi.FileMajorPart, fvi.FileMinorPart, fvi.FileBuildPart);
        }

        public static Intento Create(string intentoKey, Dictionary<string, object> auth=null, string path = "https://api.inten.to/",
            string userAgent = null)
        {
            Intento intento = new Intento(intentoKey, auth:auth, path: path, userAgent: userAgent);
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
            HttpConnector client = new HttpConnector(this);

            // async operations inside
            dynamic result = await client.GetAsync(string.Format("operations/{0}", asyncId));
            return result;
        }

        public dynamic WaitAsyncJob(string asyncId, int delay = 0)
        {
            Task<dynamic> taskResult = Task.Run<dynamic>(async () => await this.WaitAsyncJobAsync(asyncId, delay: delay));
            return taskResult.Result;
        }

        async public Task<dynamic> WaitAsyncJobAsync(string asyncId, int delay = 0)
        {
            if (delay == 0)
                delay = 3000;

            while (true)
            {
                dynamic result = await CheckAsyncJobAsync(asyncId);
                if (((bool)result.done))
                    return result;

                Thread.Sleep(delay);
            }
        }

    }
}
