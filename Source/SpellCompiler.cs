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

        public SpellCompiler() 
        {
            variables = new Dictionary<VariableSymbol, object>();
        }

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

        internal async Task<TaskResult> ReadSourceCode(string sourceText, TaskResult result)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(sourceText))
                {
                    Diagnostics.LogErrorMessage("Error: Empty Source Code");
                    result.Result = null;
                    return result;
                }

                SourceText sourceCode = SourceText.From(sourceText);

                Parser parser = new Parser(sourceCode);
                SyntaxTree syntaxTree = parser.Parse();

                Compilation complation = new Compilation(syntaxTree);

                var evaluationResult = complation.Evaluate(variables);

                result.Result = evaluationResult;
            }
            catch (Exception e)
            {
                Diagnostics.LogErrorMessage($"Error: {e.Message}");
            }

            return result;
        }

        #endregion

        public void Read(string sourceText)
        {
            Diagnostics.LogMessage("-----Reading File------");

            try
            {
                if (string.IsNullOrWhiteSpace(sourceText))
                {
                    Diagnostics.LogErrorMessage("Error: Empty Source Code");
                    return;
                }

                SourceText sourceCode = SourceText.From(sourceText);

                Parser parser = new Parser(sourceCode);
                SyntaxTree syntaxTree = parser.Parse();

                Diagnostics.LogMessage(syntaxTree.Root.ToString());

                Compilation complation = new Compilation(syntaxTree);
                var result = complation.Evaluate(variables);

                if (result.Value != null) 
                {
                    Diagnostics.LogMessage($"Result: {result.Value}");
                }
            }
            catch (Exception e)
            {
                Diagnostics.LogErrorMessage($"Error: {e.Message}");
            }
        }
    }
}