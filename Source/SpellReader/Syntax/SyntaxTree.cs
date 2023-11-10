using System.Collections.Generic;
using System.Linq;

namespace Spell.Syntax
{
    public sealed class SyntaxTree 
    {
        public ExpressionSyntaxNode Root { get; }
        public SyntaxToken EndOfFileToken { get; }

        public SyntaxTree(ExpressionSyntaxNode root, SyntaxToken endOfFileToken) 
        {
            Root = root;
            EndOfFileToken = endOfFileToken;
        }

        public static SyntaxTree Parse(string text) 
        {
            var parser = new Parser(text);
            return parser.Parse();
        }

        public static IEnumerable<SyntaxToken> ParseTokens(string text) 
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