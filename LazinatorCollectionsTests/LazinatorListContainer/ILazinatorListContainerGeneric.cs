using Lazinator.Attributes;
using Lazinator.Core;
using LazinatorCollections;

namespace LazinatorCollectionsTests
{
    [Lazinator((int)CollectionsTestsObjectIDs.ListContainerGeneric)]
    public interface ILazinatorListContainerGeneric<T> where T : ILazinator
    {
        LazinatorList<T> MyList { get; set; }
    }
}