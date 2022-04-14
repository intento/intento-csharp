using Newtonsoft.Json;

namespace Intento.SDK.Translate.DTO
{
    public class Error
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("reason")]
        public string Reason { get; set; }

        [JsonProperty("data")]
        public DataErrorItem[] Data { get; set; }
    }

    public class DataErrorItem
    {
        [JsonProperty("item")]
        public int Item { get; set; }

        [JsonProperty("position")]
        public int Position { get; set; }

        [JsonProperty("error")]
        public bool Error { get; set; }

        [JsonProperty("provider_id")]
        public string ProviderId { get; set; }

        [JsonProperty("request")]
        public RequestErrorInfo Request { get; set; }

        [JsonProperty("response")]
        public ResponseErrorInfo Response { get; set; }
    }

    public class RequestErrorInfo
    {
        [JsonProperty("url")]
        public string Url { get; set; }

        [JsonProperty("method")]
        public string Method { get; set; }

        [JsonProperty("body")]
        public string Body { get; set; }
    }

    public class ResponseErrorInfo
    {
        [JsonProperty("body")]
        public ResponseBodyErrorInfo Body { get; set; }
    }

    public class ResponseBodyErrorInfo
    {
        [JsonProperty("request_id")]
        public string RequestId { get; set; }

        [JsonProperty("error")]
        public TranslationRequestError Error { get; set; }
    }
    
    
    
}