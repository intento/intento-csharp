using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Intento.SDK.DI;
using Intento.SDK.Tests.Sources;
using Intento.SDK.Tests.Utils;
using Intento.SDK.Translate;
using Intento.SDK.Translate.Options;
using NUnit.Framework;

namespace Intento.SDK.Tests
{
    /// <summary>
    /// Tests for TranslationService
    /// </summary>
    [TestFixture(Category = "Translate API")]
    [Parallelizable(ParallelScope.Self)]
    public class TranslationServiceTests : IntentoTests
    {
        [Test(Description = "Get all list of languages")]
        public async Task LanguageTest()
        {
            var service = Locator.Resolve<ITranslateService>();
            var res = await service.LanguagesAsync();
            Assert.NotNull(res);
            Assert.IsNotEmpty(res);
        }

        [Test(Description = "Get all supported list of languages")]
        public async Task GetSupportedLanguagesTest()
        {
            var service = Locator.Resolve<ITranslateService>();
            var res = await service.GetSupportedLanguagesAsync();
            Assert.NotNull(res);
            Assert.IsNotEmpty(res);
        }

        [Test(Description = "Get language pairs for provider")]
        [TestCase("ai.text.translate.globalese.translate")]
        [TestCase("ai.text.translate.tilde.machine_translation_api")]
        public async Task ProviderLanguagePairsTest(string providerId)
        {
            var service = Locator.Resolve<ITranslateService>();
            var res = await service.ProviderLanguagePairsAsync(providerId);
            Assert.NotNull(res);
            Assert.IsNotEmpty(res);
        }

        [Test(Description = "Providers list")]
        public async Task ProvidersTest()
        {
            var service = Locator.Resolve<ITranslateService>();
            var res = await service.ProvidersAsync();
            Assert.NotNull(res);
            Assert.IsNotEmpty(res);
        }

        [Test(Description = "Detailed provider info")]
        [TestCaseSource(typeof(TranslationServiceTestsDataSources),
            nameof(TranslationServiceTestsDataSources.ProviderTestCaseData))]
        public async Task ProviderTest(string providerId, Dictionary<string, string> additionalParams)
        {
            var service = Locator.Resolve<ITranslateService>();
            var res = await service.ProviderAsync(providerId, additionalParams);
            Assert.NotNull(res);
            Assert.IsNotEmpty(res.Id);
        }

        [Test]
        [TestCase("best")]
        public async Task PairsTest(string srtName)
        {
            var service = Locator.Resolve<ITranslateService>();
            var res = await service.PairsAsync(srtName);
            Assert.IsNotNull(res);
            Assert.IsTrue(res.Pairs.Length > 0);
            Assert.IsTrue(res.Pairs.All(p => p.To != null));
        }

        [Test]
        [TestCase("ai.text.translate.google.translate_api.v3")]
        [TestCase("ai.text.translate.baidu.translate_api")]
        public async Task AccountsTest(string providerId)
        {
            var service = Locator.Resolve<ITranslateService>();
            var res = await service.AccountsAsync(providerId);
            Assert.NotNull(res);
        }

        [Test]
        [TestCase("ai.text.translate.google.translate_api.v3")]
        public async Task ModelsTest(string providerId)
        {
            var service = Locator.Resolve<ITranslateService>();
            var res = await service.ModelsAsync(providerId, null);
            Assert.NotNull(res);
            Assert.IsNotEmpty(res);
        }

        [Test]
        [TestCase(true)]
        public async Task RoutingTest(bool pairs)
        {
            var service = Locator.Resolve<ITranslateService>();
            var res = await service.RoutingAsync(new Dictionary<string, string>
            {
                { "pairs", pairs.ToString().ToLower() }
            });
            Assert.NotNull(res);
            Assert.IsNotEmpty(res);
        }

        /// <summary>
        /// Test for translate
        /// </summary>
        /// <param name="options"></param>
        [Test]
        [TestCaseSource(typeof(TranslationServiceTestsDataSources),
            nameof(TranslationServiceTestsDataSources.FulfillTestCaseData))]
        public async Task FulfillTest(TranslateOptions options)
        {
            var service = Locator.Resolve<ITranslateService>();
            var res = await service.FulfillAsync(options);
            Assert.IsNotNull(res);
            Assert.IsNull(res.Error);
        }

        /// <summary>
        /// Test for get AgnosticGlossariesTypes
        /// </summary>
        [Test]
        public async Task AgnosticGlossariesTypesTest()
        {
            var service = Locator.Resolve<ITranslateService>();
            var res = await service.AgnosticGlossariesTypesAsync();
            Assert.NotNull(res);
            Assert.NotNull(res.Count > 0);
        }

        /// <summary>
        /// Get list of agnostic glossaries
        /// </summary>
        [Test]
        public async Task AgnosticGlossariesTest()
        {
            var service = Locator.Resolve<ITranslateService>();
            var res = await service.AgnosticGlossariesAsync();
            Assert.NotNull(res);
            Assert.NotNull(res.Count > 0);
        }

        [Test]
        [TestCase("ai.text.translate.google.translate_api.v3", "test1")]
        public async Task GlossariesTest(string providerId, string credentialId)
        {
            var service = Locator.Resolve<ITranslateService>();
            var res = await service.GlossariesAsync(providerId, credentialId);
            Assert.NotNull(res);
        }

        [Test]
        [TestCase("Intento.SDK.Tests.Sources.Files.logging.docx")]
        public async Task FulfillFileTest(string file)
        {
            var fileContent = FileUtil.ReadBytesFileFromResources(file);
            var path = Path.GetTempFileName() + ".docx";
            await File.WriteAllBytesAsync(path, fileContent);
            var fileInfo = new FileInfo(path);
            var service = Locator.Resolve<ITranslateService>();
            await using var res = await service.FulfillFileAsync(
                new TranslateOptions
                {
                    From = "ru", To = "en"
                }, File.Open(path, FileMode.Open), fileInfo);
            Assert.IsNotNull(res);
            Assert.IsTrue(res.Length > 0);
        }

        [Test]
        [TestCase("ai.text.translate.google.translate_api.v3","projects/894683665182/locations/us-central1/glossaries/compiled_de_en_2022_02_17T11_56_35")]
        public async Task FulfillWithGlossary(string providerId, string glossary)
        {
            var service = Locator.Resolve<ITranslateService>();
            var options = new TranslateOptions
            {
                From = "de",
                To = "en",
                Async = true,
                WaitAsync = true,
                Glossary = glossary,
                Provider = providerId,
                Text = "14",
                Auth = new[]
                {
                    new AuthProviderInfo
                    {
                        Key = new [] { new KeyInfo
                        {
                            CredentialId = "test1"
                        }},
                        Provider = providerId
                    }
                }
            };
            var result = await service.FulfillAsync(options);
            Assert.IsTrue(result.Error == null);
            Assert.IsTrue(result.Results.Length == 1);
        }
    }
}