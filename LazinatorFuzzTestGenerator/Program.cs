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
            Random r = new Random(0);
            var objectTypeCollection = new LazinatorObjectTypeCollection(r, namespaceString: "n1", nullableEnabledContext: false, numObjectTypes: 100, maxClassDepth: 4, maxProperties: 3, numTests: 1000, numMutationSteps: 10);

        }

        
    }
}
