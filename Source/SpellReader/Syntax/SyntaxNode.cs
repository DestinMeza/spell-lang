using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Spell.Syntax
{
    public abstract class SyntaxNode : INodeable
    {
        public abstract string Text { get; set; }
        public abstract SyntaxKind SyntaxKind { get; }
        public virtual TextSpan Span
        {
            get
            {
                var first = GetChildren().First().Span;
                var last = GetChildren().Last().Span;
                return TextSpan.FromBounds(first.Start, last.End);
            }
        }

        public IEnumerable<INodeable> GetChildren()
        {
            var properties = GetType().GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);

            foreach (var property in properties)
            {
                if (typeof(SyntaxNode).IsAssignableFrom(property.PropertyType))
                {
                    var child = (SyntaxNode)property.GetValue(this);
                    yield return child;
                }
                else if (typeof(IEnumerable<SyntaxNode>).IsAssignableFrom(property.PropertyType)) 
                {
                    var children = (IEnumerable<SyntaxNode>)property.GetValue(this);
                    foreach (var child in children) 
                    {
                        yield return child;
                    }
                }
            }
        }

        public void WriteTo(TextWriter writer) 
        {
            WriteTree(writer, this);
        }

        private static void WriteTree(TextWriter writer, INodeable node, string indent = "", bool isLast = true)
        {
            var marker = isLast ? "`--" : "|--";

            writer.Write(indent);
            writer.Write(marker);

            if (node == null)
            {
                writer.Write("null");
                return;
            }

            writer.Write(node.SyntaxKind);

            if (node is SyntaxToken t && t.Text != null)
            {
                writer.Write(": ");
                writer.Write($"\"{t.Text}\"");
            }

            writer.WriteLine();

            indent += isLast ? "   " : "|  ";

            var lastChild = node.GetChildren().LastOrDefault();

            foreach (var child in node.GetChildren())
            {
                WriteTree(writer, child, indent, child == lastChild);
            }
        }

        public override string ToString()
        {
            using (var writer = new StringWriter()) 
            {
                WriteTo(writer);
                return writer.ToString();
            }
        }
    }
}