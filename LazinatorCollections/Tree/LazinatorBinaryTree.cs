using Lazinator.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LazinatorCollections.Tree
{
    public partial class LazinatorBinaryTree<T> : ILazinatorBinaryTree<T> where T : ILazinator
    {

        public LazinatorBinaryTree(T initialItem)
        {
            Item = initialItem;
        }

        public LazinatorBinaryTree<T> ParentTree => LazinatorParents.LastAdded as LazinatorBinaryTree<T>;

        public void Insert(T item, IComparer<T> comparer)
        {
            var comparison = comparer.Compare(item, Item);
            if (comparison < 0)
            {
                if (Left is null)
                    Left = new LazinatorBinaryTree<T>(item);
                else
                    Left.Insert(item, comparer);
            }
            else if (comparison > 0)
            {
                if (Right is null)
                    Right = new LazinatorBinaryTree<T>(item);
                else
                    Right.Insert(item, comparer);
            }
        }

        public void Delete(T item, IComparer<T> comparer)
        {
            var comparison = comparer.Compare(item, Item);
            if (comparison == 0)
                throw new Exception("Cannot delete only item in tree.");
            if (comparison < 0)
            {
                if (Left == null)
                    throw new Exception("Not found.");
                if (comparer.Compare(item, Left.Item) == 0)

            }
        }
    }
}
