using System;

namespace SpellCompiler.SpellReader.Binding
{
    internal sealed class BoundLiteralExpression : BoundExpressionNode 
    {
        public override BoundNodeKind Kind => BoundNodeKind.LiteralExpression;
        public override Type Type => Value.GetType();
        public object Value { get; }
        public BoundLiteralExpression(object value) 
        {
            Value = value;
        }
    }
}