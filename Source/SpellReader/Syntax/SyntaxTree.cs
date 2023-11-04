using System.Collections.Generic;
using System.Linq;

namespace Spell.Syntax
{
    public sealed class SyntaxTree 
    {
        public IReadOnlyList<string> Diagnostics { get; }
        public ExpressionSyntaxNode Root { get; }
        public SyntaxToken EndOfFileToken { get; }

        public SyntaxTree(IEnumerable<string> diagnostics, ExpressionSyntaxNode root, SyntaxToken endOfFileToken) 
        {
            Diagnostics = diagnostics.ToArray();
            Root = root;
            EndOfFileToken = endOfFileToken;
        }
    }
}