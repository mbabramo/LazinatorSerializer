using Lazinator.Collections.Avl;
using Lazinator.Collections.Interfaces;
using Lazinator.Core;
using Lazinator.Wrappers;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lazinator.Collections.Factories
{
    public partial class AvlListFactory<T> : IAvlListFactory<T>, ILazinatorListableFactory<T> where T : ILazinator
    {
        public AvlListFactory(IIndexableContainerFactory<T> indexableContainerFactory)
        {
            if (indexableContainerFactory == null)
                IndexableContainerFactory = new AvlIndexableTreeFactory<T>();
            else
                this.IndexableContainerFactory = indexableContainerFactory;
        }

        public ILazinatorListable<T> CreateListable()
        {
            return new AvlList<T>(IndexableContainerFactory);
        }
    }
}
