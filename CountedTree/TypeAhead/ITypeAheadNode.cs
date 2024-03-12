using Lazinator.Core;
using System;
using Lazinator.Attributes;
using System.Collections.Generic;
using Lazinator.Collections;
using Lazinator.Wrappers;

namespace CountedTree.TypeAhead
{
    [Lazinator((int)CountedTreeLazinatorUniqueIDs.TypeAheadNode)]
    public interface ITypeAheadNode<R> where R : ILazinator, IComparable, IComparable<R>, IEquatable<R>
    {
        string StringToHere { get; set; }
        List<TypeAheadItem<R>> TypeAheadItems { get; set; }
        char[] NextChars { get; set; } 
        /// <summary>
        /// The minimum number of results to maintain if possible. If the node has this many results but then one is removed, then it will request more results. 
        /// </summary>
        int MinThreshold { get; set; }
        /// <summary>
        /// The maximum number of results to maintain if possible.
        /// </summary>
        int MaxThreshold { get; set; }
        /// <summary>
        /// The most popular item that we have found insufficiently popular to add to our list, since the last time that we reflushed our buffers by scanning the children for their most popular items.
        /// </summary>
        TypeAheadItem<R> MostPopularRejected { get; set; }
        /// <summary>
        /// When an item is added as an exact match (i.e., the entire search phrase is included), then we store it here, because it will not be stored in child nodes. Then, if there is room later, we can add it back in.
        /// </summary>
        LazinatorSortedList<TypeAheadItem<R>> ExactMatches { get; set; }
    }
}