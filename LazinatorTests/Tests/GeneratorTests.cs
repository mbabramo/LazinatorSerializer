﻿using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis;
using System.Threading.Tasks;
using VerifyXunit;
using Xunit;
using Xunit.Abstractions;
using LazinatorGenerator.Generator;
using System.Runtime.CompilerServices;
using VerifyTests;
using System.Collections.Generic;
using Lazinator.Attributes;
using LazinatorTests.Support;
using System.Linq;
using FluentAssertions;
using System.IO;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities.Resources;
using System;
using System.Collections.Immutable;
using Microsoft.CodeAnalysis.Text;
using System.Threading;
using System.Reflection.Emit;

namespace LazinatorTests.Tests
{
    [UsesVerify]
    public class GeneratorTests
    {
        [Fact]
        public Task UseGeneratorForSimpleILazinator()
        {        // The source code to test
            var mainProgramSource = @"
namespace MyCode
{
    public class Program
    {
        public static void Main(string[] args)
        {
        }
    }
}";


            var interfaceSource = @"
using Lazinator.Attributes;

namespace MyCode
{
    [Lazinator((int)1)]
    public interface IMyExample
    {
        int MyInt { get; set; }
    }
}";

            var classSource = @"
namespace MyCode
{
    public partial class MyExample : IMyExample
    {
    }
}";

            var sources = new List<(string filename, string text)>()
            {
                ("Program.cs", mainProgramSource),
                ("IMyExample.cs", interfaceSource),
                ("MyExample.cs", classSource)
            };

            var executionResult = Generate(sources);

            var soleSource = executionResult.outputs.Single();

            // Check to make sure that some source generation occurred.
            soleSource.text.Contains("[Autogenerated]").Should().BeTrue();
            soleSource.text.Contains("partial class").Should().BeTrue();


            var diagnostics = executionResult.compilation.GetDiagnostics().ToList();
            // Because we haven't yet added the syntax trees for the new file to the compilation, we should still have a diagnostic (for not implementing the interface property).
            diagnostics.Count().Should().Be(1);
            GeneratorDriverRunResult runResult = executionResult.driver.GetRunResult();

            // Add the generated sources to the compilation
            foreach (var result in runResult.Results)
            {
                foreach (var source in result.GeneratedSources)
                {
                    SyntaxTree tree = CSharpSyntaxTree.ParseText(source.SourceText);
                    executionResult.compilation = executionResult.compilation.AddSyntaxTrees(tree);
                }
            }

            // Now, you can get the diagnostics from the updated compilation
            diagnostics = executionResult.compilation.GetDiagnostics().ToList();
            diagnostics.Count().Should().Be(0);

            return Task.CompletedTask;
        }

        [Theory]
        [InlineData("Lazinator")]
        [InlineData("Lazinator.Collections")]
        [InlineData("LazinatorTests")]
        public Task SourceGeneratorCanMatchExistingLazinatorFiles(string projectName)
        {
            string projectPath = ReadCodeFile.GetCodeBasePath(projectName);
            var lazinatorConfigFiles = GetMatchingFiles(projectPath, "LazinatorConfig.json").ToList();
            var lazinatorConfigFiles2 = GetMatchingFiles(projectPath, "Lazinatorconfig.json").ToList();
            lazinatorConfigFiles.AddRange(lazinatorConfigFiles2);
            var sourceFiles = GetMatchingFiles(projectPath).ToList();
            var handWritten = sourceFiles.Where(x => !x.Path.EndsWith(".g.cs") && !x.Path.EndsWith(".laz.cs")).ToList();
            var autogeneratedOriginals = sourceFiles.Where(x => x.Path.EndsWith(".g.cs") || x.Path.EndsWith(".laz.cs")).ToList();

            var result = Generate(handWritten, lazinatorConfigFiles);
            
            var outputNames = String.Join("\n", result.outputs.Select(x => x.path)) ;
            var originalNames = String.Join("\n", autogeneratedOriginals.Select(x => x.Path));
            result.outputs.Count().Should().Be(autogeneratedOriginals.Count());

            var outputsOrdered = result.outputs.OrderBy(x => x.text).ToList();
            var originalsOrdered = autogeneratedOriginals.OrderBy(x => x.Text).ToList();
            for (int i = 0; i < outputsOrdered.Count(); i++)
            {
                outputsOrdered[i].text.Should().Be(originalsOrdered[i].Text);
            }

            return Task.CompletedTask;
        }
        
