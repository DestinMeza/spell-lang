using System;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using Spell.Binding;
using Spell.Syntax;
using Spell.IO;
using Spell.Async.Tasks;
using Spell.Async;
using System.Collections.Generic;
using System.IO;

namespace Spell
{
    /// <summary>
    /// Spell Compiler is used for processing a spell's functionality. 
    /// Parsing the SPL Lang and turning it into C# code to be run and be used dynamically.
    /// </summary>
    public class SpellCompiler
    {
        private Dictionary<VariableSymbol, object> variables;
        private Compilation previousCompilation;

        public SpellCompiler() 
        {
            variables = new Dictionary<VariableSymbol, object>();
        }

        #region Async

        public void ResetCompilation() 
        {
            variables.Clear();
            previousCompilation = null;
            Diagnostics.ClearLogs();
        }

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

        internal async Task<TaskResult> ReadSourceCode(string sourceText, TaskResult result)
        {
            EvaluationResult evaluationResult = null;
            SyntaxTree syntaxTree = null;

            try
            {
                if (string.IsNullOrWhiteSpace(sourceText))
                {
                    Diagnostics.LogErrorMessage("Error: Empty Source Code");

                    var diagnostics = Diagnostics.GetLogs();
                    Diagnostics.ClearLogs();

                    evaluationResult = new EvaluationResult(null, diagnostics, null);

                    result.Result = evaluationResult;
                    return result;
                }

                syntaxTree = SyntaxTree.Parse(sourceText);

                Compilation complation = previousCompilation == null 
                    ? new Compilation(syntaxTree) 
                    : previousCompilation.ContinueWith(syntaxTree);

                evaluationResult = complation.Evaluate(variables);

                if (!evaluationResult.LogTypes.Contains(ELogType.Error)) 
                {
                    previousCompilation = complation;
                }

                result.Result = evaluationResult;
            }
            catch (Exception e)
            {
                if (TextSpan.TryFindSpan(e.Message, out TextSpan textSpan))
                {
                    Diagnostics.LogErrorMessage($"Error: {e.Message}", textSpan);
                }
                else 
                {
                    Diagnostics.LogErrorMessage($"Error: {e.Message}");
                }

                var diagnostics = Diagnostics.GetLogs();
                Diagnostics.ClearLogs();

                evaluationResult = new EvaluationResult(syntaxTree?.Root.ToString(variables) ?? null, diagnostics, null);

                result.Result = evaluationResult;
            }

            return result;
        }

        #endregion

        public EvaluationResult Read(string sourceText)
        {
            EvaluationResult evaluationResult = null;
            SyntaxTree syntaxTree = null;

            try
            {
                if (string.IsNullOrWhiteSpace(sourceText))
                {
                    Diagnostics.LogErrorMessage("Error: Empty Source Code");

                    var diagnostics = Diagnostics.GetLogs();
                    Diagnostics.ClearLogs();

                    evaluationResult = new EvaluationResult(null, diagnostics, null);

                    return evaluationResult;
                }

                syntaxTree = SyntaxTree.Parse(sourceText);

                Compilation complation = previousCompilation == null 
                    ? new Compilation(syntaxTree) 
                    : previousCompilation.ContinueWith(syntaxTree);

                evaluationResult = complation.Evaluate(variables);

                if (!evaluationResult.LogTypes.Contains(ELogType.Error))
                {
                    previousCompilation = complation;
                }
            }
            catch (Exception e)
            {
                if (TextSpan.TryFindSpan(e.Message, out TextSpan textSpan))
                {
                    Diagnostics.LogErrorMessage($"Error: {e.Message}", textSpan);
                }
                else
                {
                    Diagnostics.LogErrorMessage($"Error: {e.Message}");
                }

                var diagnostics = Diagnostics.GetLogs();
                Diagnostics.ClearLogs();

                evaluationResult = new EvaluationResult(syntaxTree?.Root.ToString(variables) ?? null, diagnostics, null);
            }

            return evaluationResult;
        }
    }
}