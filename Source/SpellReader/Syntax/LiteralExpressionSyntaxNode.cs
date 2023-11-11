using System.Collections.Generic;

namespace Spell.Syntax
{
    internal sealed class LiteralExpressionSyntaxNode : ExpressionSyntaxNode
    {
        public override SyntaxKind SyntaxKind => SyntaxKind.LiteralExpressionToken;
        public override string Text { get => $"{LiteralToken.Text}"; set { } }
        public SyntaxToken LiteralToken { get; }
        public object Value { get; }
        public LiteralExpressionSyntaxNode(SyntaxToken literalToken, object value) 
        {
            LiteralToken = literalToken;
            Value = value;
        }

        public LiteralExpressionSyntaxNode(SyntaxToken literalToken) 
            : this(literalToken, literalToken.Value)
        {
            
        }

        public override IEnumerable<INodeable> GetChildren()
        {
            yield return LiteralToken;
        }
    }
}