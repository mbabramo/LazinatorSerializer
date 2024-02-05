using Lazinator.Collections.Location;
using Lazinator.Core;
using Lazinator.Wrappers;
using LazinatorAvlCollections.Avl.BinaryTree;
using LazinatorAvlCollections.Avl.ValueTree;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PerformanceProfiling
{
    public sealed partial class AvlCountedNode_WDouble : IAvlCountedNode_WDouble, IIndexableNode<WDouble, AvlCountedNode_WDouble>, ILazinator
    {
        public AvlCountedNode_WDouble()
        {

        }
        public long SelfCount => 1;
        public long LongCount => LeftCount + SelfCount + RightCount;

        public long Index { get; set; }

        public void OnRootAccessed()
        {
            // We set the index of the root every time it is accessed by the tree.
            Index = LeftCount;
        }

        public void OnChildNodeAccessed(AvlCountedNode_WDouble childNode, bool isLeft)
        {
            // Note: We set the index of a node every time it is accessed, since its index might have changed since the last access.
            if (childNode == null)
                return;
            AvlCountedNode_WDouble countedChildNode = (AvlCountedNode_WDouble)childNode;
            if (isLeft)
                countedChildNode.Index = Index - countedChildNode.RightCount - 1;
            else
                countedChildNode.Index = Index + countedChildNode.LeftCount + 1;
        }


        public void UpdateFollowingTreeChange()
        {
            if (Left != null && Left.NodeVisitedDuringChange)
            {
                Left.UpdateFollowingTreeChange();
            }
            if (Right != null && Right.NodeVisitedDuringChange)
            {
                Right.UpdateFollowingTreeChange();
            }
            LeftCount = Left?.LongCount ?? 0;
            RightCount = Right?.LongCount ?? 0;
            if (NodeVisitedDuringChange)
                NodeVisitedDuringChange = false;
        }


        public override string ToString()
        {
            return $"Index {Index}: {Value} (Count {LongCount}: Left {LeftCount} Self {SelfCount} Right {RightCount}) (visited {NodeVisitedDuringChange}) (ParentIndex {(Parent is AvlCountedNode_WDouble p ? p.Index.ToString() : "N/A")})";
        }

        public bool NodeVisitedDuringChange { get; set; }

        public AvlCountedNode_WDouble LeftBackingField => _Left;
        public AvlCountedNode_WDouble RightBackingField => _Right;

        // Following is boilerplate, copied with changes in type from AvlNode<T>. We are avoiding inheritance to improve performance.

        // We can't serialize the Parent, because an item can't appear multiple times in a hierarchy, so we use the Lazinator built-in parent as a substitute.
        [NonSerialized]
        private AvlCountedNode_WDouble _Parent;
        public AvlCountedNode_WDouble Parent
        {
            get
            {
                if (_Parent == null)
                {
                    if (LazinatorParents.LastAdded is AvlCountedNode_WDouble p)
                        _Parent = p;
                }

                return _Parent;
            }
            set
            {
                _Parent = value;
                LazinatorParents = new LazinatorParentsCollection(value as ILazinator);
            }
        }

        public bool IsAfterCollection => false;

        public bool IsLeftNode => Parent != null && this == Parent._Left;
        public bool IsRightNode => Parent != null && this == Parent._Right;

        /// <summary>
        /// Returns either the right or left child, preferring already allocated to nonallocated child and right to left.
        /// </summary>
        public AvlCountedNode_WDouble SomeChild => _Right ?? _Left ?? Right ?? Left;


        public AvlCountedNode_WDouble CreateNode(WDouble value, AvlCountedNode_WDouble parent = null)
        {
            return new AvlCountedNode_WDouble()
            {
                Value = value,
                Parent = parent
            };
        }

        public IContainerLocation GetLocation() => new TreeLocation<WDouble, AvlCountedNode_WDouble>(this);

        public string ToTreeString() => NodeHelpers.ToTreeString<WDouble, AvlCountedNode_WDouble>(this);
    }
}
