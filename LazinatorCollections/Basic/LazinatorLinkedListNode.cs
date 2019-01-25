using Lazinator.Buffers;
using Lazinator.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace LazinatorCollections
{
    public partial class LazinatorLinkedListNode<T> : ILazinatorLinkedListNode<T> where T : ILazinator
    {
        public LazinatorLinkedListNode(T item)
        {
            Value = item;
        }
    }
}
