namespace LazinatorFuzzTestGenerator
{
    public interface ISupportedType
    {

        public bool UnannotatedIsNullable(bool nullableEnabledContext);
        public string TypeDeclaration(bool nullable, bool nullableEnabledContext);
    }
}
