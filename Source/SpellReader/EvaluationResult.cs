﻿using System.Collections.Generic;
using System.Linq;

namespace Spell
{
    public sealed class EvaluationResult
    {
        public IReadOnlyList<string> Diagnostics { get; }
        public object Value { get; }
        public EvaluationResult(IEnumerable<string> diagnostics, object value) 
        {
            Diagnostics = diagnostics.ToList();
            Value = value;
        }
    }
}