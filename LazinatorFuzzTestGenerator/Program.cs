using Microsoft.CodeAnalysis;
using LazinatorGenerator.Generator;
using CodeGenHelper;
using LazinatorFuzzTestGenerator.ObjectTypes;
using LazinatorFuzzTestGenerator.Utility;
using System.Text;

namespace LazinatorFuzzTestGenerator
{
    internal class Program
    {
        static void Main(string[] args)
        {
            for (int i = 0; i < 100; i++)
            {
                Console.WriteLine($"iteration {i}");
                Random r = new Random(i);
                bool nullableEnabledContext = r.Next(2) == 0;
                var objectTypeCollection = new LazinatorObjectTypeCollection(r, namespaceString: "n" + i.ToString(), nullableEnabledContext: nullableEnabledContext, numObjectTypes: 25, maxClassDepth: 4, maxProperties: 4, numTests: 100, numMutationSteps: 10);
            }

        }

        
    }
}
