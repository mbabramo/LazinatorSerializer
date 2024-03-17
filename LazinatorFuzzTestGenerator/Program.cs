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
            int numObjectTypes = 2;
            int maxClassDepth = 1;
            int maxProperties = 10;
            c.GenerateObjectTypes(numObjectTypes, maxClassDepth, maxProperties, r);
            var sources = c.GenerateSources();
            foreach (var source in sources)
            {
                File.WriteAllText("C:\\Users\\Admin\\Documents\\GitHub\\LazinatorSerializer\\AdditionalLazinatorProject\\Temp\\" + source.filename, source.code);
            }
            var compilation = CodeGeneration.CreateCompilation(sources);
            var types = CodeGeneration.FindTypesWithLazinatorAttribute(compilation);
        }
    }
}
