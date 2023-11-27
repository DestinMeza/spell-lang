namespace Spell.Binding
{
    internal enum BoundNodeKind 
    {
        //Statements
        ExpressionStatement,
        VariableDeclaration,
        IfStatement,
        BlockStatement,

        //Expressions
        LiteralExpression,
        UnaryExpression,
        BinaryExpression,
        VariableExpression,
        AssignmentExpression,
    }
}