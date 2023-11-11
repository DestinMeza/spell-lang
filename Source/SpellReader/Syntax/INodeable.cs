using System.Collections.Generic;

namespace Spell.Syntax
{
    /// <summary>
    /// This is an interface that is used to Implement Tree Node functionality.
    /// </summary>
    public interface INodeable 
    {
        string Text { get; set; }
        SyntaxKind SyntaxKind { get; }
        IEnumerable<INodeable> GetChildren();
    }
}