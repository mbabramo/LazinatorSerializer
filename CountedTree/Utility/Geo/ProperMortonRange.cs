using System;
using System.Collections.Generic;
using System.Linq;

namespace Utility
{
    /// <summary>
    /// A proper Morton Range represents a range of Morton numbers achieved by successively dividing the overall range into quadrants. A virtue of a proper Morton Range is that it can easily be converted to a latitude/longitude rectangle defined by its start and end points and encompassing all points in the Morton Range.
    /// </summary>
    public partial class ProperMortonRange : IProperMortonRange
    {

        public ulong EndValue()
        {
            ulong v = StartValue;
            v = v | ulong.MaxValue >> Depth * 2;
            return v;
        }

        public ProperMortonRange(ulong startValue, byte depth)
        {
            StartValue = startValue;
            Depth = depth;
            if (Depth > 31)
                throw new Exception("Depth is beyond Morton precision.");
        }

        public override string ToString()
        {
            string tabs = new string (Enumerable.Range(0, Depth * 3).Select(x => ' ').ToArray());
            return $"{tabs} {Depth}: {MortonEncoding.ConvertToBinaryString(StartValue)} - {MortonEncoding.ConvertToBinaryString(EndValue())}";
        }

        public LatLonRectangle ToLatLonRectangle()
        {
            var lowerLeft = MortonEncoding.morton2latlonpoint(StartValue);
            var upperRight = MortonEncoding.morton2latlonpoint(EndValue());
            return new LatLonRectangle(new LatLonPoint(upperRight.Latitude, lowerLeft.Longitude), new LatLonPoint(lowerLeft.Latitude, upperRight.Longitude));
        }

        public bool Intersects(LatLonRectangle rectangle)
        {
            return ToLatLonRectangle().Intersects(rectangle);
        }


        public float ApproximateMilesToPoint(LatLonPoint point)
        {
            return ToLatLonRectangle().ApproximateMilesToPoint(point);
        }

        public float ApproximateMilesToPoint(ulong point)
        {
            return ApproximateMilesToPoint(MortonEncoding.morton2latlonpoint(point));
        }

        public float ApproximateMilesToRectangle(LatLonRectangle other)
        {
            return ToLatLonRectangle().ApproximateMilesToRectangle(other);
        }

        public float ApproximateMilesToRectangle(ProperMortonRange other)
        {
            return ToLatLonRectangle().ApproximateMilesToRectangle(other.ToLatLonRectangle());
        }

        public UInt64 GetChildOfProperMortonRange(ProperMortonQuadrant childQuadrant)
        {
            // Illustrated with just 4 bits. Suppose our start value is 0000 and our startDepth is 0.
            // Then YLowXLow is still 0000, YLowXHigh quadrant is 0100, YHighXLow is 1000, YHighXHigh is 1100. If our startDepth is 1, we would affect the next two digits.

            UInt64 v = StartValue;
            const UInt64 one = 1;
            int i = 63 - 2 * Depth; // highest bit position (which is odd)
            if (i > 0)
            {
                if (childQuadrant == ProperMortonQuadrant.YHighXLow || childQuadrant == ProperMortonQuadrant.YHighXHigh)
                    v = v | (one << i); // encoding y high bit in odd bit
                i--;
                if (childQuadrant == ProperMortonQuadrant.YLowXHigh || childQuadrant == ProperMortonQuadrant.YHighXHigh)
                    v = v | (one << i); // encoding x high bit in even bit
            }
            return v;
        }

        public IEnumerable<ProperMortonRange> GetChildren()
        {
            if (Depth >= 31)
                throw new Exception();
            yield return new ProperMortonRange(StartValue, (byte)(Depth + 1));
            yield return new ProperMortonRange(GetChildOfProperMortonRange(ProperMortonQuadrant.YLowXHigh), (byte)(Depth + 1));
            yield return new ProperMortonRange(GetChildOfProperMortonRange(ProperMortonQuadrant.YHighXLow), (byte)(Depth + 1));
            yield return new ProperMortonRange(GetChildOfProperMortonRange(ProperMortonQuadrant.YHighXHigh), (byte)(Depth + 1));
        }

        public IEnumerable<ProperMortonRange> GetDescendants(int numGenerationsToSkip)
        {
            if (Depth + numGenerationsToSkip >= 31)
                yield break;
            if (numGenerationsToSkip == 0)
                foreach (var result in GetChildren())
                    yield return result;
            else foreach (ProperMortonRange mr in GetChildren())
                foreach (var result in mr.GetDescendants(numGenerationsToSkip - 1))
                    yield return result;
        }

        public override bool Equals(object obj)
        {
            ProperMortonRange other = obj as ProperMortonRange;
            if (other == null)
                return false;
            return StartValue == other.StartValue && Depth == other.Depth;
        }

        public override int GetHashCode()
        {
            return Tuple.Create(StartValue, Depth).GetHashCode();
        }
    }
}
