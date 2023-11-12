using System.Collections.Generic;

namespace Spell.Syntax
{
    internal sealed class NameExpressionSyntaxNode : ExpressionSyntaxNode
    {
        public override SyntaxKind SyntaxKind => SyntaxKind.NameExpression;
        public override string Text { get => $"{IdentifierToken.Text}"; set { } }
        public SyntaxToken IdentifierToken { get; }

        public NameExpressionSyntaxNode(SyntaxToken identifierToken) 
        {
            IdentifierToken = identifierToken;
        }
    }
}