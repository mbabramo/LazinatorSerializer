namespace Lazinator.CodeDescription
{
    public enum LazinatorPropertyType : byte
    {
        PrimitiveType,
        PrimitiveTypeNullable,
        LazinatorClassOrInterface,
        LazinatorStruct,
        NonLazinator,
        SupportedCollection,
        SupportedTuple,
        OpenGenericParameter,
        LazinatorStructNullable
    }
}
