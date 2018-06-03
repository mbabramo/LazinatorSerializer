using Lazinator.Attributes;
using Lazinator.Wrappers;
using System.Collections.Generic;

namespace LazinatorTests.Examples.Collections
{
    [Lazinator((int)ExampleUniqueIDs.DotNetList_Wrapper)]
    interface IDotNetList_Wrapper
    {
        List<WInt> MyListInt { get; set; }
        bool MyListInt_Dirty { get; set; }
        List<WNullableInt> MyListNullableInt { get; set; }
        List<WNullableByte> MyListNullableByte { get; set; }
    }
}