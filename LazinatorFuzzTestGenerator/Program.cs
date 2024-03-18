using Microsoft.CodeAnalysis;
using LazinatorGenerator.Generator;
using CodeGenHelper;

namespace LazinatorFuzzTestGenerator
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Random r = new Random(0);
            string namespaceString = "n1";
            bool nullableEnabledContext = true;
            LazinatorObjectTypeCollection c = new LazinatorObjectTypeCollection(namespaceString, nullableEnabledContext);
            int numObjectTypes = 5;
            int maxClassDepth = 5;
            int maxProperties = 10;
            c.GenerateObjectTypes(numObjectTypes, maxClassDepth, maxProperties, r);
            List<(string folder, string filename, string code)> mainSources = c.GenerateSources();
            var compilation = LazinatorCodeGeneration.CreateCompilation(mainSources);
            List<LazinatorCodeGenerationResult> results = LazinatorCodeGeneration.GenerateLazinatorCodeBehindFiles(compilation);
            bool success = !results.Any(x => x.Diagnostic != null);
            if (success)
            {
                mainSources = mainSources.ToList();
                foreach (var result in results)
                {
                    mainSources.Add((c.namespaceString, result.Path, result.GeneratedCode));
                }
                compilation = LazinatorCodeGeneration.CreateCompilation(mainSources);
                results = LazinatorCodeGeneration.GenerateLazinatorCodeBehindFiles(compilation);
                success = !results.Any(x => x.Diagnostic != null) && !compilation.GetDiagnostics().Any();
            }

            string folder = ReadCodeFile.GetCodeBasePath("LazinatorFuzzGeneratedTests") + "\\" + c.namespaceString + "\\";
            void WriteMainSources()
            {
                foreach (var source in mainSources)
                {
                    File.WriteAllText(folder + source.filename, source.code);
                }
            }

            bool writeEvenIfFailed = true;
            bool write = success || writeEvenIfFailed;
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
                    Console.WriteLine(diagnostic);
                }
                foreach (var result in results)
                {
                    if (writeEvenIfFailed)
                        File.WriteAllText(folder + "\\" + result.Path, result.GeneratedCode);
                    var diagostic = result.Diagnostic;
                    if (diagostic != null)
                    {
                        Console.WriteLine($"{result.Path}: {diagostic}");
                    }
                }
            }

        }
    }
}
