namespace Intento.SDK.Handlers
{
    /// <summary>
    /// Factory for handlers
    /// </summary>
    public interface ISymbolHandlersFactory
    {
        /// <summary>
        /// Prepare source
        /// </summary>
        /// <param name="text"></param>
        /// <param name="format"></param>
        /// <returns></returns>
        string HandleSource(string text, string format);
        
        /// <summary>
        /// Prepare result
        /// </summary>
        /// <param name="text"></param>
        /// <param name="format"></param>
        /// <returns></returns>
        string HandleResult(string text, string format);
    }
}