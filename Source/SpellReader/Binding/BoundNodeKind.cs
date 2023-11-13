namespace Spell.Binding
{
    internal enum BoundNodeKind 
    {
        //Statements
        ExpressionStatement,
        VariableDeclaration,
        BlockStatement,

        //Expressions
        LiteralExpression,
        UnaryExpression,
        BinaryExpression,
        VariableExpression,
        AssignmentExpression,
    }
}