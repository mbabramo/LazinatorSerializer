using Lazinator.Core;
using Lazinator.Attributes;

namespace LazinatorCollections.Basic.Basic
{
    [Lazinator((int)LazinatorCollectionUniqueIDs.ILazinatorSortedList)]
    internal interface ILazinatorSortedList<T> where T : ILazinator
    {
    }
}