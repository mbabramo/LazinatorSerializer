using Lazinator.Attributes;
using System.Collections.Generic;

namespace LazinatorTests.Examples.Tuples
{
    [Lazinator((int)ExampleUniqueIDs.RecordLikeCollections)]
    public interface IRecordLikeCollections
    {
        Dictionary<int, RecordLikeTypeWithLazinator> MyDictionaryWithRecordLikeTypeValues { get; set; }
        Dictionary<int, RecordLikeContainer> MyDictionaryWithRecordLikeContainers { get; set; }
        int MyInt { get; set; }
    }
}
