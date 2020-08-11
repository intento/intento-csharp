using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IntentoSDK;

namespace Demo
{

    public class Demo
    {
        Intento intento;
        IntentoAiTextTranslate translate;

        public Demo(string apikey)
        {
            intento = Intento.Create(apikey);
            translate = intento.Ai.Text.Translate;
        }

        /// <summary>
        /// Translation of one text segment with waiting for the result
        /// </summary>
        /// <param name="text">text to translate</param>
        /// <param name="from">from language code</param>
        /// <param name="to">to language code</param>
        /// <param name="provider">provider id</param>
        /// <returns></returns>
        public string TranslateAsyncAndWaitResult(string text, string from="en", string to="zh", string provider=null)
        {
            dynamic result = translate.Fulfill(text, to, from: from, provider: provider, async:true, wait_async:true);
            if (result.error != null)
            {
                // Error processing
            }
            return (string)result.response[0].results[0]; 
        }

        /// <summary>
        /// Translation of a list of text segments with waiting for the result
        /// </summary>
        /// <param name="text">text to translate</param>
        /// <param name="from">from language code</param>
        /// <param name="to">to language code</param>
        /// <param name="provider">provider id</param>
        /// <returns></returns>
        public List<string> TranslateAsyncListAndWaitResult(List<string> text, string from = "en", string to = "zh", string provider = "ai.text.translate.google.translate_api.v3")
        {
            dynamic result = translate.Fulfill(text, to, from: from, provider: provider, async: true, wait_async: true);
            if (result.error != null)
            {
                // Error processing
            }
            IEnumerable<string> res = result.response[0].results.Values<string>();
            return res.ToList();
        }

        /// <summary>
        /// Translation of one segment and return ID of the translation task
        /// </summary>
        /// <param name="text">text to translate</param>
        /// <param name="from">from language code</param>
        /// <param name="to">to language code</param>
        /// <param name="provider">provider id</param>
        /// <returns></returns>
        public string TranslateAsync(string text, string from = "en", string to = "zh", string provider=null)
        {
            dynamic result = translate.Fulfill(text, to, from: from, provider: provider, async: true, wait_async: false);
            return (string)result.id;
        }

        /// <summary>
        /// Wait for the results of the asynchronous translation job
        /// </summary>
        /// <param name="text">text to translate</param>
        /// <param name="from">from language code</param>
        /// <param name="to">to language code</param>
        /// <param name="provider">provider id</param>
        /// <returns></returns>
        public List<string> WaitAsyncJob(string job_id, int delay)
        {
            dynamic result = intento.WaitAsyncJob(job_id, delay: delay);
            if (result.done != null)
            {
                bool done = (bool)result.done;
                if (!done)
                    // Result not ready yet
                    return null;
            }


            if (result.error != null)
            {
                // Error processing
            }
            IEnumerable<string> res = result.response[0].results.Values<string>();
            return res.ToList();
        }

        public IEnumerable<string> ProviderNames()
        {
            IList<dynamic> providers_raw = translate.Providers();
            var provider_names = providers_raw.Select(i => (string)i.name);
            return provider_names;
        }

        public static void Main(string[] args)
        {
            // In this Demo, Intento ApiKey is taken from the environment
            Demo demo = new Demo(Environment.GetEnvironmentVariable("APIKEY"));

            // Simplest use with one segment
            string result1 = demo.TranslateAsyncAndWaitResult("Translation example");

            // Several segments with the indication of the language pair and provider
            List<string> result2 = demo.TranslateAsyncListAndWaitResult(new List<string> { "Translation example", "One more example" }, to:"ru", provider: "ai.text.translate.deepl.api");

            // Own waiting cycle. Please do not call WaitAsyncJob more than once every 2-3 seconds. 
            // It is recommended to specify 0 in the delay parameter.
            string job_id = demo.TranslateAsync("Asynchronous translation");
            List<string> res;
            while ((res = demo.WaitAsyncJob(job_id, -1)) == null)
            { }

        }


    }

}
