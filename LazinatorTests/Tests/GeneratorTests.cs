﻿using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;
using LazinatorGenerator.Generator;
using System.Runtime.CompilerServices;
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
using LazinatorGenerator.Support;
using LazinatorTests.Utilities;
using Lazinator.Core;
using System.Diagnostics;
using System.Reflection;

namespace LazinatorTests.Tests
{
    public class GeneratorTests
    {

        [Fact]
        public Task Generator_CanFindMinimalSetOfReferencesForGeneratorProject()
        {
            bool completeTest = false; // set to true to run the full test (time consuming). The point of this is so that we can create the correct set of PortableExecutionReferences.
            if (!completeTest)
                return Task.CompletedTask;
            var allReferencesInTestProject = AdhocWorkspaceManager.GetProjectReferences();
            bool[] toInclude = Enumerable.Range(0, allReferencesInTestProject.Count).Select(x => true).ToArray();
            for (int i = 0; i < toInclude.Length; i++)
            {
                toInclude[i] = false;
                var executionResult = RunGeneratorForSimpleLazinator(new FakeDateTimeNow(), toInclude);
                var diagnostics = executionResult.GetDiagnostics();
                if (diagnostics.Count != 0)
                {
                    Debug.WriteLine(allReferencesInTestProject[i].Display);
                    toInclude[i] = true;
                }
            }
            return Task.CompletedTask;
        }

        [Fact]
        public Task Generator_DiagnosticProducedBeforeGeneratorRuns()
        {
            List<(string path, string text)> sources = GetSourcesForSimpleLazinator();

            GenerationComponents preExecutionGeneratorComponents = GetPreExecutionGeneratorComponents(sources, null, new FakeDateTimeNow(), includeReferencesForEntireTestsProject: false);
            preExecutionGeneratorComponents.GetDiagnostics().Count().Should().Be(1);
            preExecutionGeneratorComponents.GetDiagnostics().First().ToString().Should().Contain("does not implement");

            return Task.CompletedTask;
        }

        [Fact]
        public Task GeneratorForSimpleLazinator_ProducesCode()
        {
            var executionResult = RunGeneratorForSimpleLazinator(new FakeDateTimeNow());
            var soleSource = executionResult.GetSoleGeneratedSource();

            // Check to make sure that some source generation occurred.
            soleSource.Contains("[Autogenerated]").Should().BeTrue();
            soleSource.Contains("partial class").Should().BeTrue();

            // Also, confirm that the updated compilation includes the generated source.
            executionResult.compilation.SyntaxTrees.Count().Should().Be(4); // 3 original + 1 generated

            return Task.CompletedTask;
        }

        [Fact]
        public Task GeneratorForSimpleLazinator_CachingWithNoChange()
        {

            var executionResult = RunGeneratorForSimpleLazinator(new FakeDateTimeNow());
            var sourceGeneratedInitially = executionResult.GetSoleGeneratedSource();

            var executionResultAfterRerunning = executionResult.AfterRerunningGenerators();
            var sourceGeneratedLater = executionResultAfterRerunning.GetSoleGeneratedSource();

            bool exactMatch = sourceGeneratedInitially == sourceGeneratedLater; // checks whether files match, including the date and version number.
            exactMatch.Should().BeTrue();

            return Task.CompletedTask;
        }

        [Fact]
        public Task GeneratorForSimpleLazinator_CachingWithIrrelevantChange()
        {

            var executionResult = RunGeneratorForSimpleLazinator(new FakeDateTimeNow());
            var sourceGeneratedInitially = executionResult.GetSoleGeneratedSource();
            var irrelevantClass = GetSourceForIrrelevantClass(false);
            var modified = executionResult.WithAddedFile(irrelevantClass.path, irrelevantClass.text);

            var executionResultAfterRerunning = modified.AfterRerunningGenerators();
            var sourceGeneratedLater = executionResultAfterRerunning.GetSoleGeneratedSource();

            bool exactMatch = sourceGeneratedInitially == sourceGeneratedLater; // checks whether files match, including the date and version number.
            exactMatch.Should().BeTrue();

            return Task.CompletedTask;
        }

