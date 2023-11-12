using Spell.Syntax;
using System;
using System.Collections.Generic;
using Spell;
using System.Linq;
using System.Xml.Linq;

namespace Spell.Binding
{

    internal sealed class Binder 
    {
        private readonly Dictionary<VariableSymbol, object> _variables;
        public Binder(Dictionary<VariableSymbol, object> variables) 
        {
            _variables = variables;
        }

        public BoundExpressionNode BindExpression(ExpressionSyntaxNode syntaxNode) 
        {
            if (syntaxNode == null) 
            {
                throw new NullReferenceException($"Tried binding a null expression.");
            }

            switch (syntaxNode.SyntaxKind)
            {
                case SyntaxKind.ParenthesizedExpression:
                    return BindParenthesizedExpression(((ParenthesizedExpressionSytanxNode)syntaxNode));
                case SyntaxKind.LiteralExpression:
                    return BindLiteralExpression((LiteralExpressionSyntaxNode)syntaxNode);
                case SyntaxKind.NameExpression:
                    return BindNameExpression((NameExpressionSyntaxNode)syntaxNode);
                case SyntaxKind.AssignmentExpression:
                    return BindAssignmentExpression((AssignmentExpressionSyntaxNode)syntaxNode);
                case SyntaxKind.UnaryExpression:
                    return BindUnaryExpression((UnaryExpressionSyntax)syntaxNode);
                case SyntaxKind.BinaryExpression:
                    return BindBinaryExpression((BinaryExpressionSyntaxNode)syntaxNode);
                default:
                    throw new Exception($"Unexpected syntax {syntaxNode.SyntaxKind}");
            }
        }

        private BoundExpressionNode BindParenthesizedExpression(ParenthesizedExpressionSytanxNode sytanxNode)
        {
            return BindExpression(sytanxNode.Expression);
        }

        private BoundExpressionNode BindLiteralExpression(LiteralExpressionSyntaxNode syntax) 
        {
            if (syntax.Value == null) 
            {
                throw new NullReferenceException($"Unexpected value {syntax.Value}");
            }

            return new BoundLiteralExpression(syntax.Value);
        }

        private BoundExpressionNode BindNameExpression(NameExpressionSyntaxNode syntaxNode) 
        {
            var name = syntaxNode.IdentifierToken.Text;

            var variableSymbol = _variables.Keys.FirstOrDefault(v => v.Name == name);

            if (variableSymbol == null)
            {
                Diagnostics.LogErrorMessage($"{syntaxNode.IdentifierToken.Span} variable \"{name}\" does not exist.");
            }

            return new BoundVariableExpressionNode(variableSymbol);
        }

        private BoundExpressionNode BindAssignmentExpression(AssignmentExpressionSyntaxNode syntaxNode)
        {
            if (syntaxNode == null)
            {
                throw new NullReferenceException($"Assignment expression root node is null.");       
            }

            var name = syntaxNode.IdentifierToken.Text;
            var boundExpression = BindExpression(syntaxNode.ExpressionSyntaxNode);

            var existingVariable = _variables.Keys.FirstOrDefault(v => v.Name == name);

            if (existingVariable != null) 
            {
                _variables.Remove(existingVariable);
            }

            var variable = new VariableSymbol(name, boundExpression.Type);
            _variables[variable] = null;

            return new BoundAssignmentExpressionNode(variable, boundExpression);
        }

        private BoundExpressionNode BindUnaryExpression(UnaryExpressionSyntax syntaxNode)
        {
            if (syntaxNode == null)
            {
                throw new NullReferenceException($"Unary expression root node is null.");
            }

            var boundOperand = BindExpression(syntaxNode.Operand);
            var boundOperator = BoundUnaryOperator.Bind(syntaxNode.OperatorToken.SyntaxKind, boundOperand.Type);

            if (boundOperand == null)
            {
                throw new NullReferenceException($"Unary expression's operand node is null.");
            }
            if (boundOperator == null)
            {
                string boundOperandType = boundOperand.Type?.ToString() ?? "null";

                throw new NullReferenceException($"Unary operator '{syntaxNode.OperatorToken.Text}' is not defined for type {boundOperandType}");
            }

            return new BoundUnaryExpressionNode(boundOperator, boundOperand);
        }

        private BoundExpressionNode BindBinaryExpression(BinaryExpressionSyntaxNode syntaxNode)
        {
            if (syntaxNode == null)
            {
                Diagnostics.LogErrorMessage($"Binary expression root node is null.");
                return null;
            }

            var boundLeft = BindExpression(syntaxNode.LeftNode);
            var boundRight = BindExpression(syntaxNode.RightNode);
            var boundOperator = BoundBinaryOperator.Bind(syntaxNode.OperatorToken.SyntaxKind, boundLeft.Type, boundRight.Type);


            if (boundLeft == null)
            {
                throw new NullReferenceException($"Binary expression's left node is null");
            }
            if (boundLeft == null)
            {
                throw new NullReferenceException($"Binary expression's right node is null");
            }
            if (boundOperator == null)
            {
                string boundLeftType = boundLeft.Type?.ToString() ?? "null";
                string boundRightType = boundRight.Type?.ToString() ?? "null";

                throw new NullReferenceException($"Binary operator '{syntaxNode.OperatorToken.Text}' is not defined for type \"{boundLeftType}\" and \"{boundRightType}\"");
            }

            return new BoundBinaryExpressionNode(boundLeft, boundOperator, boundRight);
        }
    }
}