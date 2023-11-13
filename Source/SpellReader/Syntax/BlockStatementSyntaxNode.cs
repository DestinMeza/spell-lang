using System.Collections.Generic;

namespace Spell.Syntax
{
    public sealed class BlockStatementSyntaxNode : StatementSyntaxNode 
    {
        public override SyntaxKind SyntaxKind => SyntaxKind.BlockStatement;
        public override string Text {
            get
            {
                var _text = $"{OpenBraceToken.Text}";

                foreach (var s in Statements) 
                {
                    _text += s.Text;
                }

                _text += CloseBraceToken.Text;

                return _text;
            }
            set { } 
        }

        public SyntaxToken OpenBraceToken { get; }
        public IReadOnlyCollection<StatementSyntaxNode> Statements { get; }
        public SyntaxToken CloseBraceToken { get; }

        public BlockStatementSyntaxNode(SyntaxToken openBraceToken, IReadOnlyCollection<StatementSyntaxNode> statements, SyntaxToken closeBraceToken) 
        {
            OpenBraceToken = openBraceToken;
            Statements = statements;
            CloseBraceToken = closeBraceToken;
        }
    }
}