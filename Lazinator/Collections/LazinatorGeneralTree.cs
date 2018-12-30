using Lazinator.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lazinator.Collections
{
    /// <summary>
    /// A general tree, where each parent can have any number of children.
    /// </summary>
    /// <typeparam name="T">The type of each tree node</typeparam>
    public partial class LazinatorGeneralTree<T> : ILazinatorGeneralTree<T> where T : ILazinator
    {
        // The following are not serialized. They are set after the child is loaded.
        public LazinatorGeneralTree<T> ParentTree { get; set; }
        public int Level { get; set; }
        public int Index { get; set; }
        public bool Initialized { get; set; }
        public bool ChildrenLoaded { get; set; }

        public LazinatorGeneralTree(T item)
        {
            Item = item;
        }

        public LazinatorGeneralTree<T> GetRoot()
        {
            var x = this;
            while (x.ParentTree != null)
                x = x.ParentTree;
            return x;
        }

        public IEnumerable<(T, int)> Traverse(int level = 0, int maxLevel = Int32.MaxValue)
        {
            yield return (Item, level);
            if (level == maxLevel)
                yield break;
            foreach (var child in GetChildren())
            foreach (var descendantWithLevel in child.Traverse(level + 1, maxLevel))
                yield return descendantWithLevel;
        }

        public IEnumerable<LazinatorGeneralTree<T>> TraverseTree()
        {
            yield return this;
            foreach (var child in GetChildren())
            foreach (var descendant in child.TraverseTree())
                yield return descendant;
        }

        public IEnumerable<LazinatorGeneralTree<T>> GetChildren()
        {
            if (Children == null)
                yield break;
            int index = 0;
            foreach (var child in Children)
            {
                if (!ChildrenLoaded)
                    SetChildInformation(child, index++, false);
                yield return child;
            }
            ChildrenLoaded = true;
        }

        public LazinatorGeneralTree<T> GetChild(int index)
        {
            return GetChildren().Skip(index).First();
        }

        protected void SetChildInformation(LazinatorGeneralTree<T> child, int index, bool addingChild)
        {
            child.ParentTree = this;
            child.Level = Level + 1;
            child.Index = index;
            if (addingChild)
            {
                OnAddChild(child);
                child.ResetDescendantIndices();
            }

            Initialized = true;
        }

        public List<int> GetLocationInTree()
        {
            if (ParentTree == null)
                return new List<int>();
            var location = ParentTree.GetLocationInTree();
            location.Add(Index);
            return location;
        }

        public LazinatorGeneralTree<T> GetTreeAtLocation(List<int> location, int index = 0)
        {
            if (index == location.Count)
                return this;
            var children = GetChildren();
            if (location[index] > children.Count() - 1)
                return null;
            return children.Skip(location[index]).First().GetTreeAtLocation(location, index + 1);
        }

        public LazinatorGeneralTree<T> GetTreeAtIndex(int index)
        {
            var children = GetChildren();
            if (index > children.Count() - 1)
                return null;
            return children.Skip(index).First();
        }

        protected void ResetDescendantIndices()
        {
            if (Children == null)
                return;
            int index = 0;
            foreach (var child in Children)
            {
                bool descendantLevelsWrong = child.Level != Level + 1;
                if (child.Index != index || child.ParentTree != this || descendantLevelsWrong)
                {
                    OnRemoveChild(child);
                    SetChildInformation(child, index, true);
                    if (descendantLevelsWrong)
                        child.ResetDescendantIndices();
                }

                index++;
            }
        }

        public LazinatorGeneralTree<T> AddChild(T child)
        {
            if (Children == null)
                Children = new LazinatorList<LazinatorGeneralTree<T>>();
            LazinatorGeneralTree<T> childTree = CreateTree(child);
            return AddChildTree(childTree);
        }

        public LazinatorGeneralTree<T> AddChildTree(LazinatorGeneralTree<T> childTree)
        {
            if (Children == null)
                Children = new LazinatorList<LazinatorGeneralTree<T>>();
            Children.Add(childTree);
            SetChildInformation(childTree, Children.Count - 1, true);
            return childTree;
        }

        public virtual LazinatorGeneralTree<T> InsertChild(T child, int index)
        {
            if (Children == null)
                Children = new LazinatorList<LazinatorGeneralTree<T>>();
            int childrenCount = Children.Count();
            LazinatorGeneralTree<T> childTree = null;
            if (Children == null || index == childrenCount)
                childTree = AddChild(child);
            else
            {
                if (index < 0 || index > childrenCount)
                    throw new ArgumentException("Invalid index");
                childTree = CreateTree(child);
                InsertChildTree(childTree, index);
            }
            return childTree;
        }

        protected internal void InsertChildTree(LazinatorGeneralTree<T> childTree, int index)
        {
            if (Children == null)
                Children = new LazinatorList<LazinatorGeneralTree<T>>();
            Children.Insert(index, childTree);
            OnAddChild(childTree);
            ResetDescendantIndices();
        }

        public bool RemoveChild(T child)
        {
            if (child.Equals(Item))
                throw new Exception("Cannot remove tree root.");
            if (Children == null)
                return false;
            var match = GetChildren().FirstOrDefault(x => x.Item.Equals(child));
            if (match != null)
            {
                return RemoveChildTree(match);
            }
            else
                return false;
        }

        public bool RemoveChildTree(LazinatorGeneralTree<T> childTree)
        {
            bool removeSuccessful = Children.Remove(childTree);
            if (removeSuccessful)
            {
                OnRemoveChild(childTree);
                ResetDescendantIndices();
                return true;
            }
            return false;
        }

        public override string ToString()
        {
            return ToTreeString(true);
        }

        public string ToTreeString(bool includeRoot)
        {
            StringBuilder s = new StringBuilder();
            foreach (var treeInfo in Traverse())
            {
                if (!includeRoot && treeInfo.Item2 == 0)
                    continue;
                TabToDepth(treeInfo.Item2, s);
                s.Append(treeInfo.Item1.ToString());
                s.Append("\n");
            }
            return s.ToString();
        }

        private static void TabToDepth(int depth, StringBuilder s)
        {
            for (int i = 0; i < depth * 2; i++)
                s.Append(' ');
        }

        public virtual LazinatorGeneralTree<T> CreateTree(T item)
        {
            return new LazinatorGeneralTree<T>(item);
        }

        /// <summary>
        /// Orders the item at each level of the tree, by the item at each level.
        /// </summary>
        public void Order()
        {
            var children = GetChildren();
            Children = new LazinatorList<LazinatorGeneralTree<T>>(children.OrderBy(x => x.Item));
            foreach (var child in Children)
                child.Order();
            if (ParentTree == null)
                ResetDescendantIndices();
        }

        /// <summary>
        /// Orders the item at each level of the tree, by something other than the item at each level.
        /// </summary>
        public void Order<T2>(Func<LazinatorGeneralTree<T>, T2> orderByItem) where T2 : IComparable<T2>
        {
            var children = GetChildren();
            Children = new LazinatorList<LazinatorGeneralTree<T>>(children.OrderBy(x => orderByItem(x)));
            foreach (var child in Children)
                child.Order(orderByItem);
            if (ParentTree == null)
                ResetDescendantIndices();
        }

        protected virtual void OnAddChild(LazinatorGeneralTree<T> child)
        {

        }
        protected virtual void OnRemoveChild(LazinatorGeneralTree<T> child)
        {

        }
    }
}



