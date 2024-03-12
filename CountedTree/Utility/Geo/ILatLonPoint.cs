using Lazinator.Core;
using System;
using Lazinator.Attributes;
using CountedTree;

namespace Utility
{
    [Lazinator((int)CountedTreeLazinatorUniqueIDs.LatLonPoint)]
    public interface ILatLonPoint
    {
        [SetterAccessibility("private")]
        float Latitude { get; }
        [SetterAccessibility("private")]
        float Longitude { get; }
    }
}