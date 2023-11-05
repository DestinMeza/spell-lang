using System;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using Spell.Binding;
using Spell.Syntax;
using Spell.IO;
using Spell.Async.Tasks;

namespace Spell
{
    /// <summary>
    /// Spell Compiler is used for processing a spell's functionality. 
    /// Parsing the SPL Lang and turning it into C# code to be run and be used dynamically.
    /// </summary>
    public class SpellCompiler
    {
        #region Async

        public void RunAsync(string sourceCode, CancellationToken ct)
        {
            TaskResult result = new TaskResult()
            {
                CancellationToken = ct
            };

            async Task<TaskResult> asyncFunction(string s, TaskResult r) 
            {
                return await Run(s, r);
            }

            AsyncTask<SpellFunction[]> task = new AsyncTask<SpellFunction[]>(result, asyncFunction(sourceCode, result));

            task.Forget();
        }
        private async Task<TaskResult> Run(string sourceCode, TaskResult result)
        {
            SpellFunction[] spellFunctions = null;

            Diagnostics.LogMessage("-----Reading File------");

            const bool isImmediateBreak = true;

            try
            {
                do
                {
                    if (string.IsNullOrWhiteSpace(sourceCode))
                    {
                        continue;
                    }

                    Parser parser = new Parser(sourceCode);
                    SyntaxTree syntaxTree = parser.Parse();

                    Diagnostics.LogMessage(GetTreeView(syntaxTree.Root));

                    Binder binder = new Binder();

                    await Task.Delay(200);
                }
                while (!result.CancellationToken.IsCancellationRequested && !isImmediateBreak);

                Diagnostics.LogMessage("End Of File Reached");
            }
            catch (Exception e)
            {
                Diagnostics.LogErrorMessage($"Error in Interpreter: {e}");
            }

            return result;
        }

        #endregion

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

        public void Run(string sourceCode) 
        {
            SpellFunction[] spellFunctions = null;

            Diagnostics.LogMessage("-----Reading File------");


            const bool isImmediateBreak = true;

            try
            {
                do
                {
                    if (string.IsNullOrWhiteSpace(sourceCode))
                    {
                        continue;
                    }

                    Parser parser = new Parser(sourceCode);
                    SyntaxTree syntaxTree = parser.Parse();

                    Diagnostics.LogMessage(GetTreeView(syntaxTree.Root));

                    Binder binder = new Binder();
                    var boundExpression = binder.BindExpression(syntaxTree.Root);
                }
                while (!isImmediateBreak);

                Diagnostics.LogMessage("End Of File Reached");
            }
            catch (Exception e)
            {
                Diagnostics.LogErrorMessage($"Error in Interpreter: {e}");
            }
        }
    }
}