namespace Spell.Syntax
{
    public sealed class CompilationUnitSyntax : SyntaxNode 
    {
        public override SyntaxKind SyntaxKind => SyntaxKind.CompilationUnit;
        public override string Text { get => $"{Expression.Text} {EndOfFileToken.Text}"; set { } }
        public ExpressionSyntaxNode Expression { get; }
        public SyntaxToken EndOfFileToken { get; }

        public CompilationUnitSyntax(ExpressionSyntaxNode expression, SyntaxToken endOfFileToken) 
        {
            Expression = expression;
            EndOfFileToken = endOfFileToken;
        }
    }
}