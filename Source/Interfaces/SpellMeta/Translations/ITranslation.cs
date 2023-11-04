using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System;

namespace SpellCompiler
{
    public interface ITranslation : ISerializable
    {

    }

    public class TranslationConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType.IsAssignableFrom(typeof(ITranslation));
        }
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            JObject jo = JObject.Load(reader);

            string mainType = jo["MainType"].Value<string>();

            if (mainType == MultiTranslation.TypeName)
            {
                return jo.ToObject<MultiTranslation>(serializer);
            }
            if (mainType == SimpleTranslation.TypeName)
            {
                return jo.ToObject<SimpleTranslation>(serializer);
            }

            return null;
        }
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            serializer.Serialize(writer, value, typeof(ITranslation));
        }
    }
}