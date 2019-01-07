﻿using Lazinator.Attributes;
using Lazinator.Core;
using System;

namespace Lazinator.Collections.Avl
{
    [Lazinator((int)LazinatorCollectionUniqueIDs.IAvlOldTree)]
    public interface IAvlOldTree<TKey, TValue> where TKey : ILazinator, IComparable<TKey> where TValue : ILazinator
    {
        bool AllowDuplicates { get; set; }
        AvlOldNode<TKey, TValue> Root { get; set; }
    }
}