using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntentoSDK.API
{
    /// <summary>
    /// Client to connect to Intento API
    /// </summary>
    public class RemoteAPIClient : BaseAPIClient
    {
        public static Guid uid = new Guid("{BCB8A56A-BB06-44F7-9888-A7A9A719FA93}");

        ///<inheritdoc/>
        public override Guid ClientUid => uid;

        ///<inheritdoc/>
        public override string DisplayName => "Translate from Intento API";

        ///<inheritdoc/>
        public override async Task<dynamic> Translate(Intento intento, object text, string to, string from = null, string provider = null,
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
            using (HttpConnector conn = new HttpConnector(intento))
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

                jsonResult = await intento.WaitAsyncJobAsync(id);
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
    }
}
