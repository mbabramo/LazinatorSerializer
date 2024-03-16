using System;

namespace LazinatorFuzzTestGenerator
{
    public interface ISupportedType
    {
        public bool UnannotatedIsNullable(bool nullableEnabledContext);
        public string UnannotatedTypeDeclaration();
        public string NullabilityNotation(bool nullable, bool nullableEnabledContext) => nullable && !UnannotatedIsNullable(nullableEnabledContext) ? "?" : "";
    }
}
