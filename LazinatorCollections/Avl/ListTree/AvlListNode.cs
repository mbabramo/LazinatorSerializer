using LazinatorCollections.Avl.ValueTree;
using Lazinator.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace LazinatorCollections.Avl.ListTree
{
    public partial class AvlListNode<T> : AvlCountedNode<T>, IAvlListNode<T> where T : ILazinator
    {
    }
}
