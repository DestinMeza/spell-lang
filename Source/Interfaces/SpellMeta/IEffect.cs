using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;

namespace SpellCompiler
{
    /// <summary>
    /// Effects are applied through out the lifetime of a spell.
    /// </summary>
    public interface IEffect : ISerializable
    {

    }

    public class EffectConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType.IsAssignableFrom(typeof(IEffect));
        }
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            JObject jo = JObject.Load(reader);

            string mainType = jo["MainType"].Value<string>();

            if (mainType == DamageEffect.TypeName)
            {
                return jo.ToObject<DamageEffect>(serializer);
            }
            if (mainType == HealEffect.TypeName)
            {
                return jo.ToObject<HealEffect>(serializer);
            }

            return null;
        }
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            serializer.Serialize(writer, value, typeof(IEffect));
        }
    }
}