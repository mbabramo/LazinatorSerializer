using Lazinator.Collections.Avl.ValueTree;
using Lazinator.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lazinator.Collections.Avl.ListTree
{
    public partial class AvlListNode<T> : AvlCountedNode<T>, IAvlListNode<T> where T : ILazinator
    {
    }
}
