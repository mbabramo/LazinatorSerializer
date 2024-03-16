namespace LazinatorFuzzTestGenerator
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Random r = new Random(0);
            string namespaceString = "n1";
            bool nullableEnabledContext = false;
            LazinatorObjectTypeCollection c = new LazinatorObjectTypeCollection(namespaceString, nullableEnabledContext);
            int numObjectTypes = 100;
            int maxClassDepth = 5;
            int maxProperties = 10;
            c.GenerateObjectTypes(numObjectTypes, maxClassDepth, maxProperties, r);
            var sources = c.GenerateSources();
            var compilation = CodeGeneration.CreateCompilation(sources);
            var types = CodeGeneration.FindTypesWithLazinatorAttribute(compilation);
        }
    }
}
