using Lazinator.Collections.Avl.ListTree;
using Lazinator.Collections.Avl.ValueTree;
using Lazinator.Collections.Interfaces;
using Lazinator.Collections.Tree;
using Lazinator.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lazinator.Collections.Factories
{
    public partial class ValueContainerFactory<T> : IValueContainerFactory<T> where T : ILazinator
    {
        public ValueContainerLevel ThisLevel { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public ValueContainerFactory<T> InteriorFactory { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public ValueContainerFactory(IEnumerable<ValueContainerLevel> levels)
        {
            ThisLevel = levels.First();
            var remaining = levels.Skip(1);
            if (remaining.Any())
                InteriorFactory = new ValueContainerFactory<T>(remaining);
        }

        public virtual IValueContainer<T> CreateContainer()
        {
            switch (ThisLevel.ValueContainerType)
            {
                case ValueContainerType.LazinatorList:
                    return new LazinatorList<T>()
                    {
                        AllowDuplicates = ThisLevel.AllowDuplicates
                    };
                case ValueContainerType.LazinatorLinkedList:
                    return new LazinatorLinkedList<T>()
                    {
                        AllowDuplicates = ThisLevel.AllowDuplicates
                    };
                case ValueContainerType.AvlTree:
                    return new AvlTree<T>()
                    {
                        AllowDuplicates = ThisLevel.AllowDuplicates,
                        Unbalanced = ThisLevel.Unbalanced
                    };
                case ValueContainerType.AvlIndexableTree:
                    return new AvlIndexableTree<T>()
                    {
                        AllowDuplicates = ThisLevel.AllowDuplicates,
                        Unbalanced = ThisLevel.Unbalanced
                    };
                case ValueContainerType.AvlListTree:
                    return new AvlListTree<T>(ThisLevel.AllowDuplicates, ThisLevel.Unbalanced, InteriorFactory);
                case ValueContainerType.AvlIndexableListTree:
                    throw new NotImplementedException();
                default:
                    throw new NotImplementedException();
            }
        }

        public IValueContainer<T> CreateInteriorContainer()
        {
            return InteriorFactory.CreateContainer();
        }

        public bool RequiresSplitting(IValueContainer<T> container)
        {
            if (container is ICountableContainer countable)
                return countable.LongCount > ThisLevel.SplitThreshold;
            if (container is BinaryTree<T> binaryTree)
            {
                return binaryTree.GetApproximateDepth() > ThisLevel.SplitThreshold;
            }
            throw new NotImplementedException();
        }

        public bool FirstIsShorter(IValueContainer<T> first, IValueContainer<T> second)
        {
            if (first is ICountableContainer countableFirst && second is ICountableContainer countableSecond)
                return countableFirst.LongCount < countableSecond.LongCount;
            if (first is BinaryTree<T> firstTree && second is BinaryTree<T> secondTree)
            {
                return firstTree.GetApproximateDepth() < secondTree.GetApproximateDepth();
            }
            throw new NotImplementedException();
        }
    }
}
