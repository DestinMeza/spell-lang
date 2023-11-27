namespace Spell.Binding
{
    internal enum BoundNodeKind 
    {
        //Statements
        ExpressionStatement,
        VariableDeclaration,
        IfStatement,
        WhileStatement,
        BlockStatement,

        //Expressions
        LiteralExpression,
        UnaryExpression,
        BinaryExpression,
        VariableExpression,
        AssignmentExpression,
    }
}