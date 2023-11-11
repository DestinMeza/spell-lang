using System.Collections.Generic;
using System.Linq.Expressions;

namespace Spell.Syntax
{
    internal sealed class UnaryExpressionSyntax : ExpressionSyntaxNode
    {
        public override SyntaxKind SyntaxKind => SyntaxKind.UnaryExpressionToken;
        public override string Text { get => $"{OperatorToken.Text} {Operand.Text}"; set { } }
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