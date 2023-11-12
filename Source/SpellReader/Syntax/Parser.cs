using System;
using System.Collections.Generic;

namespace Spell.Syntax
{

    internal sealed class Parser
    {
        private SyntaxToken CurrentSyntaxToken => GetTokenAtOffset(0);

        private readonly SourceText _text;
        private readonly SyntaxToken[] _tokens;
        private int _position;

        public Parser(SourceText text)
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
            } while (token.SyntaxKind != SyntaxKind.EndOfFileToken);

            _text = text;
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
            var endOfFileToken = StrictMatch(SyntaxKind.EndOfFileToken);

            return new SyntaxTree(_text, expression, endOfFileToken);
        }

        private ExpressionSyntaxNode ParseExpression() 
        {
            return ParseAssignmentExpression();
        }

        //TODO implement Statements. Statement will be the proper way to handle this "assignment" logic.
        //Uses a Right to Left tree reading model instead of a left to right.
        private ExpressionSyntaxNode ParseAssignmentExpression() 
        {
            if (GetTokenAtOffset(0).SyntaxKind == SyntaxKind.IdentifierToken &&
               GetTokenAtOffset(1).SyntaxKind == SyntaxKind.EqualsToken) 
            {
                var identiferToken = NextToken();
                var operatorToken = NextToken();
                var right = ParseAssignmentExpression();
                return new AssignmentExpressionSyntaxNode(identiferToken, operatorToken, right);
            }

            return ParseBinaryExpression();
        }

        private ExpressionSyntaxNode ParseBinaryExpression(int parentPrecedence = 0)
        {
            ExpressionSyntaxNode left;
            var unaryOperatorPrecdence = CurrentSyntaxToken.SyntaxKind.GetUnaryOperatorPrecedence();
            if (unaryOperatorPrecdence != 0 && unaryOperatorPrecdence >= parentPrecedence)
            {
                var operatorToken = NextToken();
                var operand = ParseBinaryExpression(unaryOperatorPrecdence);
                left = new UnaryExpressionSyntax(operatorToken, operand);
            }
            else 
            {
                left = ParsePrimaryExpression();
            }

            while (true)
            {
                var precedence = CurrentSyntaxToken.SyntaxKind.GetBinaryOperatorPrecedence();
                if (precedence == 0 || precedence <= parentPrecedence)
                {
                    break;
                }

                var operatorToken = NextToken();
                var right = ParseBinaryExpression(precedence);
                left = new BinaryExpressionSyntaxNode(left, operatorToken, right);
            }

            return left;
        }

        private ExpressionSyntaxNode ParsePrimaryExpression()
        {
            switch (CurrentSyntaxToken.SyntaxKind)
            {
                case SyntaxKind.NumberToken:            return ParseNumberExpression();
                case SyntaxKind.OpenParenthesisToken:   return ParseParenthesizedExpression();
                case SyntaxKind.FalseKeyword:           return ParseBooleanLiteral();
                case SyntaxKind.TrueKeyword:            return ParseBooleanLiteral();
                case SyntaxKind.IdentifierToken:        return ParseNameExpression();
                default:
                    Diagnostics.LogErrorMessage($"ERROR: Unexpected SyntaxKind <{CurrentSyntaxToken.SyntaxKind}>, the following is not parsable.");
                    return null;
            };
        }

        private LiteralExpressionSyntaxNode ParseNumberExpression()
        {
            var numberToken = MatchCurrentIncrement(SyntaxKind.NumberToken);

            if (string.IsNullOrEmpty(numberToken.Text) || numberToken.Value == null)
            {
                Diagnostics.LogErrorMessage($"ERROR: Unexpected token <{CurrentSyntaxToken.SyntaxKind}>, expected <{SyntaxKind.NumberToken}>");
            }

            return new LiteralExpressionSyntaxNode(numberToken);
        }

        private ParenthesizedExpressionSytanxNode ParseParenthesizedExpression()
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

        private LiteralExpressionSyntaxNode ParseBooleanLiteral()
        {
            var isTrue = CurrentSyntaxToken.SyntaxKind == SyntaxKind.TrueKeyword;
            var keywordToken = isTrue ? MatchCurrentIncrement(SyntaxKind.TrueKeyword) : MatchCurrentIncrement(SyntaxKind.FalseKeyword);
            return new LiteralExpressionSyntaxNode(keywordToken, isTrue);
        }

        private NameExpressionSyntaxNode ParseNameExpression()
        {
            var identifierToken = NextToken();
            return new NameExpressionSyntaxNode(identifierToken);
        }
    }
}
