using Lazinator.Buffers;
using Lazinator.Collections.Interfaces;
using Lazinator.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lazinator.Collections.Factories
{
    public partial class AvlListTreeWithInteriorLazinatorListFactory<T> : IAvlListTreeWithInteriorLazinatorListFactory<T>, IAvlListTreeInteriorCollectionFactory<T> where T : ILazinator
    {
        public bool Unbalanced { get => false; set => throw new NotSupportedException(); }

        public AvlListTreeWithInteriorLazinatorListFactory(int interiorMaxCapacity)
        {
            InteriorMaxCapacity = interiorMaxCapacity;
        }

        public IMultivalueContainer<T> CreateMultivalueContainer()
        {
            return new LazinatorList<T>()
            {
                AllowDuplicates = AllowDuplicates
            };
        }

        public bool FirstIsShorter(IMultivalueContainer<T> first, IMultivalueContainer<T> second)
        {
            return ((LazinatorList<T>)first).Count < ((LazinatorList<T>)second).Count;
        }

        public bool RequiresSplitting(IMultivalueContainer<T> container)
        {
            return ((LazinatorList<T>)container).Count > InteriorMaxCapacity;
        }
    }
}
