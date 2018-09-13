using Lazinator.Attributes;
using System.Collections.Generic;

namespace LazinatorTests.Examples.Collections
{
    [Lazinator((int)ExampleUniqueIDs.DotNetQueue_Lazinator)]
    interface IDotNetQueue_Lazinator
    {
        Queue<ExampleChild> MyQueueSerialized { get; set; }
    }
}