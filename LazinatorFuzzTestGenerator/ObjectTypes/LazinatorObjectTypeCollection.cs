using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeGenHelper;
using LazinatorFuzzTestGenerator.ObjectValues;
using LazinatorFuzzTestGenerator.Utility;
using LazinatorGenerator.Generator;
using Microsoft.CodeAnalysis;

namespace LazinatorFuzzTestGenerator.ObjectTypes
{
    public class LazinatorObjectTypeCollection
    {
        public string NamespaceString { get; init; }
        public bool NullableContextEnabled { get; init; }
        public List<LazinatorObjectType> ObjectTypes { get; set; } = new List<LazinatorObjectType>();
        public IEnumerable<LazinatorObjectType> InstantiableObjectTypes => ObjectTypes.Where(x => x.Instantiable);
        public IEnumerable<LazinatorClassType> InheritableClassTypes => ObjectTypes.Where(x => x.Inheritable).Select(x => (LazinatorClassType)x);
        public UniqueCSharpNameGenerator UniqueCSharpNameGenerator { get; set; } = new UniqueCSharpNameGenerator();
        int UniqueIDCounter = 10_000;
        public bool Succeeded { get; set; }

        public LazinatorObjectTypeCollection(string namespaceString, bool nullableContextEnabled)
        {
            this.NamespaceString = namespaceString;
            this.NullableContextEnabled = nullableContextEnabled;
        }

        public LazinatorObjectTypeCollection(Random r, string namespaceString, bool nullableContextEnabled, int numObjectTypes, int maxClassDepth, int maxProperties, int numTests, int numMutationSteps, bool writeIfSuccessfullyGenerated = false) : this(namespaceString, nullableContextEnabled)
        {
            string folder = ReadCodeFile.GetCodeBasePath("LazinatorFuzzGeneratedTests" + (nullableContextEnabled ? "2" : "")) + "\\" + NamespaceString + "\\";
            GenerateObjectTypes(numObjectTypes, maxClassDepth, maxProperties, r);
            List<(string folder, string filename, string code)> originalSources = GenerateSources();
            Compilation compilationOriginalSources = LazinatorCodeGeneration.CreateCompilation(originalSources, nullableContextEnabled); // note that this compilation will have plenty of errors, because it will be missing the generated code
            Compilation? compilationIncludingGeneratedSources = null;
            Compilation? compilationIncludingTestingCode = null;
            List<string> codeForTests = GenerateCodeForIndividualTests(r, numTests, numMutationSteps);
            string testsFileCode_ForTestingProject = GenerateTestsFile(codeForTests, true);
            string testsFileCode_ForImmediateExecution = GenerateTestsFile(codeForTests, false);
            List<(string folder, string filename, string code)> originalSourcesPlusGenerated = new List<(string folder, string filename, string code)>();
            List<LazinatorCodeGenerationResult> codeGenerationResults = LazinatorCodeGeneration.GenerateLazinatorCodeBehindFiles(compilationOriginalSources);
            bool success = !codeGenerationResults.Any(x => x.Diagnostic != null);
            if (success)
            {
                originalSourcesPlusGenerated = originalSources.ToList();
                foreach (var codeGenerationResult in codeGenerationResults)
                {
                    originalSourcesPlusGenerated.Add((NamespaceString, codeGenerationResult.Path, codeGenerationResult.GeneratedCode));
                }
                compilationIncludingGeneratedSources = LazinatorCodeGeneration.CreateCompilation(originalSourcesPlusGenerated, nullableContextEnabled);
                success = AssessCompilationSuccess(compilationIncludingGeneratedSources);
                if (success)
                {
                    // combine everything in compilation
                    var sourcesPlusTestCode = originalSourcesPlusGenerated.ToList();
                    sourcesPlusTestCode.Add((NamespaceString, "Tests.cs", testsFileCode_ForImmediateExecution));
                    compilationIncludingTestingCode = LazinatorCodeGeneration.CreateCompilation(sourcesPlusTestCode, nullableContextEnabled);

                    success = AssessCompilationSuccess(compilationIncludingTestingCode);
                    if (success)
                        success = ExecuteCode.ExecuteTestingCode(compilationIncludingTestingCode, "FuzzTests." + NamespaceString, "TestRunner", "RunAllTests");
                }
            }
            Succeeded = success;

            void WriteMainSources()
            {
                foreach (var source in originalSourcesPlusGenerated)
                {
                    File.WriteAllText(folder + source.filename, source.code);
                }
                File.WriteAllText(folder + "Tests.cs", testsFileCode_ForTestingProject);
                //File.WriteAllText(folder + "TestsAlt.cs", testsFileCode_ForImmediateExecution);
            }

            bool writeIfNotSuccessfullyGenerated = true;
            bool write = (writeIfSuccessfullyGenerated && success) || (writeIfNotSuccessfullyGenerated && !success);
            if (write || (writeIfSuccessfullyGenerated == false && writeIfNotSuccessfullyGenerated == true))
            {
                // we actually want to delete the files if they were generated a previous time
                // when it was not successful, or if we are going to be writing new files
                if (Path.Exists(folder))
                    Directory.Delete(folder, true);
            }
            if (write)
            {
                if (!Path.Exists(folder))
                    Directory.CreateDirectory(folder);
                WriteMainSources();
            }
            if (!success || write)
            {
                var recentCompilation = compilationIncludingTestingCode ?? compilationIncludingGeneratedSources ?? compilationOriginalSources;
                foreach (var diagnostic in recentCompilation.GetDiagnostics())
                {
                    if (diagnostic.DefaultSeverity != Microsoft.CodeAnalysis.DiagnosticSeverity.Hidden) // suppress, e.g., diagnostics about unnecessary using statements
                        Console.WriteLine(diagnostic);
                }
                foreach (var result in codeGenerationResults)
                {
                    var diagostic = result.Diagnostic;
                    if (diagostic != null)
                    {
                        Console.WriteLine($"{result.Path}: {diagostic}");
                    }
                }
            }

            static bool AssessCompilationSuccess(Compilation compilation)
            {
                var diagnostics = compilation.GetDiagnostics().Where(x => x.DefaultSeverity != Microsoft.CodeAnalysis.DiagnosticSeverity.Hidden).ToList();
                bool successCompiling = !diagnostics.Any();
                return successCompiling;
            }
        }

