using System;
using System.Collections.Generic;
using System.Text;

namespace Spell
{
    public sealed class SourceText
    {
        private readonly string _text;
        public readonly TextLine[] Lines;
        public char this[int index] => _text[index];
        public int Length => _text.Length;
        public override string ToString() => _text;
        public string ToString(int start, int length) => _text.Substring(start, length);
        public string ToString(TextSpan span) => _text.Substring(span.Start, span.Length);

        private SourceText(string text) 
        {
            _text = text;
            Lines = ParseLines(this, text);
        }

        public int GetLineIndex(int position) 
        {
            var lower = 0;
            var upper = Lines.Length - 1;

            while (lower <= upper) 
            {
                var index = lower + (upper - lower) / 2;
                var start = Lines[index].Start;

                if (position == start) 
                {
                    return index;
                }

                if (start > position)
                {
                    upper = index - 1;
                }
                else 
                {
                    lower = index + 1;
                }
            }

            return lower - 1;
        }

        private static TextLine[] ParseLines(SourceText sourceText, string text) 
        {
            List<TextLine> textLines = new List<TextLine>();

            var position = 0;
            var lineStart = 0;

            while(position < text.Length)
            {
                var lineBreakWidth = GetLineBreakWidth(text, position);

                if (lineBreakWidth == 0)
                {
                    position++;
                }
                else
                {
                    AddLine(textLines, sourceText, position, lineStart, lineBreakWidth);

                    position += lineBreakWidth;
                    lineStart = position;
                }
            }

            if (position >= lineStart)
            {
                AddLine(textLines, sourceText, position, lineStart, 0);
            }

            return textLines.ToArray();
        }

        private static void AddLine(List<TextLine> textLines, SourceText sourceText, int position, int lineStart, int lineBreakWidth)
        {
            var lineLength = position - lineStart;
            var lineLengthIncludingLineBreak = lineLength + lineBreakWidth;
            var line = new TextLine(sourceText, lineStart, lineLength, lineLengthIncludingLineBreak);

            textLines.Add(line);
        }

        private static int GetLineBreakWidth(string text, int i) 
        {
            var current = text[i];
            var lookahead = i + 1 >= text.Length ? '\0' : text[i + 1];

            if (current == '\r' && lookahead == '\n') 
            {
                return 2;
            }
            if (current == '\r' || lookahead == '\n')
            {
                return 1;
            }
            return 0;
        }

        public static SourceText From(string text) 
        {
            return new SourceText(text);
        }

    }
}
