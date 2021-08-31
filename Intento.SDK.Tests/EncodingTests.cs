using System;
using Intento.SDK.Tests.Sources;
using Intento.SDK.Tests.Utils;
using NUnit.Framework;

namespace Intento.SDK.Tests
{
    /// <summary>
    /// Encoding tests
    /// </summary>
    [TestFixture(Category = "Encoding tests")]
    public class EncodingTests
    {
        [Test]
        [TestCaseSource(typeof(EncodingTestsDataSources), nameof(EncodingTestsDataSources.Base64EncodingTestCaseData))]
        public void Base64Encoding(string expected, string file)
        {
            var fileContent = FileUtil.ReadBytesFileFromResources("Intento.SDK.Tests.Sources.Files.logging.docx");
            var base64String = Convert.ToBase64String(fileContent, Base64FormattingOptions.InsertLineBreaks);
            Assert.IsNotNull(base64String);
            Assert.AreEqual(expected, base64String);
        }
    }
}