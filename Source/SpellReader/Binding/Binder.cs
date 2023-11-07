using Spell.Syntax;
using System;
using System.Collections.Generic;
using Spell;

namespace Spell.Binding
{

    internal sealed class Binder 
    {
        public BoundExpressionNode BindExpression(ExpressionSyntaxNode syntaxNode) 
        {
            switch (syntaxNode.SyntaxKind)
            {
                case SyntaxKind.ParenthesizedExpressionToken:
                    return BindParenthesizedExpression(((ParenthesizedExpressionSytanxNode)syntaxNode));
                case SyntaxKind.LiteralExpressionToken:
                    return BindLiteralExpression((LiteralExpressionSyntaxNode)syntaxNode);
                case SyntaxKind.NameExpressionToken:
                    return BindNameExpression((NameExpressionSyntaxNode)syntaxNode);
                case SyntaxKind.AssignmentExpressionToken:
                    return BindAssignmentExpression((AssignmentExpressionSyntaxNode)syntaxNode);
                case SyntaxKind.UnaryExpressionToken:
                    return BindUnaryExpression((UnaryExpressionSyntax)syntaxNode);
                case SyntaxKind.BinaryExpressionToken:
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
                throw new Exception($"Unexpected value {syntax.Value}");
            }

            return new BoundLiteralExpression(syntax.Value);
        }

        private BoundExpressionNode BindNameExpression(NameExpressionSyntaxNode syntaxNode) 
        {
            throw new NotImplementedException();
        }

        private BoundExpressionNode BindAssignmentExpression(AssignmentExpressionSyntaxNode syntaxNode)
        {
            throw new NotImplementedException();
        }

        private BoundExpressionNode BindUnaryExpression(UnaryExpressionSyntax syntax)
        {
            var boundOperand = BindExpression(syntax.Operand);
            var boundOperator = BoundUnaryOperator.Bind(syntax.OperatorToken.SyntaxKind, boundOperand.Type);

            if (boundOperator == null) 
            {
                Diagnostics.LogErrorMessage($"Unary operator '{syntax.OperatorToken.Text}' is not defined for type {boundOperand.Type}");
            }

            return new BoundUnaryExpressionNode(boundOperator, boundOperand);
        }

        private BoundExpressionNode BindBinaryExpression(BinaryExpressionSyntaxNode syntax)
        {
            var boundLeft = BindExpression(syntax.LeftNode);
            var boundRight = BindExpression(syntax.RightNode);
            var boundOperator = BoundBinaryOperator.Bind(syntax.OperatorToken.SyntaxKind, boundLeft.Type, boundRight.Type);

            if (boundOperator == null)
            {
                Diagnostics.LogErrorMessage($"Binary operator '{syntax.OperatorToken.Text}' is not defined for type {boundLeft.Type} and {boundRight.Type}");
            }

            return new BoundBinaryExpressionNode(boundLeft, boundOperator, boundRight);
        }


    }
}