using Lazinator.Collections.Dictionary;
using Lazinator.Core;
using Lazinator.Wrappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lazinator.Collections
{
    /// <summary>
    /// A LazinatorGeneralTree with the ability to look up an element of the tree by location.
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
                Locations = new LazinatorDictionary<T, LazinatorList<WInt>>();
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

        private LazinatorList<WInt> ConvertToLazinatorList(List<int> list)
        {
            return new LazinatorList<WInt>(list.Select(x => new WInt(x)));
        }

        private List<int> ConvertToRegularList(LazinatorList<WInt> list)
        {
            return list.Select(x => x.WrappedValue).ToList();
        }

        public void MergeIn(LazinatorLocationAwareTree<T> treeToMergeIn)
        {
            foreach (var descendant in treeToMergeIn.TraverseLocationAwareTree())
            {
                var existing = GetTreeForItem(descendant.Item);
                if (existing == null)
                {
                    var parent = descendant.ParentTree;
                    if (parent == null)
                        throw new Exception("Cannot merge in tree because trees have different roots.");
                    if (parent != null)
                    {
                        LazinatorLocationAwareTree<T> existingParent = (LazinatorLocationAwareTree<T>) GetTreeForItem(parent.Item);
                        if (existingParent == null)
                            throw new Exception("Internal exception in MergeIn algorithm."); // DEBUG -- eliminate this check
                        var originalChildren = descendant.Children;
                        descendant.Children = null;
                        var descendantClone = descendant.CloneNoBuffer();
                        existingParent.AddChildTree(descendantClone);
                        descendant.Children = originalChildren;
                    }
                }
            }
        }
    }
}
