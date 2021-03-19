using Lazinator.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LazinatorCollections.Tree
{
    public partial class LazinatorBinaryTreeNode<T> : ILazinatorBinaryTreeNode<T> where T : ILazinator, IComparable<T>
    {
    }
}
