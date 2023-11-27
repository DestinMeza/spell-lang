namespace Spell.Syntax
{
    public sealed class ElseClauseSyntax : SyntaxNode 
    {
        public override SyntaxKind SyntaxKind => SyntaxKind.ElseClause;
        public override string Text { get => $"{ElseKeyword} {ElseStatement}"; set { } }

        public SyntaxToken ElseKeyword { get; }
        public StatementSyntaxNode ElseStatement { get; }

        public ElseClauseSyntax(SyntaxToken elseKeyword, StatementSyntaxNode elseStatement) 
        {
            ElseKeyword = elseKeyword;
            ElseStatement = elseStatement;
        }
    }
}