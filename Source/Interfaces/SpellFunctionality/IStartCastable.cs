using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace SpellCompiler
{
    /// <summary>
    /// IStartCastable is where spell properties are processed when OnCast is processed.
    /// </summary>
    internal interface IStartCastable : ISpellFunctionable
    {
        /// <summary>
        /// OnStartCasting Takes in a spell property params and also a function that processes the param of spells.
        /// </summary>
        /// <param name="spellProperties"></param>
        void OnStartCasting(params SpellFunction[] spellFunctions);
    }
}