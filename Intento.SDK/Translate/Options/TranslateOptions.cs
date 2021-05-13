using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Intento.SDK.Translate.DTO;
using Intento.SDK.Translate.Options;
using Intento.SDK.Validation;

namespace IntentoSDK.Translate.Options
{
    /// <summary>
    /// Options for translate
    /// </summary>
    public class TranslateOptions: BaseOptions
    {
        /// <summary>
        /// Text to translate (string or string[])
        /// </summary>
        [Required(ErrorMessage = "Text to translate can't be empty")]        
        public object Text { get; set; }

        /// <summary>
        /// Target language
        /// </summary>
        [Required(ErrorMessage = "Target language can't be empty")]
        public string To { get; set; }

        /// <summary>
        /// Source language
        /// </summary>
        public string From { get; set; }

        /// <summary>
        /// Provider for translate
        /// </summary>
        public string Provider { get; set; }

        /// <summary>
        /// Is async translate?
        /// </summary>
        public bool Async { get; set; }

        /// <summary>
        /// Should we waiting to result
        /// </summary>
        public bool WaitAsync { get; set; }

        /// <summary>
        /// Format of translate text
        /// </summary>
        public string Format { get; set; }

        public AuthProviderInfo[] Auth { get; set; }

        public string CustomModel { get; set; }

        public string Glossary { get; set; }

        public string[] PreProcessing { get; set; }

        public string[] PostProcessing { get; set; }

        public bool Failover { get; set; }

        public string[] FailoverList { get; set; }

        public string Routing { get; set; }

        public bool Trace { get; set; }

        public IDictionary<string, string> SpecialHeaders { get; set; }

        public bool UseSyncwrapper { get; set; }
    }


    
}
