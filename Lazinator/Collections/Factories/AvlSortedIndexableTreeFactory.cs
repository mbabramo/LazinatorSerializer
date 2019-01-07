using Lazinator.Buffers;
using Lazinator.Collections.Avl;
using Lazinator.Collections.Interfaces;
using Lazinator.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lazinator.Collections.Factories
{
    public partial class AvlSortedIndexableTreeFactory<T> : IAvlSortedIndexableTreeFactory<T> where T : ILazinator, IComparable<T>
    {
        public ISortedIndexableContainer<T> CreateSortedIndexableContainer()
        {
            return new AvlSortedIndexableTree<T>();
        }
    }
}
