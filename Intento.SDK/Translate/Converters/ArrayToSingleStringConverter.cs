using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Intento.SDK.Translate.Converters
{
    /// <summary>
    /// Convert string[] to string if needed 
    /// </summary>
    internal class ArrayToSingleStringConverter: JsonConverter<string[]>
    {
        public override void WriteJson(JsonWriter writer, string[]? value, JsonSerializer serializer)
        {
            if (value == null)
            {
                return;
            }

            object val = value.Length switch
            {
                0 => string.Empty,
                1 => value[0],
                _ => value
            };
            
            var t = JToken.FromObject(val);
            t.WriteTo(writer);
        }

        public override string[]? ReadJson(JsonReader reader, Type objectType, string[]? existingValue, bool hasExistingValue,
            JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}