using System;
using System.Reflection;
using System.Reflection.Emit;

namespace Spell.IO
{
    /// <summary>
    /// Spell Function is used for allowing a simple way to Invoke dynamically created functions at runtime.
    /// </summary>
    public class SpellFunction
    {
        public SpellProperty[] spellProperties;
        private string _methodName;

        public SpellFunction(string methodName, string methodTextToParse, Type returnType, Type[] parameters) 
        {
            _methodName = methodName;

            // Build Function here.
            DynamicMethod method = new DynamicMethod("SpellFunction", returnType, parameters, GetType());
        }

        public void Invoke() 
        {
            GetType().GetMethod(_methodName);
        }
    }
}