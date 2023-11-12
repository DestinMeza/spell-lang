namespace Spell
{
    public sealed class TextLine 
    {
        public SourceText Text { get; private set; }
        public int Start { get; }
        public int End => Start + Length;
        public int Length { get; }
        public int LengthincludingLineBreak { get; }
        public TextSpan Span => new TextSpan(Start, Length);
        public TextSpan SpanIncludingLineBreak => new TextSpan(Start, LengthincludingLineBreak);

        public override string ToString() => Text.ToString(Span);
        public TextLine(SourceText text, int start, int length, int lengthIncludingLineBreak) 
        {
            Text = text;
            Start = start;
            Length = length;
            LengthincludingLineBreak = lengthIncludingLineBreak;
        }
    }
}
