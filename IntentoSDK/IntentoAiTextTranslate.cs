using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using IntentoSDK.API;
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
        { 
            get 
            { 
                return intento; 
            } 
        }

        public IntentoAiText Parent
        { 
            get 
            { 
                return parent; 
            } 
        }
      
        public dynamic Fulfill(object text, string to, string from = null, string provider = null,
            bool async = false, bool wait_async = false, string format = null, object auth = null,
            string custom_model = null, string glossary = null,
            object pre_processing = null, object post_processing = null,
            bool failover = false, object failover_list = null, string routing = null, bool trace = false,
            Dictionary<string, string> special_headers = null, Guid? clientAPIProvider = null, string filePath = null)
        {
            Task<dynamic> taskReadResult = Task.Run<dynamic>(async () => await this.FulfillAsync(text, to, from: from, provider: provider,
                async: async, wait_async: wait_async, format: format, auth: auth,
                custom_model: custom_model, glossary: glossary,
                pre_processing: pre_processing, post_processing: post_processing,
                failover: failover, failover_list: failover_list, routing: routing, trace: trace, special_headers: special_headers, clientAPIProvider: clientAPIProvider, filePath: filePath));
            return taskReadResult.Result;
        }

        async public Task<dynamic> FulfillAsync(object text, string to, string from = null, string provider = null,
            bool async = false, bool wait_async = false, string format = null, object auth = null,
            string custom_model = null, string glossary = null,
            object pre_processing = null, object post_processing = null,
            bool failover = false, object failover_list = null, string routing = null, bool trace = false,
            Dictionary<string, string> special_headers = null, Guid? clientAPIProvider = null, string filePath = null)
        {
            APIClientFactory.Current.Init(filePath);
            var clientApi = APIClientFactory.Current.Get(clientAPIProvider);
            dynamic jsonResult = await clientApi.Translate(Intento, text, to, from, provider,
                async, wait_async, format, auth, custom_model, glossary, pre_processing, post_processing,
                failover, failover_list, routing, trace, special_headers);            
            

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
