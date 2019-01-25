﻿using Lazinator.Core;
using Lazinator.Attributes;
using System;
using LazinatorCollections.Interfaces;

namespace LazinatorCollections.Avl.ValueTree
{
    [Lazinator((int)LazinatorCollectionUniqueIDs.IAvlSortedTree)]
    public interface IAvlSortedTree<T> : ISortedMultivalueContainer<T> where T : IComparable<T>, ILazinator
    {
    }
}