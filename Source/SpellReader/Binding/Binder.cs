using SpellCompiler.SpellReader.Syntax;
using System;
using System.Collections.Generic;

namespace SpellCompiler.SpellReader.Binding
{

    internal sealed class Binder 
    {
        private readonly List<string> _diagnostics = new List<string>();
        public IEnumerable<string> Diagnostics => _diagnostics;

        public BoundExpressionNode BindExpression(ExpressionSyntaxNode syntaxNode) 
        {
            switch (syntaxNode.SyntaxKind) 
            {
                case SyntaxKind.LiteralExpressionToken:
                    return BindLiteralExpression((LiteralExpressionSyntaxNode)syntaxNode);
                case SyntaxKind.UnaryExpressionToken:
                    return BindUnaryExpression((UnaryExpressionSyntax)syntaxNode);
                case SyntaxKind.BinaryExpressionToken:
                    return BindBinaryExpression((BinaryExpressionSyntaxNode)syntaxNode);
                case SyntaxKind.ParenthesizedExpressionToken:
                    return BindExpression(((ParenthesizedExpressionSytanxNode)syntaxNode).Expression);
                default:
                    throw new Exception($"Unexpected syntax {syntaxNode.SyntaxKind}");
            }
        }

        private BoundExpressionNode BindLiteralExpression(LiteralExpressionSyntaxNode syntax) 
        {
            var value = syntax.Value ?? 0;
            return new BoundLiteralExpression(value);
        }

        private BoundExpressionNode BindUnaryExpression(UnaryExpressionSyntax syntax)
        {
            var boundOperand = BindExpression(syntax.Operand);
            var boundOperator = BoundUnaryOperator.Bind(syntax.OperatorToken.SyntaxKind, boundOperand.Type);

            if (boundOperator == null) 
            {
                _diagnostics.Add($"Unary operator '{syntax.OperatorToken.Text}' is not defined for type {boundOperand.Type}");
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
                _diagnostics.Add($"Unary operator '{syntax.OperatorToken.Text}' is not defined for type {boundLeft.Type} and {boundRight.Type}");
            }

            return new BoundBinaryExpressionNode(boundLeft, boundOperator, boundRight);
        }
    }
}