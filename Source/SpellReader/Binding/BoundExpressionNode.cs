using System;

namespace Spell.Binding
{
    internal abstract class BoundExpressionNode : BoundNode 
    {
        public abstract Type Type { get; } //TODo make our own Type representation
    }
}