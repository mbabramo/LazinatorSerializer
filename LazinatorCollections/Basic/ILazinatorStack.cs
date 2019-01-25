using Lazinator.Core;
using Lazinator.Attributes;

namespace LazinatorCollections
{
    [Lazinator((int)LazinatorCollectionUniqueIDs.ILazinatorStack)]
    internal interface ILazinatorStack<T> where T : ILazinator
    {
    }
}