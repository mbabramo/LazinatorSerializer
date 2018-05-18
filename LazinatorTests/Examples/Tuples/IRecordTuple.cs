using System;
using System.Collections.Generic;
using System.Text;
using Lazinator.Attributes;

namespace LazinatorTests.Examples.Tuples
{
    [Lazinator((int)ExampleUniqueIDs.RecordLikeType)]
    public interface IRecordTuple
    {
        RecordLikeType MyRecordLikeType { get; set; }
        RecordLikeClass MyRecordLikeClass { get; set; }
        MismatchedRecordLikeType MyMismatchedRecordLikeType { get; set; }
    }
}
