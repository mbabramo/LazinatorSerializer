using Lazinator.Core;
using System;
using Lazinator.Attributes;
using System.Collections.Generic;

namespace CountedTree.Updating
{
    [Lazinator((int)CountedTreeLazinatorUniqueIDs.RedundancyAvoider)]
    public interface IRedundancyAvoider
    {
        Dictionary<Guid, LastClientAccess> LastAccessDictionary { get; set; }
    }
}