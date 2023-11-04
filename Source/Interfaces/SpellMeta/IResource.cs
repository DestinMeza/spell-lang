using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;

namespace Spell
{
    /// <summary>
    /// Resources are what get expended on use of a spell.
    /// </summary>
    public interface IResource : ISerializable
    {

    }

    public class ResourceConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType.IsAssignableFrom(typeof(IResource));
        }
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            JObject jo = JObject.Load(reader);

            string mainType = jo["MainType"].Value<string>();

            if (mainType == ManaResource.TypeName) 
            {
                return jo.ToObject<ManaResource>(serializer);
            }

            return null;
        }
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            serializer.Serialize(writer, value, typeof(IDepletion));
        }
    }
}