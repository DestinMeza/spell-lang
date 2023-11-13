using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Spell.Binding;
using Spell.Syntax;

namespace Spell
{
    public sealed class Compilation
    {
        public SyntaxTree SyntaxTree { get; }
        public Compilation Previous { get; }

        private BoundGlobalScope _globalScope;

        internal BoundGlobalScope GlobalScope
        {
            get
            {
                if (_globalScope == null) 
                {
                    var globalScope = Binder.BindGlobalScope(Previous?.GlobalScope, SyntaxTree.Root);
                    Interlocked.CompareExchange(ref _globalScope, globalScope, null);
                }

                return _globalScope;
            }
        }

        public Compilation(SyntaxTree syntaxTree) : this(null, syntaxTree)
        {
            SyntaxTree = syntaxTree;
        }

        private Compilation(Compilation previous, SyntaxTree syntaxTree) 
        {
            Previous = previous;
            SyntaxTree = syntaxTree;
        }

        public Compilation ContinueWith(SyntaxTree syntaxTree)
        {
            return new Compilation(this, syntaxTree);
        }

        public EvaluationResult Evaluate(Dictionary<VariableSymbol, object> variables) 
        {
            var diagnostics = Diagnostics.GetLogs();
            Diagnostics.ClearLogs();

            if (diagnostics.Any(x => x.ELogType == ELogType.Error)) 
            {
                return new EvaluationResult(SyntaxTree.Root.ToString(), diagnostics, null);
            }

            var evaluator = new Evaluator(GlobalScope.Statement, variables);
            var value = evaluator.Evaluate();

            return new EvaluationResult(SyntaxTree.Root.ToString(), diagnostics, value);
        }
    }
}