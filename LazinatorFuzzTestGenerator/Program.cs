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
                success = !results.Any(x => x.Diagnostic != null);
            }

            if (success)
            {
                string folder = ReadCodeFile.GetCodeBasePath("LazinatorFuzzGeneratedTests") + "\\" + c.namespaceString + "\\"; 
                if (!Path.Exists(folder))
                    Directory.CreateDirectory(folder);
                foreach (var source in mainSources)
                {
                    File.WriteAllText(folder + source.filename, source.code);
                }
            }
            else
            {
                bool writeEvenIfFailed = false;
                string folder = Path.Combine(ReadCodeFile.GetCodeBasePath("LazinatorFuzzGeneratedTests"), c.namespaceString);
                if (writeEvenIfFailed && !Path.Exists(folder))
                    Directory.CreateDirectory(folder);
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
