using Lazinator.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lazinator.Collections.BigList
{
    public partial class BigListTree<T> : LazinatorGeneralTree<BigListNode<T>>, IBigListTree<T> where T : ILazinator
    {
        public BigListTree() : base()
        {
            Item = null; // no item at the root
            InitializeWithChildLeaf();
        }

        public BigListTree(BigListNode<T> node)
        {
            Item = node;
            Item.CorrespondingTree = this;
            if (node.IsLeaf == false)
                InitializeWithChildLeaf();
        }

        public override LazinatorGeneralTree<BigListNode<T>> CreateTree(BigListNode<T> item)
        {
            return new BigListTree<T>(item);
        }

        protected internal void InitializeWithChildLeaf()
        {
            Children = new LazinatorList<LazinatorGeneralTree<BigListNode<T>>>();
            BigListLeafNode<T> leaf = new BigListLeafNode<T>();
            BigListTree<T> treeForLeaf = new BigListTree<T>(leaf);
            leaf.CorrespondingTree = treeForLeaf;
            base.Children.Add(treeForLeaf);
        }

        public BigListNode<T> BigListNode
        {
            get
            {
                var item = (BigListNode<T>)Item;
                if (item == null)
                    return item;
                item.CorrespondingTree = this;
                return item;
            }
        }

        public IEnumerable<BigListTree<T>> BigListChildTrees => Children.Select(x => (BigListTree<T>)x);

        public IEnumerable<BigListNode<T>> BigListChildNodes => BigListChildTrees.Select(x => x.BigListNode);

        public BigListTree<T> BigListParentTree => (BigListTree<T>) ParentTree;

        public BigListNode<T> BigListParentNode => BigListParentTree.BigListNode;

        protected internal BigListInteriorNode<T> DemoteChildTree(BigListTree<T> childTree, bool separateItemsIntoSeparateLeaves)
        {
            int childIndex = childTree.Index;
            BigListInteriorNode<T> interiorNode = new BigListInteriorNode<T>(childTree.BigListNode.MaxLeafCount, null);
            BigListTree<T> treeForInteriorNode = new BigListTree<T>(interiorNode);
            Children[childIndex] = treeForInteriorNode; // add interior node as a child of this
            SetChildInformation(treeForInteriorNode, childIndex, false); // make sure level is set correctly
            if (separateItemsIntoSeparateLeaves)
            {
                BigListLeafNode<T> leafNode = (BigListLeafNode<T>)childTree.BigListNode;
                foreach (T t in leafNode.Items)
                {
                    BigListLeafNode<T> newLeafNode = new BigListLeafNode<T>(childTree.BigListNode.MaxLeafCount, null);
                    newLeafNode.Items.Add(t); // only item for this node for now
                    treeForInteriorNode.AddChild(newLeafNode);
                }
            }
            else
            {
                treeForInteriorNode.AddChild(childTree.BigListNode); // add node being demoted as child of interior node
                childTree.BigListNode.CorrespondingTree = treeForInteriorNode.BigListChildTrees.First();
                // now, childTree is no longer attached to the tree, but its node is within a new tree.
            }
            return interiorNode;
        }

        protected internal void ModifyCount(long increment)
        {
            if (BigListNode is BigListNode<T> n)
            {
                n.Count += increment;
                BigListParentTree.ModifyCount(increment);
            }
        }
    }
}
