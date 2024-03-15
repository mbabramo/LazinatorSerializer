namespace LazinatorFuzzTestWriter
{
    public interface ISupportedType
    {
        public string TypeDeclaration(bool nullable, bool nullableEnabledContext);
    }
}
