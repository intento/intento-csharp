using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Intento.SDK.Exceptions;
using Intento.SDK.Extensions;
using Intento.SDK.Logging;
using Intento.SDK.Settings;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Intento.SDK.Client
{
    public class BaseHttpClient
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
        protected BaseHttpClient(HttpClient client, Options options, ILogger logger)
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
        /// <param name="specialHeaders"></param>
        /// <param name="additionalParams"></param>
        /// <param name="useSyncwrapper"></param>
        /// <param name="isTranslateRequest"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<TResponse> PostAsync<TRequest, TResponse>(string path, TRequest json, IDictionary<string, string> specialHeaders = null,
            IDictionary<string, string> additionalParams = null, bool useSyncwrapper = false, bool isTranslateRequest = false)
            where TResponse : class
        {
            try
            {
                var jsonData = JsonConvert.SerializeObject(json, Formatting.None, new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore
                });
                using var httpContent = new StringContent(jsonData, Encoding.UTF8, "application/json");
                var url = MakeUrl(path, additionalParams, useSyncwrapper, isTranslateRequest);
                if (specialHeaders != null && specialHeaders.Count != 0)
                {
                    foreach (var pair in specialHeaders)
                    {
                        httpContent.Headers.Add(pair.Key, pair.Value);
                    }
                }

                LogHttpRequest("POST", url, jsonData, Client.DefaultRequestHeaders.ToString());
                using (var response = await Client.PostAsync(url, httpContent))
                {
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        var jsonResult = await GetJson<TResponse>(response);
                        return jsonResult;
                    }

                    var ex = response.Create(response);
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

        /// <summary>
        /// Execute request to url (GET)
        /// </summary>
        /// <param name="path"></param>
        /// <param name="isTranslateRequest"></param>
        /// <param name="additionalParams"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<T> GetAsync<T>(string path, bool isTranslateRequest = false, IDictionary<string, string> additionalParams = null)
            where T:class
        {
            var url = MakeUrl(path, additionalParams, false, isTranslateRequest);
            LogHttpRequest("GET", url, null, Client.DefaultRequestHeaders.ToString());
            try
            {
                using var response = await Client.GetAsync(url);
                using var contentStream = await response.Content.ReadAsStreamAsync();
                using var streamReader = new StreamReader(contentStream);
                using var jsonReader = new JsonTextReader(streamReader);
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var jsonSerializer = new JsonSerializer();
                    var jsonResult = jsonSerializer.Deserialize<T>(jsonReader);
                    LogHttpResponse(jsonResult);
                    return jsonResult;
                }
                Exception ex = response.Create(await streamReader.ReadToEndAsync());
                LogHttpException(ex);
                throw ex;
            }
            catch (Exception ex)
            {
                LogHttpException(ex);
                throw;
            }
        }

        private string MakeUrl(string path, IDictionary<string, string> additionalParams, bool useSyncwrapper, bool isTranslateRequest)
        {
            var url = isTranslateRequest && !string.IsNullOrEmpty(Options.TmsServerUrl)
                ? Options.TmsServerUrl
                : (useSyncwrapper && !string.IsNullOrEmpty(Options.SyncwrapperUrl) ? Options.SyncwrapperUrl : Options.ServerUrl);
            var fullUrl = url.CombineUrl(path);
            var uri = new UriBuilder(new Uri(fullUrl, UriKind.RelativeOrAbsolute));
            if (additionalParams == null)
            {
                return uri.ToString();
            }
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

        private void LogHttpRequest(string method, string url, string body, string headers)
        {
            Logger.LogDebug(SdkLogEvents.START_HTTP_REQUEST, "method: {Method}\r\nurl: {Url}\r\nbody: {Body}\r\nheaders: {Headers}\r\n", method, url, body, headers);
        }

        private void LogHttpResponse(object jsonResult)
        {
            var result = jsonResult != null ? JsonConvert.SerializeObject(jsonResult) : null;
            Logger.LogDebug(SdkLogEvents.HTTP_RESPONSE, "body: {Result}", result ?? "empty");
        }

        private void LogHttpException(Exception ex)
        {
            Logger.LogError(SdkLogEvents.HTTP_ERROR, ex, "HTTP request return error");
        }
    }
}