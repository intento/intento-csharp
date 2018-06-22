using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Net.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace IntentoSDK
{
    public class IntentoAiTextTranslate
    {
        Intento intento;
        IntentoAiText parent;

        public IntentoAiTextTranslate(IntentoAiText parent)
        {
            this.parent = parent;
            this.intento = parent.Intento;
        }


        public Intento Intento
        { get { return intento; } }

        public IntentoAiText Parent
        { get { return parent; } }

        public dynamic Fulfill(string text, string to, string from = null, string provider = null, 
            List<Dictionary<string, object>> auth = null, bool async = false)
        {
            Task<dynamic> taskReadResult = Task.Run<dynamic>(async () => await this.FulfillAsync(text, to, from: from, 
                provider: provider, auth: auth, async: async));
            return taskReadResult.Result;
        }

        async public Task<dynamic> FulfillAsync(string text, string to, string from = null, string provider = null, 
            List<Dictionary<string, object>> auth = null, bool async = false)
        {
            // Create body for Intento API request
            string jsonData = JsonConvert.SerializeObject(new {
                context = new { text = text, from = from, to = to },
                service = new { async = async, provider = provider },
            });
            HttpContent requestBody = new StringContent(jsonData);

            // Open connection to Intento API and set ApiKey
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("apikey", intento.apiKey);

            // Call to Intento API and get json result
            HttpResponseMessage response = await client.PostAsync(intento.serverUrl + "ai/text/translate", requestBody);
            string stringRresult = await response.Content.ReadAsStringAsync();
            dynamic jsonResult = JObject.Parse(stringRresult);

            if (response.IsSuccessStatusCode)
                return jsonResult;

            Exception ex = IntentoException.Make(response, jsonResult);
            throw ex;
        }

        public dynamic CheckAsyncJob(string asyncId)
        {
            Task<dynamic> taskReadResult = Task.Run<dynamic>(async () => await this.CheckAsyncJobAsync(asyncId));
            return taskReadResult.Result;
        }

        async public Task<dynamic> CheckAsyncJobAsync(string asyncId)
        {
            // Open connection to Intento API and set ApiKey
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("apikey", intento.apiKey);

            // Call to Intento API and get json result
            HttpResponseMessage response = await client.GetAsync(string.Format("{0}operation/{1}", intento.serverUrl, asyncId));
            string stringRresult = await response.Content.ReadAsStringAsync();
            dynamic jsonResult = JObject.Parse(stringRresult);

            if (response.IsSuccessStatusCode)
                return jsonResult;

            Exception ex = IntentoException.Make(response, jsonResult);
            throw ex;
        }
    }
}
