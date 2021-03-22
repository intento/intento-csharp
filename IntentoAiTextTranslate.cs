using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
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
            bool async = false, bool wait_async = false, string format = null, object auth = null,
            string custom_model = null, string glossary = null,
            object pre_processing = null, object post_processing = null,
            bool failover = false, object failover_list = null, string routing = null, bool trace = false,
            Dictionary<string, string> special_headers = null)
        {
            Task<dynamic> taskReadResult = Task.Run<dynamic>(async () => await this.FulfillAsync(text, to, from: from, provider: provider,
                async: async, wait_async: wait_async, format: format, auth: auth,
                custom_model: custom_model, glossary: glossary,
                pre_processing: pre_processing, post_processing: post_processing,
                failover: failover, failover_list: failover_list, routing: routing, trace: trace, special_headers: special_headers));
            return taskReadResult.Result;
        }

        async public Task<dynamic> FulfillAsync(object text, string to, string from = null, string provider = null,
            bool async = false, bool wait_async = false, string format = null, object auth = null,
            string custom_model = null, string glossary = null,
            object pre_processing = null, object post_processing = null,
            bool failover = false, object failover_list = null, string routing = null, bool trace = false,
            Dictionary<string, string> special_headers = null)
        {
            dynamic preProcessingJson = GetJson(pre_processing, "pre_processing");
            dynamic postProcessingJson = GetJson(post_processing, "post_processing");

            dynamic json = new JObject();

            // ------ context section
            dynamic context = new JObject();

            // text
            if (text == null)
                context.text = "";
            else if (text is IEnumerable<string>)
                context.text = GetJson(((IEnumerable<string>)text).Select(i => i == null ? "" : i), "text");
            else
                context.text = GetJson(text.ToString(), "text") ?? "";

            // to
            context.to = to;

            // from
            if (!string.IsNullOrWhiteSpace(from))
                context.from = from;

            // format
            if (!string.IsNullOrWhiteSpace(format))
                context.format = format;

            // custom_model
            if (!string.IsNullOrWhiteSpace(custom_model))
                context.category = custom_model;

            // glossary
            if (!string.IsNullOrWhiteSpace(glossary))
                context.glossary = glossary;

            json.context = context;

            // ----- service section
            dynamic service = new JObject();

            // provider
            if (!string.IsNullOrWhiteSpace(provider))
                service.provider = provider;

            // async parameter
            if (async)
                service.async = true;

            // auth parameter
            service.auth = GetJson(auth, "auth");

            // routing
            if (!string.IsNullOrEmpty(routing))
                service.routing = routing;

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
            }

            json.service = service;

            dynamic jsonResult;
            string url = "ai/text/translate";
            if (trace)
                url += "?trace=true";

            // Call to Intento API and get json result
            using (HttpConnector conn = new HttpConnector(Intento))
            {
                jsonResult = await conn.PostAsync(url, json);
            }

            if (async && wait_async)
            {   // async opertation (in terms of IntentoApi) and we need to wait result of it
                string id = jsonResult.id;

                // In case of Sandbox key and some errors in parameters request to IntentoAPI may return:
                // 1. id: async operation started
                // 2. result of translation: Sandcox key
                // 3. error: validation of arameters failed (like request to translate html with provider with no such capabilities)
                if (id == null)
                    // Not a (1) - return result immediately, nothing to wait
                    return jsonResult;

                jsonResult = await Intento.WaitAsyncJobAsync(id);
            }

            return jsonResult;
        }

        private dynamic GetJson(object data, string name)
        {
            // Convert data to json. 
            // String is deserialized in case it has a for of json list or dict. 
            // data=="" is trated as null

            if (data == null)
                return null;
            else if (data is string)
            {
                string z = ((string)data).Trim();
                if (string.IsNullOrEmpty(z))
                    return null;
                try
                {
                    if (z[0] == '[')
                        return JArray.Parse(z);
                    if (z[0] == '{')
                        return JObject.Parse(z);
                }
                catch { }

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
        /// Details of the models stored by the provider
        /// </summary>
        /// <param name="provider">Provider id</param>
        /// <param name="credential_id">Credential id</param>
        /// <param name="additionalParams">additional url params</param>
        /// <returns>dynamic (json) with requested information</returns>
        public IList<dynamic> Models(string provider, Dictionary<string, string> credentials, Dictionary<string, string> additionalParams = null)
        {
            Task<dynamic> taskReadResult = Task.Run<dynamic>(async () =>
                await this.ModelsAsync(provider, credentials, additionalParams: additionalParams));
            return taskReadResult.Result;
        }

        /// <summary>
        /// Details of the models stored by the provider
        /// </summary>
        /// <param name="provider">Provider id</param>
        /// <param name="credential_id">Credential id</param>
        /// <param name="additionalParams">additional url params</param>
        /// <returns>dynamic (json) with requested information</returns>
        async public Task<IList<dynamic>> ModelsAsync(string providerId, Dictionary<string, string> credentials, Dictionary<string, string> additionalParams = null)
        {
            string path = string.Format("ai/text/translate/models?provider={0}", providerId);
            if (credentials != null)
            {
                string json;
                if (credentials.Count != 0)
                {
                    if (credentials.Count == 1 && credentials.ContainsKey("credential_id"))
                        json = credentials["credential_id"];
                    else
                    {
                        json = JsonConvert.SerializeObject(credentials, Formatting.None);
                        json = HttpUtility.UrlEncode(json);
                    }
                    path += String.Format("&credential_id={0}", json);
                }
            }

            dynamic jsonResult;
            // Call to Intento API and get json result
            using (HttpConnector conn = new HttpConnector(Intento))
                jsonResult = await conn.GetAsync(path, additionalParams: additionalParams);

            List<dynamic> models = new List<dynamic>();

            foreach (dynamic model in ((JContainer)jsonResult).First.First)
                models.Add(model);

            return models;
        }

        /// <summary>
        /// Details of the models stored by the provider
        /// </summary>
        /// <param name="providerId">Provider id</param>
        /// <param name="additionalParams">additional url params</param>
        /// <returns>dynamic (json) with requested information</returns>
        public IList<dynamic> Accounts(string providerId = null, Dictionary<string, string> additionalParams = null)
        {
            Task<dynamic> taskReadResult = Task.Run<dynamic>(async () =>
                await this.AccountsAsync(providerId, additionalParams: additionalParams));
            return taskReadResult.Result;
        }

        /// <summary>
        /// Details of the models stored by the provider
        /// </summary>
        /// <param name="provider">Provider id</param>
        /// <param name="additionalParams">additional url params</param>
        /// <returns>dynamic (json) with requested information</returns>
        async public Task<IList<dynamic>> AccountsAsync(string providerId = null, Dictionary<string, string> additionalParams = null)
        {
            string path = string.Format("accounts" + (providerId != null ? $"?provider={providerId}" : null));

            dynamic jsonResult;
            // Call to Intento API and get json result
            using (HttpConnector conn = new HttpConnector(Intento))
                jsonResult = await conn.GetAsync(path, additionalParams: additionalParams);

            List<dynamic> models = new List<dynamic>();

            foreach (dynamic model in ((JContainer)jsonResult).First.First)
                models.Add(model);

            return models;
        }

        /// <summary>
        /// Details of the models stored by the provider
        /// </summary>
        /// <param name="providerId">Provider id</param>
        /// <param name="additionalParams">additional url params</param>
        /// <returns>dynamic (json) with requested information</returns>
        public IList<dynamic> Routing(Dictionary<string, string> additionalParams = null)
        {
            Task<dynamic> taskReadResult = Task.Run<dynamic>(async () =>
                await this.RoutingAsync(additionalParams: additionalParams));
            return taskReadResult.Result;
        }

        /// <summary>
        /// Details of the models stored by the provider
        /// </summary>
        /// <param name="provider">Provider id</param>
        /// <param name="additionalParams">additional url params</param>
        /// <returns>dynamic (json) with requested information</returns>
        async public Task<IList<dynamic>> RoutingAsync(Dictionary<string, string> additionalParams = null)
        {
            string path = "ai/text/translate/routing";

            dynamic jsonResult;
            // Call to Intento API and get json result
            using (HttpConnector conn = new HttpConnector(Intento))
                jsonResult = await conn.GetAsync(path, additionalParams: additionalParams);

            List<dynamic> models = new List<dynamic>();

            foreach (dynamic model in ((JContainer)jsonResult).First.First)
                models.Add(model);

            return models;
        }

        /// <summary>
        /// Details of the glossaries stored by the provider
        /// </summary>
        /// <param name="provider">Provider id</param>
        /// <param name="credential_id">Credential id</param>
        /// <param name="additionalParams">additional url params</param>
        /// <returns>dynamic (json) with requested information</returns>
        public IList<dynamic> Glossaries(string provider, Dictionary<string, string> credentials, Dictionary<string, string> additionalParams = null)
        {
            Task<dynamic> taskReadResult = Task.Run<dynamic>(async () =>
                await this.GlossariesAsync(provider, credentials: credentials, additionalParams: additionalParams));
            return taskReadResult.Result;
        }

        /// <summary>
        /// Details of the glossaries stored by the provider
        /// </summary>
        /// <param name="provider">Provider id</param>
        /// <param name="credential_id">Credential id</param>
        /// <param name="additionalParams">additional url params</param>
        /// <returns>dynamic (json) with requested information</returns>
        async public Task<IList<dynamic>> GlossariesAsync(string providerId, Dictionary<string, string> credentials, Dictionary<string, string> additionalParams = null)
        {
            string path = string.Format("ai/text/translate/glossaries?provider={0}", providerId);
            if (credentials != null)
            {
                string json;
                if (credentials.Count != 0)
                {
                    if (credentials.Count == 1 && credentials.ContainsKey("credential_id"))
                        json = credentials["credential_id"];
                    else
                    {
                        json = JsonConvert.SerializeObject(credentials, Formatting.None);
                        json = HttpUtility.UrlEncode(json);
                    }
                    path += String.Format("&credential_id={0}", json);
                }
            }

            dynamic jsonResult;
            // Call to Intento API and get json result
            using (HttpConnector conn = new HttpConnector(Intento))
                jsonResult = await conn.GetAsync(path, additionalParams: additionalParams);

            List<dynamic> glossaries = new List<dynamic>();

            foreach (dynamic glossary in ((JContainer)jsonResult).First.First)
                glossaries.Add(glossary);

            return glossaries;
        }

        /// <summary>
        /// Get a list of available delegated credentials
        /// </summary>
        /// <param name="additionalParams">additional url params</param>
        /// <returns>dynamic (json) with requested information</returns>
        [Obsolete]
        public IList<dynamic> DelegatedCredentials(Dictionary<string, string> additionalParams = null)
        {
            Task<dynamic> taskReadResult = Task.Run<dynamic>(async () =>
                await this.DelegatedCredentialsAsync(additionalParams: additionalParams));
            return taskReadResult.Result;
        }

        /// <summary>
        /// Get a list of available delegated credentials
        /// </summary>
        /// <param name="additionalParams">additional url params</param>
        /// <returns>dynamic (json) with requested information</returns>
        [Obsolete]
        async public Task<IList<dynamic>> DelegatedCredentialsAsync(Dictionary<string, string> additionalParams = null)
        {
            string path = "delegated_credentials";

            dynamic jsonResult;
            // Call to Intento API and get json result
            using (HttpConnector conn = new HttpConnector(Intento))
                jsonResult = await conn.GetAsync(path, additionalParams: additionalParams);

            List<dynamic> credentials = new List<dynamic>();

            foreach (dynamic credential in jsonResult)
                credentials.Add(credential);

            return credentials;
        }

        /// <summary>
        /// Detailed information on provider features
        /// </summary>
        /// <param name="provider">Provider id</param>
        /// <param name="additionalParams">additional url params</param>
        /// <returns>dynamic (json) with requested information</returns>
        public dynamic Provider(string provider, Dictionary<string, string> additionalParams = null)
        {
            Task<dynamic> taskReadResult = Task.Run<dynamic>(async () => await this.ProviderAsync(provider, additionalParams: additionalParams));
            return taskReadResult.Result;
        }

        /// <summary>
        /// Detailed information on provider features
        /// </summary>
        /// <param name="provider">Provider id</param>
        /// <returns>dynamic (json) with requested information</returns>
        async public Task<dynamic> ProviderAsync(string providerId, Dictionary<string, string> additionalParams = null)
        {
            string path = string.Format("ai/text/translate/{0}", providerId);

            dynamic jsonResult;
            // Call to Intento API and get json result
            using (HttpConnector conn = new HttpConnector(Intento))
                jsonResult = await conn.GetAsync(path, additionalParams: additionalParams);

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
            Dictionary<string, string> f = filter == null ? new Dictionary<string, string>() : new Dictionary<string, string>(filter);
            if (!string.IsNullOrEmpty(to))
                f["to"] = to;
            if (!string.IsNullOrEmpty(from))
                f["from"] = from;
            if (lang_detect)
                f["lang_detect"] = "true";
            if (bulk)
                f["bulk"] = "true";

            List<string> p = new List<string>();
            foreach (KeyValuePair<string, string> pair in f)
                p.Add(string.Format("{0}={1}", pair.Key, HttpUtility.UrlEncode(pair.Value)));
            string url = "ai/text/translate";
            if (p.Count != 0)
                url += "?" + string.Join("&", p);

            dynamic jsonResult;
            // Call to Intento API and get json result
            using (HttpConnector conn = new HttpConnector(Intento))
                jsonResult = await conn.GetAsync(url);

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

            dynamic jsonResult;
            // Call to Intento API and get json result
            using (HttpConnector conn = new HttpConnector(Intento))
                jsonResult = await conn.GetAsync(url);

            List<dynamic> languages = new List<dynamic>();
            foreach (dynamic languageInfo in jsonResult)
                languages.Add(languageInfo);

            return languages;
        }

		public Task<dynamic> Pairs(string srtName)
		{
			Task<dynamic> taskReadResult = Task.Run<dynamic>(async () => await this.PairsAsync(srtName));
			return taskReadResult.Result;
		}

		async public Task<dynamic> PairsAsync(string srtName) 
        {
            string url = String.Format("ai/text/translate/routing/{0}/pairs", srtName);

            dynamic jsonResult;

            using (HttpConnector conn = new HttpConnector(Intento))
                jsonResult = await conn.GetAsync(url);

            return jsonResult;
        }

		public IList<IList<string>> RoutingLanguagePairs(string providerId)
		{
			Task<IList<IList<string>>> taskReadResult = Task.Run<IList<IList<string>>>(async () => await this.RoutingLanguagePairsAsync(providerId));
			return taskReadResult.Result;
		}

		async public Task<IList<IList<string>>> RoutingLanguagePairsAsync(string providerId)
		{
			dynamic routingInfo = await PairsAsync(providerId);
			List<IList<string>> res = new List<IList<string>>();

			dynamic pairs = routingInfo.pairs;
			if (pairs != null)
			{
				foreach (dynamic pair in pairs)
					res.Add(new List<string> { (string)pair.from, (string)pair.to });
			}

			return res;
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
                    res.Add(new List<string> { (string)pair.from, (string)pair.to });
            }

            return res;
        }
    }
}
