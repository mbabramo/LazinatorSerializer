using Lazinator.Attributes;

namespace LazinatorTests.Examples.Tuples
{
    [Lazinator((int)ExampleUniqueIDs.RecordLikeType)]
    public interface IRecordLikeContainer
    {
        RecordLikeType MyRecordLikeType { get; set; }
        RecordLikeTypeWithLazinator MyRecordLikeTypeWithLazinator { get; set; }
        RecordLikeClass MyRecordLikeClass { get; set; }
        MismatchedRecordLikeType MyMismatchedRecordLikeType { get; set; }
        int MyInt { get; set; }
    }
}
