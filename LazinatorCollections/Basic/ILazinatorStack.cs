using Lazinator.Core;
using Lazinator.Attributes;

namespace LazinatorCollections.Basic.Basic
{
    [Lazinator((int)LazinatorCollectionUniqueIDs.ILazinatorStack)]
    internal interface ILazinatorStack<T> where T : ILazinator
    {
    }
}