        [Fact]
        public Task GeneratorForSimpleLazinator_NoCachingWithILazinatorAddedComment()
        {
            var executionResult = RunGeneratorForSimpleLazinator(new FakeDateTimeNow());
            var sourceGeneratedInitially = executionResult.GetSoleGeneratedSource();
            var ilazinatorWithAddedComment = GetILazinatorClassWithAddedComment();
            var modified = executionResult.WithUpdatedFile(ilazinatorWithAddedComment.path, ilazinatorWithAddedComment.text);

            var executionResultAfterRerunning = modified.AfterRerunningGenerators();
            var sourceGeneratedLater = executionResultAfterRerunning.GetSoleGeneratedSource();

            bool exactMatch = sourceGeneratedInitially == sourceGeneratedLater; // checks whether files match, including the date and version number.
            exactMatch.Should().BeFalse();

            return Task.CompletedTask;
        }

        [Fact]
        public Task GeneratorForSimpleLazinator_NoCachingWithSubstantiveILazinatorChange()
        {

            var executionResult = RunGeneratorForSimpleLazinator(new FakeDateTimeNow());
            var sourceGeneratedInitially = executionResult.GetSoleGeneratedSource();
            var ilazinatorWithAddedComment = GetILazinatorClassExpanded();
            var modified = executionResult.WithUpdatedFile(ilazinatorWithAddedComment.path, ilazinatorWithAddedComment.text);

            var executionResultAfterRerunning = modified.AfterRerunningGenerators();
            var sourceGeneratedLater = executionResultAfterRerunning.GetSoleGeneratedSource();

            bool exactMatch = sourceGeneratedInitially == sourceGeneratedLater; // checks whether files match, including the date and version number.
            exactMatch.Should().BeFalse();

            return Task.CompletedTask;
        }

        [Fact]
        public Task GeneratorForSimpleLazinator_NoCachingWithLazinatorChange()
        {

            Debug; // Broad problem here is that there is only one ILazinator file. So, no dependencies can change. 
            // Maybe that's actually the right thing -- we need to look at the code to see if we actually need changes based on the Lazinator (as opposed to the iLazinator). Ideally, we would be able
            // to determine caching solely by iLazinators. We need to think about partial methods for this.
            // More broadly, the strategy of listing all dependencies may be flawed. For one thing, we're getting a lot of dependencies in System.Numerics, which is a waste of time.
            // Ideally, we would work solely based on the iLazinator dependencies. So, we need a list of hashes of the iLazinator dependencies, including any properties, including within generics, whether iLazinator or Lazinator.
            // That is, we want dependencies of the iLazinator, specified as a list of other iLazinators, but we want this list to include iLazinators that are referred to indirectly through Lazinators that are referenced in properties.
            // Still, if we can do it that way, this should be much faster and simpler. 
            // The further complication is that in general, we are only dependent on whether each of our types is an iLazinator or not. With base classes (including indirect ones), we are dependent on what the members of the base class are.
            // So, really, we need a list of the referenced iLazinators. If this list changes (regardless of whether the underlying code changse), then we need to update. Plus, we need a list of the base classes, and for those, if anything
            // changes in the iLazinator (maybe we could narrow that to when specific information changes, such as the signature or the properties), we would need to rebuild the code.

            var executionResult = RunGeneratorForSimpleLazinator(new FakeDateTimeNow());
            var sourceGeneratedInitially = executionResult.GetSoleGeneratedSource();
            var lazinatorWithAddedMethod = GetLazinatorClassExpanded();
            var modified = executionResult.WithUpdatedFile(lazinatorWithAddedMethod.path, lazinatorWithAddedMethod.text);

            var executionResultAfterRerunning = modified.AfterRerunningGenerators();
            var sourceGeneratedLater = executionResultAfterRerunning.GetSoleGeneratedSource();

            bool exactMatch = sourceGeneratedInitially == sourceGeneratedLater; // checks whether files match, including the date and version number.
            exactMatch.Should().BeFalse();

            return Task.CompletedTask;
        }

