using System.Collections.Generic;
using Lazinator.Attributes;
using Lazinator.Collections;

namespace LazinatorTests.Examples.Collections
{
    [Lazinator((int)ExampleUniqueIDs.ListContainer)]
    public interface ILazinatorListContainer
    {
        int MyInt { get; set; }
        LazinatorList<ExampleChild> MyList { get; set; }
        long MyLong { get; set; }
    }
}