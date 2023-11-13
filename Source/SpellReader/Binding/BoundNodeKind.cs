namespace Spell.Binding
{
    internal enum BoundNodeKind 
    {
        //Statements
        ExpressionStatement,
        BlockStatement,

        //Expressions
        LiteralExpression,
        UnaryExpression,
        BinaryExpression,
        VariableExpression,
        AssignmentExpression,
    }
}