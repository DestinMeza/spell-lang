using System.Collections.Generic;
using System.Linq;

namespace Spell.Syntax
{
    public sealed class SyntaxTree 
    {
        public ExpressionSyntaxNode Root { get; }
        public SyntaxToken EndOfFileToken { get; }

        public SyntaxTree(ExpressionSyntaxNode root, SyntaxToken endOfFileToken) 
        {
            Root = root;
            EndOfFileToken = endOfFileToken;
        }
    }
}