using Newtonsoft.Json;

namespace Intento.SDK.Translate.DTO
{
    /// <summary>
    /// Base provider info
    /// </summary>
    public class BaseProvider
    {
        [JsonProperty("id")]
        public string Id { get; set; }
        
        [JsonProperty("vendor")]
        public string Vendor { get; set; }
        
        [JsonProperty("description")]
        public string Description { get; set; }
        
        [JsonProperty("production")]
        public bool Production { get; set; }
        
        [JsonProperty("integrated")]
        public bool Integrated { get; set; }
        
        [JsonProperty("billable")]
        public bool Billable { get; set; }
        
        [JsonProperty("own_auth")]
        public bool OwnAuth { get; set; }

        [JsonProperty("stock_model")]
        public bool StockModel { get; set; }

        [JsonProperty("custom_model")]
        public bool CustomModel { get; set; }        
    }
}