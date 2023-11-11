using System.Collections.Generic;

namespace Spell.Syntax
{
    internal sealed class AssignmentExpressionSyntaxNode : ExpressionSyntaxNode
    {
        public override SyntaxKind SyntaxKind => SyntaxKind.AssignmentExpressionToken;
        public override string Text { get => $"{IdentifierToken.Text} {EqualsToken.Text} {ExpressionSyntaxNode.Text}"; set { } }
        public SyntaxToken IdentifierToken { get; }
        public SyntaxToken EqualsToken { get; }
        
        public ExpressionSyntaxNode ExpressionSyntaxNode { get; }

        public AssignmentExpressionSyntaxNode(SyntaxToken identifierToken, SyntaxToken equalsToken, ExpressionSyntaxNode expressionSyntaxNode)
        {
            IdentifierToken = identifierToken;
            EqualsToken = equalsToken;
            ExpressionSyntaxNode = expressionSyntaxNode;
        }

        public override IEnumerable<INodeable> GetChildren()
        {
            yield return IdentifierToken;
            yield return EqualsToken;
            yield return ExpressionSyntaxNode;
        }
    }
}