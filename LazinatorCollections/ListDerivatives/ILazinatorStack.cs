using Lazinator.Core;
using Lazinator.Attributes;

namespace LazinatorCollections.ListDerivatives
{
    [Lazinator((int)LazinatorCollectionUniqueIDs.ILazinatorStack)]
    internal interface ILazinatorStack<T> where T : ILazinator
    {
    }
}