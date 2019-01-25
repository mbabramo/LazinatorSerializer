using Lazinator.Attributes;
using Lazinator.Core;
using LazinatorCollections;

namespace LazinatorCollectionsTests
{
    [Lazinator((int) CollectionsTestsObjectIDs.DerivedLazinatorList)]
    interface IDerivedLazinatorList<T> : ILazinatorList<T> where T : ILazinator
    {
        string MyListName { get; set; }
    }
}
