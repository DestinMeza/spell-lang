using System.Collections.Generic;

namespace Spell.Syntax
{
    internal sealed class UnaryExpressionSyntax : ExpressionSyntaxNode
    {
        public override SyntaxKind SyntaxKind => SyntaxKind.UnaryExpressionToken;

        public SyntaxToken OperatorToken { get; }
        public ExpressionSyntaxNode Operand { get; }

        public UnaryExpressionSyntax(SyntaxToken operatorToken, ExpressionSyntaxNode operand)
        {
            OperatorToken = operatorToken;
            Operand = operand;
        }

        public override IEnumerable<INodeable> GetChildren()
        {
            yield return OperatorToken;
            yield return Operand;
        }
    }
}