using Lazinator.Core;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CountedTree.TypeAhead
{
    public partial class TypeAheadItemsForParent<R> : ITypeAheadItemsForParent<R> where R : ILazinator,
              IComparable,
              IComparable<R>,
              IEquatable<R>
    {

        public TypeAheadItemsForParent(TypeAheadNode<R> typeAheadNode, string searchString, TypeAheadItem<R> lowestExistingItemOnParent, int maxNumberResults)
        {
            // We want to return only items that are WORSE than the lowest existing item on parent. Otherwise, these items will already be on the parent.
            // It could be that even after returning the number of results requested, there are more items that we could return IF we loaded more items
            // from the child node's own children. 
            // This would be so if this node (the child) has rejected something. 
            CandidatesForParent = typeAheadNode.TypeAheadItems.Where(x => x.CompareTo(lowestExistingItemOnParent) > 0).ToList(); // anything later would already BE on parent
            MostPopularRejected = typeAheadNode.MostPopularRejected;
            SearchString = searchString;
            if (MostPopularRejected != null)
                NextChars = typeAheadNode.NextChars; // we need to tell the parent its grandchildren, so that it can do more searching
        }
    }
}
