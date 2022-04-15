using System;
using Intento.SDK.Translate.DTO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Intento.SDK.Translate.Converters
{
    internal class GlossaryConverter: JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            switch (value)
            {
                case null:
                    return;
                case string s:
                    writer.WriteValue(s);
                    break;
                case Glossary[] glossaries:
                {
                    var t = JToken.FromObject(glossaries);
                    t.WriteTo(writer);
                    break;
                }
            }
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var t = JToken.Load(reader);
            return t switch
            {
                null => existingValue,
                JArray array => array.ToObject<Glossary[]>(),
                _ => t.ToObject<string>()
            };
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(object);
        }
    }
}