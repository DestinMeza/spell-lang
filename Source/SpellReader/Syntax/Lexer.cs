using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;

namespace Spell.Syntax
{
    internal sealed class Lexer
    {
        private TextLine CurrentLine => _text.Lines[_text.GetLineIndex(_position)];

        private readonly SourceText _text;
        private SyntaxKind _syntaxKind;

        private int _position;
        private int _start;
        private object _value;

        public Lexer(SourceText text)
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

        public SyntaxToken Lex()
        {
            _start = _position;
            _syntaxKind = SyntaxKind.BadToken;
            _value = null;

            switch (Current)
            {
                case '\0':
                    _syntaxKind = SyntaxKind.EndOfFileToken;
                    break;
                case '+':
                    _syntaxKind = SyntaxKind.PlusToken;
                    _position++;
                    break;
                case '-':
                    _syntaxKind = SyntaxKind.MinusToken;
                    _position++;
                    break;
                case '*':
                    _syntaxKind = SyntaxKind.StarToken;
                    _position++;
                    break;
                case '/':
                    _syntaxKind = SyntaxKind.SlashToken;
                    _position++;
                    break;
                case '(':
                    _syntaxKind = SyntaxKind.OpenParenthesisToken;
                    _position++;
                    break;
                case ')':
                    _syntaxKind = SyntaxKind.CloseParenthesisToken;
                    _position++;
                    break;
                case '{':
                    _syntaxKind = SyntaxKind.OpenBraceToken;
                    _position++;
                    break;
                case '}':
                    _syntaxKind = SyntaxKind.CloseBraceToken;
                    _position++;
                    break;
                case '&':
                    if (Lookahead == '&')
                    {
                        _syntaxKind = SyntaxKind.AmpersandAmpersandToken;
                        _position += 2;
                        break;
                    }
                    break;
                case '|':
                    if (Lookahead == '|')
                    {
                        _syntaxKind = SyntaxKind.PipePipeToken;
                        _position += 2;
                    }
                    break;
                case '=':
                    _position++;
                    if (Current != '=')
                    {
                        _syntaxKind = SyntaxKind.EqualsToken;
                    }
                    else
                    {
                        _syntaxKind = SyntaxKind.EqualsEqualsToken;
                        _position++;
                    }
                    break;

                case '!':
                    _position++;
                    if (Current != '=')
                    {
                        _syntaxKind = SyntaxKind.BangToken;
                    }
                    else
                    {
                        _syntaxKind = SyntaxKind.BangEqualsToken;
                        _position++;
                    }
                    break;
                case '<':
                    _position++;
                    if (Current != '=')
                    {
                        _syntaxKind = SyntaxKind.LessToken;
                    }
                    else
                    {
                        _syntaxKind = SyntaxKind.LessOrEqualsToken;
                        _position++;
                    }
                    break;
                case '>':
                    _position++;
                    if (Current != '=')
                    {
                        _syntaxKind = SyntaxKind.GreaterToken;
                    }
                    else
                    {
                        _syntaxKind = SyntaxKind.GreaterOrEqualsToken;
                        _position++;
                    }
                    break;
                case '0': case '1': case '2': case '3': case '4': 
                case '5': case '6': case '7': case '8': case '9':
                    ReadNumberToken();
                    break;
                case ' ': case '\t': case '\n': case '\r':
                    ReadWhiteSpaceToken();
                    break;
                default:
                    if (char.IsLetter(Current))
                    {
                        ReadIdentiferOrKeyword();
                    }
                    else if (char.IsWhiteSpace(Current))
                    {
                        ReadWhiteSpaceToken();
                    }
                    else 
                    {
                        Diagnostics.LogErrorMessage($"Error: {CurrentLine.Span} bad character input: '{Current}'\n\t" +
                            $"{CurrentLine}", CurrentLine.Span);
                        _position++;
                    }
                    break;
            }

            var length = _position - _start;
            var text = SyntaxFacts.GetText(_syntaxKind);
            if (text == null) 
            {
                text = _text.ToString(_start, length);
            }

            return new SyntaxToken(_syntaxKind, _start, text, _value);
        }

        private void ReadWhiteSpaceToken()
        {
            while (char.IsWhiteSpace(Current))
            {
                _position++;
            }

            _syntaxKind = SyntaxKind.WhitespaceToken;
        }

        private void ReadNumberToken() 
        {
            while (char.IsDigit(Current))
            {
                _position++;
            }

            var length = _position - _start;
            var text = _text.ToString(_start, length);
            if (!int.TryParse(text, out int value))
            {
                Diagnostics.LogErrorMessage($"Error: {CurrentLine.Span} The number {text} isn't a valid Int32." +
                    $"\n\t{CurrentLine}", CurrentLine.Span);
            }

            _value = value;
            _syntaxKind = SyntaxKind.NumberToken;
        }

        private void ReadIdentiferOrKeyword()
        {
            while (char.IsLetter(Current))
            {
                _position++;
            }

            var length = _position - _start;
            var text = _text.ToString(_start, length);
            _syntaxKind = SyntaxFacts.GetKeywordKind(text);
        }
    }
}
