using Lazinator.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lazinator.Collections
{
    /// <summary>
    /// A basic tree class, for relating each parent to any number of children. This class does not index items by key.
    /// </summary>
    /// <typeparam name="T">The type of each tree node</typeparam>
    public partial class LazinatorTree<T> : ILazinatorTree<T> where T : ILazinator
    {
        // The following are not serialized. They are set after the child is loaded.
        public LazinatorTree<T> ParentTree { get; set; }
        public int Level { get; set; }
        public int Index { get; set; }
        public bool Initialized { get; set; }
        public bool ChildrenLoaded { get; set; }

        public virtual void OnAddChild(LazinatorTree<T> child)
        {

        }
        public virtual void OnRemoveChild(LazinatorTree<T> child)
        {

        }

        public IEnumerable<LazinatorTree<T>> GetChildren()
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

        private void SetChildInformation(LazinatorTree<T> child, int index, bool addingChild)
        {
            child.ParentTree = this;
            child.Level = Level + 1;
            child.Index = index;
            if (addingChild)
                OnAddChild(child);
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

        public LazinatorTree<T> GetTreeAtLocation(List<int> location, int index = 0)
        {
            if (index == location.Count)
                return this;
            var children = GetChildren();
            if (index > children.Count() - 1)
                return null;
            return children.Skip(location[index]).First().GetTreeAtLocation(location, index + 1);
        }

        public LazinatorTree<T> GetTreeAtIndex(int index)
        {
            var children = GetChildren();
            if (index > children.Count() - 1)
                return null;
            return children.Skip(index).First();
        }

        public void ResetDescendantIndices()
        {
            if (Children == null)
                return;
            int index = 0;
            foreach (var child in Children)
            {
                if (child.Index != index || child.ParentTree != this || child.Level != Level + 1)
                {
                    OnRemoveChild(child);
                    SetChildInformation(child, index, true);
                    child.ResetDescendantIndices();
                }

                index++;
            }
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

        public LazinatorTree(T item)
        {
            Item = item;
        }

        public LazinatorTree<T> AddChild(T child)
        {
            if (Children == null)
                Children = new LazinatorList<LazinatorTree<T>>();
            LazinatorTree<T> childTree = new LazinatorTree<T>(child);
            return AddChildTree(childTree);
        }

        public LazinatorTree<T> AddChildTree(LazinatorTree<T> childTree)
        {
            if (Children == null)
                Children = new LazinatorList<LazinatorTree<T>>();
            Children.Add(childTree);
            SetChildInformation(childTree, Children.Count - 1, true);
            return childTree;
        }

        public LazinatorTree<T> InsertChild(T child, int index)
        {
            int childrenCount = Children.Count();
            LazinatorTree<T> childTree = null;
            if (Children == null || index == childrenCount)
                childTree = AddChild(child);
            else
            {
                if (index < 0 || index > childrenCount)
                    throw new ArgumentException("Invalid index");
                childTree = new LazinatorTree<T>(child);
                InsertChildTree(childTree, index);
            }
            return childTree;
        }

        private void InsertChildTree(LazinatorTree<T> childTree, int index)
        {
            if (Children == null)
                Children = new LazinatorList<LazinatorTree<T>>();
            Children.Insert(index, childTree);
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

        public bool RemoveChildTree(LazinatorTree<T> childTree)
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
    }
}
