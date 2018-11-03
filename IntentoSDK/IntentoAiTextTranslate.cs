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
using System.Threading;

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
            bool async = false, bool wait_async = false, string format = null, object auth = null, string custom_model = null,
            object pre_processing = null, object post_processing = null,
            bool failover = false, object failover_list = null, string bidding = null)
        {
            Task<dynamic> taskReadResult = Task.Run<dynamic>(async () => await this.FulfillAsync(text, to, from: from, provider: provider,
                async: async, wait_async: wait_async, format: format, auth: auth, custom_model: custom_model,
                pre_processing: pre_processing, post_processing: post_processing,
                failover: failover, failover_list: failover_list, bidding: bidding));
            return taskReadResult.Result;
        }

        async public Task<dynamic> FulfillAsync(object text, string to, string from = null, string provider = null,
            bool async = false, bool wait_async = false, string format = null, object auth = null, string custom_model = null,
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
            service.provider = provider == "" ? null : provider;

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
                        throw new IntentoInvalidParameterException("failover_list", "need to json-list-string or Newtonsoft JArray");
                    service.failover_list = failover_list;
                }
                if (!string.IsNullOrEmpty(bidding))
                    service.bidding = bidding;
            }
            json.service = service;

            // Call to Intento API and get json result
            HttpConnector conn = new HttpConnector(Intento);
            dynamic jsonResult = await conn.PostAsync("ai/text/translate", json);

            if (async && wait_async)
            {   // async opertation (in terms of IntentoApi) and we need to wait result of it
                string id = jsonResult.id;
                jsonResult = await Intento.WaitAsyncJobAsync(id);
            }

            return jsonResult;
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
            throw new IntentoInvalidParameterException(name, "need to be null or string or json-string or Newtonsoft JObject/JArray");
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
        async public Task<dynamic> ProviderAsync(string providerId)
        {
            string path = "ai/text/translate/" + providerId;

            // Call to Intento API and get json result
            HttpConnector conn = new HttpConnector(Intento);
            dynamic jsonResult = await conn.GetAsync(path);

            return jsonResult;
        }

        public IList<dynamic> Providers(string to = null, string from = null, bool lang_detect = false, bool bulk = false,
            Dictionary<string, string> filter = null)
        {
            Task<IList<dynamic>> taskReadResult = Task.Run<IList<dynamic>>(async () => 
                await this.ProvidersAsync(to: to, from: from, lang_detect: lang_detect, bulk: bulk, filter: filter));
            return taskReadResult.Result;
        }

        async public Task<IList<dynamic>> ProvidersAsync(string to = null, string from = null, bool lang_detect = false, bool bulk = false,
            Dictionary<string, string> filter = null)
        {
            Dictionary<string, string> f = new Dictionary<string, string>(filter);
            if (!string.IsNullOrEmpty(to))
                f["to"] = to;
            if (!string.IsNullOrEmpty(from))
                f["from"] = from;
            if (lang_detect)
                f["lang_detect"] = "true";
            if (bulk)
                f["bulk"] = "true";

            List<string> p = new List<string>();
            foreach(KeyValuePair<string, string> pair in f)
                p.Add(string.Format("{0}={1}", pair.Key, System.Web.HttpUtility.UrlEncode(pair.Value)));
            string url = "ai/text/translate";
            if (p.Count != 0)
                url += "?" + string.Join("&", p);

            // Call to Intento API and get json result
            HttpConnector conn = new HttpConnector(Intento);
            dynamic jsonResult = await conn.GetAsync(url);

            List<dynamic> providers = new List<dynamic>();

            foreach (dynamic providerInfo in jsonResult)
                providers.Add(providerInfo);

            return providers;
        }

        public IList<dynamic> Languages()
        {
            Task<IList<dynamic>> taskReadResult = Task.Run<IList<dynamic>>(async () => await this.LanguagesAsync());
            return taskReadResult.Result;
        }

        async public Task<IList<dynamic>> LanguagesAsync()
        {
            string url = "ai/text/translate/languages";

            // Call to Intento API and get json result
            HttpConnector conn = new HttpConnector(Intento);
            dynamic jsonResult = await conn.GetAsync(url);

            List<dynamic> languages = new List<dynamic>();
            foreach (dynamic languageInfo in jsonResult)
                languages.Add(languageInfo);

            return languages;
        }

        public IList<IList<string>> ProviderLanguagePairs(string providerId)
        {
            Task<IList<IList<string>>> taskReadResult = Task.Run<IList<IList<string>>>(async () => await this.ProviderLanguagePairsAsync(providerId));
            return taskReadResult.Result;
        }

        async public Task<IList<IList<string>>> ProviderLanguagePairsAsync(string providerId)
        {
            dynamic providerInfo = await ProviderAsync(providerId);

            List<IList<string>> res = new List<IList<string>>();
            dynamic languages = providerInfo.languages;

            dynamic symmetric = languages.symmetric;
            if (symmetric != null)
            {
                foreach (string l1 in symmetric)
                    foreach (string l2 in symmetric)
                        if (l1 != l2)
                            res.Add(new List<string> { l1, l2 });
            }

            dynamic pairs = languages.pairs;
            if (pairs != null)
            {
                foreach (dynamic pair in pairs)
                    res.Add(new List<string> { pair.from, pair.to });
            }

            return res;
        }
    }
}
