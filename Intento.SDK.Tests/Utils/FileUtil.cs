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
            using var stream = ReadFileStreamFromResources(filePath);
            using var reader = new StreamReader(stream);
            var content = reader.ReadToEnd();
            return content;
        }

        /// <summary>
        /// Read resource file to stream
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static Stream ReadFileStreamFromResources(string filePath)
        {
            return typeof(FileUtil).Assembly.GetManifestResourceStream(filePath);
        }

        /// <summary>
        /// Read resource file to byte[]
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static byte[] ReadBytesFileFromResources(string filePath)
        {
            using var stream = ReadFileStreamFromResources(filePath);
            return StreamToByteArray(stream);
        }
        
        private static byte[] StreamToByteArray(Stream input)
        {
            var ms = new MemoryStream();
            input.CopyTo(ms);
            return ms.ToArray();
        }
    }
}