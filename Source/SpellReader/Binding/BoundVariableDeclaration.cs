using Spell.Syntax;

namespace Spell.Binding
{
    internal sealed class BoundVariableDeclaration : BoundStatement
    {
        public override BoundNodeKind Kind => BoundNodeKind.VariableDeclaration;

        public VariableSymbol Variable { get; }
        public BoundExpressionNode Initalizer { get; }

        public BoundVariableDeclaration(VariableSymbol variable, BoundExpressionNode initalizer) 
        {
            Variable = variable;
            Initalizer = initalizer;
        }
    }
}