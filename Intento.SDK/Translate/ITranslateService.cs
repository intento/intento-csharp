﻿using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Intento.SDK.Exceptions;
using Intento.SDK.Translate.DTO;
using Intento.SDK.Translate.Options;

namespace Intento.SDK.Translate
{
    /// <summary>
    /// Service to call Intento API
    /// </summary>
    public interface ITranslateService
    {
        /// <summary>
        /// Do translate by Intento
        /// </summary>
        /// <param name="options">Options for translate</param>
        /// <returns></returns>
        /// <exception cref="IntentoInvalidParameterException"></exception>
        Task<TranslateResponse> FulfillAsync(TranslateOptions options);

        /// <summary>
        ///  Do translate by Intento
        /// </summary>
        /// <param name="options">Options for translate</param>
        /// <returns></returns>
        TranslateResponse Fulfill(TranslateOptions options);

        /// <summary>
        /// Translate file
        /// </summary>
        /// <param name="sourcePath"></param>
        /// <param name="options"></param>
        /// <param name="outputPath"></param>
        /// <returns></returns>
        Task FulfillFileAsync(TranslateOptions options, string sourcePath, string outputPath);
        
        /// <summary>
        /// Translate file
        /// </summary>
        /// <param name="sourcePath"></param>
        /// <param name="options"></param>
        /// <param name="outputPath"></param>
        /// <returns></returns>
        void FulfillFile(TranslateOptions options, string sourcePath, string outputPath);

        /// <summary>
        /// Translate file
        /// </summary>
        /// <param name="options"></param>
        /// <param name="source"></param>
        /// <param name="fileInfo"></param>
        /// <returns></returns>
        Task<Stream> FulfillFileAsync(TranslateOptions options, Stream source, FileInfo fileInfo);
        
        /// <summary>
        /// Translate file
        /// </summary>
        /// <param name="options"></param>
        /// <param name="source"></param>
        /// <param name="fileInfo"></param>
        /// <returns></returns>
        Stream FulfillFile(TranslateOptions options, Stream source, FileInfo fileInfo);

        /// <summary>
        /// Details of the models stored by the provider
        /// </summary>
        /// <param name="provider">Provider id</param>
        /// <param name="credentials">Credential id</param>
        /// <param name="additionalParams">additional url params</param>
        /// <returns>dynamic (json) with requested information</returns>
        IList<Model> Models(string provider, IDictionary<string, string> credentials,
            IDictionary<string, string> additionalParams = null);

        /// <summary>
        /// Details of the models stored by the provider
        /// </summary>
        /// <param name="providerId">Provider id</param>
        /// <param name="credentials">Credential id</param>
        /// <param name="additionalParams">additional url params</param>
        /// <returns>dynamic (json) with requested information</returns>
        Task<IList<Model>> ModelsAsync(string providerId, IDictionary<string, string> credentials,
            IDictionary<string, string> additionalParams = null);

        /// <summary>
        /// Details of the models stored by the provider
        /// </summary>
        /// <param name="providerId">Provider id</param>
        /// <param name="additionalParams">additional url params</param>
        /// <returns>dynamic (json) with requested information</returns>
        IList<Account> Accounts(string providerId = null, IDictionary<string, string> additionalParams = null);

        /// <summary>
        /// Details of the models stored by the provider
        /// </summary>
        /// <param name="providerId">Provider id</param>
        /// <param name="additionalParams">additional url params</param>
        /// <returns>dynamic (json) with requested information</returns>
        Task<IList<Account>> AccountsAsync(string providerId = null,
            IDictionary<string, string> additionalParams = null);

        /// <summary>
        /// Details of the models stored by the provider
        /// </summary>
        /// <param name="additionalParams">additional url params</param>
        /// <returns>dynamic (json) with requested information</returns>
        IList<Routing> Routing(IDictionary<string, string> additionalParams = null);

        /// <summary>
        /// Details of the models stored by the provider
        /// </summary>
        /// <param name="additionalParams">additional url params</param>
        /// <returns>dynamic (json) with requested information</returns>
        Task<IList<Routing>> RoutingAsync(IDictionary<string, string> additionalParams = null);

        /// <summary>
        /// Details of the glossaries stored by the provider
        /// </summary>
        /// <param name="provider">Provider id</param>
        /// <param name="credentials">Credential id</param>
        /// <param name="additionalParams">additional url params</param>
        /// <returns>dynamic (json) with requested information</returns>
        IList<NativeGlossary> Glossaries(string provider, string credentials,
            IDictionary<string, string> additionalParams = null);

        /// <summary>
        /// Details of the glossaries stored by the provider
        /// </summary>
        /// <param name="providerId">Provider id</param>
        /// <param name="credentials">Credential id</param>
        /// <param name="additionalParams">additional url params</param>
        /// <returns>dynamic (json) with requested information</returns>
        Task<IList<NativeGlossary>> GlossariesAsync(string providerId, string credentials,
            IDictionary<string, string> additionalParams = null);

