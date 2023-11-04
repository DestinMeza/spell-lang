using System.Collections.Generic;

namespace SpellCompiler.SpellReader.Syntax
{
    internal sealed class LiteralExpressionSyntaxNode : ExpressionSyntaxNode
    {
        public override SyntaxKind SyntaxKind => SyntaxKind.LiteralExpressionToken;
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