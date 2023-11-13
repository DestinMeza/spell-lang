using System.Linq.Expressions;

namespace Spell.Syntax
{
    public sealed class CompilationUnitSyntax : SyntaxNode 
    {
        public override SyntaxKind SyntaxKind => SyntaxKind.CompilationUnit;
        public override string Text { get => $"{Statement.Text} {EndOfFileToken.Text}"; set { } }
        public StatementSyntaxNode Statement { get; }
        public SyntaxToken EndOfFileToken { get; }

        public CompilationUnitSyntax(StatementSyntaxNode statement, SyntaxToken endOfFileToken) 
        {
            Statement = statement;
            EndOfFileToken = endOfFileToken;
        }
    }
}