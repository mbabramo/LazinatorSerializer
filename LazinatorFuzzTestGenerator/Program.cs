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
            string namespaceString = "n1";
            bool nullableEnabledContext = true;
            int numObjectTypes = 10;
            int maxClassDepth = 5;
            int maxProperties = 5;
            int numTests = 100;
            int numMutationSteps = 10;
            var objectTypeCollection = new LazinatorObjectTypeCollection(r, namespaceString, nullableEnabledContext, numObjectTypes, maxClassDepth, maxProperties, numTests, numMutationSteps);

        }

        
    }
}
