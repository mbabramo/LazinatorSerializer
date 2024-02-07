
using Lazinator.Core;
using Lazinator.Collections;
using Lazinator.Wrappers;
using System.Diagnostics;

namespace ProjectForDebuggingGenerator
{
    internal class Program
    {
        static Task Main(string[] args)
        {
            MyLazinator myLazinator = new MyLazinator();
            //myLazinator.MyInt = 5;
            myLazinator.MyLazinatorList = new LazinatorList<WInt32>() { 1, 2, 3 };
            myLazinator.MyList = new List<int>() { 4, 5 };
            var ml2 = myLazinator.CloneLazinatorTyped();

            return Task.CompletedTask;

            //CodeGenTest t = new CodeGenTest();
            //await t.CodeGenerationProducesActualCode_CoreCollections();
            //await t.CodeGenerationProducesActualCode_Wrappers();
            //await t.CodeGenerationProducesActualCode_BuffersAndPersistence();
        }
    }
}
