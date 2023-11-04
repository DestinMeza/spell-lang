using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace SpellCompiler
{
    /// <summary>
    /// Depletion is how a spell drains resources as it's being used.
    /// </summary>
    public interface IDepletion : ISerializable
    {
        /// <summary>
        /// This function is called in <see cref="GameSystem.SpellBuilder"/>
        /// </summary>
        IDepletion OnCasting();

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        IDepletion OnTranslation();
    }

    public class DepletionConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType.IsAssignableFrom(typeof(IDepletion));
        }
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            JObject jo = JObject.Load(reader);

            string mainType = jo["MainType"].Value<string>();

            if (mainType == ResourceBurn.TypeName)
            {
                return jo.ToObject<ResourceBurn>(serializer);
            }

            return null;
        }
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            serializer.Serialize(writer, value, typeof(IDepletion));
        }
    }
}