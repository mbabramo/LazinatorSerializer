using Lazinator.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LazinatorCollections.Tree
{
    public partial class LazinatorBinaryTree<T> : ILazinatorBinaryTree<T> where T : ILazinator, IComparable<T>
    {

        // code adapted from http://csharpexamples.com/c-binary-search-tree-implementation/

        public bool Add(T value)
        {
            LazinatorBinaryTreeNode<T> before = null, after = this.Root;

            while (after != null)
            {
                before = after;
                int comparison = value.CompareTo(after.Data);
                if (comparison < 0) //Is new node in left tree? 
                    after = after.LeftNode;
                else if (comparison > 0) //Is new node in right tree?
                    after = after.RightNode;
                else
                {
                    //Exist same value
                    return false;
                }
            }

            LazinatorBinaryTreeNode<T> newNode = new LazinatorBinaryTreeNode<T>();
            newNode.Data = value;

            if (this.Root == null)//Tree ise empty
                this.Root = newNode;
            else
            {
                int comparison = value.CompareTo(before.Data);
                if (comparison < 0)
                    before.LeftNode = newNode;
                else
                    before.RightNode = newNode;
            }

            return true;
        }

        public LazinatorBinaryTreeNode<T> Find(T value)
        {
            return this.Find(value, this.Root);
        }

        public void Remove(T value)
        {
            this.Root = Remove(this.Root, value);
        }

        private LazinatorBinaryTreeNode<T> Remove(LazinatorBinaryTreeNode<T> parent, T key)
        {
            if (parent == null) return parent;

            int comparison = key.CompareTo(parent.Data);
            if (comparison < 0) parent.LeftNode = Remove(parent.LeftNode, key);
            else if (comparison > 0)
                parent.RightNode = Remove(parent.RightNode, key);

            // if value is same as parent's value, then this is the node to be deleted  
            else
            {
                // node with only one child or no child  
                if (parent.LeftNode == null)
                    return parent.RightNode;
                else if (parent.RightNode == null)
                    return parent.LeftNode;

                // node with two children: Get the inorder successor (smallest in the right subtree)  
                parent.Data = MinValue(parent.RightNode);

                // Delete the inorder successor  
                parent.RightNode = Remove(parent.RightNode, parent.Data);
            }

            return parent;
        }

        private T MinValue(LazinatorBinaryTreeNode<T> node)
        {
            T minv = node.Data;

            while (node.LeftNode != null)
            {
                minv = node.LeftNode.Data;
                node = node.LeftNode;
            }

            return minv;
        }

        private LazinatorBinaryTreeNode<T> Find(T value, LazinatorBinaryTreeNode<T> parent)
        {
            if (parent != null)
            {
                int comparison = value.CompareTo(parent.Data);
                if (comparison == 0) return parent;
                if (comparison < 0)
                    return Find(value, parent.LeftNode);
                else
                    return Find(value, parent.RightNode);
            }

            return null;
        }

        public int GetTreeDepth()
        {
            return this.GetTreeDepth(this.Root);
        }

        private int GetTreeDepth(LazinatorBinaryTreeNode<T> parent)
        {
            return parent == null ? 0 : Math.Max(GetTreeDepth(parent.LeftNode), GetTreeDepth(parent.RightNode)) + 1;
        }

        public void TraversePreOrder(LazinatorBinaryTreeNode<T> parent)
        {
            if (parent != null)
            {
                Console.Write(parent.Data + " ");
                TraversePreOrder(parent.LeftNode);
                TraversePreOrder(parent.RightNode);
            }
        }

        public void TraverseInOrder(LazinatorBinaryTreeNode<T> parent)
        {
            if (parent != null)
            {
                TraverseInOrder(parent.LeftNode);
                Console.Write(parent.Data + " ");
                TraverseInOrder(parent.RightNode);
            }
        }

        public void TraversePostOrder(LazinatorBinaryTreeNode<T> parent)
        {
            if (parent != null)
            {
                TraversePostOrder(parent.LeftNode);
                TraversePostOrder(parent.RightNode);
                Console.Write(parent.Data + " ");
            }
        }
    }
}
