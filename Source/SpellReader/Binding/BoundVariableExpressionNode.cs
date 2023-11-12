using Spell.Syntax;
using System;

namespace Spell.Binding
{
    internal sealed class BoundVariableExpressionNode : BoundExpressionNode 
    {
        public override BoundNodeKind Kind => BoundNodeKind.VariableExpression;
        public override Type Type => VariableSymbol?.Type ?? null;
        public VariableSymbol VariableSymbol { get; }

        public BoundVariableExpressionNode(VariableSymbol variableSymbol) 
        {
            VariableSymbol = variableSymbol;
        }
    }
}