using Lazinator.Attributes;
using System.Collections.Generic;

namespace LazinatorTests.Examples.Collections
{
    [Lazinator((int)ExampleUniqueIDs.DotNetQueue_SelfSerialized)]
    interface IDotNetQueue_SelfSerialized
    {
        Queue<ExampleChild> MyQueueSerialized { get; set; }
    }
}