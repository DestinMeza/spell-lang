using System.Collections.Generic;

namespace Spell.Syntax
{
    internal sealed class BinaryExpressionSyntaxNode : ExpressionSyntaxNode
    {
        public override SyntaxKind SyntaxKind => SyntaxKind.BinaryExpressionToken;

        public ExpressionSyntaxNode LeftNode { get; }
        public SyntaxToken OperatorToken { get; }
        public ExpressionSyntaxNode RightNode { get; }

        public BinaryExpressionSyntaxNode(ExpressionSyntaxNode leftNode, SyntaxToken operatorToken, ExpressionSyntaxNode right) 
        {
            LeftNode = leftNode;
            OperatorToken = operatorToken;
            RightNode = right;
        }

        public override IEnumerable<INodeable> GetChildren()
        {
            yield return LeftNode;
            yield return OperatorToken;
            yield return RightNode;
        }
    }
}