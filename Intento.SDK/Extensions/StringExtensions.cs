using System;

namespace Intento.SDK.Extensions
{
    public static class StringExtensions
    {
        public static string CombineUrl(this string urlBase, string relativeUrl)
        {
            if (string.IsNullOrWhiteSpace(urlBase))
            {
                throw new ArgumentNullException(nameof(urlBase));
            }

            if (string.IsNullOrWhiteSpace(relativeUrl))
            {
                return urlBase;
            }

            urlBase = urlBase.TrimEnd('/');
            relativeUrl = relativeUrl.TrimStart('/');
            return string.Format("{0}/{1}", new object[]
            {
                urlBase,
                relativeUrl
            });
        }
    }
}