using Spell.Syntax;
using System;

namespace Spell.Binding
{
    internal sealed class BoundExpressionStatement : BoundStatement
    {
        public BoundExpressionNode Expression { get; }
        public override BoundNodeKind Kind => BoundNodeKind.ExpressionStatement;

        public BoundExpressionStatement(BoundExpressionNode expression)
        {
            Expression = expression;
        }
    }
}