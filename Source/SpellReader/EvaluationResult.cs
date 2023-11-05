using System.Collections.Generic;
using System.Linq;

namespace Spell
{
    public sealed class EvaluationResult
    {
        public IReadOnlyList<string> Diagnostics { get; }
        public object Value { get; }
        public EvaluationResult(IEnumerable<Log> diagnostics, object value) 
        {
            Diagnostics = diagnostics.Select(x => x.Message()).ToArray();
            Value = value;
        }
    }
}