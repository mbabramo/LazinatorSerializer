using System.Collections.Generic;
using Lazinator.Attributes;
using LazinatorTests.Examples.ExampleHierarchy;

namespace LazinatorTests.Examples.Collections
{
    [Lazinator((int)ExampleUniqueIDs.DotNetList_Values)]
    public interface IDotNetList_Values
    {
        List<int> MyListInt { get; set; }
        bool MyListInt_Dirty { get; set; }
        LinkedList<int> MyLinkedListInt { get; set; }
        bool MyLinkedListInt_Dirty { get; set; }
        SortedSet<int> MySortedSetInt { get; set; }
        bool MySortedSetInt_Dirty { get; set; }
        List<int> MyListInt2 { get; set; }
        List<NullableContextEnabled> MyListNullableContextEnabled { get; set; }
    }
}