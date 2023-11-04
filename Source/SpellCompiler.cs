using System;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using SpellCompiler.SpellReader;
using SpellCompiler.SpellReader.Binding;
using SpellCompiler.SpellReader.Syntax;
using TaskDriver.Tasks;
using static DiagnosticsManager.DiagnosticsManager;

namespace SpellCompiler
{
    /// <summary>
    /// Spell Interpreter is used for processing a spell's functionality. 
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

            GameTask<SpellFunction[]> task = new GameTask<SpellFunction[]>(result, asyncFunction(sourceCode, result));

            task.Forget();
        }
        private async Task<TaskResult> Run(string sourceCode, TaskResult result)
        {
            SpellFunction[] spellFunctions = null;

            LogMessage("-----Reading File------");

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

                    LogMessage(GetTreeView(syntaxTree.Root));

                    Binder binder = new Binder();
                    var boundExpression = binder.BindExpression(syntaxTree.Root);

                    var diagnostics = syntaxTree.Diagnostics.Concat(binder.Diagnostics).ToArray();

                    if (diagnostics.Any())
                    {
                        foreach (var diagnostic in syntaxTree.Diagnostics)
                        {
                            LogErrorMessage(diagnostic);
                        }
                    }
                    else
                    {
                        var e = new Evaluator(boundExpression);
                        var treeResult = e.Evaluate();
                        LogMessage($"Value: {treeResult}");
                    }

                    await Task.Delay(200);
                }
                while (!result.CancellationToken.IsCancellationRequested && !isImmediateBreak);

                LogMessage("End Of File Reached");
            }
            catch (Exception e)
            {
                LogErrorMessage($"Error in Interpreter: {e}");
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

            LogMessage("-----Reading File------");


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

                    LogMessage(GetTreeView(syntaxTree.Root));

                    Binder binder = new Binder();
                    var boundExpression = binder.BindExpression(syntaxTree.Root);

                    var diagnostics = syntaxTree.Diagnostics.Concat(binder.Diagnostics).ToArray();

                    if (diagnostics.Any())
                    {
                        foreach (var diagnostic in syntaxTree.Diagnostics)
                        {
                            LogErrorMessage(diagnostic);
                        }
                    }
                    else
                    {
                        var e = new Evaluator(boundExpression);
                        var treeResult = e.Evaluate();
                        LogMessage($"Value: {treeResult}");
                    }
                }
                while (!isImmediateBreak);

                LogMessage("End Of File Reached");
            }
            catch (Exception e)
            {
                LogErrorMessage($"Error in Interpreter: {e}");
            }
        }
    }
}