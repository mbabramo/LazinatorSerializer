
using Lazinator.Core;
using Lazinator.Collections;
using Lazinator.Wrappers;

namespace ConsoleAppForProfiling
{
    internal class Program
    {
        async static Task Main(string[] args)
        {
            MyLazinator myLazinator = new MyLazinator();
            myLazinator.MyInt = 5;
            myLazinator.MyLazinatorList = new LazinatorList<WInt32>() { 1, 2, 3 };
            var ml2 = myLazinator.CloneLazinatorTyped();

            //CodeGenTest t = new CodeGenTest();
            //await t.CodeGenerationProducesActualCode_CoreCollections();
            //await t.CodeGenerationProducesActualCode_Wrappers();
            //await t.CodeGenerationProducesActualCode_BuffersAndPersistence();
        }
    }
}
