using System;
using System.Collections.Generic;

namespace Spell.Syntax
{

    internal sealed class Parser
    {
        private SyntaxToken CurrentSyntaxToken => GetTokenAtOffset(0);
        private TextLine CurrentLine => _text.Lines[_text.GetLineIndex(_position)];

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

            Diagnostics.LogErrorMessage($"Error: {CurrentLine.Span} Unexpected token <{CurrentSyntaxToken.SyntaxKind}>, expected <{syntaxKind}>\n\t" +
                $"{CurrentLine}");

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

        public CompilationUnitSyntax ParseCompilationUnit()
        {
            var statement = ParseStatement();
            var endOfFileToken = StrictMatch(SyntaxKind.EndOfFileToken);

            return new CompilationUnitSyntax(statement, endOfFileToken);
        }

        private StatementSyntaxNode ParseStatement()
        {
            switch (CurrentSyntaxToken.SyntaxKind)
            {
                case SyntaxKind.OpenBraceToken:
                    return ParseBlockStatement();
                case SyntaxKind.LetKeyword:
                case SyntaxKind.VarKeyword:
                    return ParseVariableDeclaration();
            }

            return ParseExpressionStatement();
        }

        private VariableDeclarationSyntaxNode ParseVariableDeclaration()
        {
            var expected = CurrentSyntaxToken.SyntaxKind == SyntaxKind.LetKeyword ?  SyntaxKind.LetKeyword : SyntaxKind.VarKeyword;
            var keyword = MatchCurrentIncrement(expected);
            var identifer = MatchCurrentIncrement(SyntaxKind.IdentifierToken);
            var equals = MatchCurrentIncrement(SyntaxKind.EqualsToken);
            var initalizer = ParseExpression();

            return new VariableDeclarationSyntaxNode(keyword, identifer, equals, initalizer);
        }

        private BlockStatementSyntaxNode ParseBlockStatement() 
        {
            var statements = new List<StatementSyntaxNode>();

            var openBraceToken = MatchCurrentIncrement(SyntaxKind.OpenBraceToken);

            while (CurrentSyntaxToken.SyntaxKind != SyntaxKind.EndOfFileToken &&
                   CurrentSyntaxToken.SyntaxKind != SyntaxKind.CloseBraceToken) 
            {
                var statement = ParseStatement();
                statements.Add(statement);
            }

            var closeBraceToken = MatchCurrentIncrement(SyntaxKind.CloseBraceToken);

            return new BlockStatementSyntaxNode(openBraceToken, statements, closeBraceToken);
        }

        private StatementSyntaxNode ParseExpressionStatement() 
        {
            var expression = ParseExpression();
            return new ExpressionStatementSyntaxNode(expression);
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
                    Diagnostics.LogErrorMessage($"Error: {CurrentLine.Span} Unexpected SyntaxKind <{CurrentSyntaxToken.SyntaxKind}>, the following is not parsable.\n\t" +
                        $"{CurrentLine}");
                    return null;
            };
        }

        private LiteralExpressionSyntaxNode ParseNumberExpression()
        {
            var numberToken = MatchCurrentIncrement(SyntaxKind.NumberToken);

            if (string.IsNullOrEmpty(numberToken.Text) || numberToken.Value == null)
            {
                Diagnostics.LogErrorMessage($"Error:  {CurrentLine.Span} Unexpected token <{CurrentSyntaxToken.SyntaxKind}>, expected <{SyntaxKind.NumberToken}>\n\t" +
                        $"{CurrentLine}");
            }

            return new LiteralExpressionSyntaxNode(numberToken);
        }

        private ParenthesizedExpressionSytanxNode ParseParenthesizedExpression()
        {
            var leftToken = MatchCurrentIncrement(SyntaxKind.OpenParenthesisToken);

            if (string.IsNullOrEmpty(leftToken.Text))
            {
                Diagnostics.LogErrorMessage($"Error:  {CurrentLine.Span} Unexpected token <{CurrentSyntaxToken.SyntaxKind}>, expected <{SyntaxKind.OpenParenthesisToken}>\n\t" +
                        $"{CurrentLine}");
            }

            var expression = ParseExpression();

            var rightToken = MatchCurrentIncrement(SyntaxKind.CloseParenthesisToken);

            if (string.IsNullOrEmpty(rightToken.Text))
            {
                Diagnostics.LogErrorMessage($"Error: {CurrentLine.Span} Unexpected token <{CurrentSyntaxToken.SyntaxKind}>, expected <{SyntaxKind.CloseParenthesisToken}>\n\t" +
                        $"{CurrentLine}");
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
