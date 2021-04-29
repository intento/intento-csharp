using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntentoSDK.Handlers
{
    /// <summary>
    /// Base symbol handler
    /// </summary>
    public abstract class BaseSymbolHandler : ISymbolHandler
    {
        protected abstract IReadOnlyDictionary<string, string> SpecialCodesIn { get; }

        protected abstract IReadOnlyDictionary<string, string> SpecialCodesOut { get; }

        public abstract string Format { get; }

        public virtual string OnResponsing(string text)
        {
            return PrepareResult(text);
        }

        public virtual string OnSending(string text)
        {
            return PrepareText(text);
        }

        protected virtual string PrepareText(string data)
        {
            // Remove parasite character for memoq
            data = new string(data.Where(c => (int)c != 9727).ToArray());

            // Replacing some HTML codes with special tags
            foreach (KeyValuePair<string, string> pair in SpecialCodesIn)
            {
                data = data.Replace(pair.Key, pair.Value);
            }            
            return data;
        }

        protected virtual string PrepareResult(string text)
        {
            // Return HTML codes instead of special tags
            foreach (KeyValuePair<string, string> pair in SpecialCodesOut)
            {
                text = text.Replace(pair.Key, pair.Value);
            }
            
            return text;
        }

    }
}
