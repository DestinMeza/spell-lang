using System.Collections.Generic;
using System.Linq;

namespace Spell.Syntax
{
    public class SyntaxToken : INodeable
    {
        public SyntaxKind SyntaxKind { get; private set; }
        public int Position { get; private set; }
        public string Text { get; private set; }
        public object Value { get; private set; }
        public TextSpan Span => new TextSpan(Position, Text.Length);

        public SyntaxToken(SyntaxKind syntaxKind, int position, string text, object value)
        {
            SyntaxKind = syntaxKind;
            Position = position;
            Text = text;
            Value = value;
        }

        public IEnumerable<INodeable> GetChildren() 
        {
            return Enumerable.Empty<INodeable>();
        }

    }
}