        public void GenerateObjectTypes(int numObjectTypes, int maximumDepth, int maxProperties, Random r)
        {
            for (int i = 0; i < numObjectTypes; i++)
            {
                int uniqueID = UniqueIDCounter++;
                string uniqueName = UniqueCSharpNameGenerator.GetUniqueName(r, true);
                int numProperties = r.Next(0, maxProperties + 1);
                List<LazinatorObjectProperty> properties = new List<LazinatorObjectProperty>();
                for (int j = 0; j < numProperties; j++)
                {
                    properties.Add(GenerateRandomObjectProperty(r));
                }
                if (r.Next(0, 2) == 0)
                {
                    LazinatorStructType structType = new LazinatorStructType(uniqueID, uniqueName + "Struct", properties, NullableContextEnabled);
                    ObjectTypes.Add(structType);
                }
                else
                {
                    bool isAbstract = r.Next(0, 4) == 0;
                    bool isSealed = !isAbstract && r.Next(0, 3) == 0;
                    var inheritable = InheritableClassTypes.Where(x => x.ObjectDepth < maximumDepth).ToList();
                    if (r.Next(0, 2) == 0 || !inheritable.Any())
                    {
                        LazinatorClassType classType = new LazinatorClassType(uniqueID, uniqueName + "Class", isAbstract, isSealed, null, properties, NullableContextEnabled);
                        ObjectTypes.Add(classType);
                    }
                    else
                    {
                        LazinatorClassType parent = inheritable[r.Next(0, inheritable.Count())];
                        LazinatorClassType classType = new LazinatorClassType(uniqueID, uniqueName + "Class", isAbstract, isSealed, parent, properties, NullableContextEnabled);
                        ObjectTypes.Add(classType);
                    }
                }
            }
        }

