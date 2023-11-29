using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Security;

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

            throw new Exception($"Error: {CurrentLine.Span} Unexpected token <{CurrentSyntaxToken.SyntaxKind}>, expected <{syntaxKind}>\n\t" +
                $"{CurrentLine}");
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
                case SyntaxKind.IfKeyword:
                    return ParseIfStatement();
                case SyntaxKind.WhileKeyword:
                    return ParseWhileStatement();
                case SyntaxKind.ForKeyword:
                    return ParseForStatement();
                default:
                    break;
            }

            return ParseExpressionStatement();
        }

        private StatementSyntaxNode ParseWhileStatement()
        {
            var keyword = MatchCurrentIncrement(SyntaxKind.WhileKeyword);
            var condition = ParseExpression();
            var body = ParseStatement();
            return new WhileStatementSyntaxNode(keyword, condition, body);
        }

        private StatementSyntaxNode ParseForStatement() 
        {
            var keyword = MatchCurrentIncrement(SyntaxKind.ForKeyword);
            var identifer = MatchCurrentIncrement(SyntaxKind.IdentifierToken);
            var equalsToken = MatchCurrentIncrement(SyntaxKind.EqualsToken);
            var lowerBound = ParseExpression();
            var toKeyword = MatchCurrentIncrement(SyntaxKind.ToKeyword);
            var upperBound = ParseExpression();
            var body = ParseStatement();
            return new ForStatementSyntaxNode(keyword, identifer, equalsToken, lowerBound, toKeyword, upperBound, body);
        }

        private StatementSyntaxNode ParseIfStatement() 
        {
            var keyword = MatchCurrentIncrement(SyntaxKind.IfKeyword);
            var condition = ParseExpression();
            var statement = ParseStatement();
            var elseClause = ParseElseClause();
            return new IfStatmentSyntaxNode(keyword, condition, statement, elseClause);
        }

        private ElseClauseSyntax ParseElseClause() 
        {
            if (CurrentSyntaxToken.SyntaxKind != SyntaxKind.ElseKeyword) 
            {
                return null;
            }

            var keyword = NextToken();
            var statement = ParseStatement();
            return new ElseClauseSyntax(keyword, statement);
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
                var startToken = CurrentSyntaxToken;

                try
                {
                    var statement = ParseStatement();
                    statements.Add(statement);

                    //If token is not consuming to next token, force skip.
                    if (CurrentSyntaxToken == startToken)
                    {
                        NextToken();
                    }
                }
                catch (Exception e)
                {
                    string spanText = "";
                    if (TextSpan.TryFindSpan(e.Message, out TextSpan textSpan))
                    {
                        spanText = textSpan.ToString();
                    }

                    throw new Exception($"{spanText} Failed to parse statement in block.\n\t" +
                    $"{CurrentLine.Text}");
                }
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
                    throw new Exception($"{CurrentLine.Span} Unexpected SyntaxKind <{CurrentSyntaxToken.SyntaxKind}>, the following is not parsable.\n\t" +
                        $"{CurrentLine}");
            };
        }

        private LiteralExpressionSyntaxNode ParseNumberExpression()
        {
            var numberToken = MatchCurrentIncrement(SyntaxKind.NumberToken);

            if (string.IsNullOrEmpty(numberToken.Text) || numberToken.Value == null)
            {
                throw new Exception($"{CurrentLine.Span} Unexpected token <{CurrentSyntaxToken.SyntaxKind}>, expected <{SyntaxKind.NumberToken}>\n\t" +
                        $"{CurrentLine}");
            }

            return new LiteralExpressionSyntaxNode(numberToken);
        }

        private ParenthesizedExpressionSytanxNode ParseParenthesizedExpression()
        {
            var leftToken = MatchCurrentIncrement(SyntaxKind.OpenParenthesisToken);

            if (string.IsNullOrEmpty(leftToken.Text))
            {
                throw new Exception($"Error: {CurrentLine.Span} Unexpected token <{CurrentSyntaxToken.SyntaxKind}>, expected <{SyntaxKind.OpenParenthesisToken}>\n\t" +
                        $"{CurrentLine}");
            }

            var expression = ParseExpression();

            var rightToken = MatchCurrentIncrement(SyntaxKind.CloseParenthesisToken);

            if (string.IsNullOrEmpty(rightToken.Text))
            {
                throw new Exception($"Error: {CurrentLine.Span} Unexpected token <{CurrentSyntaxToken.SyntaxKind}>, expected <{SyntaxKind.CloseParenthesisToken}>\n\t" +
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
