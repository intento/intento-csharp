using System.Collections.Generic;
using System.Linq;

namespace Intento.SDK.Handlers
{
    /// <summary>
    /// Base symbol handler
    /// </summary>
    public abstract class BaseSymbolHandler : ISymbolHandler
    {
        /// <summary>
        /// Special codes dictionary for input
        /// </summary>
        protected abstract IReadOnlyDictionary<string, string> SpecialCodesIn { get; }

        /// <summary>
        /// Special codes dictionary for output
        /// </summary>
        protected abstract IReadOnlyDictionary<string, string> SpecialCodesOut { get; }

        /// <inheritdoc />
        public abstract string Format { get; }

        /// <inheritdoc />
        public virtual string OnResponsing(string text)
        {
            return PrepareResult(text);
        }

        /// <inheritdoc />
        public virtual string OnSending(string text)
        {
            return PrepareText(text);
        }

        /// <summary>
        /// Prepare input text
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        protected virtual string PrepareText(string data)
        {
            // Remove parasite character for memoq
            data = new string(data.Where(c => (int)c != 9727).ToArray());

            // Replacing some HTML codes with special tags
            return SpecialCodesIn.Aggregate(data, (current, pair) => current.Replace(pair.Key, pair.Value));
        }

        /// <summary>
        /// Prepare result text
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        protected virtual string PrepareResult(string text)
        {
            // Return HTML codes instead of special tags

            return SpecialCodesOut.Aggregate(text, (current, pair) => current.Replace(pair.Key, pair.Value));
        }

    }
}
