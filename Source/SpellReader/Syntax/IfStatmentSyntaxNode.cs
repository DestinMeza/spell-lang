namespace Spell.Syntax
{
    public sealed class IfStatmentSyntaxNode : StatementSyntaxNode 
    {
        public override SyntaxKind SyntaxKind => SyntaxKind.IfStatement;
        public override string Text { get => $"{IfKeyword} {Condition} {ThenStatement} {ElseClause}"; set { } }

        public SyntaxToken IfKeyword { get; }
        public ExpressionSyntaxNode Condition { get; }
        public StatementSyntaxNode ThenStatement { get; }
        public ElseClauseSyntax ElseClause { get; }

        public IfStatmentSyntaxNode(SyntaxToken ifKeyword, ExpressionSyntaxNode condition, StatementSyntaxNode thenStatement, ElseClauseSyntax elseClause) 
        {
            IfKeyword = ifKeyword;
            Condition = condition;
            ThenStatement = thenStatement;
            ElseClause = elseClause;
        }
    }
}