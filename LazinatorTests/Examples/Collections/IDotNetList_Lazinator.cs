using System.Collections.Generic;
using Lazinator.Attributes;

namespace LazinatorTests.Examples.Collections
{
    [Lazinator((int)ExampleUniqueIDs.DotNetList_Serialized)]
    public interface IDotNetList_Lazinator
    {
        List<ExampleChild> MyListSerialized { get; set; }
        bool MyListSerialized_Dirty { get; set; }
    }
}