        [Fact]
        public Task SourceGeneratorProvidesDiagnostics()
        {
            // we'll do something wrong by having a Lazinator interface refer to a non-Lazinator class.
            var nonlazinatorSource = @"
namespace MyCode
{
    public class MyNonLazinator
    {
    
    }
}
";
            
            // The source code to test
            var interfaceSource = @"
using Lazinator.Attributes;

namespace MyCode
{
    [Lazinator((int)1)]
    public interface IMyExample
    {
        MyNonLazinator MyMistake { get; set; }
    }
}";

            var classSource = @"
namespace MyCode
{
    public partial class MyExample : IMyExample
    {
    }
}";

            var sources = new List<(string filename, string text)>()
            {
                ("IMyExample.cs", interfaceSource),
                ("MyExample.cs", classSource)
            };

            var result = Generate(sources);
            var diagnostics = result.compilation.GetDiagnostics();

            var soleSource = result.outputs.Single();

            // Check to make sure that some source generation occurred.
            soleSource.text.Contains("[Autogenerated]").Should().BeTrue();
            soleSource.text.Contains("partial class").Should().BeTrue();

            // Use verify to snapshot test the source generator output!
            // return Verifier.Verify(driver);

            return Task.CompletedTask;
        }
        
        
        private static IEnumerable<(string Path, string Text)> GetMatchingFiles(string rootFolderPath, string searchPattern = "*.cs")
        {
            var matching = Directory.EnumerateFiles(rootFolderPath, searchPattern, SearchOption.AllDirectories);

            foreach (var csFile in matching)
            {
                string fileText = File.ReadAllText(csFile);

                yield return (csFile, fileText);
            }
        }

        private static (LazinatorIncrementalGenerator generator, CSharpCompilation compilation, GeneratorDriver driver, List<(string path, string text)> outputs) Generate(IEnumerable<(string path, string text)> sources, IEnumerable<(string path, string text)> additionalTexts = null)
        {
            // Parse the provided string into a C# syntax tree
            IEnumerable<SyntaxTree> syntaxTrees = sources.Select(x => CSharpSyntaxTree.ParseText(x.text, path: x.path)).ToArray();

            // Create references for assemblies we required
            IEnumerable <PortableExecutableReference> references = AdhocWorkspaceManager.GetProjectReferences();

            // Create a Roslyn compilation for the syntax tree.
            CSharpCompilation compilation = CSharpCompilation.Create(
                assemblyName: "Tests",
                syntaxTrees: syntaxTrees,
                references: references
                );

            // Create an instance of our incremental source generator
            var generator = new LazinatorIncrementalGenerator();

            // The GeneratorDriver is used to run our generator against a compilation
            GeneratorDriver driver = CSharpGeneratorDriver.Create(generator);
            if (additionalTexts != null)
            {
                var additionalTexts2 = ImmutableArray.CreateRange(additionalTexts.Select(x => (AdditionalText)new CustomAdditionalText(x.path, x.text)));
                driver = driver.AddAdditionalTexts(additionalTexts2);
            }

            // Run the source generator!
            driver = driver.RunGenerators(compilation);

            // Look at the resulting source file
            List<(string path, string text)> outputs = driver.GetRunResult().Results[0].GeneratedSources.Select(x => (x.SyntaxTree.FilePath, x.SourceText.ToString())).ToList();
            return (generator, compilation, driver, outputs);
        }


    }

    public static class ModuleInitializer
    {
        [ModuleInitializer]
        public static void Init()
        {
            VerifySourceGenerators.Enable();
        }
    }

    public class CustomAdditionalText : AdditionalText
    {
        private readonly string _text;

        public override string Path { get; }

        public CustomAdditionalText(string path, string text)
        {
            Path = path;
            _text = text ?? File.ReadAllText(path);
        }

        public override SourceText GetText(CancellationToken cancellationToken = new CancellationToken())
        {
            return SourceText.From(_text);
        }
    }
}
