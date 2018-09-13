using System.Collections.Generic;
using Lazinator.Attributes;

namespace LazinatorTests.Examples.Collections
{
    [Lazinator((int)ExampleUniqueIDs.DotNetHash_Lazinator)]
    public interface IDotNetHashSet_Lazinator
    {
        HashSet<ExampleChild> MyHashSetSerialized { get; set; }
    }
}