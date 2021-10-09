#nullable enable
using System;
using System.Collections.Generic;
using System.Linq;
using Intento.SDK.Translate.DTO;
using Intento.SDK.Translate.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Intento.SDK.Translate.Converters
{
    /// <summary>
    /// Converter for auth info in "service"
    /// </summary>
    internal class AuthProviderInfoConverter: JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if (value == null)
            {
                return;
            }
            var val = (AuthProviderInfo[])value;
            var dict = val
                .Where(info => info.Key != null)
                .ToDictionary(info => info.Provider, info => new[] {info.Key});
            var t = JToken.FromObject(dict);
            t.WriteTo(writer);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var result = new List<AuthProviderInfo>();
            var jObject = JObject.Load(reader);
            foreach (var property in jObject.Properties())
            {
                var info = new AuthProviderInfo
                {
                    Provider = property.Name
                };
                if (property.Value is not JArray array || array.Count == 0)
                {
                    continue;
                }
                result.Add(info);
                info.Key = array[0]?.ToObject<KeyInfo>();
            }
            return result.ToArray();
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(AuthProviderInfo[]);
        }
    }
}