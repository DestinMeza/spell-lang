namespace Spell.Syntax
{
    public sealed class ForStatementSyntaxNode : StatementSyntaxNode 
    {
        public override SyntaxKind SyntaxKind => SyntaxKind.ForStatement;
        public override string Text { get => $"{Identifier} {EqualsToken} {LowerBound} {UpperBound}"; set { } }

        public SyntaxToken Keyword { get; }
        public SyntaxToken Identifier { get; }
        public SyntaxToken EqualsToken { get; }
        public ExpressionSyntaxNode LowerBound { get; }
        public SyntaxToken ToKeyword { get; }
        public ExpressionSyntaxNode UpperBound { get; }
        public StatementSyntaxNode Body { get; }

        public ForStatementSyntaxNode(SyntaxToken keyword, SyntaxToken identifier, SyntaxToken equalsToken, ExpressionSyntaxNode lowerBound, SyntaxToken toKeyword, ExpressionSyntaxNode upperBound, StatementSyntaxNode body) 
        {
            Keyword = keyword;
            Identifier = identifier;
            EqualsToken = equalsToken;
            LowerBound = lowerBound;
            ToKeyword = toKeyword;
            UpperBound = upperBound;
            Body = body;
        }
    }
}