using Microsoft.CodeAnalysis;
using LazinatorGenerator.Generator;
using CodeGenHelper;
using LazinatorFuzzTestGenerator.ObjectTypes;
using LazinatorFuzzTestGenerator.Utility;
using System.Text;
using Lazinator.CodeDescription;

namespace LazinatorFuzzTestGenerator
{
    internal class Program
    {
        static void Main(string[] args)
        {
            for (int i = 0; i < 100; i++)
            {
                Console.WriteLine($"iteration {i}");
                CodeStringBuilder.LocationIndex = 100_000 * i; // generate unique but predictable location indices in code files, so that if there is a problem with the code, we can easily stop at that location by changing CodeStringBuilder.StopAtLocationIndex
                Random r = new Random(i);
                bool nullableEnabledContext = i % 2 == 0;
                var objectTypeCollection = new LazinatorObjectTypeCollection(r, namespaceString: "n" + i.ToString(), nullableEnabledContext: nullableEnabledContext, numObjectTypes: 10, maxClassDepth: 5, maxProperties: 5, numTests: 100, numMutationSteps: 15);
                if (objectTypeCollection.Succeeded == false)
                    break;
            }

        }

        
    }
}
