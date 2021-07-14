using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Intento.SDK.Translate.DTO;
using IntentoSDK.Translate.Options;

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
        Task<TranslateResponseDto> FulfillAsync(TranslateOptions options);

        /// <summary>
        ///  Do translate by Intento
        /// </summary>
        /// <param name="options">Options for translate</param>
        /// <returns></returns>
        TranslateResponseDto Fulfill(TranslateOptions options);

        /// <summary>
        /// Details of the models stored by the provider
        /// </summary>
        /// <param name="provider">Provider id</param>
        /// <param name="credential_id">Credential id</param>
        /// <param name="additionalParams">additional url params</param>
        /// <returns>dynamic (json) with requested information</returns>
        IList<ModelDto> Models(string provider, Dictionary<string, string> credentials,
            Dictionary<string, string> additionalParams = null);

        /// <summary>
        /// Details of the models stored by the provider
        /// </summary>
        /// <param name="provider">Provider id</param>
        /// <param name="credential_id">Credential id</param>
        /// <param name="additionalParams">additional url params</param>
        /// <returns>dynamic (json) with requested information</returns>
        Task<IList<ModelDto>> ModelsAsync(string providerId, Dictionary<string, string> credentials,
            Dictionary<string, string> additionalParams = null);

        /// <summary>
        /// Details of the models stored by the provider
        /// </summary>
        /// <param name="providerId">Provider id</param>
        /// <param name="additionalParams">additional url params</param>
        /// <returns>dynamic (json) with requested information</returns>
        IList<dynamic> Accounts(string providerId = null, Dictionary<string, string> additionalParams = null);

        /// <summary>
        /// Details of the models stored by the provider
        /// </summary>
        /// <param name="provider">Provider id</param>
        /// <param name="additionalParams">additional url params</param>
        /// <returns>dynamic (json) with requested information</returns>
        Task<IList<dynamic>> AccountsAsync(string providerId = null,
            Dictionary<string, string> additionalParams = null);

        /// <summary>
        /// Details of the models stored by the provider
        /// </summary>
        /// <param name="providerId">Provider id</param>
        /// <param name="additionalParams">additional url params</param>
        /// <returns>dynamic (json) with requested information</returns>
        IList<RoutingDto> Routing(Dictionary<string, string> additionalParams = null);

        /// <summary>
        /// Details of the models stored by the provider
        /// </summary>
        /// <param name="provider">Provider id</param>
        /// <param name="additionalParams">additional url params</param>
        /// <returns>dynamic (json) with requested information</returns>
        Task<IList<RoutingDto>> RoutingAsync(Dictionary<string, string> additionalParams = null);

        /// <summary>
        /// Details of the glossaries stored by the provider
        /// </summary>
        /// <param name="provider">Provider id</param>
        /// <param name="credential_id">Credential id</param>
        /// <param name="additionalParams">additional url params</param>
        /// <returns>dynamic (json) with requested information</returns>
        IList<dynamic> Glossaries(string provider, Dictionary<string, string> credentials,
            Dictionary<string, string> additionalParams = null);

        /// <summary>
        /// Details of the glossaries stored by the provider
        /// </summary>
        /// <param name="provider">Provider id</param>
        /// <param name="credential_id">Credential id</param>
        /// <param name="additionalParams">additional url params</param>
        /// <returns>dynamic (json) with requested information</returns>
        Task<IList<dynamic>> GlossariesAsync(string providerId, Dictionary<string, string> credentials,
            Dictionary<string, string> additionalParams = null);

        /// <summary>
        /// Get a list of available delegated credentials
        /// </summary>
        /// <param name="additionalParams">additional url params</param>
        /// <returns>dynamic (json) with requested information</returns>
        [Obsolete]
        IList<dynamic> DelegatedCredentials(Dictionary<string, string> additionalParams = null);

        /// <summary>
        /// Get a list of available delegated credentials
        /// </summary>
        /// <param name="additionalParams">additional url params</param>
        /// <returns>dynamic (json) with requested information</returns>
        [Obsolete]
        Task<IList<dynamic>> DelegatedCredentialsAsync(Dictionary<string, string> additionalParams = null);

        /// <summary>
        /// Detailed information on provider features
        /// </summary>
        /// <param name="provider">Provider id</param>
        /// <param name="additionalParams">additional url params</param>
        /// <returns>dynamic (json) with requested information</returns>
        ProviderDetailedDto Provider(string provider, Dictionary<string, string> additionalParams = null);

        /// <summary>
        /// Detailed information on provider features
        /// </summary>
        /// <param name="providerId">Provider id</param>
        /// <param name="additionalParams">Additional params</param>
        /// <returns>dynamic (json) with requested information</returns>
        Task<ProviderDetailedDto> ProviderAsync(string providerId, Dictionary<string, string> additionalParams = null);

        /// <summary>
        /// Get providers by options
        /// </summary>
        /// <param name="to">Target language</param>
        /// <param name="from">Source language</param>
        /// <param name="lang_detect"></param>
        /// <param name="bulk">Bulk executions</param>
        /// <param name="filter">Filter option</param>
        /// <returns></returns>
        IList<ProviderDto> Providers(string to = null, string from = null, bool lang_detect = false, bool bulk = false,
            Dictionary<string, string> filter = null);

        /// <summary>
        /// Get providers by options
        /// </summary>
        /// <param name="to">Target language</param>
        /// <param name="from">Source language</param>
        /// <param name="lang_detect"></param>
        /// <param name="bulk">Bulk executions</param>
        /// <param name="filter">Filter option</param>
        /// <returns></returns>
        Task<IList<ProviderDto>> ProvidersAsync(string to = null, string from = null, bool lang_detect = false,
            bool bulk = false,
            Dictionary<string, string> filter = null);

        /// <summary>
        /// Get languages list
        /// </summary>
        /// <returns></returns>
        IList<LanguageDto> Languages();

        /// <summary>
        /// Get languages list
        /// </summary>
        /// <returns></returns>
        Task<IList<LanguageDto>> LanguagesAsync();

        /// <summary>
        /// Get languages list
        /// </summary>
        /// <returns></returns>
        IList<LanguageDto> GetSupportedLanguages();
        
        /// <summary>
        /// Get supported language list
        /// </summary>
        /// <returns></returns>
        Task<IList<LanguageDto>> GetSupportedLanguagesAsync();
        
        /// <summary>
        /// Get language pairs
        /// </summary>
        /// <param name="srtName">Source name</param>
        /// <returns></returns>
        LanguagePairs Pairs(string srtName);

        /// <summary>
        /// Get language pairs
        /// </summary>
        /// <param name="srtName">Source name</param>
        /// <returns></returns>
        Task<LanguagePairs> PairsAsync(string srtName);

        /// <summary>
        /// Get routing language pairs
        /// </summary>
        /// <param name="providerId">Provider identifier</param>
        /// <returns></returns>
        IList<IList<string>> RoutingLanguagePairs(string providerId);

        /// <summary>
        /// Get routing language pairs
        /// </summary>
        /// <param name="providerId">Provider identifier</param>
        /// <returns></returns>        
        Task<IList<IList<string>>> RoutingLanguagePairsAsync(string providerId);

        /// <summary>
        /// Get providers language pairs
        /// </summary>
        /// <param name="providerId">Provider identifier</param>
        /// <returns></returns>
        IList<IList<string>> ProviderLanguagePairs(string providerId);

        /// <summary>
        /// Get providers language pairs
        /// </summary>
        /// <param name="providerId">Provider identifier</param>
        /// <returns></returns>
        Task<IList<IList<string>>> ProviderLanguagePairsAsync(string providerId);

        /// <summary>
        /// List of glossaries
        /// </summary>
        /// <returns></returns>
        IList<dynamic> AgnosticGlossaries();

        /// <summary>
        /// List of glossaries
        /// </summary>
        /// <returns></returns>
        Task<IList<dynamic>> AgnosticGlossariesAsync();

        /// <summary>
        /// Glossaries types
        /// </summary>
        /// <returns></returns>
        IList<dynamic> AgnosticGlossariesTypes();

        /// <summary>
        /// Glossaries types
        /// </summary>
        /// <returns></returns>
        Task<IList<dynamic>> AgnosticGlossariesTypesAsync();
    }
}
