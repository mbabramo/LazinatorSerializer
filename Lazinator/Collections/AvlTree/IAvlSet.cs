using System;
using System.Collections.Generic;
using System.Text;
using Lazinator.Attributes;
using Lazinator.Collections.Avl;
using Lazinator.Core;
using Lazinator.Wrappers;

namespace Lazinator.Collections.AvlTree
{
    [Lazinator((int)LazinatorCollectionUniqueIDs.AvlSet)]
    interface IAvlSet<TKey> where TKey : ILazinator, new()
    {
        AvlTree<TKey, LazinatorWrapperByte> UnderlyingTree { get; set; }
    }
}
