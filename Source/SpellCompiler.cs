using System;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using Spell.Binding;
using Spell.Syntax;
using Spell.IO;
using Spell.Async.Tasks;
using Spell.Async;

namespace Spell
{
    /// <summary>
    /// Spell Compiler is used for processing a spell's functionality. 
    /// Parsing the SPL Lang and turning it into C# code to be run and be used dynamically.
    /// </summary>
    public class SpellCompiler
    {
        #region Async

        public void ReadAsync(string sourceCode, CancellationToken ct, Action<TaskResult> callback = null)
        {
            TaskResult result = new TaskResult()
            {
                Callback = callback,
                CancellationToken = ct
            };

            async Task<TaskResult> asyncFunction(string s, TaskResult r) 
            {
                return await ReadSourceCode(s, r);
            }

            AsyncTask<object> task = new AsyncTask<object>(result, asyncFunction(sourceCode, result));

            task.RunAsync();
        }

        internal async Task<TaskResult> ReadSourceCode(string sourceCode, TaskResult result)
        {
            Diagnostics.LogMessage("-----Reading File------");

            try
            {
                if (string.IsNullOrWhiteSpace(sourceCode))
                {
                    Diagnostics.LogErrorMessage("Error: Empty Source Code");
                    result.Result = null;
                    return result;
                }

                Parser parser = new Parser(sourceCode);
                SyntaxTree syntaxTree = parser.Parse();

                Diagnostics.LogMessage(GetTreeView(syntaxTree.Root));

                Binder binder = new Binder();
                BoundExpressionNode boundNode = binder.BindExpression(syntaxTree.Root);
                Evaluator evaluator = new Evaluator(boundNode);

                result.Result = evaluator.Evaluate();

                //TODO add async Parsing. For now this is Testing for Async system

                Diagnostics.LogMessage("End Of File Reached");
            }
            catch (Exception e)
            {
                Diagnostics.LogErrorMessage($"Error in Compiler: {e}");
            }

            return result;
        }

        #endregion

        public void Read(string sourceCode)
        {
            Diagnostics.LogMessage("-----Reading File------");

            try
            {
                if (string.IsNullOrWhiteSpace(sourceCode))
                {
                    Diagnostics.LogErrorMessage("Error: Empty Source Code");
                    return;
                }

                Parser parser = new Parser(sourceCode);
                SyntaxTree syntaxTree = parser.Parse();

                Diagnostics.LogMessage(GetTreeView(syntaxTree.Root));

                Binder binder = new Binder();
                BoundExpressionNode boundNode = binder.BindExpression(syntaxTree.Root);
                Evaluator evaluator = new Evaluator(boundNode);

                Diagnostics.LogMessage($"Value: {evaluator.Evaluate()}");
                Diagnostics.LogMessage("End Of File Reached");
            }
            catch (Exception e)
            {
                Diagnostics.LogErrorMessage($"Error in Compiler: {e}");
            }
        }

        private string GetTreeView(INodeable node, int depth = 0, bool isLast = true)
        {
            string indent = isLast ? "-------" : "│-----";
            string localIndent = "";

            for (int i = 0; i < depth; i++)
            {
                localIndent += indent;
            }

            string branchVisual = isLast ? "└──" : "├──";

            string outputString = $"{localIndent}{branchVisual}{node.SyntaxKind}";

            if (node is SyntaxToken t && t.Value != null)
            {
                return outputString += $"\n{localIndent}{localIndent}{branchVisual} Value:{t.Value}";
            }

            string childrenStrings = "";

            var lastNode = node.GetChildren().LastOrDefault();

            foreach (var child in node.GetChildren())
            {
                childrenStrings += $"\n{localIndent}{GetTreeView(child, depth + 1, child == lastNode)}";
            }

            return outputString + childrenStrings;
        }
    }
}