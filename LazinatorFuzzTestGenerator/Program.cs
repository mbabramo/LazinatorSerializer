namespace LazinatorFuzzTestGenerator
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Random r = new Random(0);
            LazinatorObjectTypeCollection c = new LazinatorObjectTypeCollection();
            c.GenerateObjectTypes(100, 5, 10, r);
            var result = c.GenerateSources();
        }
    }
}
