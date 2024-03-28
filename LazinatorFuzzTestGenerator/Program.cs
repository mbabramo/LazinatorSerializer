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
            bool failureHasOccurred = false;
            int startingIteration = 14;
            int iterations = 100;
            bool useParallel = false;
            if (useParallel)
            {
                Parallel.For(startingIteration, iterations, i =>
                   {
                    failureHasOccurred = ExecuteIterationAndReportOutcome(failureHasOccurred, i);
                });
            }
            else
            {
                for (int i = startingIteration; i < iterations; i++)
                {
                    failureHasOccurred = ExecuteIterationAndReportOutcome(failureHasOccurred, i);
                };
            }
        }

        private static bool ExecuteIterationAndReportOutcome(bool failureHasOccurred, int i)
        {
            if (!failureHasOccurred)
            {
                bool success = ExecuteIteration(i);
                if (success)
                    Console.WriteLine($"Iteration {i}: Succeeded without errors.");
                else
                {
                    Console.WriteLine($"Iteration {i}: Failed.");
                    failureHasOccurred = true;
                }

            }

            return failureHasOccurred;
        }

        private static bool ExecuteIteration(int i)
        {
            CodeStringBuilder.LocationIndex = 100_000 * i; // generate unique but predictable location indices in code files, so that if there is a problem with the code, we can easily stop at that location by changing CodeStringBuilder.StopAtLocationIndex
            Random r = new Random(i);
            bool nullableContextEnabled = i % 2 == 0;
           var objectTypeCollection = new LazinatorObjectTypeCollection(r, namespaceString: "n" + i.ToString(), nullableContextEnabled: nullableContextEnabled, numObjectTypes: 4, maxClassDepth: 3, maxProperties: 3, numTests: 20 /* note: too many may cause stack overflow error on compiling */, numMutationSteps: 3, writeIfSuccessfullyGenerated: false);
            bool success = objectTypeCollection.Succeeded;
            return success;
        }

    }
}