        public List<(string folder, string filename, string code)> GenerateSources()
        {
            List<(string folder, string filename, string code)> result = new List<(string folder, string filename, string code)>();
            foreach (LazinatorObjectType objectType in ObjectTypes)
            {
                result.Add((NamespaceString, objectType.Name + ".cs", objectType.ObjectDeclaration(NamespaceString)));
                result.Add((NamespaceString, "I" + objectType.Name + ".cs", objectType.ILazinatorDeclaration(NamespaceString)));
            }
            return result;
        }

        private LazinatorObjectProperty GenerateRandomObjectProperty(Random r)
        {
            bool nullable = r.Next(0, 2) == 1;

            if (r.Next(0, 2) == 0 || !InstantiableObjectTypes.Any())
            {
                var primitiveType = new PrimitiveType(r, NullableContextEnabled);
                if (primitiveType.UnannotatedIsNullable())
                    nullable = true; // can't have non-nullable string outside nullable enabled context
                LazinatorObjectProperty property = new LazinatorObjectProperty(UniqueCSharpNameGenerator.GetUniqueName(r, true), primitiveType, nullable);
                return property;
            }
            else
            {
                var instantiableChoices = InstantiableObjectTypes.ToList();
                LazinatorObjectType instantiableObject = instantiableChoices[r.Next(0, instantiableChoices.Count)];
                if (instantiableObject.UnannotatedIsNullable())
                    nullable = true; // can't have non-nullable class outside nullable enabled context
                LazinatorObjectProperty property = new LazinatorObjectProperty(UniqueCSharpNameGenerator.GetUniqueName(r, true), instantiableObject, nullable);
                return property;
            }
        }

        private List<string> GenerateCodeForIndividualTests(Random r, int numTests, int numMutationSteps)
        {
            List<string> results = new List<string>();
            for (int i = 0; i < numTests; i++)
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine($"public void Test{i}()");
                sb.AppendLine("{");
                sb.AppendLine(GetAndTestSequenceOfMutations(r, numMutations: numMutationSteps, true, i));
                sb.AppendLine("}");
                results.Add(sb.ToString());
            }
            return results;
        }

        private string GenerateTestsFile(List<string> codeForIndividualTests, bool forTestingProject)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(@$"
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Lazinator.Core;
using Lazinator.Wrappers;
using Lazinator.Collections.Tuples;
using Lazinator.Buffers;
using System.Threading.Tasks;
using Lazinator.Exceptions;
{(forTestingProject ? @"using FluentAssertions;
using Xunit;" : "")}

namespace FuzzTests.{NamespaceString}
{{
    public partial class Tests{(forTestingProject? "" : "Alt")}
    {{

");
            for (int i = 0; i < codeForIndividualTests.Count; i++)
            {
                if (forTestingProject)
                    sb.AppendLine("[Fact]");
                sb.AppendLine(codeForIndividualTests[i]);
            }
            sb.AppendLine("}"); // end of class Tests
            if (!forTestingProject)
                sb.AppendLine($@"

    public static class TestRunner
    {{
        public static bool RunAllTests()
        {{
            var runner = new TestsAlt();
{String.Join("\r\n", Enumerable.Range(0, codeForIndividualTests.Count).Select(x => $"                runner.Test{x}();"))}
            return true;
        }}
    }};
");
            sb.AppendLine(@$"
}}
");
            return CodeFormatter.IndentCode(sb.ToString());
        }

        public string GetAndTestSequenceOfMutations(Random r, int numMutations, bool checkOnlyAfterAll, int testNumber)
        {
            if (InstantiableObjectTypes.Any())
            {
                var objectType = InstantiableObjectTypes.ElementAt(r.Next(InstantiableObjectTypes.Count()));
                LazinatorMutator mutator = new LazinatorMutator(r, objectType);
                string s = mutator.GetAndTestSequenceOfMutations(numMutations, checkOnlyAfterAll);
                return s;
            }
            return "";
        }
    }
}
