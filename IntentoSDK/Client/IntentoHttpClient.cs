using IntentoSDK.Logging;
using IntentoSDK.Settings;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace IntentoSDK.Client
{
    internal class IntentoHttpClient
    {
        protected HttpClient Client { get; }

        protected Options Options { get; }

        protected ILogger Logger { get; }

        public IntentoHttpClient(HttpClient client, Options options, ILogger logger)
        {
            client.DefaultRequestHeaders.Add("apikey", options.ApiKey);
            client.DefaultRequestHeaders.Add("User-Agent", $"Intento.CSharpSDK/{options.Version} {options.OtherUserAgent}");
            Client = client;
            Options = options;
            Logger = logger;
        }

        async public Task<dynamic> PostAsync(string path, dynamic json, Dictionary<string, string> special_headers = null, 
											Dictionary<string, string> additionalParams = null, bool useSyncwrapper = false)
        {
            try
            {
                string jsonData = JsonConvert.SerializeObject(json);
                using (var httpContent = new StringContent(jsonData))
                {
                    var url = MakeUrl(path, additionalParams, useSyncwrapper);
                    if (special_headers != null && special_headers.Count != 0)
                    {
                        foreach (KeyValuePair<string, string> pair in special_headers)
                        {
                            httpContent.Headers.Add(pair.Key, pair.Value);
                        }
                    }

                    LogHttpRequest("POST", url, jsonData, Client.DefaultRequestHeaders.ToString());
                    using (var response = await Client.PostAsync(url, httpContent))
                    {
                        LogHttpAfterSend(url);
                        dynamic jsonResult = await GetJson(response);
                        LogHttpResponse(jsonResult);

                        if (response.StatusCode != HttpStatusCode.OK)
                        {
                            Exception ex = IntentoException.Make(response, jsonResult);
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
                LogHttpRequest("GET", url, null, Client.DefaultRequestHeaders.ToString());
                using (var response = await Client.GetAsync(url))
                {
                    LogHttpAfterSend(url);
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

        private string MakeUrl(string path, Dictionary<string, string> additionalParams, bool useSyncwrapper = false)
        {
			string url = useSyncwrapper ? Options.SyncWrapperUrl : Options.ServerUrl;
            if (additionalParams == null)
            {
                return url;
            }

            var uri = new UriBuilder(url + path);
            var query = HttpUtility.ParseQueryString(uri.Query);
            foreach (KeyValuePair<string, string> pair in additionalParams)
            {
                query[pair.Key] = pair.Value;
            }
            uri.Query = query.ToString();
            return uri.ToString();
        }

        async private Task<dynamic> GetJson(HttpResponseMessage response)
        {
            string stringResult = await response.Content.ReadAsStringAsync();
            dynamic jsonResult;
            if (stringResult[0] == '{')
            {
                jsonResult = JObject.Parse(stringResult);
            }
            else if (stringResult[0] == '[')
            {
                jsonResult = JArray.Parse(stringResult);
            }
            else
            {
                throw new IntentoException(string.Format("Invalid json returned from IntentoAPI: '{0}'", stringResult));
            }
            return jsonResult;
        }

        private void LogHttpRequest(string method, string url, string body, string headers)
        {
            Logger.LogDebug(SDKLogEvents.StartHttpRequest, "method: {method}\r\nurl: {url}\r\nbody: {body}\r\nheaders: {headers}\r\n", method, url, body, headers);
        }

        private void LogHttpAfterSend(string url)
        {
            Logger.LogDebug(SDKLogEvents.AfterSendHttpRequest, "url: {url}", url);
        }

        private void LogHttpResponse(dynamic jsonResult)
        {
            Logger.LogDebug(SDKLogEvents.HttpResponse, "body: {jsonResult}", jsonResult != null ? (string)jsonResult.ToString() : "empty");
        }

        private void LogHttpException(Exception ex)
        {
            Logger.LogError(SDKLogEvents.HttpError, ex, ex.Message);
        }
        
    }
}
