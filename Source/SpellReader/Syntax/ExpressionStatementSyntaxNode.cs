namespace Spell.Syntax
{
    public sealed class ExpressionStatementSyntaxNode : StatementSyntaxNode 
    {
        public override SyntaxKind SyntaxKind => SyntaxKind.ExpressionStatement;
        public override string Text { get => $"{Expression.Text}"; set { } }
        public ExpressionSyntaxNode Expression { get; }
        public ExpressionStatementSyntaxNode(ExpressionSyntaxNode expression) 
        {
            Expression = expression;
        }
    }
}