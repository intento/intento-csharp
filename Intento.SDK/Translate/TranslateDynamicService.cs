using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using Intento.SDK.Client;
using Intento.SDK.Exceptions;
using Intento.SDK.Logging;
using Intento.SDK.Translate.DTO;
using Intento.SDK.Validation;
using IntentoSDK.Translate.Options;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Intento.SDK.Translate
{
    /// <summary>
    /// Intento API
    /// </summary>
    internal sealed class TranslateDynamicService: ITranslateService
    {
        private IntentoHttpClient Client { get; }
        private ILogger Logger { get; }

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="client"></param>
        /// <param name="logger"></param>
        public TranslateDynamicService(IntentoHttpClient client, ILogger<TranslateDynamicService> logger)
        {
            Client = client;
            Logger = logger;
        }

        /// <inheritdoc />
        public async Task<TranslateResponseDto> FulfillAsync(TranslateOptions options)
        {
            options.ValidateAndThrow();

            var request = new TranslateRequestDto();
            var context = new TranslateContextDto();
            request.Context = context;
            
            var textLength = 0;
            switch (options.Text)
            {
                // text
                case null:
                    context.Text = new[] { "" };
                    break;
                case IEnumerable<string> enumerable:
                    var array = enumerable as string[] ?? enumerable.ToArray();
                    var testArr = array.Select(i => i ?? "").ToArray();
                    context.Text = testArr;
                    textLength = testArr.Sum(i => i.Length);
                    break;
                default:
                    var text = options.Text.ToString();
                    context.Text = new[] { text ?? "" };
                    textLength = text.Length;
                    break;
            }

            // determination of the possibility of using the syncwrapper
            // maximum 10k characters
            if (options.UseSyncwrapper)
            {
                if (textLength < 10000)
                {
                    options.Async = !options.UseSyncwrapper;
                }
                else
                {
                    options.UseSyncwrapper = false;
                }
            }

            // to
            context.To = options.To;

            // from
            if (!string.IsNullOrWhiteSpace(options.From))
            {
                context.From = options.From;
            }

            // format
            if (!string.IsNullOrWhiteSpace(options.Format))
            {
                context.Format = options.Format;
            }

            // custom_model
            if (!string.IsNullOrWhiteSpace(options.CustomModel))
            {
                context.Category = options.CustomModel;
            }

            // glossary
            if (!string.IsNullOrWhiteSpace(options.Glossary))
            {
                context.Glossary = options.Glossary;
            }
            
            // intento glossary
            if (options.IntentoGlossary is {Length: > 0})
            {
                context.Glossaries = options.IntentoGlossary.Select(g => new GlossaryInfo { Id = g }).ToArray();
            }

            // service section
            var service = new TranslateServiceDto();
            request.Service = service;

            // provider
            if (!string.IsNullOrWhiteSpace(options.Provider))
            {
                service.Provider = options.Provider;
            }

            // async parameter
            if (options.Async)
            {
                service.Async = true;
            }

            // auth parameter
            service.Auth = options.Auth;

            // routing
            if (!string.IsNullOrEmpty(options.Routing))
            {
                service.Routing = options.Routing;
            }

            // pre-post processing paramters
            if (options.PreProcessing != null || options.PostProcessing != null)
            {
                var processing = new TranslateProcessingDto();
                if (options.PreProcessing != null)
                {
                    processing.Pre = options.PreProcessing;
                }
                if (options.PostProcessing != null)
                {
                    processing.Post = options.PostProcessing;
                }
                service.Processing = processing;
            }

            // failover parameters
            if (options.Failover)
            {
                service.Failover = true;
                service.FailoverList = options.FailoverList;
            }

            var url = "ai/text/translate";
            if (options.Trace)
            {
                url += "?trace=true";
            }

            // Call to Intento API and get json result
            var jsonResult = await Client.PostAsync<TranslateRequestDto, TranslateResponseDto>(url, request, useSyncwrapper: options.UseSyncwrapper);
            
            if (options.Async && options.WaitAsync)
            {   // async operation (in terms of IntentoApi) and we need to wait result of it
                var id = jsonResult.Id;

                // In case of Sandbox key and some errors in parameters request to IntentoAPI may return:
                // 1. id: async operation started
                // 2. result of translation: Sandcox key
                // 3. error: validation of arameters failed (like request to translate html with provider with no such capabilities)
                if (id == null)
                {
                    // Not a (1) - return result immediately, nothing to wait
                    return jsonResult;
                }

                jsonResult = await WaitAsyncJobAsync(id);
            }

            if (!options.UseSyncwrapper)
            {
                return jsonResult;
            }

            /*var response = new JObject
            {
                ["results"] = jsonResult.results, ["meta"] = jsonResult.meta, ["service"] = jsonResult.service
            };
            ((JObject)jsonResult)["response"] = new JArray() { response };
            jsonResult.meta["providers"] = new JArray() { jsonResult.service.provider };*/

            return jsonResult;
        }

        /// <inheritdoc />
        public TranslateResponseDto Fulfill(TranslateOptions options)
        {
            var taskReadResult = Task.Run(async () => await FulfillAsync(options));
            return taskReadResult.Result;
        }
        
        /// <inheritdoc />
        public IList<ModelDto> Models(string provider, Dictionary<string, string> credentials, Dictionary<string, string> additionalParams = null)
        {
            var taskReadResult = Task.Run(async () =>
                await ModelsAsync(provider, credentials, additionalParams));
            return taskReadResult.Result;
        }

        /// <inheritdoc />
        public async Task<IList<ModelDto>> ModelsAsync(string providerId, Dictionary<string, string> credentials, Dictionary<string, string> additionalParams = null)
        {
            var path = $"ai/text/translate/models?provider={providerId}";
            if (credentials != null)
            {
                if (credentials.Count != 0)
                {
                    string json;
                    if (credentials.Count == 1 && credentials.ContainsKey("credential_id"))
                        json = credentials["credential_id"];
                    else
                    {
                        json = JsonConvert.SerializeObject(credentials, Formatting.None);
                        json = HttpUtility.UrlEncode(json);
                    }
                    path += $"&credential_id={json}";
                }
            }

            // Call to Intento API and get json result
            var jsonResult = await Client.GetAsync<ModelsResponseDto>(path, additionalParams);
            return jsonResult.Models;
        }

        /// <inheritdoc />
        public IList<dynamic> Accounts(string providerId = null, Dictionary<string, string> additionalParams = null)
        {
            var taskReadResult = Task.Run<dynamic>(async () =>
                await AccountsAsync(providerId, additionalParams));
            return taskReadResult.Result;
        }

        /// <inheritdoc />
        public async Task<IList<dynamic>> AccountsAsync(string providerId = null, Dictionary<string, string> additionalParams = null)
        {
            var path = string.Format("accounts" + (providerId != null ? $"?provider={providerId}" : null));
            // Call to Intento API and get json result
            var jsonResult = await Client.GetAsync(path, additionalParams);
            return ((JContainer) jsonResult).First.First.Cast<dynamic>().ToList();
        }
        
        /// <inheritdoc />
        public IList<RoutingDto> Routing(Dictionary<string, string> additionalParams = null)
        {
            var taskReadResult = Task.Run(async () =>
                await RoutingAsync(additionalParams));
            return taskReadResult.Result;
        }

        /// <inheritdoc />
        public async Task<IList<RoutingDto>> RoutingAsync(Dictionary<string, string> additionalParams = null)
        {
            const string path = "ai/text/translate/routing";
            // Call to Intento API and get json result
            var jsonResult = await Client.GetAsync<RoutingResponseDto>(path, additionalParams);
            return jsonResult.Routing;
        }
        
        /// <inheritdoc />
        public IList<dynamic> Glossaries(string providerId, Dictionary<string, string> credentials, Dictionary<string, string> additionalParams = null)
        {
            var taskReadResult = Task.Run<dynamic>(async () =>
                await GlossariesAsync(providerId, credentials, additionalParams));
            return taskReadResult.Result;
        }

        /// <inheritdoc />
        public async Task<IList<dynamic>> GlossariesAsync(string providerId, Dictionary<string, string> credentials, Dictionary<string, string> additionalParams = null)
        {
            var path = $"ai/text/translate/glossaries?provider={providerId}";
            if (credentials != null)
            {
                if (credentials.Count != 0)
                {
                    string json;
                    if (credentials.Count == 1 && credentials.ContainsKey("credential_id"))
                        json = credentials["credential_id"];
                    else
                    {
                        json = JsonConvert.SerializeObject(credentials, Formatting.None);
                        json = HttpUtility.UrlEncode(json);
                    }
                    path += $"&credential_id={json}";
                }
            }

            // Call to Intento API and get json result
            var jsonResult = await Client.GetAsync(path, additionalParams);

            return ((JContainer) jsonResult).First.First.Cast<dynamic>().ToList();
        }
        
        /// <inheritdoc />
        [Obsolete]
        public IList<dynamic> DelegatedCredentials(Dictionary<string, string> additionalParams = null)
        {
            var taskReadResult = Task.Run<dynamic>(async () =>
                await DelegatedCredentialsAsync(additionalParams));
            return taskReadResult.Result;
        }

        /// <inheritdoc />
        [Obsolete]
        public async Task<IList<dynamic>> DelegatedCredentialsAsync(Dictionary<string, string> additionalParams = null)
        {
            const string path = "delegated_credentials";
            // Call to Intento API and get json result
            var jsonResult = await Client.GetAsync(path, additionalParams);
            var credentials = new List<dynamic>();
            foreach (var credential in jsonResult)
            {
                credentials.Add(credential);
            }
            return credentials;
        }
        
        /// <inheritdoc />
        public ProviderDetailedDto Provider(string provider, Dictionary<string, string> additionalParams = null)
        {
            var taskReadResult = Task.Run(async () => await ProviderAsync(provider, additionalParams));
            return taskReadResult.Result;
        }

        /// <inheritdoc />
        public async Task<ProviderDetailedDto> ProviderAsync(string providerId, Dictionary<string, string> additionalParams = null)
        {
            var path = $"ai/text/translate/{providerId}";
            // Call to Intento API and get json result
            var jsonResult = await Client.GetAsync<ProviderDetailedDto>(path, additionalParams);
            return jsonResult;
        }

        /// <inheritdoc />
        public IList<ProviderDto> Providers(string to = null, string from = null, bool lang_detect = false, bool bulk = false,
            Dictionary<string, string> filter = null)
        {
            var taskReadResult = Task.Run(async () =>
                await ProvidersAsync(to, from, lang_detect, bulk, filter));
            return taskReadResult.Result;
        }

        /// <inheritdoc />
        public async Task<IList<ProviderDto>> ProvidersAsync(string to = null, string from = null, bool lang_detect = false, bool bulk = false,
            Dictionary<string, string> filter = null)
        {
            var f = filter == null ? new Dictionary<string, string>() : new Dictionary<string, string>(filter);
            if (!string.IsNullOrEmpty(to))
            {
                f["to"] = to;
            }
            if (!string.IsNullOrEmpty(from))
            {
                f["from"] = from;
            }
            if (lang_detect)
            {
                f["lang_detect"] = "true";
            }
            if (bulk)
            {
                f["bulk"] = "true";
            }
            var p = f.Select(pair => $"{pair.Key}={HttpUtility.UrlEncode(pair.Value)}").ToList();
            var url = "ai/text/translate";
            if (p.Count != 0)
            {
                url += "?" + string.Join("&", p);
            }
            // Call to Intento API and get json result
            var jsonResult = await Client.GetAsync<List<ProviderDto>>(url);
            var providers = new List<ProviderDto>();
            foreach (var providerInfo in jsonResult)
            {
                providers.Add(providerInfo);
            }
            return providers;
        }
        
        /// <inheritdoc />
        public IList<LanguageDto> Languages()
        {
            var taskReadResult = Task.Run(async () => await LanguagesAsync());
            return taskReadResult.Result;
        }

        /// <inheritdoc />
        public async Task<IList<LanguageDto>> LanguagesAsync()
        {
            const string url = "ai/text/translate/languages";
            var jsonResult = await Client.GetAsync<IList<LanguageDto>>(url);
            return jsonResult.ToList();
        }

        /// <inheritdoc />
        public IList<LanguageDto> GetSupportedLanguages()
        {
            var taskReadResult = Task.Run(async () => await GetSupportedLanguagesAsync());
            return taskReadResult.Result;
        }

        /// <inheritdoc />
        public async Task<IList<LanguageDto>> GetSupportedLanguagesAsync()
        {
            const string url = "ai/text/transliterate/languages";
            var jsonResult = await Client.GetAsync<IList<LanguageDto>>(url);
            return jsonResult.ToList();
        }
        
        /// <inheritdoc />
        public LanguagePairs Pairs(string srtName)
        {
            var taskReadResult = Task.Run(async () => await PairsAsync(srtName));
            return taskReadResult.Result;
        }

        /// <inheritdoc />
        public async Task<LanguagePairs> PairsAsync(string srtName) 
        {
            var url = $"ai/text/translate/routing/{srtName}/pairs";
            var jsonResult = await Client.GetAsync<LanguagePairs>(url);
            return jsonResult;
        }
        
        /// <inheritdoc />
        public IList<IList<string>> RoutingLanguagePairs(string providerId)
        {
            var taskReadResult = Task.Run(async () => await RoutingLanguagePairsAsync(providerId));
            return taskReadResult.Result;
        }

        /// <inheritdoc />
        public async Task<IList<IList<string>>> RoutingLanguagePairsAsync(string providerId)
        {
            var routingInfo = await PairsAsync(providerId);
            var res = new List<IList<string>>();

            var pairs = routingInfo.Pairs;
            if (pairs == null)
            {
                return res;
            }

            foreach (var pair in pairs)
            {
                res.Add(new List<string> {(string) pair.From, (string) pair.To});
                
            }

            return res;
        }
        
        /// <inheritdoc />
        public IList<IList<string>> ProviderLanguagePairs(string providerId)
        {
            var taskReadResult = Task.Run(async () => await ProviderLanguagePairsAsync(providerId));
            return taskReadResult.Result;
        }

        /// <inheritdoc />
        public async Task<IList<IList<string>>> ProviderLanguagePairsAsync(string providerId)
        {
            var providerInfo = await ProviderAsync(providerId);
            var res = new List<IList<string>>();
            var languages = providerInfo.Languages;
            var symmetric = languages.Symmetric;
            if (symmetric != null)
            {
                foreach (var l1 in symmetric)
                {
                    res.AddRange(from l2 in symmetric where l1 != l2 select new List<string> {l1, l2});
                }
            }

            var pairs = languages.Pairs;
            if (pairs == null)
            {
                return res;
            }

            res.AddRange(pairs.Select(pair => new List<string> {pair.From, pair.To}));

            return res;
        }
        
        /// <inheritdoc />
        public IList<dynamic> AgnosticGlossaries()
        {
            Task<dynamic> taskReadResult = Task.Run<dynamic>(async () =>
                await AgnosticGlossariesAsync());
            return taskReadResult.Result;
        }

        /// <inheritdoc />
        public async Task<IList<dynamic>> AgnosticGlossariesAsync()
        {
            var path = $"ai/text/glossaries/v2/typed";
            // Call to Intento API and get json result
            var jsonResult = await Client.GetAsync(path);
            var glossaries = new List<dynamic>();
            foreach (dynamic glossary in jsonResult.glossaries)
            {
                glossaries.Add(glossary);
            }
            return glossaries;
        }

        /// <inheritdoc />
        public IList<dynamic> AgnosticGlossariesTypes()
        {
            Task<dynamic> taskReadResult = Task.Run<dynamic>(async () =>
                await AgnosticGlossariesTypesAsync());
            return taskReadResult.Result;
        }

        /// <inheritdoc />
        public async Task<IList<dynamic>> AgnosticGlossariesTypesAsync()
        {
            var path = $"ai/text/glossaries/v2/cs_types";
            var jsonResult = await Client.GetAsync(path);
            var types = new List<dynamic>();
            foreach (dynamic type in jsonResult.types)
            {
                types.Add(type);
            }
            return types;
        }
        
        private static dynamic GetJson(object data, string name)
        {
            // Convert data to json. 
            // String is deserialized in case it has a for of json list or dict. 
            // data=="" is trated as null

            switch (data)
            {
                case null:
                    return null;
                case string @string:
                    {
                        var z = @string.Trim();
                        if (string.IsNullOrEmpty(z))
                            return null;
                        try
                        {
                            switch (z[0])
                            {
                                case '[':
                                    return JArray.Parse(z);
                                case '{':
                                    return JObject.Parse(z);
                            }
                        }
                        catch { }

                        return @string;
                    }

                case JObject @object:
                    return @object;
                case JArray array:
                    return array;
                case IDictionary<string, string> dictionary:
                    return JObject.FromObject(dictionary);
                case IEnumerable<string> enumerable:
                    return JArray.FromObject(enumerable);
                default:
                    throw new IntentoInvalidParameterException(name, "need to be null or string or json-string or Newtonsoft JObject/JArray");
            }            
        }

        private dynamic CheckAsyncJob(string asyncId)
        {
            var task = Task.Run(async () => await CheckAsyncJobAsync(asyncId));
            return task.Result;
        }

        private async Task<dynamic> CheckAsyncJobAsync(string asyncId)
        {
            Logger.LogDebug(SdkLogEvents.INVOKE_METHOD, "CheckAsyncJobAsync: {asyncId}ms", asyncId);
            // async operations inside
            var result = await Client.GetAsync($"operations/{asyncId}");
            return result;
        }

        private dynamic WaitAsyncJob(string asyncId, int delay = 0)
        {
            var taskResult = Task.Run(async () => await this.WaitAsyncJobAsync(asyncId, delay));
            return taskResult.Result;
        }

        private List<int> CalcDelays(int delay)
        {
            var delays = new List<int>();
            do
            {
                delays.Add(delay);
                delay += 100;
            } while (delay < 3000);
            return delays;
        }

        private async Task<dynamic> WaitAsyncJobAsync(string asyncId, int delay = 0)
        {
            var dt = DateTime.Now;

            Logger.LogDebug(SdkLogEvents.INVOKE_METHOD, $"WaitAsyncJobAsync-start: {asyncId} - {delay}ms");
            List<int> delays;
            var n = 0;

            switch (delay)
            {
                case -1:
                    delays = new List<int> { 0 };
                    break;
                case 0:
                    delays = CalcDelays(400);
                    break;
                default:
                    delays = CalcDelays(delay);
                    break;
            }

            delay = delays[0];
            do
            {
                Logger.LogDebug(SdkLogEvents.INVOKE_METHOD, $"WaitAsyncJobAsync-loop: {asyncId} - {delay}ms");
                Thread.Sleep(delay);
                Logger.LogDebug( SdkLogEvents.INVOKE_METHOD, $"WaitAsyncJobAsync-loop after sleep: {asyncId} - {delay}ms");

                var result = await CheckAsyncJobAsync(asyncId);

                Logger.LogDebug(SdkLogEvents.INVOKE_METHOD, $"WaitAsyncJobAsync-loop1a: {asyncId} - {delay}ms");
                if ((bool) result.done)
                {
                    return result;
                }

                n++;
                if (n < delays.Count)
                {
                    delay = delays[n];
                }

                Logger.LogDebug(SdkLogEvents.INVOKE_METHOD, $"WaitAsyncJobAsync-loop2: {asyncId} - {delay}ms");
            } while (DateTime.Now < dt.AddSeconds(delay));

            // Timeout
            dynamic json = new JObject();
            json.id = asyncId;
            json.done = false;
            json.response = null;

            dynamic error = new JObject();
            error.type = "Timeout";
            error.reason = "Too long response from Intento MT plugin";
            error.data = null;
            json.error = error;

            return json;
        }
    }
}
