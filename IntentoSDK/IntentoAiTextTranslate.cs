using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Web;
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

        public dynamic Fulfill(object text, string to, string from = null, string provider = null, 
            bool async = false, string format = null, object auth = null, string custom_model = null, 
            object pre_processing = null, object post_processing = null,
            bool failover=false, object failover_list=null, string bidding=null)
        {
            Task<dynamic> taskReadResult = Task.Run<dynamic>(async () => await this.FulfillAsync(text, to, from: from, provider: provider, 
                async: async, format: format, auth: auth, custom_model: custom_model, pre_processing: pre_processing, post_processing: post_processing,
                failover: failover, failover_list: failover_list, bidding: bidding));
            return taskReadResult.Result;
        }

        async public Task<dynamic> FulfillAsync(object text, string to, string from = null, string provider = null, 
            bool async = false, string format = null, object auth = null, string custom_model = null,
            object pre_processing = null, object post_processing = null,
            bool failover = false, object failover_list = null, string bidding = null)
        {
            if (string.IsNullOrEmpty(from))
                from = null;

            dynamic preProcessingJson = GetJson(pre_processing, "pre_processing");
            dynamic postProcessingJson = GetJson(post_processing, "post_processing");

            dynamic json = new JObject();

            // context section
            dynamic context = new JObject();

            // text
            context.text = GetJson(text, "text");

            // to
            context.to = to;

            // from
            if (!string.IsNullOrEmpty(from))
                context.from = from;

            // format
            if (!string.IsNullOrEmpty(format))
                context.format = format;

            // custom_model
            if (!string.IsNullOrEmpty(custom_model))
                context.category = custom_model;

            json.context = context;

            // service section
            dynamic service = new JObject();
            service.provider = provider;

            // async parameter
            if (async)
                service.async = true;

            // auth parameter
            service.auth = GetJson(auth, "auth");

            // pre-post processing paramters
            if (preProcessingJson != null || postProcessingJson != null)
            {
                dynamic processing = new JObject();
                if (preProcessingJson != null)
                    processing.pre = preProcessingJson;
                if (postProcessingJson != null)
                    processing.post = postProcessingJson;
                service.processing = processing;
            }

            // failover parameters
            if (failover)
            {
                service.failover = true;

                JArray failoverJson;
                if (failover_list != null)
                {
                    if (failover_list is string)
                        failoverJson = JArray.Parse((string)failover_list);
                    else if (failover_list is JArray)
                        failoverJson = (JArray)failover_list;
                    else
                        throw new Exception("Invalid failover_list parameter: need to json-list-string or Newtonsoft JArray");
                    service.failover_list = failover_list;
                }
                if (!string.IsNullOrEmpty(bidding))
                    service.bidding = bidding;
            }

            json.service = service;

            string jsonData = JsonConvert.SerializeObject(json);
            HttpContent requestBody = new StringContent(jsonData);

            // Open connection to Intento API and set ApiKey
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("apikey", intento.apiKey);

            // Call to Intento API and get json result
            HttpResponseMessage response = await client.PostAsync(intento.serverUrl + "ai/text/translate", requestBody);
            string stringResult = await response.Content.ReadAsStringAsync();
            dynamic jsonResult = JObject.Parse(stringResult);

            if (response.IsSuccessStatusCode)
                return jsonResult;

            Exception ex = IntentoException.Make(response, jsonResult);
            throw ex;
        }

        private dynamic GetJson(object data, string name)
        {
            if (data == null)
                return null;
            else if (data is string)
            {
                string z = ((string)data).Trim();
                if (string.IsNullOrEmpty(z))
                    return null;
                if (z[0] == '[')
                    return JArray.Parse(z);
                if (z[0] == '{')
                    return JObject.Parse(z);
                return (string)data;
            }
            else if (data is JObject)
                return (JObject)data;
            else if (data is JArray)
                return (JArray)data;
            else if (data is IDictionary<string, string>)
                return (JObject.FromObject((Dictionary<string, string>)data));
            else if (data is IEnumerable<string>)
                return (JArray.FromObject((IEnumerable<string>)data));
            throw new Exception(string.Format("Invalid {0} parameter: need to be null or string or json-string or Newtonsoft JObject/JArray", name));
        }

        /// <summary>
        /// Detailed information on provider features
        /// </summary>
        /// <param name="provider">Provider id</param>
        /// <returns>dynamic (json) with requested information</returns>
        public dynamic Provider(string provider)
        {
            Task<dynamic> taskReadResult = Task.Run<dynamic>(async () => await this.ProviderAsync(provider));
            return taskReadResult.Result;
        }

        /// <summary>
        /// Detailed information on provider features
        /// </summary>
        /// <param name="provider">Provider id</param>
        /// <returns>dynamic (json) with requested information</returns>
        async public Task<dynamic> ProviderAsync(string provider)
        {
            string url = intento.serverUrl + "ai/text/translate/" + provider;

            // Call to Intento API and get json result
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("apikey", intento.apiKey);
            HttpResponseMessage response = await client.GetAsync(url);

            string stringResult = await response.Content.ReadAsStringAsync();
            dynamic jsonResult = JObject.Parse(stringResult);

            if (response.IsSuccessStatusCode)
                return jsonResult;

            Exception ex = IntentoException.Make(response, jsonResult);
            throw ex;
        }

        public IList<dynamic> Providers(string to = null, string from = null)
        {
            Task<IList<dynamic>> taskReadResult = Task.Run<IList<dynamic>>(async () => await this.ProvidersAsync(to: to, from: from));
            return taskReadResult.Result;
        }

        async public Task<IList<dynamic>> ProvidersAsync(string to = null, string from = null, bool lang_detect = false, bool bulk = false)
        {
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("apikey", intento.apiKey);

            List<string> p = new List<string>();
            if (!string.IsNullOrEmpty(to))
                p.Add(string.Format("to={0}", System.Web.HttpUtility.UrlEncode(to)));
            if (!string.IsNullOrEmpty(to))
                p.Add(string.Format("from={0}", System.Web.HttpUtility.UrlEncode(from)));
            if (lang_detect)
                p.Add("lang_detect=true");
            if (bulk)
                p.Add("bulk=true");

            string url = intento.serverUrl + "ai/text/translate";
            if (p.Count != 0)
                url += "?" + string.Join("&", p);

            // Call to Intento API and get json result
            HttpResponseMessage response = await client.GetAsync(url);

            List<dynamic> providers = new List<dynamic>();
            string stringResult = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                JArray jsonResult = JArray.Parse(stringResult);

                foreach (dynamic providerInfo in jsonResult)
                    providers.Add(providerInfo);

                return providers;
            }

            dynamic jsonErrorResult = JObject.Parse(stringResult);
            Exception ex = IntentoException.Make(response, jsonErrorResult);
            throw ex;
        }

        public IList<dynamic> Languages()
        {
            Task<IList<dynamic>> taskReadResult = Task.Run<IList<dynamic>>(async () => await this.LanguagesAsync());
            return taskReadResult.Result;
        }

        async public Task<IList<dynamic>> LanguagesAsync()
        {
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("apikey", intento.apiKey);

            string url = intento.serverUrl + "ai/text/translate/languages";

            // Call to Intento API and get json result
            HttpResponseMessage response = await client.GetAsync(url);

            List<dynamic> languages = new List<dynamic>();
            string stringResult = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                JArray jsonResult = JArray.Parse(stringResult);

                foreach (dynamic languageInfo in jsonResult)
                    languages.Add(languageInfo);

                return languages;
            }

            dynamic jsonErrorResult = JObject.Parse(stringResult);
            Exception ex = IntentoException.Make(response, jsonErrorResult);
            throw ex;
        }
    }
}
