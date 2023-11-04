using System;

namespace SpellCompiler.SpellReader.Binding
{
    internal sealed class BoundUnaryExpressionNode : BoundExpressionNode 
    {
        public BoundUnaryOperator BoundOperator { get; set; }
        public BoundExpressionNode Operand { get; set; }

        public override Type Type => BoundOperator.ResultType;
        public override BoundNodeKind Kind => BoundNodeKind.UnaryExpression; 

        public BoundUnaryExpressionNode(BoundUnaryOperator boundOperator, BoundExpressionNode operand) 
        {
            BoundOperator = boundOperator;
            Operand = operand;
        }
    }
}