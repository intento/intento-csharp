using System.Threading.Tasks;
using Intento.SDK.Translate.DTO;

namespace Intento.SDK.Translate
{
    /// <summary>
    /// Service for additional query for TMS backend
    /// </summary>
    public interface ITmsBackendService
    {
        /// <summary>
        /// Is language pair supported for pair
        /// </summary>
        /// <param name="route">Route</param>
        /// <param name="pair">Pair</param>
        /// <returns></returns>
        Task<bool> IsLanguagePairSupportedByRoutingAsync(string route, LanguagePair pair);

        /// <summary>
        /// Is language pair supported for pair
        /// </summary>
        /// <param name="route">Route</param>
        /// <param name="pair">Pair</param>
        /// <returns></returns>
        bool IsLanguagePairSupportedByRouting(string route, LanguagePair pair);

        /// <summary>
        /// Is language pair supported for provider
        /// </summary>
        /// <param name="providerId">Provider</param>
        /// <param name="pair">Pair</param>
        /// <returns></returns>
        Task<bool> IsLanguagePairSupportedByProviderAsync(string providerId, LanguagePair pair);

        /// <summary>
        /// Is language pair supported for provider
        /// </summary>
        /// <param name="providerId">Provider</param>
        /// <param name="pair">Pair</param>
        /// <returns></returns>
        bool IsLanguagePairSupportedByProvider(string providerId, LanguagePair pair);
    }
}