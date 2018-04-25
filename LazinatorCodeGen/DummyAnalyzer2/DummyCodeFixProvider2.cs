using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Composition;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Rename;
using Microsoft.CodeAnalysis.Text;

namespace LazinatorCodeGen.DummyAnalyzer2
{
    [ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(DummyCodeFixProvider2)), Shared]
    public class DummyCodeFixProvider2 : CodeFixProvider
    {
        // This is a dummy code fix, since a library used by an analyzer should itself be an analyzer.

        public sealed override ImmutableArray<string> FixableDiagnosticIds
        {
            get { return ImmutableArray.Create("FAKEDIAGNOSTICID" + nameof(DummyCodeFixProvider2)); } // can't be uninitialized
        }

        public sealed override Task RegisterCodeFixesAsync(CodeFixContext context)
        {
            return Task.CompletedTask;
        }
        
    }
}
