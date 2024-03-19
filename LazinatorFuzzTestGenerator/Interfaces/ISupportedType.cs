using System;

namespace LazinatorFuzzTestGenerator.Interfaces
{
    public interface ISupportedType
    {
        public IObjectContents GetRandomObjectContents(Random r, int? inverseProbabilityOfNull);
        public string UnannotatedTypeDeclaration();
        public bool UnannotatedIsNullable(bool nullableEnabledContext);
        public string NullabilityNotation(bool nullable, bool nullableEnabledContext) => nullable && !UnannotatedIsNullable(nullableEnabledContext) ? "?" : "";
    }
}
