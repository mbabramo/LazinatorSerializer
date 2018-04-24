using System.Collections.Generic;
using Lazinator.Attributes;
using Lazinator.Collections;

namespace LazinatorTests.Examples.Collections
{
    [Lazinator((int)ExampleUniqueIDs.ListContainer)]
    public interface ILazinatorListContainer
    {
        LazinatorList<ExampleChild> MyList { get; set; }
    }
}