        /// <summary>
        /// Detailed information on provider features
        /// </summary>
        /// <param name="provider">Provider id</param>
        /// <param name="additionalParams">additional url params</param>
        /// <returns>dynamic (json) with requested information</returns>
        Provider Provider(string provider, IDictionary<string, string> additionalParams = null);

        /// <summary>
        /// Detailed information on provider features
        /// </summary>
        /// <param name="providerId">Provider id</param>
        /// <param name="additionalParams">Additional params</param>
        /// <returns>dynamic (json) with requested information</returns>
        Task<Provider> ProviderAsync(string providerId, IDictionary<string, string> additionalParams = null);

        /// <summary>
        /// Get providers by options
        /// </summary>
        /// <param name="to">Target language</param>
        /// <param name="from">Source language</param>
        /// <param name="langDetect"></param>
        /// <param name="bulk">Bulk executions</param>
        /// <param name="filter">Filter option</param>
        /// <returns></returns>
        IList<Provider> Providers(string to = null, string from = null, bool langDetect = false, bool bulk = false,
            IDictionary<string, string> filter = null);

        /// <summary>
        /// Get providers by options
        /// </summary>
        /// <param name="to">Target language</param>
        /// <param name="from">Source language</param>
        /// <param name="langDetect"></param>
        /// <param name="bulk">Bulk executions</param>
        /// <param name="filter">Filter option</param>
        /// <returns></returns>
        Task<IList<Provider>> ProvidersAsync(string to = null, string from = null, bool langDetect = false,
            bool bulk = false,
            IDictionary<string, string> filter = null);

        /// <summary>
        /// Get languages list
        /// </summary>
        /// <returns></returns>
        IList<Language> Languages();

        /// <summary>
        /// Get languages list
        /// </summary>
        /// <returns></returns>
        Task<IList<Language>> LanguagesAsync();

        /// <summary>
        /// Get languages list
        /// </summary>
        /// <returns></returns>
        IList<Language> GetSupportedLanguages();

        /// <summary>
        /// Get supported language list
        /// </summary>
        /// <returns></returns>
        Task<IList<Language>> GetSupportedLanguagesAsync();

        /// <summary>
        /// Get routes language pairs
        /// </summary>
        /// <param name="srtName">Source name</param>
        /// <param name="additionalParams"></param>
        /// <returns></returns>
        LanguagePairs RoutingPairs(string srtName, IDictionary<string, string> additionalParams = null);

        /// <summary>
        /// Get routes language pairs
        /// </summary>
        /// <param name="srtName">Source name</param>
        /// <param name="additionalParams"></param>
        /// <returns></returns>
        Task<LanguagePairs> RoutingPairsAsync(string srtName, IDictionary<string, string> additionalParams = null);

        /// <summary>
        /// Get routing language pairs
        /// </summary>
        /// <param name="providerId">Provider identifier</param>
        /// <param name="additionalParams"></param>
        /// <returns></returns>
        IList<IList<string>> RoutingLanguagePairs(string providerId, IDictionary<string, string> additionalParams = null);

        /// <summary>
        /// Get routing language pairs
        /// </summary>
        /// <param name="providerId">Provider identifier</param>
        /// <param name="additionalParams"></param>
        /// <returns></returns>        
        Task<IList<IList<string>>> RoutingLanguagePairsAsync(string providerId, IDictionary<string, string> additionalParams = null);

        /// <summary>
        /// Get providers language pairs
        /// </summary>
        /// <param name="providerId">Provider identifier</param>
        /// <param name="additionalParams"></param>
        /// <returns></returns>
        IList<IList<string>> ProviderLanguagePairs(string providerId, IDictionary<string, string> additionalParams = null);

        /// <summary>
        /// Get providers language pairs
        /// </summary>
        /// <param name="providerId">Provider identifier</param>
        /// <param name="additionalParams"></param>
        /// <returns></returns>
        Task<IList<IList<string>>> ProviderLanguagePairsAsync(string providerId, IDictionary<string, string> additionalParams = null);

        /// <summary>
        /// List of glossaries
        /// </summary>
        /// <returns></returns>
        IList<GlossaryDetailed> AgnosticGlossaries();

        /// <summary>
        /// List of glossaries
        /// </summary>
        /// <returns></returns>
        Task<IList<GlossaryDetailed>> AgnosticGlossariesAsync();

        /// <summary>
        /// Glossaries types
        /// </summary>
        /// <returns></returns>
        IList<AgnosticGlossaryType> AgnosticGlossariesTypes();

        /// <summary>
        /// Glossaries types
        /// </summary>
        /// <returns></returns>
        Task<IList<AgnosticGlossaryType>> AgnosticGlossariesTypesAsync();

        /// <summary>
        /// Check operation status
        /// </summary>
        /// <param name="asyncId">Id of operation</param>
        /// <returns></returns>
        TranslateResponseWrapper CheckAsyncJob(string asyncId);

        /// <summary>
        /// Check operation status
        /// </summary>
        /// <param name="asyncId">Id of operation</param>
        /// <returns></returns>
        Task<TranslateResponseWrapper> CheckAsyncJobAsync(string asyncId);
    }
}