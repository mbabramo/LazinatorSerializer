using Lazinator.Core;
using Lazinator.Attributes;

namespace Lazinator.Collections
{
    [Lazinator((int)LazinatorCollectionUniqueIDs.ILazinatorStack)]
    internal interface ILazinatorStack<T> where T : ILazinator
    {
    }
}