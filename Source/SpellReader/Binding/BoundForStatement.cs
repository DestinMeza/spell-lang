using Spell.Syntax;

namespace Spell.Binding
{
    internal sealed class BoundForStatement : BoundStatement
    {
        public override BoundNodeKind Kind => BoundNodeKind.ForStatement;

        public VariableSymbol Variable { get; }
        public BoundExpressionNode LowerBound { get; }
        public BoundExpressionNode UpperBound { get; }
        public BoundStatement Body { get; }

        public BoundForStatement(VariableSymbol variable, BoundExpressionNode lowerBound, BoundExpressionNode upperBound, BoundStatement body)
        {
            Variable = variable;
            LowerBound = lowerBound;
            UpperBound = upperBound;
            Body = body;
        }
    }
}