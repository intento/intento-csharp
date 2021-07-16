using System.Collections.Generic;
using IntentoSDK.Translate.Options;
using NUnit.Framework;

namespace Intento.SDK.Tests.Sources
{
    /// <summary>
    /// Data sources for TranslationServiceTests
    /// </summary>
    internal class TranslationServiceTestsDataSources
    {
        /// <summary>
        /// Get test case data for ProviderTest
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<TestCaseData> ProviderTestCaseData()
        {
            var providers = new List<string> {
                "ai.text.translate.deepl.api.v2",
                "ai.text.translate.amazon.translate",
                "ai.text.translate.yandex.cloud-translate.v2",
                "ai.text.translate.microsoft.translator_text_api.3-0"
            };
            var additionalParams = new Dictionary<string, string>
            {
                { "fields", "auth,custom_glossary" }
            };
            foreach (var provider in providers)
            {
                yield return new TestCaseData(provider, additionalParams);
            }
        }
        
        /// <summary>
        /// Get test case data for FulfillTest
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<TestCaseData> FulfillTestCaseData()
        {
            yield return new TestCaseData(new TranslateOptions
            {
                Text = "Yes",
                Async = false,
                From = "en",
                To = "es",
                Routing = "best",
                UseSyncwrapper = false
            });

            yield return new TestCaseData(new TranslateOptions
            {
                Text = "Yes",
                Async = false,
                From = "en",
                To = "es",
                Routing = "best",
                UseSyncwrapper = true
            });

            yield return new TestCaseData(new TranslateOptions
            {
                Text = new[] { "Yes", "No" },
                Async = true,
                WaitAsync = true,
                From = "fi",
                To = "en",
                Routing = "best"
            });
        }
    }
}