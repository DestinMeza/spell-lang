using System.Reflection;
using System.Runtime.CompilerServices;
using System;

namespace Spell
{
    /// <summary>
    /// Muttable attribute allows easy implementation for dynamic Property Manipulation at runtime.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class MuttableAttribute : Attribute
    {
        private readonly string propertyName;
        public MuttableAttribute([CallerMemberName] string propertyName = default)
        {
            this.propertyName = propertyName;
        }

        /// <summary>
        /// This method is called in <see cref="GameSystem.SpellBuilder"/> 
        /// </summary>
        /// <param name="properties"></param>
        public void ApplyProperties(object owningObject, SpellProperty[] properties)
        {
            int elementIndex = -1;

            if (owningObject is ISerializable serializableObject) 
            {
                elementIndex = serializableObject.ElementIndex;
            }

            PropertyInfo propertyInfo = owningObject.GetType().GetProperty(propertyName);

            for (int i = 0; i < properties.Length; i++)
            {
                if (properties[i].PropertyName != propertyName && properties[i].ElementIndex != elementIndex) continue;

                //Reads the value in json data.
                var reader = properties[i].Value.CreateReader();
                reader.Read();

                object value = reader.Value;

                if (reader.ValueType != propertyInfo.PropertyType)
                {
                    value = Convert.ChangeType(value, propertyInfo.PropertyType);
                }

                propertyInfo.SetValue(owningObject, value);
            }
        }
    }
}