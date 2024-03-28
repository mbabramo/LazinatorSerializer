using System;

namespace LazinatorFuzzTestGenerator.Interfaces
{
    public interface ISupportedType
    {
        public bool NullableContextEnabled { get; init; }
        public IObjectContents GetRandomObjectContents(Random r, int? inverseProbabilityOfNull);
        public string UnannotatedTypeDeclaration();
        public bool UnannotatedIsNullable();
        public string NullabilityNotation(bool nullable) => nullable && !UnannotatedIsNullable() ? "?" : "";

        public string AnnotatedTypeDeclaration(bool nullable) => $"{UnannotatedTypeDeclaration()}{NullabilityNotation(nullable)}";
    }
}
