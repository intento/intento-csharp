using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntentoSDK.Handlers
{
    /// <summary>
    /// Factory for handlers
    /// </summary>
    public class SymbolHandlersFactory
    {
        private readonly ISymbolHandler[] symbolHandlers;

        /// <summary>
        /// Ctor
        /// </summary>
        public SymbolHandlersFactory()
        {
            symbolHandlers = new ISymbolHandler[] {
                new XmlSymbolHandler(),
                new HtmlSymbolHandler()
            };
        }

        /// <summary>
        /// Prepare source
        /// </summary>
        /// <param name="text"></param>
        /// <param name="format"></param>
        /// <returns></returns>
        public string HandleSource(string text, string format)
        {
            foreach (ISymbolHandler handler in symbolHandlers.Where(h => h.Format == format))
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
            foreach (ISymbolHandler handler in symbolHandlers.Where(h => h.Format == format))
            {
                text = handler.OnResponsing(text);
            }
            return text;
        }

        private static readonly SymbolHandlersFactory current = new SymbolHandlersFactory();

        /// <summary>
        /// Current instance of factory
        /// </summary>
        public static SymbolHandlersFactory Current => current;
        
    }
}
