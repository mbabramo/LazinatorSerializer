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
    public partial class AvlIndexableTreeFactory<T> : IAvlIndexableTreeFactory<T> where T : ILazinator
    {

        public IIndexableContainer<T> CreateIndexableContainer()
        {
            return new AvlIndexableTree<T>() { Unbalanced = Unbalanced };
        }
    }
}
