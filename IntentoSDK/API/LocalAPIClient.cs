using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntentoSDK.API
{
    /// <summary>
    /// Translate from file
    /// </summary>
    public class LocalAPIClient : BaseAPIClient
    {
        private ConcurrentDictionary<string, object> translatePairs;
        public static Guid uid = new Guid("{32290946-0ED1-4F4D-97D8-4CDD2690C4D3}");

        ///<inheritdoc/>
        public override Guid ClientUid => uid;

        ///<inheritdoc/>
        public override void Init(Intento intento)
        {
            base.Init(intento);
            if (intento.ExtendedSettings == null)
            {
                return;
            }
            object dict;
            if (intento.ExtendedSettings.TryGetValue(uid.ToString(), out dict))
            {
                translatePairs = new ConcurrentDictionary<string, object>((Dictionary<string, object>)dict);
            }
        }

        ///<inheritdoc/>
        public override Task<dynamic> Translate(Intento intento, dynamic param, bool trace = false)
        {
            if (translatePairs == null)
            {
                return null;
            }
            return null;
        }


    }
}
