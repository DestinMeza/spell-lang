using Spell.Syntax;
using System.Collections.Generic;

namespace Spell.Binding
{
    internal sealed class BoundGlobalScope 
    {
        public BoundGlobalScope Previous { get; }
        public IReadOnlyCollection<VariableSymbol> Variables { get; }
        public BoundStatement Statement { get; }
        public BoundGlobalScope(BoundGlobalScope previous, IReadOnlyCollection<VariableSymbol> variables, BoundStatement statement) 
        {
            Previous = previous;
            Variables = variables;
            Statement = statement;
        }
    }
}