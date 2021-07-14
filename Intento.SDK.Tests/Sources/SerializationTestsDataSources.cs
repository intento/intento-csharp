using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Intento.SDK.Tests.Utils;
using Intento.SDK.Translate.DTO;
using Intento.SDK.Translate.Options;
using NUnit.Framework;

namespace Intento.SDK.Tests.Sources
{
    /// <summary>
    /// Data sources for SerializationTests
    /// </summary>
    internal class SerializationTestsDataSources
    {
        /// <summary>
        /// Get test case data for SerializeTranslateServiceDto
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<TestCaseData> SerializeTranslateServiceDtoTestCaseData()
        {
            var dto = new TranslateServiceDto
            {
                Provider = "Google",
                Auth = new AuthProviderInfo[]
                {
                    new() {Key = new KeyInfo {Key = "123"}, Provider = "Google.v1"},
                    new() {Key = new KeyInfo {Key = "4,5,6"}, Provider = "Yandex.v2"}
                }
            };
            yield return new TestCaseData(dto, FileUtil.ReadFileFromResources("Intento.SDK.Tests.Sources.Files.TranslateServiceDto.json"));
        }

        /// <summary>
        /// Get test case data for DeserializeTranslateServiceDto
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<TestCaseData> DeserializeTranslateServiceDtoTestCaseData()
        {
            yield return new TestCaseData(
                FileUtil.ReadFileFromResources("Intento.SDK.Tests.Sources.Files.TranslateServiceDto.json"), 2);
        }
        
        /// <summary>
        /// Get test case data for DeserializeLanguagePairs
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<TestCaseData> DeserializeLanguagePairsTestCaseData()
        {
            yield return new TestCaseData(
                FileUtil.ReadFileFromResources("Intento.SDK.Tests.Sources.Files.LanguagePairs.json"));
        }
    }
}