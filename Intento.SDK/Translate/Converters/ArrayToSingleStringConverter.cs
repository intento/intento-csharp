using System;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Intento.SDK.Translate.Converters
{
    /// <summary>
    /// Convert string[] to string if needed 
    /// </summary>
    internal class ArrayToSingleStringConverter: JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if (value == null)
            {
                return;
            }
            var values = (string[])value;
            object val = values.Length switch
            {
                0 => string.Empty,
                1 => values[0],
                _ => value
            };
            
            var t = JToken.FromObject(val);
            t.WriteTo(writer);
        }      

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var t = JToken.Load(reader);
            return t switch
            {
                null => existingValue,
                JArray array => array.ToObject<string[]>(),
                _ => new []
                {
                    t.ToObject<string>()
                }
            };
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(string[]);
        }
    }
}