using System.Collections.Generic;

namespace Spell.Syntax
{
    internal sealed class ParenthesizedExpressionSytanxNode : ExpressionSyntaxNode 
    {
        public override SyntaxKind SyntaxKind => SyntaxKind.ParenthesizedExpressionToken;
        public override string Text { get => $"{OpenParenthesisToken.Text} {Expression.Text} {CloseParenthesisToken.Text}"; set { } }
        public SyntaxToken OpenParenthesisToken { get; set; }
        public ExpressionSyntaxNode Expression { get; set; }
        public SyntaxToken CloseParenthesisToken { get; set; }

        public ParenthesizedExpressionSytanxNode(SyntaxToken openParenthesisToken, ExpressionSyntaxNode expression, SyntaxToken closeParenthesisToken) 
        {
            OpenParenthesisToken = openParenthesisToken;
            Expression = expression;
            CloseParenthesisToken = closeParenthesisToken;
        }

        public override IEnumerable<INodeable> GetChildren()
        {
            yield return OpenParenthesisToken;
            yield return Expression;
            yield return CloseParenthesisToken;
        }
    }
}