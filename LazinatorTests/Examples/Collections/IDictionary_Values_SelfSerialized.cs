using System.Collections.Generic;
using Lazinator.Attributes;

namespace LazinatorTests.Examples.Collections
{
    [Lazinator((int)ExampleUniqueIDs.Dictionary_Values_SelfSerialized)]
    public interface IDictionary_Values_SelfSerialized
    {
        Dictionary<int, ExampleChild> MyDictionary { get; set; }
        SortedDictionary<int, ExampleChild> MySortedDictionary { get; set; }
        SortedList<int, ExampleChild> MySortedList { get; set; }
    }
}