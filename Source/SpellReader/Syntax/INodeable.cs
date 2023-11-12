using System.Collections.Generic;
using System.Linq;

namespace Spell.Syntax
{
    /// <summary>
    /// This is an interface that is used to Implement Tree Node functionality.
    /// </summary>
    public interface INodeable 
    {
        string Text { get; set; }
        SyntaxKind SyntaxKind { get; }
        TextSpan Span { get; }
        IEnumerable<INodeable> GetChildren();
    }
}