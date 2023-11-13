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

        public EvaluationResult Evaluate(Dictionary<VariableSymbol, object> variables) 
        {
            var binder = new Binder(variables);
            var boundExpression = binder.BindExpression(SyntaxTree.Root.Expression);

            var diagnostics = Diagnostics.GetLogs();

            if (diagnostics.Any(x => x.ELogType == ELogType.Error)) 
            {
                return new EvaluationResult(diagnostics, null);
            }

            var evaluator = new Evaluator(boundExpression, variables);
            var value = evaluator.Evaluate();

            return new EvaluationResult(diagnostics, value);
        }
    }
}