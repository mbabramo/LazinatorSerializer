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
            for (int i = 0; i < 20; i++)
            {
                Console.WriteLine($"iteration {i}");
                Random r = new Random(0);
                var objectTypeCollection = new LazinatorObjectTypeCollection(r, namespaceString: "n" + i.ToString(), nullableEnabledContext: true, numObjectTypes: 100, maxClassDepth: 4, maxProperties: 4, numTests: 1000, numMutationSteps: 10);
            }

        }

        
    }
}
