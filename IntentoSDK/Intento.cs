using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace IntentoSDK
{
    /// <summary>
    /// Root class for all Intento services. 
    /// </summary>
    public class Intento
    {
        public string apiKey;
        public Dictionary<string, object> auth;
        public string serverUrl;

        private Intento(string apiKey, Dictionary<string, object> auth=null, string path="https://api.inten.to/")
        {
            this.apiKey = apiKey;
            this.auth = auth != null ? new Dictionary<string, object>(auth) : null;
            this.serverUrl = path;
        }

        public static Intento Create(string intentoKey, Dictionary<string, object> auth=null, string path = "https://api.inten.to/")
        {
            Intento intento = new Intento(intentoKey, auth:auth, path: path);
            return intento;
        }

        public IntentoAi Ai
        { get { return new IntentoAi(this); } }

        public dynamic CheckAsyncJob(string asyncId)
        {
            Task<dynamic> taskReadResult = Task.Run<dynamic>(async () => await this.CheckAsyncJobAsync(asyncId));
            return taskReadResult.Result;
        }

        async public Task<dynamic> CheckAsyncJobAsync(string asyncId)
        {
            // Open connection to Intento API and set ApiKey
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("apikey", apiKey);

            // Call to Intento API and get json result
            HttpResponseMessage response = await client.GetAsync(string.Format("{0}operations/{1}", serverUrl, asyncId));
            string stringRresult = await response.Content.ReadAsStringAsync();
            dynamic jsonResult = JObject.Parse(stringRresult);

            if (response.IsSuccessStatusCode)
                return jsonResult;

            Exception ex = IntentoException.Make(response, jsonResult);
            throw ex;
        }

    }
}
