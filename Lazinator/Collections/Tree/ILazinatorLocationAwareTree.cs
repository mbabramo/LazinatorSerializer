﻿using Lazinator.Attributes;
using Lazinator.Collections.Dictionary;
using Lazinator.Core;
using Lazinator.Wrappers;

namespace Lazinator.Collections.Tree
{
    [Lazinator((int)LazinatorCollectionUniqueIDs.ILazinatorLocationAwareTree)]
    public interface ILazinatorLocationAwareTree<T> where T : ILazinator
    {
        LazinatorDictionary<T, LazinatorList<WInt>> Locations { get; set; }
    }
}