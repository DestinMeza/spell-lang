using System;
using System.Collections.Generic;
using System.Xml.Linq;
using Spell.Binding;
using Spell.Syntax;

namespace Spell
{

    internal sealed class Evaluator 
    {
        private readonly BoundStatement _root;
        private readonly Dictionary<VariableSymbol, object> _variables;

        private object _lastValue;

        public Evaluator(BoundStatement root, Dictionary<VariableSymbol, object> variables) 
        {
            _root = root;
            _variables = variables;
        }

        public object Evaluate() 
        {
            EvaluateStatement(_root);
            return _lastValue;
        }

        private void EvaluateStatement(BoundStatement node)
        {
            switch (node.Kind)
            {
                case BoundNodeKind.BlockStatement: EvaluateBlockStatement((BoundBlockStatement)node); break;
                case BoundNodeKind.ExpressionStatement: EvaluateExpressionStatement((BoundExpressionStatement)node); break;
                default:
                    throw new NotSupportedException($"Node as \"{node.Kind}\" is not supported for evaluation.");
            }
        }

        private void EvaluateBlockStatement(BoundBlockStatement node)
        {
            foreach (var statement in node.Statements)
            {
                EvaluateStatement(statement);
            }
        }

        private void EvaluateExpressionStatement(BoundExpressionStatement node)
        {
            _lastValue = EvaluateExpression(node.Expression);
        }

        private object EvaluateExpression(BoundExpressionNode node) 
        {

            switch (node.Kind)
            {
                case BoundNodeKind.LiteralExpression: return EvaluateLiteralExpression((BoundLiteralExpression)node);
                case BoundNodeKind.UnaryExpression: return EvaluateUnaryExpression((BoundUnaryExpressionNode)node);
                case BoundNodeKind.BinaryExpression: return EvaluateBinaryExpression((BoundBinaryExpressionNode)node);
                case BoundNodeKind.VariableExpression: return EvaluateVariableExpression((BoundVariableExpressionNode)node);
                case BoundNodeKind.AssignmentExpression:    return EvaluateAssignmentExpression((BoundAssignmentExpressionNode)node);
                default:
                    throw new NotSupportedException($"Node as \"{node.Type}\" is not supported for evaluation.");
            }
        }

        private static object EvaluateLiteralExpression(BoundLiteralExpression n)
        {
            return n.Value;
        }

        private object EvaluateUnaryExpression(BoundUnaryExpressionNode u)
        {
            var operand = EvaluateExpression(u.Operand);

            switch (u.BoundOperator.Kind)
            {
                case BoundUnaryOperatorKind.Identity:
                    return (int)operand;
                case BoundUnaryOperatorKind.Negation:
                    return -(int)operand;
                case BoundUnaryOperatorKind.LogicalNegation:
                    return !(bool)operand;
                default: throw new Exception($"Unexpected unary operator {u.BoundOperator.Kind}");
            }
        }

        private object EvaluateBinaryExpression(BoundBinaryExpressionNode b)
        {
            var left = EvaluateExpression(b.Left);
            var right = EvaluateExpression(b.Right);

            switch (b.BoundOperator.Kind)
            {
                case BoundBinaryOperatorKind.Addition:
                    return (int)left + (int)right;
                case BoundBinaryOperatorKind.Subtraction:
                    return (int)left - (int)right;
                case BoundBinaryOperatorKind.Multiplication:
                    return (int)left * (int)right;
                case BoundBinaryOperatorKind.Division:
                    return (int)left / (int)right;
                case BoundBinaryOperatorKind.LogicalAnd:
                    return (bool)left && (bool)right;
                case BoundBinaryOperatorKind.LogicalOr:
                    return (bool)left || (bool)right;
                case BoundBinaryOperatorKind.Equals:
                    return Equals(left, right);
                case BoundBinaryOperatorKind.NotEquals:
                    return !Equals(left, right);
                default: throw new Exception($"Unexpected binary operator {b.BoundOperator.Kind}");
            }
        }

        private object EvaluateVariableExpression(BoundVariableExpressionNode v)
        {
            return _variables[v.VariableSymbol];
        }

        private object EvaluateAssignmentExpression(BoundAssignmentExpressionNode a)
        {
            var value = EvaluateExpression(a.Expression);
            _variables[a.VariableSymbol] = value;
            return value;
        }
    }
}