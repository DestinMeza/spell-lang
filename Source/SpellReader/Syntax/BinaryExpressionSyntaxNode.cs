using System.Collections.Generic;

namespace Spell.Syntax
{

    internal sealed class BinaryExpressionSyntaxNode : ExpressionSyntaxNode
    {
        public override SyntaxKind SyntaxKind => SyntaxKind.BinaryExpression;
        public override string Text { get => $"{LeftNode.Text} {OperatorToken.Text} {RightNode.Text}"; set { } }

        public ExpressionSyntaxNode LeftNode { get; }
        public SyntaxToken OperatorToken { get; }
        public ExpressionSyntaxNode RightNode { get; }

        public BinaryExpressionSyntaxNode(ExpressionSyntaxNode leftNode, SyntaxToken operatorToken, ExpressionSyntaxNode right) 
        {
            LeftNode = leftNode;
            OperatorToken = operatorToken;
            RightNode = right;
        }
    }
}