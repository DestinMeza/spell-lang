using System;
using Spell.Binding;

namespace Spell
{

    internal sealed class Evaluator 
    {
        private readonly BoundExpressionNode _root;
        public Evaluator(BoundExpressionNode root) 
        {
            _root = root;
        }

        public object Evaluate() 
        {
            return EvaluateExpression(_root);
        }

        private object EvaluateExpression(BoundExpressionNode node) 
        {
            if (node is BoundLiteralExpression n) 
            {
                return n.Value;
            }
            if (node is BoundUnaryExpressionNode u) 
            {
                var operand = EvaluateExpression(u.Operand);

                switch (u.BoundOperator.Kind) 
                {
                    case BoundUnaryOperatorKind.Identity:
                        return (int) operand;
                    case BoundUnaryOperatorKind.Negation:
                        return -(int) operand;
                    case BoundUnaryOperatorKind.LogicalNegation:
                        return !(bool) operand;
                    default: throw new Exception($"Unexpected unary operator {u.BoundOperator.Kind}");
                }
            }
            if (node is BoundBinaryExpressionNode b) 
            {
                var left = EvaluateExpression(b.Left);
                var right = EvaluateExpression(b.Right);

                switch (b.BoundOperator.Kind) 
                {
                    case BoundBinaryOperatorKind.Addition:
                        return (int) left + (int) right;
                    case BoundBinaryOperatorKind.Subtraction:
                        return (int) left - (int) right;
                    case BoundBinaryOperatorKind.Multiplication:
                        return (int) left * (int) right;
                    case BoundBinaryOperatorKind.Division:
                        return (int) left / (int) right;
                    case BoundBinaryOperatorKind.LogicalAnd:
                        return (bool)left && (bool)right;
                    case BoundBinaryOperatorKind.LogicalOr:
                        return (bool)left || (bool)right;
                    case BoundBinaryOperatorKind.Equals:
                        return Equals(left, right);
                    case BoundBinaryOperatorKind.NotEquals:
                        return !Equals(left, right);
                    default : throw new Exception($"Unexpected binary operator {b.BoundOperator.Kind}");
                }
            }

            throw new Exception($"Unexpected node {node.Kind}");
        }
    }
}