using Lazinator.Buffers;
using Lazinator.Collections.Avl;
using Lazinator.Collections.Avl.ValueTree;
using Lazinator.Collections.Interfaces;
using Lazinator.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lazinator.Collections.Factories
{
    public partial class AvlSortedIndexableTreeFactory<T> : IAvlSortedIndexableTreeFactory<T> where T : ILazinator, IComparable<T>
    {
        public AvlSortedIndexableTreeFactory(bool allowDuplicates, bool unbalanced)
        {
            AllowDuplicates = allowDuplicates;
            Unbalanced = unbalanced;
        }

        public ISortedIndexableMultivalueContainer<T> CreateSortedIndexableMultivalueContainer()
        {
            return new AvlSortedIndexableTree<T>(AllowDuplicates) { Unbalanced = Unbalanced };
        }
    }
}
