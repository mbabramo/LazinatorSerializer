using Microsoft.CodeAnalysis;

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
            int numObjectTypes = 50;
            int maxClassDepth = 5;
            int maxProperties = 10;
            c.GenerateObjectTypes(numObjectTypes, maxClassDepth, maxProperties, r);
            List<(string folder, string filename, string code)> sources = c.GenerateSources();
            foreach (var source in sources)
            {
                File.WriteAllText("C:\\Users\\Admin\\Documents\\GitHub\\LazinatorSerializer\\AdditionalLazinatorProject\\Temp\\" + source.filename, source.code);
            }
            var compilation = LazinatorCodeGeneration.CreateCompilation(sources);
            List<LazinatorGenerator.Generator.LazinatorCodeGenerationResult> results = LazinatorCodeGeneration.GenerateLazinatorCodeBehindFiles(compilation);
            foreach (var result in results)
            {
                File.WriteAllText("C:\\Users\\Admin\\Documents\\GitHub\\LazinatorSerializer\\AdditionalLazinatorProject\\Temp\\" + result.Path, result.GeneratedCode);
                var diagostic = result.Diagnostic;
                if (diagostic != null)
                {
                    Console.WriteLine($"{result.Path}: {diagostic}");
                }
            }

        }
    }
}
