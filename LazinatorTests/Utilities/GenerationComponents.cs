using LazinatorGenerator.Generator;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LazinatorTests.Tests;
using System.Collections.Immutable;

namespace LazinatorTests.Utilities
{

    public record GenerationComponents(LazinatorIncrementalGenerator generator, CSharpCompilation compilation, GeneratorDriver driver)
    {
        public GenerationComponents AfterRerunningGenerators()
        {
            var updatedDriver = driver.RunGeneratorsAndUpdateCompilation(compilation, out var outputCompilation, out _); // rerun generators
            return new GenerationComponents(generator, compilation, updatedDriver); // note: use original compilation
        }

        public GenerationComponents WithAdditionalTexts(IEnumerable<(string path, string text)> additionalTexts)
        {
            if (additionalTexts != null)
            {
                ImmutableArray<AdditionalText> additionalTexts2 = GetAdditionalTextsAsArray(additionalTexts);
                var driver2 = driver.AddAdditionalTexts(additionalTexts2);
                return this with { driver = driver2 };
            }

            return this;
        }

        private static ImmutableArray<AdditionalText> GetAdditionalTextsAsArray(IEnumerable<(string path, string text)> additionalTexts)
        {
            return ImmutableArray.CreateRange(additionalTexts.Select(x => (AdditionalText)new CustomAdditionalText(x.path, x.text)));
        }

        public GenerationComponents WithAddedFile(string path, string text)
        {
            SyntaxTree newTree = CSharpSyntaxTree.ParseText(text, null, path);
            var revised = this with { compilation = this.compilation.AddSyntaxTrees(newTree) };
            return revised;
        }

        public GenerationComponents WithUpdatedFile(string path, string text)
        {
            SyntaxTree oldTree = compilation.SyntaxTrees.FirstOrDefault(x => x.FilePath == path);
            if (oldTree == null)
                throw new Exception("Source didn't exist as expected.");
            var revised = this with { compilation = compilation.RemoveSyntaxTrees(oldTree) };
            SyntaxTree newTree = CSharpSyntaxTree.ParseText(text, null, path);
            revised = revised with { compilation = revised.compilation.AddSyntaxTrees(newTree) };
            return revised;
        }

        public List<(string path, string text)> GetGeneratedSourcesWithPaths()
        {
            return driver.GetRunResult().Results.SelectMany(x => x.GeneratedSources).Select(x => (x.SyntaxTree.FilePath, x.SourceText.ToString())).ToList();
        }

        public List<GeneratedSourceResult> GetGeneratedSources()
        {
            return driver.GetRunResult().Results.SelectMany(x => x.GeneratedSources).ToList();
        }

        public List<string> GetGeneratedSourcesAsStrings() => GetGeneratedSources().Select(x => x.SourceText.ToString()).ToList();

        public (string path, string text) GetSoleGeneratedSourceWithString(string s) => GetGeneratedSources().Where(x => x.SourceText.ToString().Contains(s)).Select(x => (x.SyntaxTree.FilePath, x.SourceText.ToString())).Single();

        public string GetSoleGeneratedSourceWithStringAsString(string s) => GetSoleGeneratedSourceWithString(s).text;

        public string GetGeneratedSourceAtIndex(int i) => GetGeneratedSources()[i].SourceText.ToString();

        public string GetSoleGeneratedSource()
        {
            var generatedSources = GetGeneratedSources();
            if (generatedSources.Count == 0)
                throw new Exception("No source generated.");
            if (generatedSources.Count > 1)
                throw new Exception("Expected only one generated source.");
            return generatedSources[0].SourceText.ToString();
        }

        /// <summary>
        /// Returns the diagnostics from the compilation and the source generator combined.
        /// </summary>
        /// <returns></returns>
        public List<Diagnostic> GetDiagnostics(bool includeOriginalDiagnostics = true, bool includeDiagnosticsFromRun = true)
        {
            var diagnostics = includeOriginalDiagnostics ? compilation.GetDiagnostics().ToList() : new List<Diagnostic>();
            if (includeDiagnosticsFromRun)
                diagnostics.AddRange(driver.GetRunResult().Diagnostics.ToList());
            return diagnostics;
        }
    }
}
