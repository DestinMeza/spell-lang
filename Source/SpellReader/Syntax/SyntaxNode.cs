using System.Collections.Generic;

namespace Spell.Syntax
{
    public abstract class SyntaxNode : INodeable
    {
        public abstract string Text { get; set; }
        public abstract SyntaxKind SyntaxKind { get; }
        public abstract IEnumerable<INodeable> GetChildren();
    }
}