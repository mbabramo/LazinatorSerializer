using Lazinator.Buffers;
using Lazinator.Collections.Avl;
using Lazinator.Collections.Tuples;
using Lazinator.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lazinator.Collections.Factories
{
    public partial class AvlListNodeTreeFactory<TKey, TValue> : IAvlListNodeTreeFactory<TKey, TValue>, ILazinatorOrderedKeyableFactory<TKey, TValue> where TKey : ILazinator, IComparable<TKey> where TValue : ILazinator
    {
        public ILazinatorOrderedKeyable<TKey, TValue> Create()
        {
            return new AvlListNodeTree<TKey, TValue>(AllowDuplicates, MaxItemsPerNode, SortableListFactory);
        }
        
    }
}