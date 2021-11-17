using System;
using System.IO;
using System.Linq;
using Intento.SDK.Tests.Sources;
using Intento.SDK.Tests.Utils;
using Intento.SDK.Translate.DTO;
using Newtonsoft.Json;
using NUnit.Framework;

namespace Intento.SDK.Tests
{
    [TestFixture(Category = "Serialize model tests")]
    public class SerializationTests
    {
        /// <summary>
        /// Test TranslateService serialization
        /// </summary>
        [Test(Description = "Serialize TranslateService object")]
        [TestCaseSource(typeof(SerializationTestsDataSources), nameof(SerializationTestsDataSources.SerializeTranslateServiceDtoTestCaseData))]
        public void SerializeTranslateServiceDto(TranslateService dto, string expectedResult)
        {
            var json = JsonConvert.SerializeObject(dto);
            Assert.IsNotEmpty(json);
            Assert.AreEqual(expectedResult, json);
        }

        /// <summary>
        /// Test TranslateService deserialization
        /// </summary>
        /// <param name="json"></param>
        [Test(Description = "Deserialize TranslateService object")]
        [TestCaseSource(typeof(SerializationTestsDataSources), nameof(SerializationTestsDataSources.DeserializeTranslateServiceDtoTestCaseData))]
        public void DeserializeTranslateServiceDto(string json, int expectedLength)
        {
            var dto = JsonConvert.DeserializeObject<TranslateService>(json);
            Assert.IsNotNull(dto);
            Assert.IsNotNull(dto.Auth);
            Assert.IsTrue(dto.Auth.Length == expectedLength);
        }

        [Test]
        [TestCaseSource(typeof(SerializationTestsDataSources), nameof(SerializationTestsDataSources.DeserializeLanguagePairsTestCaseData))]
        public void DeserializeLanguagePairs(string json)
        {
            var dto = JsonConvert.DeserializeObject<LanguagePairs>(json);
            Assert.IsNotNull(dto);
            Assert.IsTrue(dto.Pairs.All(p => p.To != null));
        }

        [Test]
        [TestCaseSource(typeof(SerializationTestsDataSources), nameof(SerializationTestsDataSources.DeserializeTranslateResponseWrapperTestCaseData))]
        public void DeserializeTranslateResponseWrapper(string json)
        {
            var dto = JsonConvert.DeserializeObject<TranslateResponseWrapper>(json);
            Assert.IsNotNull(dto);
            Assert.IsTrue(dto.Response.Length > 0);
        }

        [Test]
        [TestCaseSource(typeof(SerializationTestsDataSources), nameof(SerializationTestsDataSources.DeserializeTranslateContextTestCaseData))]
        public void DeserializeTranslateContext(string json)
        {
            var dto = JsonConvert.DeserializeObject<TranslateContext>(json);
            Assert.IsNotNull(dto);
            Assert.IsTrue(dto.Text.Length > 0);
        }
    }
}