using System.Collections.Generic;

namespace Spell.Syntax
{
    internal sealed class NameExpressionSyntaxNode : ExpressionSyntaxNode
    {
        public override SyntaxKind SyntaxKind => SyntaxKind.NameExpressionToken;
        public override string Text { get => $"{IdentifierToken.Text}"; set { } }
        public SyntaxToken IdentifierToken { get; }

        public NameExpressionSyntaxNode(SyntaxToken identifierToken) 
        {
            IdentifierToken = identifierToken;
        }

        public override IEnumerable<INodeable> GetChildren()
        {
            yield return IdentifierToken;
        }
    }
}