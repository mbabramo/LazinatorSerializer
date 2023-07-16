using System;
using System.Collections.Generic;
using System.Text;

namespace LazinatorGenerator.Generator
{
    internal readonly struct LazinatorCodeGenerationResult
    {
        internal readonly string GeneratedType;
        internal readonly string Path;
        internal readonly string GeneratedCode;
        internal readonly LazinatorDependencyInfo DependencyInfo;
        internal readonly Guid PipelineRunUniqueID;
        internal readonly int HashCode;

        internal readonly bool IsEmpty => GeneratedType == null && Path == null;
        internal readonly bool ContainsSuccessfullyGeneratedCode => GeneratedCode != null;
        public LazinatorCodeGenerationResult(string generatedType, string path, string generatedCode, LazinatorDependencyInfo dependencyInfo, Guid pipelineRunUniqueID)
        {
            GeneratedType = generatedType;
            Path = path;
            GeneratedCode = generatedCode;
            DependencyInfo = dependencyInfo;
            PipelineRunUniqueID = pipelineRunUniqueID;
            HashCode = (generatedType, path, generatedCode, dependencyInfo, pipelineRunUniqueID).GetHashCode();
            if (DependencyInfo.IsUninitialized)
            {
                var DEBUG = 0;
            }
        }

        public LazinatorCodeGenerationResult WithUpdatedHashCodes(Dictionary<string, int> typeNameToHash) => new LazinatorCodeGenerationResult(GeneratedType, Path, GeneratedCode, DependencyInfo.WithUpdatedHashCodes(typeNameToHash), PipelineRunUniqueID);

        public override int GetHashCode()
        {
            return HashCode;
        }
    }
}
