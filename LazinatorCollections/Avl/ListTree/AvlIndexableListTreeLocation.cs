using Lazinator.Buffers;
using LazinatorCollections.Avl.ValueTree;
using LazinatorCollections.Interfaces;
using Lazinator.ContainerLocation;
using LazinatorCollections.Tree;
using Lazinator.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace LazinatorCollections.Avl.ListTree
{
    public struct AvlIndexableListTreeLocation<T> : IContainerLocation where T : ILazinator
    {
        public readonly AvlAggregatedTree<IIndexableMultivalueContainer<T>> Tree;
        public readonly AvlAggregatedNode<IIndexableMultivalueContainer<T>> InnerContainerNode;
        public IIndexableMultivalueContainer<T> InnerContainer => InnerContainerNode.Value;
        public readonly IContainerLocation LocationInInnerContainer;

        public AvlIndexableListTreeLocation(AvlAggregatedTree<IIndexableMultivalueContainer<T>> tree, AvlAggregatedNode<IIndexableMultivalueContainer<T>> innerContainerNode, IContainerLocation locationInInnerContainer)
        {
            Tree = tree;
            InnerContainerNode = innerContainerNode;
            LocationInInnerContainer = locationInInnerContainer;
        }

        public AvlIndexableListTreeLocation(AvlAggregatedTree<IIndexableMultivalueContainer<T>> tree, BinaryTreeLocation<IIndexableMultivalueContainer<T>> innerContainerLocation, IContainerLocation locationInInnerContainer)
        {
            Tree = tree;
            InnerContainerNode = (AvlAggregatedNode<IIndexableMultivalueContainer<T>>)innerContainerLocation.BinaryNode;
            LocationInInnerContainer = locationInInnerContainer;
        }

        public bool IsBeforeContainer => InnerContainer == null;

        public bool IsAfterContainer => false; // we will use AfterContainerLocation where necessary

        public BinaryTreeLocation<IIndexableMultivalueContainer<T>> LocationOfInnerContainer => new BinaryTreeLocation<IIndexableMultivalueContainer<T>>(InnerContainerNode);
        private AvlAggregatedNode<IIndexableMultivalueContainer<T>> NodeFromTreeLocationOfInnerContainer(IContainerLocation location) => (AvlAggregatedNode<IIndexableMultivalueContainer<T>>) ((BinaryTreeLocation<IIndexableMultivalueContainer<T>>)location).BinaryNode;

        public IContainerLocation GetNextLocation()
        {
            var nextInner = LocationInInnerContainer.GetNextLocation();
            if (!nextInner.IsAfterContainer)
                return new AvlIndexableListTreeLocation<T>(Tree, InnerContainerNode, nextInner);
            var nextOuterLocation = LocationOfInnerContainer.GetNextLocation();
            if (nextOuterLocation.IsAfterContainer)
                return new AfterContainerLocation();
            var nextInnerContainerNode = Tree.GetNodeAtNonaggregatedIndex(InnerContainerNode.Index + 1);
            var nextInnerLocation = new IndexLocation(0, nextInnerContainerNode.Value.LongCount);
            return new AvlIndexableListTreeLocation<T>(Tree, nextInnerContainerNode, nextInnerLocation);
        }

        public IContainerLocation GetPreviousLocation()
        {
            var previousInner = LocationInInnerContainer.GetPreviousLocation();
            if (!previousInner.IsBeforeContainer)
                return new AvlIndexableListTreeLocation<T>(Tree, InnerContainerNode, previousInner);
            var previousOuterLocation = LocationOfInnerContainer.GetPreviousLocation();
            if (previousOuterLocation.IsBeforeContainer)
                return new BeforeContainerLocation();
            var previousInnerContainerNode = Tree.GetNodeAtNonaggregatedIndex(InnerContainerNode.Index + 1);
            var previousInnerLocation = new IndexLocation(0, previousInnerContainerNode.Value.LongCount);
            return new AvlIndexableListTreeLocation<T>(Tree, previousInnerContainerNode, previousInnerLocation);
        }
    }
}
