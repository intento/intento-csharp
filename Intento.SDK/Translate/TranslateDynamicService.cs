﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Intento.SDK.Client;
using Intento.SDK.Logging;
using Intento.SDK.Translate.Converters;
using Intento.SDK.Translate.DTO;
using Intento.SDK.Translate.Options;
using Intento.SDK.Validation;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Intento.SDK.Translate
{
    /// <summary>
    /// Intento API
    /// </summary>
    internal sealed class TranslateDynamicService : ITranslateService
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
        public Task<TranslateResponse> FulfillAsync(TranslateOptions options)
        {
            return FulfillAsync(options, null);
        }

        private async Task<TranslateResponse> FulfillAsync(TranslateOptions options,
            Action<TranslateRequest> extendRequest)
        {
            options.ValidateAndThrow();

            var request = new TranslateRequest();
            var context = new TranslateContext();
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
            if (options.IntentoGlossary is { Length: > 0 })
            {
                context.Glossaries = options.IntentoGlossary.Select(g => new Glossary { Id = g }).ToArray();
            }

            // service section
            var service = new TranslateService();
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

            // pre-post processing parameters
            if (options.PreProcessing != null || options.PostProcessing != null)
            {
                var processing = new TranslateProcessing();
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

            extendRequest?.Invoke(request);

            var url = "ai/text/translate";
            var modifiers = new List<string>();
            if (options.Trace)
            {
                modifiers.Add("trace=true");
            }

            if (options.PlainText)
            {
                modifiers.Add("plainText=true");
            }

            if (modifiers.Count > 0)
            {
                url += $"?{string.Join("&", modifiers)}";
            }

            // Call to Intento API and get json result
            var jsonResult =
                await Client.PostAsync<TranslateRequest, TranslateResponse>(url, request,
                    useSyncwrapper: options.UseSyncwrapper, isTranslateRequest: true);

            if (options.Async && options.WaitAsync)
            {
                // async operation (in terms of IntentoApi) and we need to wait result of it
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
                
                var taskCompletion = new TaskCompletionSource<bool>();
                var wrapperTask = await Task.WhenAny(
                    WaitAsync(taskCompletion, id, 1000),
                    TimeoutError());
                var wrapper = await wrapperTask;
                taskCompletion.SetResult(true);

                if (wrapper.Done)
                {
                    jsonResult = wrapper.Response is { Length: > 0 } ? wrapper.Response[0] : new TranslateResponse
                    {
                        Error = new TranslationRequestError
                        {
                            Message = wrapper.Error?.Reason,
                            Data = wrapper.Error != null ? JsonConvert.SerializeObject(wrapper.Error) : null
                        }
                    };
                }
            }

            return jsonResult;
        }

        /// <inheritdoc />
        public TranslateResponse Fulfill(TranslateOptions options)
        {
            var taskReadResult = Task.Run(async () => await FulfillAsync(options));
            return taskReadResult.Result;
        }

        /// <inheritdoc />
        public async Task FulfillFileAsync(TranslateOptions options, string sourcePath, string outputPath)
        {
            if (!File.Exists(sourcePath))
            {
                Logger.LogWarning($"File not found in path {sourcePath}");
                return;
            }

            var fileInfo = new FileInfo(sourcePath);
            using var stream = new FileStream(sourcePath, FileMode.Open, FileAccess.Read);
            using var result = await FulfillFileAsync(options, stream, fileInfo);
            using var writer = new FileStream(outputPath, FileMode.Create, FileAccess.Write);
            await result.CopyToAsync(writer);
        }

        /// <inheritdoc />
        public void FulfillFile(TranslateOptions options, string sourcePath, string outputPath)
        {
            var taskReadResult = Task.Run(async () => await FulfillFileAsync(options, sourcePath, outputPath));
            taskReadResult.Start();
            taskReadResult.Wait();
        }

        /// <inheritdoc />
        public async Task<Stream> FulfillFileAsync(TranslateOptions options, Stream source, FileInfo fileInfo)
        {
            options.Text = ConvertToBase64String(source);
            options.Async = true;
            options.WaitAsync = true;
            var res = await FulfillAsync(options, (request) =>
            {
                request.Context.Format = request.Context.OutputFileExtension =
                    request.Context.OutputFormat = fileInfo.Extension.Replace(".", "");
                request.Context.Size = fileInfo.Length;
                var userData = new TranslateUserData
                {
                    FileName = fileInfo.Name
                };
                request.UserData = userData;
            });
            if (res.Results is not { Length: > 0 })
            {
                return null;
            }

            var fileContent = Convert.FromBase64String(res.Results[0]);
            return new MemoryStream(fileContent);
        }

        /// <inheritdoc />
        public Stream FulfillFile(TranslateOptions options, Stream source, FileInfo fileInfo)
        {
            var taskReadResult = Task.Run(async () => await FulfillFileAsync(options, source, fileInfo));
            return taskReadResult.Result;
        }

        /// <inheritdoc />
        public IList<Model> Models(string provider, IDictionary<string, string> credentials,
            IDictionary<string, string> additionalParams = null)
        {
            var taskReadResult = Task.Run(async () =>
                await ModelsAsync(provider, credentials, additionalParams));
            return taskReadResult.Result;
        }

        /// <inheritdoc />
        public async Task<IList<Model>> ModelsAsync(string providerId, IDictionary<string, string> credentials,
            IDictionary<string, string> additionalParams = null)
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
            var jsonResult = await Client.GetAsync<ModelsResponse>(path, false, additionalParams);
            return jsonResult.Models;
        }

        /// <inheritdoc />
        public IList<Account> Accounts(string providerId = null, IDictionary<string, string> additionalParams = null)
        {
            var taskReadResult = Task.Run<dynamic>(async () =>
                await AccountsAsync(providerId, additionalParams));
            return taskReadResult.Result;
        }

        /// <inheritdoc />
        public async Task<IList<Account>> AccountsAsync(string providerId = null,
            IDictionary<string, string> additionalParams = null)
        {
            var path = "accounts" + (providerId != null ? $"?provider={providerId}" : null);
            // Call to Intento API and get json result
            var jsonResult = await Client.GetAsync<AccountsResult>(path, false, additionalParams);
            return jsonResult.Result;
        }

        /// <inheritdoc />
        public IList<Routing> Routing(IDictionary<string, string> additionalParams = null)
        {
            var taskReadResult = Task.Run(async () =>
                await RoutingAsync(additionalParams));
            return taskReadResult.Result;
        }

        /// <inheritdoc />
        public async Task<IList<Routing>> RoutingAsync(IDictionary<string, string> additionalParams = null)
        {
            const string path = "ai/text/translate/routing";
            // Call to Intento API and get json result
            var jsonResult = await Client.GetAsync<RoutingResponse>(path, true, additionalParams);
            return jsonResult.Routing;
        }

        /// <inheritdoc />
        public IList<NativeGlossary> Glossaries(string providerId, string credentials,
            IDictionary<string, string> additionalParams = null)
        {
            var taskReadResult = Task.Run<dynamic>(async () =>
                await GlossariesAsync(providerId, credentials, additionalParams));
            return taskReadResult.Result;
        }

        /// <inheritdoc />
        public async Task<IList<NativeGlossary>> GlossariesAsync(string providerId, string credentials,
            IDictionary<string, string> additionalParams = null)
        {
            var path = $"ai/text/translate/glossaries?provider={providerId}";
            if (!string.IsNullOrEmpty(credentials))
            {
                path += $"&credential_id={credentials}";
            }

            // Call to Intento API and get json result
            var jsonResult = await Client.GetAsync<NativeGlossaryResult>(path, false, additionalParams);
            return jsonResult.Response;
        }

        /// <inheritdoc />
        public Provider Provider(string provider, IDictionary<string, string> additionalParams = null)
        {
            var taskReadResult = Task.Run(async () => await ProviderAsync(provider, additionalParams));
            return taskReadResult.Result;
        }

        /// <inheritdoc />
        public async Task<Provider> ProviderAsync(string providerId,
            IDictionary<string, string> additionalParams = null)
        {
            var path = $"ai/text/translate/{providerId}";
            // Call to Intento API and get json result
            var jsonResult = await Client.GetAsync<Provider>(path, true, additionalParams);
            return jsonResult;
        }

        /// <inheritdoc />
        public IList<Provider> Providers(string to = null, string @from = null, bool langDetect = false,
            bool bulk = false,
            IDictionary<string, string> filter = null)
        {
            var taskReadResult = Task.Run(async () =>
                await ProvidersAsync(to, @from, langDetect, bulk, filter));
            return taskReadResult.Result;
        }

        /// <inheritdoc />
        public async Task<IList<Provider>> ProvidersAsync(string to = null, string @from = null,
            bool langDetect = false,
            bool bulk = false,
            IDictionary<string, string> filter = null)
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

            if (langDetect)
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
            var jsonResult = await Client.GetAsync<List<Provider>>(url, true);
            var providers = new List<Provider>();
            foreach (var providerInfo in jsonResult)
            {
                providers.Add(providerInfo);
            }

            return providers;
        }

        /// <inheritdoc />
        public IList<Language> Languages()
        {
            var taskReadResult = Task.Run(async () => await LanguagesAsync());
            return taskReadResult.Result;
        }

        /// <inheritdoc />
        public async Task<IList<Language>> LanguagesAsync()
        {
            const string url = "ai/text/translate/languages";
            var jsonResult = await Client.GetAsync<IList<Language>>(url);
            return jsonResult.ToList();
        }

        /// <inheritdoc />
        public IList<Language> GetSupportedLanguages()
        {
            var taskReadResult = Task.Run(async () => await GetSupportedLanguagesAsync());
            return taskReadResult.Result;
        }

        /// <inheritdoc />
        public async Task<IList<Language>> GetSupportedLanguagesAsync()
        {
            const string url = "ai/text/transliterate/languages";
            var jsonResult = await Client.GetAsync<IList<Language>>(url);
            return jsonResult.ToList();
        }

        /// <inheritdoc />
        public LanguagePairs RoutingPairs(string srtName, IDictionary<string, string> additionalParams = null)
        {
            var taskReadResult = Task.Run(async () => await RoutingPairsAsync(srtName, additionalParams));
            return taskReadResult.Result;
        }

        /// <inheritdoc />
        public async Task<LanguagePairs> RoutingPairsAsync(string srtName, IDictionary<string, string> additionalParams = null)
        {
            var url = $"ai/text/translate/routing/{srtName}/pairs";
            var jsonResult = await Client.GetAsync<LanguagePairs>(url, true, additionalParams: additionalParams);
            return jsonResult;
        }

        /// <inheritdoc />
        public IList<IList<string>> RoutingLanguagePairs(string providerId,
            IDictionary<string, string> additionalParams = null)
        {
            var taskReadResult = Task.Run(async () => await RoutingLanguagePairsAsync(providerId, additionalParams));
            return taskReadResult.Result;
        }

        /// <inheritdoc />
        public async Task<IList<IList<string>>> RoutingLanguagePairsAsync(string providerId, IDictionary<string, string> additionalParams = null)
        {
            var routingInfo = await RoutingPairsAsync(providerId, additionalParams);
            var res = new List<IList<string>>();

            var pairs = routingInfo.Pairs;
            if (pairs == null)
            {
                return res;
            }

            res.AddRange(pairs.Select(pair => new List<string> { pair.From, pair.To }));

            return res;
        }

        /// <inheritdoc />
        public IList<IList<string>> ProviderLanguagePairs(string providerId,
            IDictionary<string, string> additionalParams = null)
        {
            var taskReadResult = Task.Run(async () => await ProviderLanguagePairsAsync(providerId, additionalParams));
            return taskReadResult.Result;
        }

        /// <inheritdoc />
        public async Task<IList<IList<string>>> ProviderLanguagePairsAsync(string providerId, IDictionary<string, string> additionalParams = null)
        {
            var providerInfo = await ProviderAsync(providerId, additionalParams);
            var res = new List<IList<string>>();
            var languages = providerInfo.Languages;
            var symmetric = languages.Symmetric;
            if (symmetric != null)
            {
                foreach (var l1 in symmetric)
                {
                    res.AddRange(from l2 in symmetric where l1 != l2 select new List<string> { l1, l2 });
                }
            }

            var pairs = languages.Pairs;
            if (pairs == null)
            {
                return res;
            }

            res.AddRange(pairs.Select(pair => new List<string> { pair.From, pair.To }));

            return res;
        }

        /// <inheritdoc />
        public IList<GlossaryDetailed> AgnosticGlossaries()
        {
            var taskReadResult = Task.Run<dynamic>(async () =>
                await AgnosticGlossariesAsync());
            return taskReadResult.Result;
        }

        /// <inheritdoc />
        public async Task<IList<GlossaryDetailed>> AgnosticGlossariesAsync()
        {
            var path = $"ai/text/glossaries/v2/typed";
            // Call to Intento API and get json result
            var jsonResult = await Client.GetAsync<AgnosticGlossariesResult>(path);
            return jsonResult.Glossaries.ToList();
        }

        /// <inheritdoc />
        public IList<AgnosticGlossaryType> AgnosticGlossariesTypes()
        {
            var taskReadResult = Task.Run<dynamic>(async () =>
                await AgnosticGlossariesTypesAsync());
            return taskReadResult.Result;
        }

        /// <inheritdoc />
        public async Task<IList<AgnosticGlossaryType>> AgnosticGlossariesTypesAsync()
        {
            var path = $"ai/text/glossaries/v2/cs_types";
            var jsonResult = await Client.GetAsync<AgnosticGlossariesTypesResult>(path);
            return jsonResult.Types.ToList();
        }

        /// <inheritdoc />
        public TranslateResponseWrapper CheckAsyncJob(string asyncId)
        {
            var task = Task.Run(async () => await CheckAsyncJobAsync(asyncId));
            return task.Result;
        }

        /// <inheritdoc />
        public async Task<TranslateResponseWrapper> CheckAsyncJobAsync(string asyncId)
        {
            Logger.LogDebug(SdkLogEvents.INVOKE_METHOD, "CheckAsyncJobAsync: {asyncId}ms", asyncId);
            // async operations inside
            var result = await Client.GetAsync<TranslateResponseWrapper>($"operations/{asyncId}");
            return result;
        }
        
        private async Task<TranslateResponseWrapper> WaitAsync(TaskCompletionSource<bool> taskCompletion,
            string asyncId, int delay = 1000)
        {
            await Task.WhenAny(taskCompletion.Task, Task.Delay(delay));
            var response = await CheckAsyncJobAsync(asyncId);
            if (response.Done)
            {
                return response;
            }

            return await WaitAsync(taskCompletion, asyncId, delay);
        }
        
        private async Task<TranslateResponseWrapper> TimeoutError()
        {
            await Task.Delay(20000);
            return new TranslateResponseWrapper
            {
                Done = true,
                Error = new Error
                {
                    Reason = "Timeout of async operation"
                }
            };
        }

        private string ConvertToBase64String(Stream stream)
        {
            var ms = new MemoryStream();
            stream.CopyTo(ms);
            return Convert.ToBase64String(ms.ToArray());
        }
    }
}