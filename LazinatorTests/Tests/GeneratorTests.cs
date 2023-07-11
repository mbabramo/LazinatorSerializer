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

namespace LazinatorTests.Tests
{
    [UsesVerify]
    public class GeneratorTests
    {
        [Fact]
        public Task DEBUG()
        {        // The source code to test
            var source = @"
using Lazinator.Attributes;

namespace MyCode
{
    [Lazinator((int)1)]
    public interface IMyExample
    {
        int MyInt { get; set; }
    }
}";

            // Pass the source code to our helper and snapshot test the output
            return VerifySourceGenerator(source);
        }

        public static Task VerifySourceGenerator(string source)
        {
            // Parse the provided string into a C# syntax tree
            SyntaxTree syntaxTree = CSharpSyntaxTree.ParseText(source);

            // Create references for assemblies we required
            IEnumerable<PortableExecutableReference> references = AdhocWorkspaceManager.GetProjectReferences();

            // Create a Roslyn compilation for the syntax tree.
            CSharpCompilation compilation = CSharpCompilation.Create(
                assemblyName: "Tests",
                syntaxTrees: new[] { syntaxTree },
                references: references
                );


            // Create an instance of our incremental source generator
            var generator = new LazinatorSourceGenerator();

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
