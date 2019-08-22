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
    public class HttpConnector : IDisposable
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
            try
            {
                string jsonData = JsonConvert.SerializeObject(json);
                using (HttpContent requestBody = new StringContent(jsonData))
                {
                    LogHttpRequest("POST", intento.serverUrl + path, jsonData, client.DefaultRequestHeaders.ToString());
                    using (HttpResponseMessage response = await client.PostAsync(intento.serverUrl + path, requestBody))
                    {
                        LogHttpAfterSend();
                        dynamic jsonResult = await GetJson(response);
                        LogHttpResponse(jsonResult);

                        if (response.StatusCode != HttpStatusCode.OK)
                        {
                            Exception ex = (Exception)IntentoException.Make(response, jsonResult);
                            LogHttpException(ex);
                            throw ex;
                        }
                        return jsonResult;
                    }
                }
            }
            catch (Exception ex)
            {
                LogHttpException(ex);
                throw;
            }
        }

        async public Task<dynamic> GetAsync(string path)
        {
            try
            {
                var url = intento.serverUrl + path;
                LogHttpRequest("GET", intento.serverUrl + path, null, client.DefaultRequestHeaders.ToString());
                using (HttpResponseMessage response = await client.GetAsync(url))
                {
                    LogHttpAfterSend();
                    dynamic jsonResult = await GetJson(response);
                    LogHttpResponse(jsonResult);

                    if (response.StatusCode != HttpStatusCode.OK)
                    {
                        Exception ex = (Exception)IntentoException.Make(response, jsonResult);
                        LogHttpException(ex);
                        throw ex;
                    }
                    return jsonResult;
                }
            }
            catch (Exception ex)
            {
                LogHttpException(ex);
                throw;
            }
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

        private void LogHttpRequest(string method, string url, string body, string headers)
        {
            intento.Log("HTTP Request", string.Format("method: {0}\r\nurl: {1}\r\nbody: {2}\r\nheaders: {3}", method, url, body, headers));
        }

        private void LogHttpAfterSend()
        {
            intento.Log("HTTP after send");
        }

        private void LogHttpResponse(dynamic jsonResult)
        {
            intento.Log("HTTP response", string.Format("body: {0}", jsonResult));
        }

        private void LogHttpException(Exception ex)
        {
            intento.Log("HTTP exception", ex: ex);
        }

        public void Dispose()
        {
            client.Dispose();
        }
    }
}
