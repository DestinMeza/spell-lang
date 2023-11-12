using System.Collections.Generic;
using System.Linq;

namespace Spell.Syntax
{
    public class SyntaxToken : SyntaxNode
    {
        public override SyntaxKind SyntaxKind { get; }
        public override string Text { get; set; }
        public int Position { get; private set; }
        public object Value { get; private set; }
        public override TextSpan Span => new TextSpan(Position, Text.Length);

        public SyntaxToken(SyntaxKind syntaxKind, int position, string text, object value)
        {
            SyntaxKind = syntaxKind;
            Position = position;
            Text = text;
            Value = value;
        }
    }
}