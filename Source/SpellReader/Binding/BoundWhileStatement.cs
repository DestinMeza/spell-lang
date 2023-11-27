namespace Spell.Binding
{
    internal sealed class BoundWhileStatement : BoundStatement
    {
        public override BoundNodeKind Kind => BoundNodeKind.WhileStatement;
        public BoundExpressionNode Condition { get; }
        public BoundStatement Body { get; }
        public BoundWhileStatement(BoundExpressionNode condition, BoundStatement body)
        {
            Condition = condition;
            Body = body;
        }
    }
}