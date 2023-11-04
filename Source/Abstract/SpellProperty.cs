using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;

namespace SpellCompiler
{
    /// <summary>
    /// Spell Properties are used for post Json Deserialization and for OnCreation for inputing spell config.
    /// refs: <see cref="IDynamicallyMuttable"/>
    /// </summary>
    [System.Serializable]
    public class SpellProperty
    {
        public int ElementIndex { get; set; } = -1;
        public string PropertyName { get; set; }
        public JToken OriginalValue { get; set; }
        public JToken Value { get; set; }
    }
}