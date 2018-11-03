using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Threading;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Web;
using System.Net.Http;
using System.Threading.Tasks;

namespace IntentoSDK
{
    public class HttpConnector
    {
        Intento intento;
        HttpClient client;
        private ManualResetEvent allDone = new ManualResetEvent(false);

        public HttpConnector(Intento _intento)
        {
            intento = _intento;

            client = new HttpClient();
            client.DefaultRequestHeaders.Add("apikey", intento.apiKey);
            string userAgent = string.Format("Intento.CSharpSDK/{0} {1}", intento.version, intento.otherUserAgent);
            client.DefaultRequestHeaders.Add("User-Agent", userAgent);
        }

        async public Task<dynamic> PostAsync(string path, dynamic json)
        {
            string jsonData = JsonConvert.SerializeObject(json);
            HttpContent requestBody = new StringContent(jsonData);

            HttpResponseMessage response = await client.PostAsync(intento.serverUrl + path, requestBody);
            dynamic jsonResult = await GetJson(response);

            if (response.StatusCode != HttpStatusCode.OK)
                throw (Exception)IntentoException.Make(response, jsonResult);
            return jsonResult;
        }

        async public Task<dynamic> GetAsync(string path)
        {
            HttpResponseMessage response = await client.GetAsync(intento.serverUrl + path);
            dynamic jsonResult = await GetJson(response);

            if (response.StatusCode != HttpStatusCode.OK)
                throw (Exception)IntentoException.Make(response, jsonResult);
            return jsonResult;
        }

        async private Task<dynamic> GetJson(HttpResponseMessage response)
        {
            string stringResult = await response.Content.ReadAsStringAsync();
            dynamic jsonResult;
            if (stringResult[0] == '{')
                jsonResult = JObject.Parse(stringResult);
            else if (stringResult[0] == '[')
                jsonResult = JArray.Parse(stringResult);
            else
                throw new IntentoException(string.Format("Invalid json returned from IntentoAPI: '{0}'", stringResult));
            return jsonResult;
        }

    }
}
