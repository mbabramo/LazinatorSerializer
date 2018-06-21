using Lazinator.Core;
using Lazinator.Attributes;

namespace Lazinator.Collections
{
    [Lazinator((int)LazinatorCollectionUniqueIDs.LazinatorQueue)]
    interface ILazinatorQueue<T> where T : ILazinator
    {
    }
}