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
        private BoundScope _scope;

        public Binder(BoundScope parent) 
        {
            _scope = new BoundScope(parent);
        }

        public static BoundGlobalScope BindGlobalScope(BoundGlobalScope previous, CompilationUnitSyntax syntax) 
        {
            var parentScope = CreateParentScopes(previous);
            var binder = new Binder(parentScope);
            var expression = binder.BindStatement(syntax.Statement);
            var variables = binder._scope.GetDeclaredVariables();

            return new BoundGlobalScope(previous, variables, expression);
        }

        private static BoundScope CreateParentScopes(BoundGlobalScope previous) 
        {
            //submission 3 -> submission -> 2 submission -> 1
            var stack = new Stack<BoundGlobalScope>();

            while (previous != null) 
            {
                stack.Push(previous);
                previous = previous.Previous;
            }

            BoundScope parent = null;

            while (stack.Count > 0) 
            {
                previous = stack.Pop();
                var scope = new BoundScope(parent);
                foreach (var v in previous.Variables) 
                {
                    scope.TryDeclare(v);
                }

                parent = scope;
            }

            return parent;
        }

        private BoundStatement BindStatement(StatementSyntaxNode syntaxNode) 
        {
            if (syntaxNode == null)
            {
                throw new NullReferenceException($"Tried binding a null expression.");
            }

            switch (syntaxNode.SyntaxKind)
            {
                case SyntaxKind.BlockStatement:
                    return BindBlockStatement(((BlockStatementSyntaxNode)syntaxNode));
                case SyntaxKind.VariableDeclaration:
                    return BindVariableDeclaration(((VariableDeclarationSyntaxNode)syntaxNode));
                case SyntaxKind.ExpressionStatement:
                    return BindExpressionStatement(((ExpressionStatementSyntaxNode)syntaxNode));
                default:
                    throw new Exception($"Unexpected syntax {syntaxNode.SyntaxKind} " +
                        $"\n\r{syntaxNode.Text}");
            }
        }

        private BoundStatement BindVariableDeclaration(VariableDeclarationSyntaxNode syntaxNode)
        {
            var name = syntaxNode.Identifier.Text;
            var isReadOnly = syntaxNode.Keyword.SyntaxKind == SyntaxKind.LetKeyword;
            var initalizer = BindExpression(syntaxNode.Initalizer);
            var variable = new VariableSymbol(name, isReadOnly, initalizer.Type);

            if (!_scope.TryDeclare(variable)) 
            {
                throw new Exception($"{syntaxNode.Identifier.Span} Variable {name} has already been defined. " +
                    $"\n\r{syntaxNode.Text}");
            }

            return new BoundVariableDeclaration(variable, initalizer);
        }

        private BoundStatement BindBlockStatement(BlockStatementSyntaxNode syntaxNode)
        {
            var statements = new List<BoundStatement>();
            _scope = new BoundScope(_scope);

            foreach (var statementSyntax in syntaxNode.Statements) 
            {
                var statement = BindStatement(statementSyntax);
                statements.Add(statement);
            }

            _scope = _scope.Parent;

            return new BoundBlockStatement(statements);
        }

        private BoundStatement BindExpressionStatement(ExpressionStatementSyntaxNode syntaxNode)
        {
            var expression = BindExpression(syntaxNode.Expression);

            return new BoundExpressionStatement(expression);
        }

        private BoundExpressionNode BindExpression(ExpressionSyntaxNode syntaxNode) 
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
                    throw new Exception($"Unexpected syntax {syntaxNode.SyntaxKind} " +
                        $"\n\r{syntaxNode.Text}");
            }
        }

        private BoundExpressionNode BindParenthesizedExpression(ParenthesizedExpressionSytanxNode sytanxNode)
        {
            return BindExpression(sytanxNode.Expression);
        }

        private BoundExpressionNode BindLiteralExpression(LiteralExpressionSyntaxNode syntaxNode) 
        {
            if (syntaxNode.Value == null) 
            {
                throw new NullReferenceException($"Unexpected value {syntaxNode.Value} " +
                    $"\n\r{syntaxNode.Text}");
            }

            return new BoundLiteralExpressionNode(syntaxNode.Value);
        }

        private BoundExpressionNode BindNameExpression(NameExpressionSyntaxNode syntaxNode) 
        {
            var name = syntaxNode.IdentifierToken.Text;

            if (!_scope.TryLookup(name, out VariableSymbol variable))
            {
                throw new Exception($"{syntaxNode.IdentifierToken.Span} variable \"{name}\" does not exist. " +
                    $"\n\r{syntaxNode.Text}");
            }

            return new BoundVariableExpressionNode(variable);
        }

        private BoundExpressionNode BindAssignmentExpression(AssignmentExpressionSyntaxNode syntaxNode)
        {
            if (syntaxNode == null)
            {
                throw new NullReferenceException($"Assignment expression root node is null.");
            }

            var name = syntaxNode.IdentifierToken.Text;
            var boundExpression = BindExpression(syntaxNode.ExpressionSyntaxNode);

            if (!_scope.TryLookup(name, out var variable))
            {
                throw new Exception($"{syntaxNode.IdentifierToken.Span} Variable \"{name}\" does not exist. " +
                    $"\n\r{syntaxNode.Text}");
            }

            if (variable.IsReadOnly) 
            {
                throw new Exception($"{syntaxNode.IdentifierToken.Span} Variable \"{name}\" is readonly and cannot be assigned to. " +
                    $"\n\r{syntaxNode.Text}");
            }

            if (boundExpression.Type != variable.Type) 
            {
                throw new Exception($"{syntaxNode.IdentifierToken.Span} Cannot convert variable {name}. " +
                    $"\n\r{syntaxNode.Text}");
            }

            return new BoundAssignmentExpressionNode(variable, boundExpression);
        }

        private BoundExpressionNode BindUnaryExpression(UnaryExpressionSyntax syntaxNode)
        {
            if (syntaxNode == null)
            {
                throw new NullReferenceException($"Unary expression root node is null. ");
            }

            var boundOperand = BindExpression(syntaxNode.Operand);
            var boundOperator = BoundUnaryOperator.Bind(syntaxNode.OperatorToken.SyntaxKind, boundOperand.Type);

            if (boundOperand == null)
            {
                throw new NullReferenceException($"Unary expression's operand node is null. " +
                    $"\n\r{syntaxNode.Text}");
            }
            if (boundOperator == null)
            {
                string boundOperandType = boundOperand.Type?.ToString() ?? "null";

                throw new NullReferenceException($"Unary operator '{syntaxNode.OperatorToken.Text}' is not defined for type {boundOperandType} " +
                    $"\n\r{syntaxNode.Text}");
            }

            return new BoundUnaryExpressionNode(boundOperator, boundOperand);
        }

        private BoundExpressionNode BindBinaryExpression(BinaryExpressionSyntaxNode syntaxNode)
        {
            if (syntaxNode == null)
            {
                throw new NullReferenceException($"Binary expression root node is null.");
            }

            var boundLeft = BindExpression(syntaxNode.LeftNode);
            var boundRight = BindExpression(syntaxNode.RightNode);
            var boundOperator = BoundBinaryOperator.Bind(syntaxNode.OperatorToken.SyntaxKind, boundLeft.Type, boundRight.Type);


            if (boundLeft == null)
            {
                throw new NullReferenceException($"Binary expression's left node is null \n\r{syntaxNode.Text}");
            }
            if (boundLeft == null)
            {
                throw new NullReferenceException($"Binary expression's right node is null \n\r{syntaxNode.Text}");
            }
            if (boundOperator == null)
            {
                string boundLeftType = boundLeft.Type?.ToString() ?? "null";
                string boundRightType = boundRight.Type?.ToString() ?? "null";

                throw new NullReferenceException($"Binary operator '{syntaxNode.OperatorToken.Text}' is not defined for type \"{boundLeftType}\" and \"{boundRightType}\" " +
                    $"\n\r{syntaxNode.Text}");
            }

            return new BoundBinaryExpressionNode(boundLeft, boundOperator, boundRight);
        }
    }
}