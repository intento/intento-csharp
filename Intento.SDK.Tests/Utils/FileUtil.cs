using System;
using System.IO;
using System.Reflection;

namespace Intento.SDK.Tests.Utils
{
    /// <summary>
    /// Utils for do work with files
    /// </summary>
    internal static class FileUtil
    {
        /// <summary>
        /// Read file content from files with flag "EmbeddedResource"
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static string ReadFileFromResources(string filePath)
        {
            using var stream = typeof(FileUtil).Assembly.GetManifestResourceStream(filePath);
            using var reader = new StreamReader(stream);
            var content = reader.ReadToEnd();
            return content;
        }
    }
}