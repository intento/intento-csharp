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
            if (intento.proxy?.ProxyUri != null && intento.proxy.ProxyEnabled)
            {
                var proxy = new WebProxy()
                {
                    Address = intento.proxy.ProxyUri,
                    UseDefaultCredentials = false
                };

                if (!string.IsNullOrWhiteSpace(intento.proxy.ProxyUserName))
                    proxy.Credentials = new NetworkCredential(userName: intento.proxy.ProxyUserName, password: intento.proxy.ProxyPassword);

                var httpClientHandler = new HttpClientHandler()
                {
                    Proxy = proxy,
                };
                client = new HttpClient(httpClientHandler, false);
            }
            else
                client = new HttpClient();
            client.DefaultRequestHeaders.Add("apikey", intento.apiKey);
            string userAgent = string.Format("Intento.CSharpSDK/{0} {1}", intento.version, intento.otherUserAgent);
            client.DefaultRequestHeaders.Add("User-Agent", userAgent);
        }

        async public Task<dynamic> PostAsync(string path, dynamic json, Dictionary<string, string> special_headers = null, Dictionary<string, string> additionalParams = null)
        {
            try
            {
                string jsonData = JsonConvert.SerializeObject(json);
                using (HttpContent httpContent = new StringContent(jsonData))
                {
                    var url = MakeUrl(path, additionalParams);
                    if (special_headers != null && special_headers.Count != 0)
                    {
                        foreach (KeyValuePair<string, string> pair in special_headers)
                            httpContent.Headers.Add(pair.Key, pair.Value);
                    }

                    LogHttpRequest("POST", url, jsonData, client.DefaultRequestHeaders.ToString());
                    using (HttpResponseMessage response = await client.PostAsync(url, httpContent))
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

        async public Task<dynamic> GetAsync(string path, Dictionary<string, string> additionalParams = null)
        {
            try
            {
                var url = MakeUrl(path, additionalParams);
                LogHttpRequest("GET", url, null, client.DefaultRequestHeaders.ToString());
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

        private string MakeUrl(string path, Dictionary<string, string> additionalParams)
        {
            if (additionalParams == null)
                return intento.serverUrl + path;

            UriBuilder uri = new UriBuilder(intento.serverUrl + path);
            System.Collections.Specialized.NameValueCollection query = HttpUtility.ParseQueryString(uri.Query);
            foreach (KeyValuePair<string, string> pair in additionalParams)
                query[pair.Key] = pair.Value;
            uri.Query = query.ToString();
            return uri.ToString();
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
