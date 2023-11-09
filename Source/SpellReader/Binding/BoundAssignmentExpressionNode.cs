using Spell.Syntax;
using System;

namespace Spell.Binding
{
    internal sealed class BoundAssignmentExpressionNode : BoundExpressionNode
    {
        public override BoundNodeKind Kind => BoundNodeKind.AssignmentExpression;
        public override Type Type => Expression.Type;
        public VariableSymbol VariableSymbol { get; }
        public BoundExpressionNode Expression { get; }

        public BoundAssignmentExpressionNode(VariableSymbol variableSymbol, BoundExpressionNode expressionNode)
        {
            VariableSymbol = variableSymbol;
            Expression = expressionNode;
        }
    }
}