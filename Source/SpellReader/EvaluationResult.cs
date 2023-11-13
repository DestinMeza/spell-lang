using System.Collections.Generic;
using System.Linq;

namespace Spell
{
    public sealed class EvaluationResult
    {
        public string Tree { get; }
        public IReadOnlyCollection<string> Diagnostics { get; }
        public IReadOnlyCollection<ELogType> LogTypes { get; }
        public object Value { get; }
        public EvaluationResult(string tree, Log[] diagnostics, object value) 
        {
            Tree = tree;
            Diagnostics = diagnostics.Select(x => x.Message()).ToArray();
            LogTypes = diagnostics.Select(x => x.ELogType).ToArray();
            Value = value;
        }
    }
}