using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Intento.SDK.Handlers;

namespace IntentoSDK.Handlers
{
    /// <summary>
    /// Factory for handlers
    /// </summary>
    internal class SymbolHandlersFactory: ISymbolHandlersFactory
    {
        private readonly ISymbolHandler[] symbolHandlers;

        /// <summary>
        /// Ctor
        /// </summary>
        public SymbolHandlersFactory(ISymbolHandler[] symbolHandlers)
        {
            this.symbolHandlers = symbolHandlers;
        }

        /// <summary>
        /// Prepare source
        /// </summary>
        /// <param name="text"></param>
        /// <param name="format"></param>
        /// <returns></returns>
        public string HandleSource(string text, string format)
        {
            foreach (var handler in symbolHandlers.Where(h => h.Format == format))
            {
                text = handler.OnSending(text);
            }
            return text;
        }

        /// <summary>
        /// Prepare result
        /// </summary>
        /// <param name="text"></param>
        /// <param name="format"></param>
        /// <returns></returns>
        public string HandleResult(string text, string format)
        {
            foreach (var handler in symbolHandlers.Where(h => h.Format == format))
            {
                text = handler.OnResponsing(text);
            }

            return text;
        }

    }
}