        [Fact]
        public Task GeneratorForSimpleLazinator_NoCachingWithConfigChange()
        {
            var dateTimeNowProvider = new FakeDateTimeNow();

            var executionResult = RunGeneratorForSimpleLazinator(dateTimeNowProvider);

            var diagnostics = executionResult.GetDiagnostics();
            diagnostics.Count.Should().Be(0);
            var firstFileGeneratedInitially = executionResult.GetSoleGeneratedSource();
            firstFileGeneratedInitially.Should().Contain("generated by the Lazinator tool");

            dateTimeNowProvider.Advance(TimeSpan.FromMinutes(1));

            List<(string path, string text)> additionalTexts = new List<(string path, string text)>() { ("LazinatorConfig.json", @"{
  ""Comment"": ""This is a change that should trigger regeneration of code.""
}") };
            var executionResultsWithAdditionalTexts = executionResult.WithAdditionalTexts(additionalTexts);
            var executionResultsAfterRerunning = executionResultsWithAdditionalTexts.AfterRerunningGenerators();
            var firstFileGeneratedAfter = executionResultsAfterRerunning.GetSoleGeneratedSource();
            firstFileGeneratedAfter.Should().Contain("generated by the Lazinator tool");

            bool exactMatch = firstFileGeneratedInitially == firstFileGeneratedAfter; // checks whether files match, including the date and version number.
            exactMatch.Should().BeFalse(); // time should be different, indicating that cached version was not used

            return Task.CompletedTask;
        }

        [Fact]
        public Task Generator_CachingWhenChangeMadeToBaseClass()
        {
            throw new NotImplementedException();
        }

        [Fact]
        public Task Generator_NoCachingWhenChangeMadeToPropertyName()
        {
            throw new NotImplementedException();
        }

        private static GenerationComponents RunGeneratorForSimpleLazinator(IDateTimeNow dateTimeNowProvider, bool[] specificReferencesToInclude = null)
        {
            List<(string path, string text)> sources = GetSourcesForSimpleLazinator();
            var executionResult = ExecuteGenerator(sources, null, dateTimeNowProvider, includeReferencesForEntireTestsProject: false, specificReferencesToInclude: specificReferencesToInclude);
            return executionResult;
        }

        [Fact]
        public Task SourceGeneratorProvidesDiagnosticForUsingNonLazinatorSource()
        {
            List<(string path, string text)> sources = GetSourcesForLazinatorReferringToNonlazinator();

            var result = ExecuteGenerator(sources, null, new FakeDateTimeNow(), false);
            var diagnostics = result.compilation.GetDiagnostics();

            diagnostics.Count().Should().BeGreaterThan(0);
            diagnostics.Any(x => x.ToString().Contains("MyNonLazinator")).Should().BeTrue();

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

            var executionResult = ExecuteGenerator(handWritten, lazinatorConfigFiles, new RealDateTimeNow(), includeReferencesForEntireTestsProject: true);

            var outputNames = String.Join("\n", executionResult.GetGeneratedSourcesWithPaths().Select(x => x.path)) ;
            var originalNames = String.Join("\n", autogeneratedOriginals.Select(x => x.Path));
            outputNames.Count().Should().Be(autogeneratedOriginals.Count());

            var orderedGeneratedSources = executionResult.GetGeneratedSourcesWithPaths().OrderBy(x => x.text).ToList();
            var originalsOrdered = autogeneratedOriginals.OrderBy(x => x.Text).ToList();
            for (int i = 0; i < orderedGeneratedSources.Count(); i++)
            {
                bool matchesIgnoringGenerationInfo = CompareExcludingLinesWithPrefix(orderedGeneratedSources[i].text, originalsOrdered[i].Text, "//     This code was generated by the Lazinator tool");
                matchesIgnoringGenerationInfo.Should().Be(true);
            }

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

        public static bool CompareExcludingLinesWithPrefix(string s1, string s2, string prefix)
        {
            using (var reader1 = new StringReader(s1))
            using (var reader2 = new StringReader(s2))
            {
                string line1 = reader1.ReadLine();
                string line2 = reader2.ReadLine();

                while (line1 != null || line2 != null)
                {
                    // Skip lines that start with the prefix
                    if (line1 != null && line1.StartsWith(prefix))
                    {
                        line1 = reader1.ReadLine();
                        continue;
                    }
                    if (line2 != null && line2.StartsWith(prefix))
                    {
                        line2 = reader2.ReadLine();
                        continue;
                    }

                    // If both lines are not null, compare them
                    if (line1 != null && line2 != null)
                    {
                        if (!line1.Equals(line2, StringComparison.Ordinal))
                            return false;

                        // Advance to the next lines
                        line1 = reader1.ReadLine();
                        line2 = reader2.ReadLine();
                    }
                    // If only one line is null, then the strings are not equal
                    else if (line1 != null || line2 != null)
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        #region Sources for generator tests

        private static List<(string path, string text)> GetSourcesForSimpleLazinator()
        {
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

            var sources = new List<(string path, string text)>()
            {
                ("Program.cs", mainProgramSource),
                ("IMyExample.cs", interfaceSource),
                ("MyExample.cs", classSource)
            };
            return sources;
        }

        private static List<(string path, string text)> GetSourceForLazinatorWithBaseClass()
        {
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


            var parentInterfaceSource = @"
using Lazinator.Attributes;

namespace MyCode
{
    [Lazinator((int)1)]
    public interface IMyExample
    {
        int MyInt { get; set; }
    }
}";

            var childInterfaceSource = @"
using Lazinator.Attributes;

namespace MyCode
{
    [Lazinator((int)2)]
    public interface IMyExampleChild
    {
        int MyOtherInt { get; set; }
    }
}";

            var parentClassSource = @"
namespace MyCode
{
    public partial class MyExample : IMyExample
    {
    }
}";

            var childClassSource = @"
namespace MyCode
{
    public partial class MyExampleChild : MyExample, IMyExampleChild
    {
    }
}";

            var sources = new List<(string path, string text)>()
            {
                ("Program.cs", mainProgramSource),
                ("IMyExample.cs", parentInterfaceSource),
                ("MyExample.cs", parentClassSource),
                ("IMyExampleChild.cs", childInterfaceSource),
                ("MyExampleChild.cs", childClassSource)
            };
            return sources;
        }

        private static List<(string path, string text)> GetSourcesForLazinatorReferringToNonlazinator()
        {
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

            var sources = new List<(string path, string text)>()
            {
                ("Program.cs", mainProgramSource),
                ("IMyExample.cs", interfaceSource),
                ("MyExample.cs", classSource)
            };
            return sources;
        }

        private static (string path, string text) GetLazinatorClassExpanded()
        {
            return ("MyExample.cs", @"
using Lazinator.Attributes;

namespace MyCode
{
    public partial class MyExample : IMyExample
    {
        public void ExtraMethod()
        {
        }
    }
}}");
        }

        private static (string path, string text) GetILazinatorClassWithAddedComment()
        {
            return ("IMyExample.cs", @"
using Lazinator.Attributes;

namespace MyCode
{
    [Lazinator((int)1)]
    public interface IMyExample
    {
        int MyInt { get; set; }
        string MyString { get; set; } 
    }
    // This comment shouldn't change anything.
}");
        }

        private static (string path, string text) GetILazinatorClassExpanded()
        {
            return ("IMyExample.cs", @"
using Lazinator.Attributes;

namespace MyCode
{
    [Lazinator((int)1)]
    public interface IMyExample
    {
        int MyInt { get; set; }
        string MyString { get; set; }
    }
}");
        }

        private static (string path, string text) GetSourceForIrrelevantClass(bool useVariation)
        {

            // we'll do something wrong by having a Lazinator interface refer to a non-Lazinator class.
            var nonlazinatorSource = $@"
namespace MyCode
{{
    public class MyIrrelevantNonLazinator
    {{
        {(useVariation ? "public int MyInt { get; set; }" : "")}
    }}
}}
";
            return ("MyIrrelevantNonLazinator.cs", nonlazinatorSource);
        }

        private static List<(string path, string text)> ReplaceInSources(List<(string path, string text)> originals, string searchFor, string replaceWith) => originals.Select(x => (x.path, x.text.Replace(searchFor, replaceWith))).ToList();

        #endregion

        #region Generator execution

        private static GenerationComponents ExecuteGenerator(IEnumerable<(string path, string text)> sources, IEnumerable<(string path, string text)> additionalTexts, IDateTimeNow dateTimeNowProvider, bool includeReferencesForEntireTestsProject, bool[] specificReferencesToInclude = null)
        {
            GenerationComponents preExecutionGeneratorComponents = GetPreExecutionGeneratorComponents(sources, additionalTexts, dateTimeNowProvider, includeReferencesForEntireTestsProject, specificReferencesToInclude);

            // Run the source generator
            var updatedDriver = preExecutionGeneratorComponents.driver.RunGeneratorsAndUpdateCompilation(preExecutionGeneratorComponents.compilation, out var outputCompilation, out _);

            // Look at the resulting source file
            return new GenerationComponents(preExecutionGeneratorComponents.generator, (CSharpCompilation)outputCompilation, updatedDriver);
        }

        private static GenerationComponents GetPreExecutionGeneratorComponents(IEnumerable<(string path, string text)> sources, IEnumerable<(string path, string text)> additionalTexts, IDateTimeNow dateTimeNowProvider, bool includeReferencesForEntireTestsProject, bool[] specificReferencesToInclude = null)
        {
            CSharpCompilation compilation = CreateCompilation(sources, includeReferencesForEntireTestsProject, specificReferencesToInclude);
            LazinatorIncrementalGenerator generator;
            GeneratorDriver driver;
            CreateGeneratorAndDriver(additionalTexts, dateTimeNowProvider, out generator, out driver);
            GenerationComponents preExecutionGeneratorComponents = new GenerationComponents(generator, compilation, driver);
            return preExecutionGeneratorComponents;
        }

        private static void CreateGeneratorAndDriver(IEnumerable<(string path, string text)> additionalTexts, IDateTimeNow dateTimeNowProvider, out LazinatorIncrementalGenerator generator, out GeneratorDriver driver)
        {
            // Create an instance of our incremental source generator
            generator = new LazinatorIncrementalGenerator() { DateTimeNowProvider = dateTimeNowProvider };

            // The GeneratorDriver is used to run our generator against a compilation
            driver = CSharpGeneratorDriver.Create(generator);
            if (additionalTexts != null)
                driver = driver.AddAdditionalTexts(ImmutableArray.CreateRange(additionalTexts.Select(x => (AdditionalText)new CustomAdditionalText(x.path, x.text))));
        }

        private static CSharpCompilation CreateCompilation(IEnumerable<(string path, string text)> sources, bool includeReferencesForEntireTestsProject, bool[] specificReferencesToInclude)
        {
            // Parse the provided string into a C# syntax tree
            IEnumerable<SyntaxTree> syntaxTrees = sources.Select(x => CSharpSyntaxTree.ParseText(x.text, path: x.path)).ToArray();

            // Create references for assemblies we required
            IEnumerable<PortableExecutableReference> references = GetPortableExecutableReferences(includeReferencesForEntireTestsProject, specificReferencesToInclude);

            // Create a Roslyn compilation for the syntax tree.
            CSharpCompilation compilation = CSharpCompilation.Create(
                assemblyName: "Tests",
                syntaxTrees: syntaxTrees,
                references: references
                );
            return compilation;
        }

        private static IEnumerable<PortableExecutableReference> GetPortableExecutableReferences(bool includeReferencesForEntireTestsProject, bool[] specificReferencesToInclude)
        {
            IEnumerable<PortableExecutableReference> references;
            if (includeReferencesForEntireTestsProject || specificReferencesToInclude != null)
                references = AdhocWorkspaceManager.GetProjectReferences();
            else
            {
                string netCorePath = Path.GetDirectoryName(typeof(object).GetTypeInfo().Assembly.Location);
                references = new[]
                {
                    MetadataReference.CreateFromFile(typeof(ILazinator).Assembly.Location), // Lazinator.dll
                    MetadataReference.CreateFromFile(Path.Combine(netCorePath, "System.Runtime.dll")), // System.Runtime.dll
                    MetadataReference.CreateFromFile(Path.Combine(netCorePath, "System.Collections.dll")), // System.Collections.dll
                    MetadataReference.CreateFromFile(typeof(Queryable).Assembly.Location), // System.Linq.Queryable.dll
                    MetadataReference.CreateFromFile(typeof(int).Assembly.Location), // System.Private.CoreLib.dll
                };
            }

            if (specificReferencesToInclude != null)
            {
                references = references.Zip(specificReferencesToInclude, (x, y) => (x, y)).Where(x => x.y).Select(x => x.x).ToArray();
            }

            return references;
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

        #endregion
    }
}
