namespace Lazinator.CodeDescription
{
    public enum LazinatorPropertyType : byte
    {
        PrimitiveType,
        PrimitiveTypeNullable,
        LazinatorNonnullableClassOrInterface,
        LazinatorClassOrInterface,
        LazinatorStruct,
        NonLazinator,
        SupportedCollection,
        SupportedTuple,
        OpenGenericParameter,
        LazinatorStructNullable
    }
}
