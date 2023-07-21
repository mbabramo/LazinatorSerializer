using Microsoft.CodeAnalysis;
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
        internal readonly Diagnostic Diagnostic;
        internal readonly int HashCode;

        internal readonly bool IsEmpty => GeneratedType == null && Path == null;
        internal readonly bool ContainsSuccessfullyGeneratedCode => GeneratedCode != null;
        internal readonly bool ContainsDiagnosticInformation => Diagnostic != null;
        public LazinatorCodeGenerationResult(string generatedType, string path, string generatedCode, LazinatorDependencyInfo dependencyInfo, Guid pipelineRunUniqueID, Diagnostic diagnostic)
        {
            GeneratedType = generatedType;
            Path = path;
            GeneratedCode = generatedCode;
            DependencyInfo = dependencyInfo;
            PipelineRunUniqueID = pipelineRunUniqueID;
            Diagnostic = diagnostic;
            HashCode = (generatedType, path, generatedCode, dependencyInfo, pipelineRunUniqueID, diagnostic).GetHashCode();
        }
       
        public LazinatorCodeGenerationResult WithUpdatedHashCodes(Dictionary<string, int> typeNameToHash) => new LazinatorCodeGenerationResult(GeneratedType, Path, GeneratedCode, DependencyInfo.WithUpdatedHashCodes(typeNameToHash), PipelineRunUniqueID, Diagnostic);

        public override int GetHashCode()
        {
            return HashCode;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is LazinatorCodeGenerationResult other))
                return false;

            return other.GeneratedType == GeneratedType && other.Path == Path && other.GeneratedCode == GeneratedCode && other.DependencyInfo.Equals(DependencyInfo) && other.PipelineRunUniqueID == PipelineRunUniqueID && ((other.Diagnostic == null && Diagnostic == null) || (other.Diagnostic != null && Diagnostic != null && other.Diagnostic.Id == Diagnostic.Id && other.Diagnostic.GetMessage() == Diagnostic.GetMessage()));
        }
    }
}
