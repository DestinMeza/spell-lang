namespace Spell.Syntax
{
    public sealed class WhileStatementSyntaxNode : StatementSyntaxNode
    {
        public override SyntaxKind SyntaxKind => SyntaxKind.WhileStatement;
        public override string Text { get => $"{Keyword} {Condition} {Body}"; set { } }
        public SyntaxToken Keyword { get; }
        public ExpressionSyntaxNode Condition { get; }
        public StatementSyntaxNode Body { get; }

        public WhileStatementSyntaxNode(SyntaxToken keyword, ExpressionSyntaxNode condition, StatementSyntaxNode body)
        {
            Keyword = keyword;
            Condition = condition;
            Body = body;
        }
    }
}