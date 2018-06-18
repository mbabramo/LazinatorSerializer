using System.Collections.Generic;
using Lazinator.Attributes;

namespace LazinatorTests.Examples.Collections
{
    [Lazinator((int)ExampleUniqueIDs.DotNetList_NonSelfSerializable)]
    public interface IDotNetList_NonSelfSerializable
    {
        List<NonLazinatorClass> MyListNonLazinatorType { get; set; }
        bool MyListNonLazinatorType_Dirty { get; set; }
        List<NonLazinatorClass> MyListNonLazinatorType2 { get; set; }
    }
}