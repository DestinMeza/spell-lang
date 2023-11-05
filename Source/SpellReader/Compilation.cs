using System;
using System.Collections.Generic;
using System.Linq;
using Spell.Binding;
using Spell.Syntax;

namespace Spell
{
    public sealed class Compilation
    {
        public SyntaxTree SyntaxTree { get; }

        public Compilation(SyntaxTree syntaxTree)
        {
            SyntaxTree = syntaxTree;
        }

        public EvaluationResult Evaluate() 
        {
            var binder = new Binder();
            var boundExpression = binder.BindExpression(SyntaxTree.Root);

            var fromActualLogs = Diagnostics.GetLogs();

            var diagnostics = new List<Log>(fromActualLogs);

            Diagnostics.ClearLogs();

            if (diagnostics.Any(x => x.ELogType == ELogType.Error)) 
            {
                return new EvaluationResult(diagnostics, null);
            }

            var evaluator = new Evaluator(boundExpression);
            var value = evaluator.Evaluate();

            return new EvaluationResult(diagnostics, value);
        }
    }
}