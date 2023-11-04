using System;
using System.Linq;
using SpellCompiler.SpellReader.Binding;
using SpellCompiler.SpellReader.Syntax;

namespace SpellCompiler.SpellReader
{
    public sealed class Compilation
    {
        public SyntaxTree SyntaxTree { get; }

        public EvaluationResult Evaluate() 
        {
            var binder = new Binder();
            var boundExpression = binder.BindExpression(SyntaxTree.Root);

            var diagnostics = SyntaxTree.Diagnostics.Concat(binder.Diagnostics).ToArray();
            if (diagnostics.Any()) 
            {
                return new EvaluationResult(diagnostics, null);
            }

            var evaluator = new Evaluator(boundExpression);
            var value = evaluator.Evaluate();

            return new EvaluationResult(Array.Empty<string>(), value);
        }

        public Compilation(SyntaxTree syntaxTree) 
        {
            SyntaxTree = syntaxTree;
        }
    }
}