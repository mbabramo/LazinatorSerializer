using LazinatorTests.Tests;

namespace ConsoleAppForProfiling
{
    internal class Program
    {
        async static Task Main(string[] args)
        {
            CodeGenTest t = new CodeGenTest();
            await t.CodeGenerationProducesActualCode_CoreCollections();
            await t.CodeGenerationProducesActualCode_Wrappers();
            await t.CodeGenerationProducesActualCode_BuffersAndPersistence();
        }
    }
}
