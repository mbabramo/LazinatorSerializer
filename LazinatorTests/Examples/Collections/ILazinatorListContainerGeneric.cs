using Lazinator.Attributes;
using Lazinator.Collections;
using Lazinator.Core;

namespace LazinatorTests.Examples.Collections
{
    [Lazinator((int)ExampleUniqueIDs.ListContainerGeneric)]
    public interface ILazinatorListContainerGeneric<T> where T : ILazinator
    {
        LazinatorList<T> MyList { get; set; }
    }
}