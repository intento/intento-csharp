using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntentoSDK.API
{
    /// <summary>
    /// Client to connect to Intento API
    /// </summary>
    public class RemoteAPIClient : BaseAPIClient
    {
        public static Guid uid = new Guid("{BCB8A56A-BB06-44F7-9888-A7A9A719FA93}");

        ///<inheritdoc/>
        public override Guid ClientUid => uid;

        ///<inheritdoc/>
        public override async Task<dynamic> Translate(Intento intento, dynamic param, bool trace = false)
        {
            dynamic jsonResult;
            string url = "ai/text/translate";
            if (trace)
                url += "?trace=true";

            // Call to Intento API and get json result
            using (HttpConnector conn = new HttpConnector(intento))
            {
                jsonResult = await conn.PostAsync(url, param);
            }
            return jsonResult;
        }
    }
}
