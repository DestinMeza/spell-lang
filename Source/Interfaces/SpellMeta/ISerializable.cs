using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;

namespace SpellCompiler
{
    /// <summary>
    /// Serializable data that is managed via custom JsonConverters.
    /// </summary>
    public interface ISerializable
    {
        int ElementIndex { get; set; }
        string MainType { get; set; }
    }
}