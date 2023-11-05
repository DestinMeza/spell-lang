using System;
using System.Collections.Generic;

namespace Spell.Syntax
{

    internal sealed class Parser
    {
        private SyntaxToken CurrentSyntaxToken => GetTokenAtOffset(0);

        private readonly SyntaxToken[] _tokens;

        private int _position;

        public Parser(string text)
        {
            var tokens = new List<SyntaxToken>();

            var lexer = new Lexer(text);
            SyntaxToken token;
            do
            {
                token = lexer.Lex();

                switch (token.SyntaxKind)
                {
                    case SyntaxKind.BadToken:
                    case SyntaxKind.WhitespaceToken:
                    case SyntaxKind.NullToken:
                        break;
                    default:
                        tokens.Add(token);
                        break;
                }
            } while (token.SyntaxKind != SyntaxKind.EndOfLineToken);

            _tokens = tokens.ToArray();
        }

        private SyntaxToken GetTokenAtOffset(int offset)
        {
            var index = _position + offset;

            if (index >= _tokens.Length)
            {
                return _tokens[_tokens.Length - 1];
            }

            return _tokens[index];
        }

        private SyntaxToken IncrementPointer() 
        {
            return GetTokenAtOffset(++_position);
        }

        private SyntaxToken NextToken() 
        {
            var current = CurrentSyntaxToken;
            IncrementPointer();
            return current;
        }

        private SyntaxToken StrictMatch(SyntaxKind syntaxKind) 
        {
            if (CurrentSyntaxToken.SyntaxKind == syntaxKind)
            {
                return NextToken();
            }

            Diagnostics.LogErrorMessage($"ERROR: Unexpected token <{CurrentSyntaxToken.SyntaxKind}>, expected <{syntaxKind}>");

            return new SyntaxToken(syntaxKind, CurrentSyntaxToken.Position, null, null);
        }

        private SyntaxToken MatchCurrentIncrement(SyntaxKind syntaxKind)
        {
            if (CurrentSyntaxToken.SyntaxKind == syntaxKind)
            {
                return NextToken();
            }

            return new SyntaxToken(syntaxKind, CurrentSyntaxToken.Position, null, null);
        }

        public SyntaxTree Parse()
        {
            var expression = ParseExpression();
            var endOfFileToken = StrictMatch(SyntaxKind.EndOfLineToken);

            return new SyntaxTree(expression, endOfFileToken);
        }

        private ExpressionSyntaxNode ParseExpression(int parentPrecedence = 0)
        {
            ExpressionSyntaxNode left;
            var unaryOperatorPrecdence = CurrentSyntaxToken.SyntaxKind.GetUnaryOperatorPrecedence();
            if (unaryOperatorPrecdence != 0 && unaryOperatorPrecdence >= parentPrecedence)
            {
                var operatorToken = NextToken();
                var operand = ParseExpression(unaryOperatorPrecdence);
                left = new UnaryExpressionSyntax(operatorToken, operand);
            }
            else 
            {
                left = ParsePrimaryExpression();
            }

            while (true)
            {
                var precedence = CurrentSyntaxToken.SyntaxKind.GetBinaryOperatorPrecedence();
                if (precedence == 0 || precedence < parentPrecedence)
                {
                    break;
                }

                var operatorToken = NextToken();
                var right = ParseExpression(precedence);
                left = new BinaryExpressionSyntaxNode(left, operatorToken, right);
            }

            return left;
        }

        private ExpressionSyntaxNode ParsePrimaryExpression()
        {
            ParenthesizedExpressionSytanxNode ParseParenthesizedExpression()
            {
                var leftToken = MatchCurrentIncrement(SyntaxKind.OpenParenthesisToken);

                if (string.IsNullOrEmpty(leftToken.Text))
                {
                    Diagnostics.LogErrorMessage($"ERROR: Unexpected token <{CurrentSyntaxToken.SyntaxKind}>, expected <{SyntaxKind.OpenParenthesisToken}>");
                }

                var expression = ParseExpression();

                var rightToken = MatchCurrentIncrement(SyntaxKind.CloseParenthesisToken);

                if (string.IsNullOrEmpty(rightToken.Text))
                {
                    Diagnostics.LogErrorMessage($"ERROR: Unexpected token <{CurrentSyntaxToken.SyntaxKind}>, expected <{SyntaxKind.CloseParenthesisToken}>");
                }

                return new ParenthesizedExpressionSytanxNode(leftToken, expression, rightToken);
            }

            LiteralExpressionSyntaxNode ParseTrueOrFalseKeyword() 
            {
                var keywordToken = NextToken();
                var value = keywordToken.SyntaxKind == SyntaxKind.TrueKeyword;
                return new LiteralExpressionSyntaxNode(keywordToken, value);
            }

            LiteralExpressionSyntaxNode ParseNumberExpression()
            {
                var numberToken = MatchCurrentIncrement(SyntaxKind.NumberToken);

                if (string.IsNullOrEmpty(numberToken.Text) || numberToken.Value == null)
                {
                    Diagnostics.LogErrorMessage($"ERROR: Unexpected token <{CurrentSyntaxToken.SyntaxKind}>, expected <{SyntaxKind.NumberToken}>");
                }

                return new LiteralExpressionSyntaxNode(numberToken);
            }

            switch (CurrentSyntaxToken.SyntaxKind)
            {
                case SyntaxKind.NumberToken: return ParseNumberExpression();
                case SyntaxKind.OpenParenthesisToken: return ParseParenthesizedExpression();
                case SyntaxKind.FalseKeyword: return ParseTrueOrFalseKeyword();
                case SyntaxKind.TrueKeyword: return ParseTrueOrFalseKeyword();
                default: throw new NotSupportedException();
            };
        }
    }
}
