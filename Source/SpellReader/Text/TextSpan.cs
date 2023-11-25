using System;
using System.Diagnostics;
using System.Text.RegularExpressions;
using static System.Net.Mime.MediaTypeNames;

namespace Spell
{
    public struct TextSpan
    {
        public int Start { get; }
        public int Length { get; }
        public int End => Start + Length;
        public TextSpan(int start, int length)
        {
            Start = start;
            Length = length;
        }

        public override string ToString()
        {
            return $"({Start} : {End})";
        }

        public static TextSpan FromBounds(int start, int end)
        {
            var length = end - start;
            return new TextSpan(start, length);
        }

        public static bool TryFindSpan(string text, out TextSpan textSpan)
        {
            textSpan = default;

            var match = Regex.Match(text, "\\(\\d+\\s:\\s\\d+\\)");

            if (match == null || !match.Success) 
            {
                Diagnostics.LogErrorMessage($"Found no match in Regex {text}");
                return false;
            }

            int startingIndex = match.Index;
            int endingIndex = match.Length;
            string spanText = text.Substring(startingIndex, endingIndex);

            if (!TryParseFromString(spanText, out textSpan)) 
            {
                Diagnostics.LogErrorMessage($"Could not parse {spanText} to Span.");
            }

            return true;
        }

        public static bool TryParseFromString(string spanText, out TextSpan textSpan) 
        {
            textSpan = default;

            spanText = spanText.Replace(" ", string.Empty);

            int middleIndex = spanText.IndexOf(":");

            string startString = spanText.Substring(1, middleIndex - 1);

            string endString = spanText.Substring(middleIndex + 1, spanText.Length - middleIndex - 2);

            if (!int.TryParse(startString, out int start)) 
            {
                Diagnostics.LogErrorMessage($"\"{spanText}\" could not be parsed as a Span Object");
                return false;
            }
            if (!int.TryParse(endString, out int end))
            {
                Diagnostics.LogErrorMessage($"\"{spanText}\" could not be parsed as a Span Object");
                return false;
            }

            textSpan = new TextSpan(start, end);

            return true;
        }
    }
}