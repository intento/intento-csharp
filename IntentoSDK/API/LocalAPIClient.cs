using Newtonsoft.Json.Linq;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace IntentoSDK.API
{
    /// <summary>
    /// Translate from file
    /// </summary>
    public class LocalAPIClient : BaseAPIClient
    {
        private ConcurrentDictionary<string, string> translatePairs;
        public static Guid uid = new Guid("{32290946-0ED1-4F4D-97D8-4CDD2690C4D3}");

        ///<inheritdoc/>
        public override Guid ClientUid => uid;

        ///<inheritdoc/>
        public override string DisplayName => "Translate from file";

        ///<inheritdoc/>
        public override bool Init(string filePath)
        {
            if (string.IsNullOrEmpty(filePath))
            {
                return false;
            }
            if (!base.Init(filePath))
            {
                return false;
            }
            var json = File.ReadAllText(filePath);
            translatePairs = new ConcurrentDictionary<string, string>();
            dynamic translateData = JObject.Parse(json);
            foreach (dynamic data in translateData.data)
            {
                var key = (string)data[1];
                var value = (string)data[2];
                if (!translatePairs.ContainsKey(key))
                {
                    translatePairs.TryAdd(key, value);
                }
            }
            return true;
        }

        ///<inheritdoc/>
        public override Task<dynamic> Translate(Intento intento, object text, string to, string from = null, string provider = null,
            bool async = false, bool wait_async = false, string format = null, object auth = null,
            string custom_model = null, string glossary = null,
            object pre_processing = null, object post_processing = null,
            bool failover = false, object failover_list = null, string routing = null, bool trace = false,
            Dictionary<string, string> special_headers = null)
        {           
            dynamic jsonResult = new ExpandoObject();
            dynamic response = new ExpandoObject();
            jsonResult.response = response;
            dynamic first = new ExpandoObject();
            response.First = first;
            if (text == null || translatePairs == null)
            {
                first.results = new string[] { "" };               
            }                
            else if (text is IEnumerable<string>)
            {
                first.results = 
                  ((IEnumerable<string>)text).Select(i => i == null ? "" : i).Select(i => {
                        var key = PrepareKey(i.ToString());
                        string tran;
                        translatePairs.TryGetValue(key, out tran);
                        return !string.IsNullOrEmpty(tran) ? tran : $"No translation in dictionary for: {key}";
                    }).ToArray();                 
            }
            else
            { 
                var key = PrepareKey(text.ToString());
                string translation;
                if (translatePairs.TryGetValue(key, out translation))
                {
                    first.results = new string[] { !string.IsNullOrEmpty(translation) ? translation : $"No translation in dictionary for: {key}" };
                }                
            }
            return Task.FromResult<dynamic>(jsonResult);
        }

        private string PrepareKey(string key)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                return "";
            }
            return HttpUtility.HtmlDecode(key);
        }

    }
}
