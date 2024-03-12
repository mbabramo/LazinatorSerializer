using System;
using System.Linq;

namespace Utility
{
    public partial class LatLonRectangle : ILatLonRectangle
    {
        public float Top => UpperLeft.Latitude;
        public float Bottom => LowerRight.Latitude;
        public float Left => UpperLeft.Longitude;
        public float Right => LowerRight.Longitude;
        public LatLonPoint LowerLeft => new LatLonPoint(Bottom, Left);
        public LatLonPoint UpperRight => new LatLonPoint(Top, Right);
        public bool CutOffAtPrimeMeridian => OriginalRight != null;
        public LatLonRectangle CutOffArea => CutOffAtPrimeMeridian ? new LatLonRectangle(new LatLonPoint(Top, -180F), new LatLonPoint(Bottom, (float)OriginalRight)) : null;

        public LatLonRectangle(LatLonPoint upperLeft, LatLonPoint lowerRight)
        {
            UpperLeft = upperLeft.CorrectRange();
            LowerRight = lowerRight.CorrectRange();
            bool crossesPrimeMeridian = Right < Left; // e.g., left is 179 and right wraps around to -179
            if (crossesPrimeMeridian)
            { // if we construct a LatLonRectangle that overlaps the PrimeMeridian, we are going to cut it off on the right side at the prime meridian. This is an incomplete solution but will suffice fornow.
                OriginalRight = Right;
                LowerRight = new LatLonPoint(Bottom, 180F);
            }
            if (Bottom > Top || Top > 90 || Bottom < -90 || Left < -180 || Right > 180)
                throw new Exception("Invalid latitude/longitude rectangle.");
        }

        public bool PointIsWithin(LatLonPoint point)
        {
            return point.Latitude >= Bottom && point.Latitude <= Top && point.Longitude >= Left && point.Longitude <= Right;
        }

        // longitudes could be anything here
        public LatLonPoint NorthPole => new LatLonPoint(90, 0);
        public LatLonPoint SouthPole => new LatLonPoint(-90, 0);

        public LatLonPoint ClosestPointInOrOnRectangle(LatLonPoint outsidePoint)
        {
            LatLonPoint point = ClosestPointInOrOnRectangle_AwayFromPoles(outsidePoint);
            if (PointIsWithin(outsidePoint))
                return outsidePoint;
            const float topThreshold = 60F;
            const float bottomThreshold = -60F;
            if (Top >= topThreshold || Bottom <= bottomThreshold)
            { // Suppose you are one mile south of the North Pole. Draw a small rectangle about yourself. Then walk to the North Pole and keep going another half mile. If we use the typical formula, it would seem that you are several miles from that rectangle, because we would be measuring the route using constant latitude. But it's faster to walk through the pole. So, this messes up our calculations. This shouldn't matter as we get farther from the poles. Even Ottawa is at the 45th parallel, so this is really an edge case in typical use.
                if (Top >= topThreshold)
                {
                    LatLonPoint point2 = ClosestPointOnHorizontalSegment(outsidePoint, Top, Left, Right);
                    point = (GetCloserPoint(outsidePoint, point, point2));
                }
                if (Bottom <= bottomThreshold)
                {
                    LatLonPoint point3 = ClosestPointOnHorizontalSegment(outsidePoint, Bottom, Left, Right);
                    point = (GetCloserPoint(outsidePoint, point, point3));
                }
                // compare to corners for completeness
                point = GetCloserPoint(outsidePoint, point, LowerLeft);
                point = GetCloserPoint(outsidePoint, point, LowerRight);
                point = GetCloserPoint(outsidePoint, point, UpperLeft);
                point = GetCloserPoint(outsidePoint, point, UpperRight);
                return point;
            }
            else
                return point;
        }

        private LatLonPoint ClosestPointOnHorizontalSegment(LatLonPoint outsidePoint, float latitude, float low_long, float high_long)
        {
            GoldenSectionOptimizer opt = new GoldenSectionOptimizer() { LowExtreme = low_long, HighExtreme = high_long, Precision = 0.01, Minimizing = true, TheFunction = long_value => new LatLonPoint(latitude, (float) long_value).ApproximateMilesTo(outsidePoint) };
            float result = (float) opt.Optimize();
            return new LatLonPoint(latitude, result);
        }

