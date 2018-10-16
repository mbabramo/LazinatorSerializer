using Lazinator.Attributes;
using Lazinator.Core;

namespace Lazinator.Collections.Avl
{
    [Lazinator((int)LazinatorCollectionUniqueIDs.AvlNode)]
    interface IAvlNode<TKey, TValue> where TKey : ILazinator, new() where TValue : ILazinator, new()
    {
        AvlNode<TKey, TValue> Left { get; set; }
        AvlNode<TKey, TValue> Right { get; set; }
        TKey Key { get; set; }
        TValue Value { get; set; }
        int Count { get; set; }
        int Balance { get; set; }
    }
}
