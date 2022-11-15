using System.Collections.Generic;
using System.Threading.Tasks;
using Intento.SDK.Client;
using Intento.SDK.Translate;
using Intento.SDK.Translate.DTO;
using Microsoft.Extensions.Logging;

namespace Intento.SDK.Tms
{
    public class TmsBackendService: ITmsBackendService
    {
        private TmsBackendClient Client { get; }
        
        private ILogger Logger { get; }
        
        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="client"></param>
        /// <param name="logger"></param>
        public TmsBackendService(TmsBackendClient client, ILogger<TmsBackendService> logger)
        {
            Client = client;
            Logger = logger;
        }

        /// <inheritdoc />
        public async Task<bool> IsLanguagePairSupportedByRoutingAsync(string route, LanguagePair pair)
        {
            var url = $"ai/text/translate/routing/{route}/support";
            var additionalParams = new Dictionary<string, string>
            {
                { "from", pair.From },
                { "to", pair.To }
            };
            var response = await Client.GetAsync<OperationResponse>(url, true, additionalParams);
            return response.Result;
        }

        /// <inheritdoc />
        public bool IsLanguagePairSupportedByRouting(string route, LanguagePair pair)
        {
            var taskReadResult = Task.Run(async () => await IsLanguagePairSupportedByRoutingAsync(route, pair));
            return taskReadResult.Result;
        }

        /// <inheritdoc />
        public async Task<bool> IsLanguagePairSupportedByProviderAsync(string providerId, LanguagePair pair)
        {
            var url = $"ai/text/translate/{providerId}/support";
            var additionalParams = new Dictionary<string, string>
            {
                { "from", pair.From },
                { "to", pair.To }
            };
            var response =
                await Client.GetAsync<OperationResponse>(url, true, additionalParams);
            return response.Result;
        }

        /// <inheritdoc />
        public bool IsLanguagePairSupportedByProvider(string providerId, LanguagePair pair)
        {
            var taskReadResult = Task.Run(async () => await IsLanguagePairSupportedByProviderAsync(providerId, pair));
            return taskReadResult.Result;
        }
    }
}