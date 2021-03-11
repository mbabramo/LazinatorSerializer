using Lazinator.Core;
using Lazinator.Attributes;

namespace LazinatorCollections
{
    [Lazinator((int)LazinatorCollectionUniqueIDs.ILazinatorSortedList)]
    internal interface ILazinatorSortedList<T> : ILazinatorList<T> where T : ILazinator
    {
    }
}