using System.Collections.Generic;
using Lazinator.Attributes;
using Lazinator.Wrappers;

namespace LazinatorTests.Examples.Collections
{
    [Lazinator((int)ExampleUniqueIDs.Dictionary_Values_Lazinator)]
    public interface IDictionary_Values_Lazinator
    {
        Dictionary<int, ExampleChild> MyDictionary { get; set; }
        Dictionary<WInt, WInt> MyDictionaryStructs { get; set; }
        SortedDictionary<int, ExampleChild> MySortedDictionary { get; set; }
        SortedList<int, ExampleChild> MySortedList { get; set; }
    }
}