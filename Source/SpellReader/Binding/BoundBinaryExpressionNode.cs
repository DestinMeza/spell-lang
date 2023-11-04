using System;

namespace SpellCompiler.SpellReader.Binding
{

    internal sealed class BoundBinaryExpressionNode : BoundExpressionNode
    {
        public BoundExpressionNode Left { get; set; }
        public BoundBinaryOperator BoundOperator { get; set; }
        public BoundExpressionNode Right { get; set; }

        public override Type Type => BoundOperator.ResultType;
        public override BoundNodeKind Kind => BoundNodeKind.BinaryExpression;
        public BoundBinaryExpressionNode(BoundExpressionNode left, BoundBinaryOperator boundOperator, BoundExpressionNode right)
        {
            Left = left;
            BoundOperator = boundOperator;
            Right = right;
        }
    }
}