using System;
using System.Collections.Generic;
using System.Text;
using Lazinator.Attributes;
using Lazinator.Core;
using Lazinator.Wrappers;

namespace Lazinator.Collections.Avl
{
    [Lazinator((int)LazinatorCollectionUniqueIDs.AvlMultiset)]
    interface IAvlMultiset<T> where T : ILazinator, new()
    {
        AvlSet<LazinatorTuple<T, WInt>> UnderlyingSet { get; set; }
        int NumItemsAdded { get; set; }
    }
}
