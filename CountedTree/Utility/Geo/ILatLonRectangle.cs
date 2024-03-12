using Lazinator.Core;
using System;
using Lazinator.Attributes;
using CountedTree;

namespace Utility
{
    [Lazinator((int)CountedTreeLazinatorUniqueIDs.LatLonRectangle)]
    public interface ILatLonRectangle
    {
        [SetterAccessibility("private")]
        LatLonPoint UpperLeft { get; }
        [SetterAccessibility("private")]
        LatLonPoint LowerRight { get; }
        // Top should be > Bottom. (Latitude goes from -90 to 90). Ordinarily, Right > Left. (Longitude goes from -180 to 180.) When a rectangle crosses the prime meridian, the incoming parameters may not meet this requirement, so we break up the rectangle in two (see below).
        [SetterAccessibility("private")]
        float? OriginalRight { get; }
    }
}