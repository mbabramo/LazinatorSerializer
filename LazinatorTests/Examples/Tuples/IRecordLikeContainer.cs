using Lazinator.Attributes;

namespace LazinatorTests.Examples.Tuples
{
    [Lazinator((int)ExampleUniqueIDs.RecordLikeType)]
    public interface IRecordLikeContainer
    {
        RecordLikeStruct MyRecordLikeStruct { get; set; }
        RecordLikeTypeWithLazinator MyRecordLikeTypeWithLazinator { get; set; }
        RecordLikeClass MyRecordLikeClass { get; set; }
        MismatchedRecordLikeType MyMismatchedRecordLikeType { get; set; }
        NonLazinatorRecordWithConstructor MyNonLazinatorRecordWithConstructor { get; set; }
        NonLazinatorRecordWithoutConstructor MyNonLazinatorRecordWithoutConstructor { get; set; }
        NonLazinatorSubrecordWithConstructor MyNonLazinatorSubrecordWithConstructor { get; set; }
        NonLazinatorSubrecordWithoutConstructor MyNonLazinatorSubrecordWithoutConstructor { get; set; }
        int MyInt { get; set; }
    }
}
