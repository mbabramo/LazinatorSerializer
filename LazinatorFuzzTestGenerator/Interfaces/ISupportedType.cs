using System;

namespace LazinatorFuzzTestGenerator.Interfaces
{
    public interface ISupportedType
    {
        public string UnannotatedTypeDeclaration();
        public bool UnannotatedIsNullable(bool nullableEnabledContext);
        public string NullabilityNotation(bool nullable, bool nullableEnabledContext) => nullable && !UnannotatedIsNullable(nullableEnabledContext) ? "?" : "";
    }
}
