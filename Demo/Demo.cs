using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IntentoSDK;

namespace Demo
{

    public static class Demo
    {
        public const string API_KEY = "muGX5Bi8olr2m40WA2IPv8pEz4WNs19i";

        public static string TranslateSync (string text, string from="en", string to="zh", string provider = null)
        {
            var intento = IntentoSDK.Intento.Create(API_KEY);
            var translate = intento.Ai.Text.Translate;
            dynamic result = translate.Fulfill(text, to, from: from, provider: provider);
            return (string)result.results[0]; 
        }

        public static List<string> TranslateSyncList(List<string> text, string from = "en", string to = "zh", string provider = null)
        {
            var intento = IntentoSDK.Intento.Create(API_KEY);
            var translate = intento.Ai.Text.Translate;
            dynamic result = translate.Fulfill(text, to, from: from, provider: provider);
            IEnumerable<string> res = result.results.Values<string>();
            return res.ToList();
        }

        public static IEnumerable<string> ProviderNames()
        {
            var translate = IntentoSDK.Intento.Create(API_KEY).Ai.Text.Translate;
            IList<dynamic> providers_raw = translate.Providers();
            var provider_names = providers_raw.Select(i => (string)i.name);
            return provider_names;
        }

    }
}
