namespace Intento.SDK.Handlers
{
    /// <summary>
    /// Process text before and after sending
    /// </summary>
    public interface ISymbolHandler
    {
        /// <summary>
        /// Type of text format
        /// </summary>
        string Format { get; }

        /// <summary>
        /// Prepare text before send
        /// </summary>
        /// <param name="text">Text</param>
        /// <returns></returns>
        string OnSending(string text);

        /// <summary>
        /// Prepare text after send
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        string OnResponsing(string text);
    }
}
