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
        protected Intento intento;

        /// <summary>
        /// Уникальный идентификатор клиента
        /// </summary>
        public abstract Guid ClientUid { get; }

        /// <summary>
        /// Translate text
        /// </summary>
        /// <param name="intento"></param>
        /// <param name="param"></param>
        /// <param name="trace"></param>
        /// <returns></returns>
        public abstract Task<dynamic> Translate(Intento intento, dynamic param, bool trace = false);

        /// <summary>
        /// Init API Client
        /// </summary>
        /// <param name="intento"></param>
        public virtual void Init(Intento intento)
        {
            this.intento = intento;
        }
    }
}
