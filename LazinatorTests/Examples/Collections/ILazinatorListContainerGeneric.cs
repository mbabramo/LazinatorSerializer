using System.Collections.Generic;
using Lazinator.Attributes;
using Lazinator.Collections;
using Lazinator.Support;
using Lazinator.Core;

namespace LazinatorTests.Examples.Collections
{
    [Lazinator((int)ExampleUniqueIDs.ListContainerGeneric)]
    public interface ILazinatorListContainerGeneric<T> where T : ILazinator
    {
        LazinatorList<T> MyList { get; set; }
    }
}