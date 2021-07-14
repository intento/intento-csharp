using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using Intento.SDK.Exceptions;
using Intento.SDK.Logging;
using Intento.SDK.Settings;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Intento.SDK.Client
{
    /// <summary>
    /// Connection client to API
    /// </summary>
    internal class IntentoHttpClient
    {
        private HttpClient Client { get; }

        private ILogger Logger { get; }

        private Options Options { get; }

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="client">HttpClient for requests</param>
        /// <param name="options">Options of connections</param>
        /// <param name="logger">Logger implementation</param>
        public IntentoHttpClient(HttpClient client, Options options, ILogger<IntentoHttpClient> logger)
        {
            var version = IntentoHelpers.GetVersion();
            client.DefaultRequestHeaders.Add("apikey", options.ApiKey);
            client.DefaultRequestHeaders.Add("User-Agent", $"Intento.CSharpSDK/{version} {options.ClientUserAgent}");
            Client = client;
            Logger = logger;
            Options = options;
        }

        /// <summary>
        /// Execute request to url with params (POST)
        /// </summary>
        /// <param name="path"></param>
        /// <param name="json"></param>
        /// <param name="special_headers"></param>
        /// <param name="additionalParams"></param>
        /// <param name="useSyncwrapper"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        [Obsolete]
        public async Task<dynamic> PostAsync(string path, dynamic json, Dictionary<string, string> special_headers = null, 
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
                        foreach (var pair in special_headers)
                        {
                            httpContent.Headers.Add(pair.Key, pair.Value);
                        }
                    }

                    LogHttpRequest("POST", url, jsonData, Client.DefaultRequestHeaders.ToString());
                    using (var response = await Client.PostAsync(url, httpContent))
                    {
                        LogHttpAfterSend(url);
                        var jsonResult = await GetJson(response);
                        LogHttpResponse(jsonResult);

                        if (response.StatusCode == HttpStatusCode.OK)
                        {
                            return jsonResult;
                        }

                        Exception ex = response.Create((object)jsonResult);
                        LogHttpException(ex);
                        throw ex;
                    }
                }
            }
            catch (Exception ex)
            {
                LogHttpException(ex);
                throw;
            }
        }

        /// <summary>
        /// Execute request to url with params (POST)
        /// </summary>
        /// <param name="path"></param>
        /// <param name="json"></param>
        /// <param name="special_headers"></param>
        /// <param name="additionalParams"></param>
        /// <param name="useSyncwrapper"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<ResponseT> PostAsync<RequestT, ResponseT>(string path, RequestT json, Dictionary<string, string> special_headers = null,
            Dictionary<string, string> additionalParams = null, bool useSyncwrapper = false)
            where ResponseT : class
        {
            try
            {
                var jsonData = JsonConvert.SerializeObject(json, Formatting.None, new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore
                });
                using (var httpContent = new StringContent(jsonData))
                {
                    var url = MakeUrl(path, additionalParams, useSyncwrapper);
                    if (special_headers != null && special_headers.Count != 0)
                    {
                        foreach (var pair in special_headers)
                        {
                            httpContent.Headers.Add(pair.Key, pair.Value);
                        }
                    }

                    LogHttpRequest("POST", url, jsonData, Client.DefaultRequestHeaders.ToString());
                    using (var response = await Client.PostAsync(url, httpContent))
                    {
                        LogHttpAfterSend(url);
                        var jsonResult = await GetJson<ResponseT>(response);
                        LogHttpResponse(jsonResult);

                        if (response.StatusCode == HttpStatusCode.OK)
                        {
                            return jsonResult;
                        }

                        Exception ex = response.Create(jsonResult);
                        LogHttpException(ex);
                        throw ex;
                    }
                }
            }
            catch (Exception ex)
            {
                LogHttpException(ex);
                throw;
            }
        }

        /// <summary>
        /// Execute request to url (GET)
        /// </summary>
        /// <param name="path"></param>
        /// <param name="additionalParams"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        [Obsolete]
        public async Task<dynamic> GetAsync(string path, Dictionary<string, string> additionalParams = null)
        {
            var url = MakeUrl(path, additionalParams);
            LogHttpRequest("GET", url, null, Client.DefaultRequestHeaders.ToString());
            try
            {
                using (var response = await Client.GetAsync(url))
                {
                    LogHttpAfterSend(url);
                    var jsonResult = await GetJson(response);
                    LogHttpResponse(jsonResult);

                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        return jsonResult;
                    }

                    Exception ex = response.Create((object)jsonResult);
                    LogHttpException(ex);
                    throw ex;
                }
            }
            catch (Exception ex)
            {
                LogHttpException(ex);
                throw;
            }
        }

        public async Task<T> GetAsync<T>(string path, Dictionary<string, string> additionalParams = null)
            where T:class
        {
            var url = MakeUrl(path, additionalParams);
            LogHttpRequest("GET", url, null, Client.DefaultRequestHeaders.ToString());
            try
            {
                using (var response = await Client.GetAsync(url))
                {
                    LogHttpAfterSend(url);
                    using (var contentStream = await response.Content.ReadAsStreamAsync())
                    {
                        using (var streamReader = new StreamReader(contentStream))
                        {
                            using (var jsonReader = new JsonTextReader(streamReader))
                            {
                                var jsonSerializer = new JsonSerializer();
                                var jsonResult = jsonSerializer.Deserialize<T>(jsonReader);
                                LogHttpResponse(jsonResult);
                                if (response.StatusCode == HttpStatusCode.OK)
                                {
                                    return jsonResult;
                                }
                                Exception ex = response.Create(jsonResult);
                                LogHttpException(ex);
                                throw ex;
                            }
                        }
                    }
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
			var url = useSyncwrapper ? Options.SyncwrapperUrl : Options.ServerUrl;
            var fullUrl = url + path;
            if (additionalParams == null)
            {
                return fullUrl;
            }

            var uri = new UriBuilder(fullUrl);
            var query = HttpUtility.ParseQueryString(uri.Query);
            foreach (var pair in additionalParams)
            {
                query[pair.Key] = pair.Value;
            }
            uri.Query = query.ToString();
            return uri.ToString();
        }

        private async Task<T> GetJson<T>(HttpResponseMessage response)
        {
            var stringResult = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(stringResult);
        }

        [Obsolete]
        private async Task<dynamic> GetJson(HttpResponseMessage response)
        {
            var stringResult = await response.Content.ReadAsStringAsync();
            dynamic jsonResult;
            switch (stringResult[0])
            {
                case '{':
                    jsonResult = JObject.Parse(stringResult);
                    break;
                case '[':
                    jsonResult = JArray.Parse(stringResult);
                    break;
                default:
                    throw new IntentoException($"Invalid json returned from IntentoAPI: '{stringResult}'");
            }
            return jsonResult;
        }

        private void LogHttpRequest(string method, string url, string body, string headers)
        {
            Logger.LogDebug(SdkLogEvents.START_HTTP_REQUEST, "method: {method}\r\nurl: {url}\r\nbody: {body}\r\nheaders: {headers}\r\n", method, url, body, headers);
        }

        private void LogHttpAfterSend(string url)
        {
            Logger.LogDebug(SdkLogEvents.AFTER_SEND_HTTP_REQUEST, "url: {url}", url);
        }

        private void LogHttpResponse(dynamic jsonResult)
        {
            Logger.LogDebug(SdkLogEvents.HTTP_RESPONSE, "body: {jsonResult}", jsonResult != null ? (string)jsonResult.ToString() : "empty");
        }

        private void LogHttpException(Exception ex)
        {
            Logger.LogError(SdkLogEvents.HTTP_ERROR, ex, ex.Message);
        }
        
    }
}
