namespace Spell.Syntax
{
    public enum SyntaxKind
    {
        BadToken,
        EndOfLineToken,
        NullToken,

        WhitespaceToken,
        NumberToken,
        PlusToken,
        MinusToken,
        StarToken,
        SlashToken,
        OpenParenthesisToken,
        CloseParenthesisToken,
        EqualsEqualsToken,
        BangEqualsToken,
        AmpersandAmpersandToken,
        PipePipeToken,
        BangToken,
        IdentifierToken,

        //Keywords
        FalseKeyword,
        TrueKeyword,

        //Expressions
        LiteralExpressionToken,
        UnaryExpressionToken,
        BinaryExpressionToken,
        ParenthesizedExpressionToken,
    }

    internal static class SyntaxFacts 
    {
        public static int GetUnaryOperatorPrecedence(this SyntaxKind syntaxKind) 
        {
            switch (syntaxKind)
            {
                case SyntaxKind.PlusToken:
                case SyntaxKind.MinusToken:
                case SyntaxKind.BangToken:
                    return 6;

                default:
                    return 0;
            }
        }
        public static int GetBinaryOperatorPrecedence(this SyntaxKind syntaxKind)
        {
            switch (syntaxKind)
            {
                case SyntaxKind.StarToken:
                case SyntaxKind.SlashToken:
                    return 5;

                case SyntaxKind.PlusToken:
                case SyntaxKind.MinusToken:
                    return 4;

                case SyntaxKind.EqualsEqualsToken:
                case SyntaxKind.BangEqualsToken:
                    return 3;

                case SyntaxKind.AmpersandAmpersandToken:
                    return 2;

                case SyntaxKind.PipePipeToken:
                    return 1;

                default:
                    return 0;
            }
        }

        public static SyntaxKind GetKeywordKind(string text) 
        {
            switch (text) 
            {
                case "true":
                    return SyntaxKind.TrueKeyword;
                case "false":
                    return SyntaxKind.FalseKeyword;
                default:
                    return SyntaxKind.IdentifierToken;
            }
        }
    }
}
