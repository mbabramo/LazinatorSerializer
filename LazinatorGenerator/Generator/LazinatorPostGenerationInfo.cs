using LazinatorGenerator.Support;
using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Text;

namespace LazinatorGenerator.Generator
{
    /// <summary>
    /// This struct contains information to be stored about a type after initial generation of source is complete. Initially, LazinatorPreGenerationInfo produces a LazinatorPostGenerationInfo. We then combine all of these into an immutable array, some elements of which may include newly generated source but some elements of which may be cached. Our challenge is that we will need to regenerate source that was cached, if any of the dependencies have changed. Thus, we update each element based on the other elements of the immutable array, so that if there is a dependency change, the LazinatorPostGenerationInfo will produce a new hash code and it will not be cached.
    /// </summary>
    internal readonly record struct LazinatorPostGenerationInfo
    {
        internal readonly LazinatorPreGenerationInfo PreGenerationInfo { get; }
        internal readonly LazinatorCodeGenerationResult AlreadyGeneratedCode { get; }
        internal readonly int HashCode { get; }

        public LazinatorPostGenerationInfo(LazinatorPreGenerationInfo preGenerationInfo, LazinatorCodeGenerationResult alreadyGeneratedCode)
        {
            PreGenerationInfo = preGenerationInfo;
            AlreadyGeneratedCode = alreadyGeneratedCode;
            HashCode = (PreGenerationInfo, AlreadyGeneratedCode).GetHashCode();
        }

        public override int GetHashCode()
        {
            return HashCode;
        }

        /// <summary>
        /// This methods separates an immutable array of this type into individual items, modifying each so that it contains the most up-to-date hash codes of any other item on which it has dependencies. This helps with caching. 
        /// </summary>
        /// <param name="collectedInputs"></param>
        /// <returns></returns>
        public static IEnumerable<LazinatorPostGenerationInfo> SeparateForNextPipelineStep(ImmutableArray<LazinatorPostGenerationInfo> collectedInputs)
        {
            Dictionary<string, int> typeNameToHash = new Dictionary<string, int>();
            foreach (var input in collectedInputs)
                if (input.AlreadyGeneratedCode.ContainsSuccessfullyGeneratedCode)
                    typeNameToHash[input.AlreadyGeneratedCode.GeneratedType] = input.HashCode;

            foreach (var input in collectedInputs)
                yield return input.WithUpdatedHashCodes(typeNameToHash);
        }

        public LazinatorPostGenerationInfo WithUpdatedHashCodes(Dictionary<string, int> typeNameToHash)
        {
            return new LazinatorPostGenerationInfo(PreGenerationInfo, AlreadyGeneratedCode.WithUpdatedHashCodes(typeNameToHash));
        }
        public void GenerateSource(SourceProductionContext spc, IDateTimeNow dateTimeNowProvider)
        {
            if (AlreadyGeneratedCode.GeneratedCode.Contains("CloneOrChange_List_GT_g(List<T> itemToClone"))
            {
                var DEBUG = 0;
            }
            if (AlreadyGeneratedCode.GeneratedCode != null)
            {
                spc.AddSource(AlreadyGeneratedCode.Path, AlreadyGeneratedCode.GeneratedCode); // We already generated the path and text at an earlier stage of this run through the pipeline. This was called because this LazinatorPostGenerationInfo had not been cached yet, but that doesn't matter. We know that we have just generated the source, and so we don't need to update it.
            }
            else
            {
                // Maybe we had a problem generating the code. If so, we should report the diagnostic.
                if (AlreadyGeneratedCode.Diagnostic != null)
                {
                    var diagnosticString = AlreadyGeneratedCode.Diagnostic.ToString();
                    spc.ReportDiagnostic(AlreadyGeneratedCode.Diagnostic);
                }
            }
        }

    }
}
