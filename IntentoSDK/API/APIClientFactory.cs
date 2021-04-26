using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntentoSDK.API
{
    public class APIClientFactory
    {
        private readonly BaseAPIClient[] clients;

        public APIClientFactory()
        {
            clients = new BaseAPIClient[] {
                new RemoteAPIClient(),
                new LocalAPIClient()
            };
        }

        /// <summary>
        /// Get all apis
        /// </summary>
        public BaseAPIClient[] ClientApis
        {
            get { return clients; }
        }

        /// <summary>
        /// Get client for connect to API
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        public BaseAPIClient Get(Guid? uid = null)
        {
            if (uid == null)
            {
                return clients[0];
            }
            return clients.FirstOrDefault(c => c.ClientUid == uid);
        }

        public void Init(string filePath)
        {
            foreach (var client in clients)
            {
                client.Init(filePath);
            }
        }
       
        /// <summary>
        /// Get factory for clients API
        /// </summary>
        public static APIClientFactory Current
        {
            get => current;
        }

        private static readonly APIClientFactory current = new APIClientFactory();
    }
}
