using System.Collections.Generic;
using Lazinator.Attributes;

namespace LazinatorTests.Examples.Collections
{
    [Lazinator((int)ExampleUniqueIDs.DotNetHash_SelfSerialized)]
    public interface IDotNetHashSet_SelfSerialized
    {
        HashSet<ExampleChild> MyHashSetSerialized { get; set; }
    }
}