using System.Collections.Generic;

namespace Spell.Binding
{
    internal sealed class BoundBlockStatement : BoundStatement
    {
        public IReadOnlyCollection<BoundStatement> Statements { get; }
        public override BoundNodeKind Kind => BoundNodeKind.BlockStatement;

        public BoundBlockStatement(IReadOnlyCollection<BoundStatement> statements)
        {
            Statements = statements;
        }
    }
}