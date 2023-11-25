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

        public void WriteTo(TextWriter writer, Dictionary<VariableSymbol, object> variables = null) 
        {
            WriteTree(writer, this, variables);
        }

        private static void WriteTree(TextWriter writer, INodeable node, Dictionary<VariableSymbol, object> variables = null, string indent = "", bool isLast = true)
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
                writer.Write($"Text: \"{t.Text}\"");


                object value = null;

                if (variables != null)
                {
                    var keyRef = variables.Keys.Where(x => x.Name == t.Text).Select(x => x).FirstOrDefault();

                    if (keyRef != null)
                    {
                        variables.TryGetValue(keyRef, out value);
                        writer.Write($" Value: \"{value}\"");
                    }
                }
            }

            writer.WriteLine();

            indent += isLast ? "   " : "|  ";

            var lastChild = node.GetChildren().LastOrDefault();

            foreach (var child in node.GetChildren())
            {
                WriteTree(writer, child, variables, indent, isLast: child == lastChild);
            }
        }

        public string ToString(Dictionary<VariableSymbol, object> variables) 
        {
            using (var writer = new StringWriter())
            {
                WriteTo(writer, variables);
                return writer.ToString();
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