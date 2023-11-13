﻿using System.Collections.Generic;
using System.Linq;

namespace Spell.Syntax
{
    public sealed class SyntaxTree 
    {
        public SourceText Text { get; }
        public CompilationUnitSyntax Root { get; }
        public SyntaxToken EndOfFileToken { get; }

        private SyntaxTree(SourceText text) 
        {
            var parser = new Parser(text);
            var root = parser.ParseCompilationUnit();

            Text = text;
            Root = root;
        }

        public static SyntaxTree Parse(string text) 
        {
            var sourceText = SourceText.From(text);
            return Parse(sourceText);
        }
        public static SyntaxTree Parse(SourceText text)
        {
            return new SyntaxTree(text);
        }

        public static IEnumerable<SyntaxToken> ParseTokens(string text) 
        {
            var sourceText = SourceText.From(text);
            return ParseTokens(sourceText);
        }

        public static IEnumerable<SyntaxToken> ParseTokens(SourceText text)
        {
            var lexer = new Lexer(text);
            while (true)
            {
                var token = lexer.Lex();
                if (token.SyntaxKind == SyntaxKind.EndOfFileToken)
                {
                    break;
                }

                yield return token;
            }
        }
    }
}