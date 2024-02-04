
using Lazinator.Core;
using System;
using System.Collections.Generic;
using System.Text;
using LazinatorAvlCollections.Avl.BinaryTree;
using Lazinator.Collections.Location;
using LazinatorAvlCollections.Avl;
using Lazinator.Wrappers;

namespace PerformanceProfiling
{
    public sealed partial class AvlNode_WDouble : IAvlNode_WDouble, ITreeString, IUpdatableNode<WDouble, AvlNode_WDouble>, ILazinator
    {
        public bool NodeVisitedDuringChange { get; set; }

        public void UpdateFollowingTreeChange()
        {
        }

        public AvlNode_WDouble LeftBackingField => _Left;
        public AvlNode_WDouble RightBackingField => _Right;

        // Following is boilerplate, copied with changes in type from BinaryNode<T>. We are avoiding inheritance to improve performance.

        // We can't serialize the Parent, because an item can't appear multiple times in a hierarchy, so we use the Lazinator built-in parent as a substitute.
        [NonSerialized]
        private AvlNode_WDouble _Parent;
        public AvlNode_WDouble Parent
        {
            get
            {
                if (_Parent == null)
                {
                    if (LazinatorParents.LastAdded is AvlNode_WDouble p)
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

        public void OnRootAccessed()
        {

        }

        /// <summary>
        /// Returns either the right or left child, preferring already allocated to nonallocated child and right to left.
        /// </summary>
        public AvlNode_WDouble SomeChild => _Right ?? _Left ?? Right ?? Left;

        public AvlNode_WDouble()
        {

        }

        public AvlNode_WDouble CreateNode(WDouble value, AvlNode_WDouble parent = null)
        {
            return new AvlNode_WDouble()
            {
                Value = value,
                Parent = parent
            };
        }

        public IContainerLocation GetLocation() => new TreeLocation<WDouble, AvlNode_WDouble>(this);

        public string ToTreeString() => NodeHelpers.ToTreeString<WDouble, AvlNode_WDouble>(this);

        public override string ToString()
        {
            return $"{Value}";
        }

    }
}
