using Microsoft.CodeAnalysis.CSharp;
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

namespace LazinatorTests.Tests
{
    [UsesVerify]
    public class GeneratorTests
    {
        [Fact]
        public Task DEBUG()
        {        // The source code to test
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
using Lazinator.Attributes;

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

            return VerifySourceGenerator(sources);
        }

        private static Task VerifySourceGenerator(IEnumerable<(string filename, string text)> sources)
        {
            // Parse the provided string into a C# syntax tree
            IEnumerable<SyntaxTree> syntaxTrees = sources.Select(x => CSharpSyntaxTree.ParseText(x.text, path: x.filename)).ToArray();

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

            // Run the source generator!
            driver = driver.RunGenerators(compilation);

            // Use verify to snapshot test the source generator output!
            return Verifier.Verify(driver);
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
}
