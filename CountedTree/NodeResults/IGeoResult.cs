using Lazinator.Core;
using System;
using Lazinator.Attributes;
using CountedTree.Core;
using Lazinator.Wrappers;

namespace CountedTree.NodeResults
{
    [Lazinator((int)CountedTreeLazinatorUniqueIDs.GeoResult)]
    public interface IGeoResult
    {
        [SetterAccessibility("private")]
        public KeyAndID<WUInt64> KeyAndID { get; }
        [SetterAccessibility("private")]
        public float Distance { get; }
    }
}