using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace LazinatorCodeGen.DummyAnalyzer2
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class DummyAnalyzer2 : DiagnosticAnalyzer
    {
        // This is a dummy analyzer. We don't really want to use it as an analyzer, but we need an analyzer 
        // in the library that our real analyzer uses.

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get { return new ImmutableArray<DiagnosticDescriptor>(); } }

        public override void Initialize(AnalysisContext context)
        {
        }

        private static void AnalyzeSymbol(SymbolAnalysisContext context)
        {
        }
    }
}
