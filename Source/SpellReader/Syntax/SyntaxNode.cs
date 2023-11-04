using System.Collections.Generic;

namespace Spell.Syntax
{
    public abstract class SyntaxNode : INodeable
    {
        public abstract SyntaxKind SyntaxKind { get; }
        public abstract IEnumerable<INodeable> GetChildren();
    }
}