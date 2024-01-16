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

    public record GeneratorExecutionResult(LazinatorIncrementalGenerator generator, CSharpCompilation compilation, GeneratorDriver driver, List<(string path, string text)> outputs)
    {
        public GeneratorExecutionResult WithGeneratedFileAddedToCompilation()
        {
            // Add the generated sources to the compilation
            GeneratorDriverRunResult overallRunResult = driver.GetRunResult();
            var revised = this;
            foreach (var specificRunResult in overallRunResult.Results)
            {
                foreach (var source in specificRunResult.GeneratedSources)
                {
                    SyntaxTree tree = CSharpSyntaxTree.ParseText(source.SourceText);
                    var revisedCompilation = compilation.AddSyntaxTrees(tree);
                    revised = revised with { compilation = revisedCompilation };
                }
            }
            return revised;
        }

        public GeneratorExecutionResult AfterRerunningGenerators()
        {
            var updatedDriver = driver.RunGeneratorsAndUpdateCompilation(compilation, out var outputCompilation, out _); // rerun generators
            return new GeneratorExecutionResult(generator, compilation, updatedDriver, outputs); // note: use original compilation
        }

        public GeneratorExecutionResult WithAdditionalTexts(IEnumerable<(string path, string text)> additionalTexts)
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

        public GeneratorExecutionResult WithUpdatedFile(string path, string text)
        {
            SyntaxTree oldTree = compilation.SyntaxTrees.FirstOrDefault(x => x.FilePath == path);
            if (oldTree == null)
                throw new Exception("Source didn't exist as expected.");
            var revised = this with { compilation = compilation.RemoveSyntaxTrees(oldTree) };
            SyntaxTree newTree = CSharpSyntaxTree.ParseText(text, null, path);
            revised = revised with { compilation = revised.compilation.AddSyntaxTrees(newTree) };
            return revised;
        }

        public List<GeneratedSourceResult> GetGeneratedSources()
        {
            return driver.GetRunResult().Results.SelectMany(x => x.GeneratedSources).ToList();
        }

        public string GetGeneratedSourceAtIndex(int i) => GetGeneratedSources()[i].SourceText.ToString();

        public string GetSoleGeneratedSource()
        {
            var generatedSources = GetGeneratedSources();
            if (generatedSources.Count != 1)
                throw new Exception("Expected only one generated source.");
            return generatedSources[0].SourceText.ToString();
        }

        public List<Diagnostic> GetDiagnostics() => compilation.GetDiagnostics().ToList();
    }
}
