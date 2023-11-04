using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace SpellCompiler
{
    /// <summary>
    /// IRunningCastable is where spell properties are processed when OnCast is processed.
    /// </summary>
    internal interface IRunningCastable : ISpellFunctionable
    {

    }
}