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
                bool nullableEnabledContext = i % 2 == 0;
                var objectTypeCollection = new LazinatorObjectTypeCollection(r, namespaceString: "n" + i.ToString(), nullableEnabledContext: nullableEnabledContext, numObjectTypes: 3, maxClassDepth: 3, maxProperties: 2, numTests: 100, numMutationSteps: 10);
                if (objectTypeCollection.Succeeded == false)
                    break;
            }

        }

        
    }
}
