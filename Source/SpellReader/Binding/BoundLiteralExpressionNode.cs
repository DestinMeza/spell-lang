using System;

namespace Spell.Binding
{
    internal sealed class BoundLiteralExpressionNode : BoundExpressionNode 
    {
        public override BoundNodeKind Kind => BoundNodeKind.LiteralExpression;
        public override Type Type => Value.GetType();
        public object Value { get; }
        public BoundLiteralExpressionNode(object value) 
        {
            Value = value;
        }
    }
}