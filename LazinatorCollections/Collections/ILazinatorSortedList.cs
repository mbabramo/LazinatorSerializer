using Lazinator.Core;
using Lazinator.Attributes;

namespace Lazinator.Collections
{
    [Lazinator((int)LazinatorCollectionUniqueIDs.ILazinatorSortedList)]
    internal interface ILazinatorSortedList<T> where T : ILazinator
    {
    }
}