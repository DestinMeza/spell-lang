using System.Collections.Generic;
using System.Linq;

namespace Spell
{
    public sealed class EvaluationResult
    {
        public List<string> Diagnostics { get; }
        public object Value { get; }
        public EvaluationResult(Log[] diagnostics, object value) 
        {
            Diagnostics = diagnostics.Select(x => x.Message()).ToList();
            Value = value;
        }
    }
}