using System;

namespace Spell.Binding
{
    internal abstract class BoundExpressionNode : BoundNode 
    {
        public abstract Type Type { get; } //TODO make our own Type representation
    }
}