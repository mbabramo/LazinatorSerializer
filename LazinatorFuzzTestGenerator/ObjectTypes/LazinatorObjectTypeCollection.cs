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

namespace LazinatorFuzzTestGenerator.ObjectTypes
{
    public class LazinatorObjectTypeCollection
    {
        public string NamespaceString { get; init; }
        public bool NullableEnabledContext { get; init; }
        public List<LazinatorObjectType> ObjectTypes { get; set; } = new List<LazinatorObjectType>();
        public IEnumerable<LazinatorObjectType> InstantiableObjectTypes => ObjectTypes.Where(x => x.Instantiable);
        public IEnumerable<LazinatorClassType> InheritableClassTypes => ObjectTypes.Where(x => x.Inheritable).Select(x => (LazinatorClassType)x);
        public UniqueCSharpNameGenerator UniqueCSharpNameGenerator { get; set; } = new UniqueCSharpNameGenerator();
        int UniqueIDCounter = 10_000;

        public LazinatorObjectTypeCollection(string namespaceString, bool nullableEnabledContext)
        {
            this.NamespaceString = namespaceString;
            this.NullableEnabledContext = nullableEnabledContext;
        }

        public LazinatorObjectTypeCollection(Random r, string namespaceString, bool nullableEnabledContext, int numObjectTypes, int maxClassDepth, int maxProperties, int numTests, int numMutationSteps) : this(namespaceString, nullableEnabledContext)
        {
            string folder = ReadCodeFile.GetCodeBasePath("LazinatorFuzzGeneratedTests" + (nullableEnabledContext ? "2" : "")) + "\\" + NamespaceString + "\\";
            GenerateObjectTypes(numObjectTypes, maxClassDepth, maxProperties, r);
            List<(string folder, string filename, string code)> mainSources = GenerateSources();
            var compilation = LazinatorCodeGeneration.CreateCompilation(mainSources);
            List<LazinatorCodeGenerationResult> results = LazinatorCodeGeneration.GenerateLazinatorCodeBehindFiles(compilation);
            bool success = !results.Any(x => x.Diagnostic != null);
            if (success)
            {
                mainSources = mainSources.ToList();
                foreach (var result in results)
                {
                    mainSources.Add((NamespaceString, result.Path, result.GeneratedCode));
                }
                compilation = LazinatorCodeGeneration.CreateCompilation(mainSources);
                results = LazinatorCodeGeneration.GenerateLazinatorCodeBehindFiles(compilation);
                success = !results.Any(x => x.Diagnostic != null) && !compilation.GetDiagnostics().Any();
                if (success)
                {
                    // combine everything in compilation
                    var allSources = mainSources.ToList();
                    allSources.AddRange(results.Select(x => (NamespaceString, x.Path, x.GeneratedCode)));
                    allSources.Add((NamespaceString, folder, GenerateTestsFile(r, numTests, numMutationSteps)));
                    compilation = LazinatorCodeGeneration.CreateCompilation(allSources);
                    success = !results.Any(x => x.Diagnostic != null) && !compilation.GetDiagnostics().Any();
                    if (success)
                        success = ExecuteCode.ExecuteTestingCode(compilation, NamespaceString, "TestRunner", "RunAllTests");
                }
            }

            void WriteMainSources()
            {
                foreach (var source in mainSources)
                {
                    File.WriteAllText(folder + source.filename, source.code);
                }
            }

            bool writeIfSuccessfullyGenerated = true;
            bool writeIfNotSuccessfullyGenerated = true;
            bool write = (writeIfSuccessfullyGenerated && success) || (writeIfNotSuccessfullyGenerated && !success);
            if (write)
            {
                if (!Path.Exists(folder))
                    Directory.CreateDirectory(folder);
                WriteMainSources();
            }
            if (!success || write)
            {
                foreach (var diagnostic in compilation.GetDiagnostics())
                {
                    if (diagnostic.DefaultSeverity != Microsoft.CodeAnalysis.DiagnosticSeverity.Hidden) // suppress, e.g., diagnostics about unnecessary using statements
                        Console.WriteLine(diagnostic);
                }
                foreach (var result in results)
                {
                    File.WriteAllText(folder + "\\" + result.Path, result.GeneratedCode);
                    var diagostic = result.Diagnostic;
                    if (diagostic != null)
                    {
                        Console.WriteLine($"{result.Path}: {diagostic}");
                    }
                }
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
                    LazinatorStructType structType = new LazinatorStructType(uniqueID, uniqueName + "Struct", properties);
                    ObjectTypes.Add(structType);
                }
                else
                {
                    bool isAbstract = r.Next(0, 4) == 0;
                    bool isSealed = !isAbstract && r.Next(0, 3) == 0;
                    var inheritable = InheritableClassTypes.Where(x => x.ObjectDepth < maximumDepth).ToList();
                    if (r.Next(0, 2) == 0 || !inheritable.Any())
                    {
                        LazinatorClassType classType = new LazinatorClassType(uniqueID, uniqueName + "Class", isAbstract, isSealed, null, properties);
                        ObjectTypes.Add(classType);
                    }
                    else
                    {
                        LazinatorClassType parent = inheritable[r.Next(0, inheritable.Count())];
                        LazinatorClassType classType = new LazinatorClassType(uniqueID, uniqueName + "Class", isAbstract, isSealed, parent, properties);
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
                result.Add((NamespaceString, objectType.Name + ".cs", objectType.ObjectDeclaration(NamespaceString, NullableEnabledContext)));
                result.Add((NamespaceString, "I" + objectType.Name + ".cs", objectType.ILazinatorDeclaration(NamespaceString, NullableEnabledContext)));
            }
            return result;
        }

        private LazinatorObjectProperty GenerateRandomObjectProperty(Random r)
        {
            bool nullable = r.Next(0, 2) == 1;

            if (r.Next(0, 2) == 0 || !InstantiableObjectTypes.Any())
            {
                var primitiveType = new PrimitiveType(r);
                if (primitiveType.UnannotatedIsNullable(NullableEnabledContext))
                    nullable = true; // can't have non-nullable string outside nullable enabled context
                LazinatorObjectProperty property = new LazinatorObjectProperty(UniqueCSharpNameGenerator.GetUniqueName(r, true), primitiveType, nullable);
                return property;
            }
            else
            {
                var instantiableChoices = InstantiableObjectTypes.ToList();
                LazinatorObjectType instantiableObject = instantiableChoices[r.Next(0, instantiableChoices.Count)];
                if (instantiableObject.UnannotatedIsNullable(NullableEnabledContext))
                    nullable = true; // can't have non-nullable class outside nullable enabled context
                LazinatorObjectProperty property = new LazinatorObjectProperty(UniqueCSharpNameGenerator.GetUniqueName(r, true), instantiableObject, nullable);
                return property;
            }
        }

        public string GenerateTestsFile(Random r, int numTests, int numMutationSteps)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(@$"
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using FluentAssertions;
using Lazinator.Core;
using Xunit;
using Lazinator.Wrappers;
using Lazinator.Collections.Tuples;
using Lazinator.Buffers;
using System.Threading.Tasks;
using Lazinator.Exceptions;

namespace FuzzTests.{NamespaceString}
{{
    public partial class Tests
    {{

");
            for (int i = 0; i < numTests; i++)
            {
                sb.AppendLine("[Fact]");
                sb.AppendLine($"public void Test{i}()");
                sb.AppendLine("{");
                sb.AppendLine(GetAndTestSequenceOfMutations(r, numMutations: numMutationSteps, true, i));
                sb.AppendLine("}");
            }
            sb.AppendLine($@"

    public static class TestRunner
    {{
        public bool static RunAllTests()
        {{
            var runner = new Tests();
            {String.Join("\n", Enumerable.Range(0, numTests).Select(x => $"            runner.Test{x}();"))}
            return true;
        }}
    }};
");
            sb.AppendLine(@$"
    }}
}}
");
            return CodeFormatter.IndentCode(sb.ToString());
        }

        public string GetAndTestSequenceOfMutations(Random r, int numMutations, bool checkOnlyAfterAll, int testNumber)
        {
            if (InstantiableObjectTypes.Any())
            {
                var objectType = InstantiableObjectTypes.ElementAt(r.Next(InstantiableObjectTypes.Count()));
                var objectContents = (LazinatorObjectContents) objectType.GetRandomObjectContents(r, 5);
                string s = objectContents.GetAndTestSequenceOfMutations(r, numMutations, checkOnlyAfterAll);
                return s;
            }
            return "";
        }
    }
}
