using Lazinator.Core;
using Lazinator.Attributes;

namespace Lazinator.List
{
    [Lazinator((int)LazinatorCoreUniqueIDs.ILazinatorSortedList)]
    internal interface ILazinatorSortedList<T> where T : ILazinator
    {
    }
}