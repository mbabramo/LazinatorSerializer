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

            Debug.WriteLine($"Postgeneration info with hash code {GetHashCode()}"); // DEBUG
        }

        public override int GetHashCode()
        {
            return HashCode;
        }

        // Scenario 1. First time generation of this type occurred just before creation of this object. The already-generated source will be stored. Later in the pipeline, dependencies will be stored. When GenerateSource is called, we will see that the pipeline run unique ID matches and that we can return the source that was created.
        // Scenario 2. This type had been previously generated, and there has been no change in the ILazinator file or in any dependency. The cached version of LazinatorPreGenerationInfo will be used to generate this LazinatorPostGenerationInfo, using the earlier pipeline run uniqueID. After dependency information is added to create another LazinatorPostGenerationInfo, it will match what was used when the type was originally generated, and so GenerateSource in this object will not be called. The cached version of the ultimate sources to generate will be used instead.
        // Scenario 3. This type had been previously generated, but there has been a change in the ILazinator file. Thus, the cached version of LazinatorPreGenerationInfo will not be used by the pipeline, and this LazinatorPostGenerationInfo will be newly created instead, with the new pipeline run unique ID and the most recent source. For that reason and because of the changed text, there will be no cached version of LazinatorPostGenerationInfo's output.  Thus, when GenerateSource is called, we will see that the pipeline run unique ID matches (i.e., we're still on the same pipeline), and we will return that rather than regenerating the exact same thing.
        // Scenario 4. This type had been previously generated, and there has been no change in the ILazinator file, but there has been a change in a dependency. The cache will see a match of the LazinatorPreGenerationInfo that will thus be used to lookup an already existing LazinatorPostGenerationInfo, but we will then change the dependency information. When GenerateSource is called, we will see that the pipeline run unique ID does not match, and thus the source stored in this object cannot be used. So, we will regenerate the source. (Subsequent run if there is no change: Suppose afterwards, the code runs again. Once again, LazinatorPreGenerationInfo will pull up this LazinatorPostGenerationInfo. The text listed will still be out of date. However, at this point the cache will associate this object with the source that we just regenerated, and so that correct source will be used and GenerateSource will not be called. That is, we're in Scenario 2, and it doesn't matter that this object remains out of date.)
        // Scenario 5. This type had been previously generated, and there has been a change in this ILazinator file, and there has also been a change in at least one dependency. Because of the change in the Lazinator file itself, a fresh LazinatorPostGenerationInfo will be created with newly generated source, and then it will be updated with the latest dependencies. Because the pipeline run ID will match, we will use the stored source. This is essentially the same as Scenario 3, as the fact that there has been a change in a dependency doesn't matter.

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
        public void GenerateSource(SourceProductionContext spc, Guid pipelineRunUniqueID, IDateTimeNow dateTimeNowProvider)
        {
            if (pipelineRunUniqueID == AlreadyGeneratedCode.PipelineRunUniqueID)
            {
                if (AlreadyGeneratedCode.GeneratedCode != null)
                    spc.AddSource(AlreadyGeneratedCode.Path, AlreadyGeneratedCode.GeneratedCode); // We already generated the path and text at an earlier stage of this run through the pipeline. This was called because this LazinatorPostGenerationInfo had not been cached yet, but that doesn't matter. We know that we have just generated the source, and so we don't need to update it.
                else
                {
                    // Maybe we had a problem generating the code. If so, we should report the diagnostic.
                    if (AlreadyGeneratedCode.Diagnostic != null)
                        spc.ReportDiagnostic(AlreadyGeneratedCode.Diagnostic);
                }
                return;
            }
            // If we get here, we're at Scenario 4. We know that the Lazinator interface itself has not changed, but the source needs to be regenerated. A challenge here is that we need the generated code to be cached. 
            // Now we need to generate the source again. There was no cached version of LazinatorPostGenerationInformation, and the source that was generated with the old pipeline run unique ID is stale, because some dependency has changed. .Net hasn't cached this object, as a result of the change in the dependency information. So, we do need to regenerate the source and add it to the source production context. Note that this object (i.e., the LazinatorPostGenerationInfo) won't change and will thus continue to have stale source, but now the pipeline will use it as the cache key to generate the correct source, and so this should not be called repeatedly. If somehow there was a cache miss, we would do the source generation again here, but we would then be in the cache.
            var result = PreGenerationInfo.ExecuteSourceGeneration(pipelineRunUniqueID, dateTimeNowProvider);
            if (result.ContainsSuccessfullyGeneratedCode)
                spc.AddSource(result.Path, result.GeneratedCode);
            else if (result.Diagnostic != null)
                spc.ReportDiagnostic(result.Diagnostic);
        }

    }
}
