using Lazinator.Buffers;
using Lazinator.Collections.Interfaces;
using Lazinator.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lazinator.Collections.Factories
{
    public partial class AvlListTreeFactory<T> : IAvlListTreeFactory<T> where T : ILazinator
    {
        public enum TreeLevelType
        {
            AvlTree,
            AvlListTree,
            LazinatorList,
            LazinatorLinkedList
        }

        public AvlListTreeFactory(bool useLinkedList, int interiorMaxCapacity)
        {
            UseLinkedList = useLinkedList;
            InteriorMaxCapacity = interiorMaxCapacity;
        }

        public bool FirstIsShorter(IMultivalueContainer<T> first, IMultivalueContainer<T> second)
        {
            if (UseLinkedList)
                return ((LazinatorLinkedList<T>)first).Count < ((LazinatorLinkedList<T>)second).Count;
            return ((LazinatorList<T>)first).Count < ((LazinatorList<T>)second).Count;
        }

        public bool RequiresSplitting(IMultivalueContainer<T> container)
        {
            if (UseLinkedList)
                return ((LazinatorLinkedList<T>)container).Count > InteriorMaxCapacity;
            return ((LazinatorList<T>)container).Count > InteriorMaxCapacity;
        }

        public IMultivalueContainer<T> CreateInteriorCollection(bool allowDuplicates)
        {
            if (UseLinkedList)
                return new LazinatorLinkedList<T>()
                {
                    AllowDuplicates = allowDuplicates
                };
            return new LazinatorList<T>()
            {
                AllowDuplicates = allowDuplicates
            };
        }
    }
}
