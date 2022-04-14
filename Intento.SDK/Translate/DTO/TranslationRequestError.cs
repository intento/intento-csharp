using Newtonsoft.Json;

namespace Intento.SDK.Translate.DTO
{
    public class TranslationRequestError
    {
        [JsonProperty("code")]
        public string Code { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("data")]
        public string Data { get; set; }

        [JsonProperty("intent")]
        public string Intent { get; set; }

        [JsonProperty("provider")]
        public TranslationProviderRequestError Provider { get; set; }
    }

    public class TranslationProviderRequestError
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("response")]
        public TranslationProviderErrorInfo Response { get; set; }
    }

    public class TranslationProviderErrorInfo
    {
        [JsonProperty("code")]
        public string Code { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("body")]
        public TranslationProviderBodyErrorInfo Body { get; set; }
    }

    public class TranslationProviderBodyErrorInfo
    {
        [JsonProperty("message")]
        public string Message { get; set; }
    }
}