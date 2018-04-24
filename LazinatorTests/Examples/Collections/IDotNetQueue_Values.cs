using System.Collections.Generic;
using Lazinator.Attributes;

namespace LazinatorTests.Examples.Collections
{
    [Lazinator((int)ExampleUniqueIDs.DotNetQueue_Values)]
    public interface IDotNetQueue_Values
    {
        Queue<int> MyQueueInt { get; set; }
        bool MyQueueInt_Dirty { get; set; }
    }
}