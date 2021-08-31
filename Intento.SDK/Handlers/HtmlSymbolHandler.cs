using System;
using System.Collections.Generic;

namespace Intento.SDK.Handlers
{
    /// <summary>
    /// Prepare HTML text
    /// </summary>
    internal class HtmlSymbolHandler : BaseSymbolHandler
    {
        protected override IReadOnlyDictionary<string, string> SpecialCodesIn => new Dictionary<string, string>();

        protected override IReadOnlyDictionary<string, string> SpecialCodesOut => 
            new Dictionary<string, string> 
            {
                { "&lt;", "<" },
                { "&gt;", ">" }
            };

        public override string Format => "html";

        protected override string PrepareResult(string text)
        {
            text = base.PrepareResult(text);

            // Remove <meta> and </meta> tags
            int n1 = text.IndexOf("<meta", StringComparison.Ordinal);
            string text2 = text;
            if (n1 != -1)
            {
                int n2 = text.IndexOf(">", StringComparison.Ordinal);
                text2 = text.Substring(n2 + 1);
            }
            return text2;
        }
    }
}
