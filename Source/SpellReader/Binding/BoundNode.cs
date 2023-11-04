namespace SpellCompiler.SpellReader.Binding
{
    internal abstract class BoundNode 
    {
        public abstract BoundNodeKind Kind { get; }
    }
}