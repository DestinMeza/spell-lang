using System.Collections;
using System.Collections.Generic;

namespace Spell.Syntax
{
    internal sealed class Lexer
    {
        private readonly string _text;
        private int _position;
        public Lexer(string text)
        {
            _text = text;
        }

        private char Current => Peek(0);
        private char Lookahead => Peek(1);

        private char Peek(int offset) 
        {
            var index = _position + offset;
            if (index >= _text.Length) return '\0';

            return _text[index];
        }

        private void Next()
        {
            _position++;
        }

        public SyntaxToken Lex()
        {
            //<numbers>
            // + - * / ( )
            //<whitespace>

            if (_position >= _text.Length)
            {
                return new SyntaxToken(SyntaxKind.EndOfLineToken, _position, "\0", null);
            }

            var start = _position;

            if (char.IsDigit(Current))
            {

                while (char.IsDigit(Current))
                {
                    Next();
                }

                var length = _position - start;
                var text = _text.Substring(start, length);
                if (!int.TryParse(text, out int value)) 
                {
                    Diagnostics.LogErrorMessage($"The number {_text} isn't valid Int32.");
                }

                return new SyntaxToken(SyntaxKind.NumberToken, start, text, value);
            }

            if (char.IsWhiteSpace(Current))
            {
                while (char.IsWhiteSpace(Current))
                {
                    Next();
                }

                var length = _position - start;
                var text = _text.Substring(start, length);
                return new SyntaxToken(SyntaxKind.WhitespaceToken, start, text, " ");
            }

            if (char.IsLetter(Current)) 
            {
                while (char.IsLetter(Current))
                {
                    Next();
                }

                var length = _position - start;
                var text = _text.Substring(start, length);
                var kind = SyntaxFacts.GetKeywordKind(text);
                return new SyntaxToken(kind, start, text, null);
            }

            SyntaxToken operationToken = null;

            switch (Current) 
            {
                case '+':
                    operationToken = new SyntaxToken(SyntaxKind.PlusToken, _position++, "+", null);
                    break;
                case '-':
                    operationToken = new SyntaxToken(SyntaxKind.MinusToken, _position++, "-", null);
                    break;
                case '*':
                    operationToken = new SyntaxToken(SyntaxKind.StarToken, _position++, "*", null);
                    break;
                case '/':
                    operationToken = new SyntaxToken(SyntaxKind.SlashToken, _position++, "/", null);
                    break;
                case '(':
                    operationToken = new SyntaxToken(SyntaxKind.OpenParenthesisToken, _position++, "(", null);
                    break;
                case ')':
                    operationToken = new SyntaxToken(SyntaxKind.CloseParenthesisToken, _position++, ")", null);
                    break;
                case '&':
                    if (Lookahead == '&') 
                    {
                        _position += 2;
                        operationToken = new SyntaxToken(SyntaxKind.AmpersandAmpersandToken, start, "&&", null);
                    }
                    break;
                case '|':
                    if (Lookahead == '|')
                    {
                        _position += 2;
                        operationToken = new SyntaxToken(SyntaxKind.PipePipeToken, start, "||", null);
                    }
                    break;
                case '=':
                    if (Lookahead == '=')
                    {
                        _position += 2;
                        operationToken = new SyntaxToken(SyntaxKind.EqualsEqualsToken, start, "==", null);
                    }
                    break;
                case '!':
                    if (Lookahead == '=')
                    {
                        _position += 2;
                        operationToken = new SyntaxToken(SyntaxKind.BangEqualsToken, start, "!=", null);
                    }
                    else 
                    {
                        _position += 1;
                        operationToken = new SyntaxToken(SyntaxKind.BangToken, start, "!", null);
                    }
                    break;
                default:
                    break;
            }


            if (operationToken != null) 
            {
                return operationToken;
            }

            Diagnostics.LogErrorMessage($"ERROR: bad character input: '{Current}'");
            return new SyntaxToken(SyntaxKind.BadToken, _position++, _text.Substring(_position - 1, 1), null);
        }

    }
}