        public LatLonPoint ClosestPointInOrOnRectangle_AwayFromPoles(LatLonPoint outsidePoint)
        {
            LatLonPoint candidate1 = ClosestPointInOrOnRectangle_AwayFromPoles_MainRectOnly(outsidePoint);
            if (!CutOffAtPrimeMeridian)
                return candidate1;
            LatLonPoint candidate2 = CutOffArea.ClosestPointInOrOnRectangle_AwayFromPoles_MainRectOnly(outsidePoint);
            return GetCloserPoint(outsidePoint, candidate1, candidate2);
        }

        private static LatLonPoint GetCloserPoint(LatLonPoint outsidePoint, LatLonPoint candidate1, LatLonPoint candidate2)
        {
            if (outsidePoint.ApproximateMilesTo(candidate1) < outsidePoint.ApproximateMilesTo(candidate2))
                return candidate1;
            else
                return candidate2;
        }

        public LatLonPoint ClosestPointInOrOnRectangle_AwayFromPoles_MainRectOnly(LatLonPoint outsidePoint)
        { 
            float latitude = Math.Min(Math.Max(outsidePoint.Latitude, Bottom), Top);
            float longitude;
            if (Left < outsidePoint.Longitude && outsidePoint.Longitude < Right)
                longitude = outsidePoint.Longitude; // longitude is between the extremes, so just keep it where it is
            else if (outsidePoint.Longitude > 0 == Left > 0 && Left > 0 == Right > 0)
                longitude = Math.Min(Math.Max(outsidePoint.Longitude, Left), Right); // all longitude values are same sign
            else
            {
                LatLonPoint candidate1 = new LatLonPoint(latitude, Left);
                LatLonPoint candidate2 = new LatLonPoint(latitude, Right);
                LatLonPoint closer = GetCloserPoint(outsidePoint, candidate1, candidate2);
                longitude = closer.Longitude;
            }
            return new LatLonPoint(latitude, longitude);
        }

        public float ApproximateMilesToRectangle(LatLonRectangle other)
        {
            if (Intersects(other))
                return 0;
            return new float[] {
                ApproximateMilesToPoint(other.UpperLeft),
                ApproximateMilesToPoint(other.UpperRight),
                ApproximateMilesToPoint(other.LowerLeft),
                ApproximateMilesToPoint(other.LowerRight)
            }.Min();
        }

        public float ApproximateMilesToPoint(LatLonPoint point)
        {
            if (CutOffAtPrimeMeridian)
                return Math.Min(ApproximateMilesToPoint_SingleRectangle(point), CutOffArea.ApproximateMilesToPoint_SingleRectangle(point));
            else
                return ApproximateMilesToPoint_SingleRectangle(point);
        }

        private float ApproximateMilesToPoint_SingleRectangle(LatLonPoint point)
        {
            if (PointIsWithin(point))
                return 0;
            else return point.ApproximateMilesTo(ClosestPointInOrOnRectangle(point));
        }

        public bool EntirelyContains(LatLonRectangle other)
        {
            return PointIsWithin(other.UpperLeft) && PointIsWithin(other.LowerRight);
        }

        public bool Intersects(LatLonRectangle other)
        {
            if (Intersects_Helper(other))
                return true;
            // We must check the rectangles to see if any cut-off area intersects
            if (CutOffAtPrimeMeridian && CutOffArea.Intersects_Helper(other))
                return true;
            if (other.CutOffAtPrimeMeridian && Intersects_Helper(other.CutOffArea))
                return true;
            if (CutOffAtPrimeMeridian && other.CutOffAtPrimeMeridian && CutOffArea.Intersects_Helper(other.CutOffArea))
                return true;
            return false;
        }

        public bool Intersects_Helper(LatLonRectangle other)
        {
            return (Left < other.Right && Right > other.Left && Bottom < other.Top && Top > other.Bottom);
        }

        public override string ToString()
        {
            return $"Long: {Left}-{Right} Lat:{Bottom}-{Top}";
        }
    }
}
