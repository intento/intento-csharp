using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntentoSDK.API
{
    /// <summary>
    /// Base provider to connecto to API
    /// </summary>
    public abstract class BaseAPIClient
    {
        protected string filePath;

        /// <summary>
        /// Uid of client
        /// </summary>
        public abstract Guid ClientUid { get; }

        /// <summary>
        /// Display name of client API provider
        /// </summary>
        public abstract string DisplayName { get; }

        /// <summary>
        /// Translate text
        /// </summary>
        /// <param name="intento"></param>
        /// <param name="param"></param>
        /// <param name="trace"></param>
        /// <returns></returns>
        public abstract Task<dynamic> Translate(Intento intento, object text, string to, string from = null, string provider = null,
            bool async = false, bool wait_async = false, string format = null, object auth = null,
            string custom_model = null, string glossary = null,
            object pre_processing = null, object post_processing = null,
            bool failover = false, object failover_list = null, string routing = null, bool trace = false,
            Dictionary<string, string> special_headers = null);

        /// <summary>
        /// Init API Client
        /// </summary>
        /// <param name="intento"></param>
        public virtual bool Init(string filePath)
        {
            if (this.filePath != filePath)
            { 
                this.filePath = filePath;
                return true;
            }
            return false;
        }
    }
}
