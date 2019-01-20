using Lazinator.Core;
using Lazinator.Attributes;
using Lazinator.Collections.Tuples;
using Lazinator.Collections.Avl.ValueTree;
using Lazinator.Collections.Interfaces;

namespace Lazinator.Collections.Avl.KeyValueTree
{
    [Lazinator((int)LazinatorCollectionUniqueIDs.IAvlKeyValueTree)]
    public interface IAvlKeyValueTree<TKey, TValue>
        where TKey : ILazinator
        where TValue : ILazinator
    {
        [SetterAccessibility("protected")]
        bool AllowDuplicates { get; }
        [SetterAccessibility("protected")]
        bool Unbalanced { get; }
        IMultivalueContainer<LazinatorKeyValue<TKey, TValue>> UnderlyingContainer { get; set; }
    }
}