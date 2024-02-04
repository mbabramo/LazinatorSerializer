using Lazinator.Buffers;
using LazinatorAvlCollections.Avl.ValueTree;
using Lazinator.Collections.Interfaces;
using Lazinator.Collections.Location;
using LazinatorAvlCollections.Avl.BinaryTree;
using Lazinator.Core;
using System;
using System.Collections.Generic;
using System.Text;
using Lazinator.Collections;

namespace LazinatorAvlCollections.Avl.ListTree
{
    /// <summary>
    /// A means of referencing an item within an indexable Avl list tree, specifying the location of the inner container
    /// in the outer Avl list tree as well as the location in the inner container, while also providing references
    /// to both the outer and inner containers.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public struct AvlIndexableListTreeLocation<T> : IContainerLocation where T : ILazinator
    {
        public readonly AvlAggregatedTree<IIndexableMultivalueContainer<T>> UnderlyingTree;
        public readonly AvlAggregatedNode<IIndexableMultivalueContainer<T>> InnerContainerNode;
        public IIndexableMultivalueContainer<T> InnerContainer => InnerContainerNode.Value;
        public readonly IContainerLocation LocationInInnerContainer;

        public AvlIndexableListTreeLocation(AvlAggregatedTree<IIndexableMultivalueContainer<T>> tree, AvlAggregatedNode<IIndexableMultivalueContainer<T>> innerContainerNode, IContainerLocation locationInInnerContainer)
        {
            UnderlyingTree = tree;
            InnerContainerNode = innerContainerNode;
            LocationInInnerContainer = locationInInnerContainer;
        }

        public AvlIndexableListTreeLocation(AvlAggregatedTree<IIndexableMultivalueContainer<T>> tree, TreeLocation<IIndexableMultivalueContainer<T>, AvlAggregatedNode<IIndexableMultivalueContainer<T>>> innerContainerLocation, IContainerLocation locationInInnerContainer)
        {
            UnderlyingTree = tree;
            InnerContainerNode = (AvlAggregatedNode<IIndexableMultivalueContainer<T>>)innerContainerLocation.Node;
            LocationInInnerContainer = locationInInnerContainer;
        }

        public bool IsBeforeContainer => InnerContainer == null;

        public bool IsAfterContainer => false; // we will use AfterContainerLocation where necessary

        public TreeLocation<IIndexableMultivalueContainer<T>, AvlAggregatedNode<IIndexableMultivalueContainer<T>>> LocationOfInnerContainer => new TreeLocation<IIndexableMultivalueContainer<T>, AvlAggregatedNode<IIndexableMultivalueContainer<T>>>(InnerContainerNode);
        private AvlAggregatedNode<IIndexableMultivalueContainer<T>> NodeFromTreeLocationOfInnerContainer(IContainerLocation location) => (AvlAggregatedNode<IIndexableMultivalueContainer<T>>) ((TreeLocation<IIndexableMultivalueContainer<T>, AvlAggregatedNode<IIndexableMultivalueContainer<T>>>)location).Node;

        public IContainerLocation GetNextLocation()
        {
            var nextInner = LocationInInnerContainer.GetNextLocation();
            if (!nextInner.IsAfterContainer)
                return new AvlIndexableListTreeLocation<T>(UnderlyingTree, InnerContainerNode, nextInner);
            var nextOuterLocation = LocationOfInnerContainer.GetNextLocation();
            if (nextOuterLocation.IsAfterContainer)
                return new AfterContainerLocation();
            var nextInnerContainerNode = UnderlyingTree.GetNodeAtNonaggregatedIndex(InnerContainerNode.Index + 1);
            var nextInnerLocation = new IndexLocation(0, nextInnerContainerNode.Value.LongCount);
            return new AvlIndexableListTreeLocation<T>(UnderlyingTree, nextInnerContainerNode, nextInnerLocation);
        }

        public IContainerLocation GetPreviousLocation()
        {
            var previousInner = LocationInInnerContainer.GetPreviousLocation();
            if (!previousInner.IsBeforeContainer)
                return new AvlIndexableListTreeLocation<T>(UnderlyingTree, InnerContainerNode, previousInner);
            var previousOuterLocation = LocationOfInnerContainer.GetPreviousLocation();
            if (previousOuterLocation.IsBeforeContainer)
                return new BeforeContainerLocation();
            var previousInnerContainerNode = UnderlyingTree.GetNodeAtNonaggregatedIndex(InnerContainerNode.Index + 1);
            var previousInnerLocation = new IndexLocation(0, previousInnerContainerNode.Value.LongCount);
            return new AvlIndexableListTreeLocation<T>(UnderlyingTree, previousInnerContainerNode, previousInnerLocation);
        }
    }
}
