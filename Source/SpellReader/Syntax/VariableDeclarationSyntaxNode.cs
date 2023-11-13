namespace Spell.Syntax
{
    public sealed class VariableDeclarationSyntaxNode : StatementSyntaxNode 
    {
        public override string Text { get => throw new System.NotImplementedException(); set { } }
        public override SyntaxKind SyntaxKind => SyntaxKind.VariableDeclaration;

        public SyntaxToken Keyword { get; }
        public SyntaxToken Identifier { get; }
        public SyntaxToken EqualsToken { get; }
        public ExpressionSyntaxNode Initalizer { get; }

        public VariableDeclarationSyntaxNode(SyntaxToken keyword, SyntaxToken identifier, SyntaxToken equalsToken, ExpressionSyntaxNode initalizer) 
        {
            Keyword = keyword;
            Identifier = identifier;
            EqualsToken = equalsToken;
            Initalizer = initalizer;
        }
    }
}