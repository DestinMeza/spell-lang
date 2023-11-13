using Spell.Syntax;
using System.Collections.Generic;

namespace Spell.Binding
{
    internal sealed class BoundGlobalScope 
    {
        public BoundGlobalScope Previous { get; }
        public IReadOnlyCollection<VariableSymbol> Variables { get; }
        public BoundExpressionNode Expression { get; }
        public BoundGlobalScope(BoundGlobalScope previous, IReadOnlyCollection<VariableSymbol> variables, BoundExpressionNode expression) 
        {
            Previous = previous;
            Variables = variables;
            Expression = expression;
        }
    }
}