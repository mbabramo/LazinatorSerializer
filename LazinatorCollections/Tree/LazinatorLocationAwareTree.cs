using LazinatorCollections.Dictionary;
using Lazinator.Core;
using Lazinator.Wrappers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LazinatorCollections.Tree
{
    /// <summary>
    /// A LazinatorGeneralTree with the ability to look up an element of the tree by location. This class maintains a dictionary of
    /// locations, each of which is a list of integers.
    /// </summary>
    /// <typeparam name="T">The type of each tree node</typeparam>
    public partial class LazinatorLocationAwareTree<T> : LazinatorGeneralTree<T>, ILazinatorLocationAwareTree<T> where T : ILazinator
    {
        public LazinatorLocationAwareTree(T item) : base(item)
        {
            OnAddChild(this);
        }

        public IEnumerable<LazinatorLocationAwareTree<T>> TraverseLocationAwareTree()
        {
            foreach (var tree in TraverseTree())
                yield return (LazinatorLocationAwareTree<T>) tree;
        }

        public override LazinatorGeneralTree<T> CreateTree(T item)
        {
            return new LazinatorLocationAwareTree<T>(item);
        }

        private void InitializeLocationsIfNecessary()
        {
            if (Locations == null)
                Locations = new LazinatorDictionary<T, LazinatorList<WInt32>>();
        }

        public LazinatorLocationAwareTree<T> GetLocationAwareRoot()
        {
            return (LazinatorLocationAwareTree<T>) GetRoot();
        }

        protected override void OnAddChild(LazinatorGeneralTree<T> child)
        {
            GetLocationAwareRoot().OnAddChildComplete(child);
        }

        private void OnAddChildComplete(LazinatorGeneralTree<T> child)
        {
            InitializeLocationsIfNecessary();
            var location = ConvertToLazinatorList(child.GetLocationInTree());
            Locations[child.Item] = location;
            foreach (var grandchild in child.GetChildren())
                OnAddChildComplete(grandchild);
        }

        protected override void OnRemoveChild(LazinatorGeneralTree<T> child)
        {
            GetLocationAwareRoot().OnRemoveChildComplete(child);
        }

        private void OnRemoveChildComplete(LazinatorGeneralTree<T> child)
        {
            InitializeLocationsIfNecessary();
            Locations.Remove(child.Item);
            foreach (var grandchild in child.GetChildren())
                OnRemoveChildComplete(grandchild);
        }

        public LazinatorLocationAwareTree<T> GetTreeForItem(T item)
        {
            InitializeLocationsIfNecessary();
            if (Locations.ContainsKey(item))
            {
                var locationAsLazinatorList = Locations[item];
                var locationAsList = ConvertToRegularList(locationAsLazinatorList);
                return (LazinatorLocationAwareTree<T>) GetTreeAtLocation(locationAsList);
            }
            return null;
        }

        private LazinatorList<WInt32> ConvertToLazinatorList(List<int> list)
        {
            return new LazinatorList<WInt32>(list.Select(x => new WInt32(x)), false);
        }

        private List<int> ConvertToRegularList(LazinatorList<WInt32> list)
        {
            return list.Select(x => x.WrappedValue).ToList();
        }

        /// <summary>
        /// Merge another tree sharing the same root into this one.
        /// </summary>
        /// <param name="treeToMergeIn"></param>
        public void MergeIn(LazinatorLocationAwareTree<T> treeToMergeIn)
        {
            if (!treeToMergeIn.Item.Equals(Item))
                throw new Exception("Cannot merge in tree because trees have different roots.");
            foreach (var descendant in treeToMergeIn.TraverseLocationAwareTree().Skip(1)) // skip the root
            {
                var existing = GetTreeForItem(descendant.Item);
                if (existing == null)
                {
                    // Descendant in treeToMergeIn doesn't exist in this tree. Find its parent in this tree, and then merge descendant into that spot.
                    var parentInTreeToMergeIn = descendant.ParentTree;
                    MergeInSingleNode(descendant, parentInTreeToMergeIn);
                }
                else
                {
                    // Descendant in treeToMergeIn already exists in this tree. We may or may not need to do something.
                    // If the descendant is just on the root of the tree to merge in, then there is nothing to do.
                    if (descendant.ParentTree != treeToMergeIn)
                    {
                        if (existing.ParentTree == this)
                        {
                            // Suppose that this tree has A-B and the other tree has C-A. We start by adding C, so now the tree is A-B, C. Then, when we get to A in the other tree, we see that it's already in this tree (thus existing != null). Then, we confirm that A in this tree has the root as a parent, and that the A in the other tree does not have its root as a parent. So, the correct thing for us to do is to move the A in this tree under the C in this tree. 
                            var replacementParent =
                                GetTreeForItem(descendant.ParentTree.Item); // i.e., find the C in this tree
                            RemoveChildTree(existing);
                            replacementParent.AddChildTree(existing);
                        }
                        else
                        {
                            if (!EqualityComparer<T>.Default.Equals(existing.ParentTree.Item,
                                descendant.ParentTree.Item))
                                throw new Exception(
                                    $"An item {existing.Item} exists in both trees, but their parents are not the same.");
                        }
                    }
                }
            }
        }

        private void MergeInSingleNode(LazinatorLocationAwareTree<T> nodeToMergeIn, LazinatorGeneralTree<T> placeInTreeToMergeIn)
        {
            LazinatorLocationAwareTree<T> parentInThisTree = (LazinatorLocationAwareTree<T>)GetTreeForItem(placeInTreeToMergeIn.Item);
            var originalChildren = nodeToMergeIn.Children;
            nodeToMergeIn.Children = null;
            var descendantClone = nodeToMergeIn.CloneNoBuffer();
            parentInThisTree.AddChildTree(descendantClone);
            nodeToMergeIn.Children = originalChildren;
        }
    }
}
