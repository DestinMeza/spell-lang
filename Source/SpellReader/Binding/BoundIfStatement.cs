namespace Spell.Binding
{
    internal sealed class BoundIfStatement : BoundStatement 
    {
        public override BoundNodeKind Kind => BoundNodeKind.IfStatement;
        public BoundExpressionNode Condition { get; }
        public BoundStatement ThenStatement { get; }
        public BoundStatement ElseStatement { get; }
        public BoundIfStatement(BoundExpressionNode condition, BoundStatement thenStatement, BoundStatement boundStatement) 
        {
            Condition = condition;
            ThenStatement = thenStatement;
            ElseStatement = boundStatement;
        }
    }
}