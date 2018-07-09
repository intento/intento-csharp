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

        public dynamic Fulfill(string text, string to, string from = null, string provider = null, 
            bool async = false, string format = null)
        {
            Task<dynamic> taskReadResult = Task.Run<dynamic>(async () => await this.FulfillAsync(text, to, from: from, 
                provider: provider, async: async, format: format));
            return taskReadResult.Result;
        }

        async public Task<dynamic> FulfillAsync(string text, string to, string from = null, string provider = null, 
            bool async = false, string format = null)
        {
            if (string.IsNullOrEmpty(from))
                from = null;

            // Create body for Intento API request
            string jsonData = JsonConvert.SerializeObject(new {
                context = new Dictionary<string, string>{ { "text", text }, { "from", from }, { "to", to }, { "format", format } },
                service = new { async = async, provider = provider },
            });
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
