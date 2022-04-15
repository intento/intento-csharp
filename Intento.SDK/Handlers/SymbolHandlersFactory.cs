using System.Linq;

namespace Intento.SDK.Handlers
{
    /// <summary>
    /// Factory for handlers
    /// </summary>
    internal class SymbolHandlersFactory: ISymbolHandlersFactory
    {
        private readonly ISymbolHandler[] _symbolHandlers;

        /// <summary>
        /// Ctor
        /// </summary>
        public SymbolHandlersFactory(ISymbolHandler[] symbolHandlers)
        {
            this._symbolHandlers = symbolHandlers;
        }

        /// <summary>
        /// Prepare source
        /// </summary>
        /// <param name="text"></param>
        /// <param name="format"></param>
        /// <returns></returns>
        public string HandleSource(string text, string format)
        {
            return _symbolHandlers.Where(h => h.Format == format)
                .Aggregate(text, (current, handler) => handler.OnSending(current));
        }

        /// <summary>
        /// Prepare result
        /// </summary>
        /// <param name="text"></param>
        /// <param name="format"></param>
        /// <returns></returns>
        public string HandleResult(string text, string format)
        {
            return _symbolHandlers.Where(h => h.Format == format)
                .Aggregate(text, (current, handler) => handler.OnResponsing(current));
        }

    }
}
