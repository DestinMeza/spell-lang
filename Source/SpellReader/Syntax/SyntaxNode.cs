using System.Collections.Generic;

namespace SpellCompiler.SpellReader.Syntax
{
    public abstract class SyntaxNode : INodeable
    {
        public abstract SyntaxKind SyntaxKind { get; }
        public abstract IEnumerable<INodeable> GetChildren();
    }
}