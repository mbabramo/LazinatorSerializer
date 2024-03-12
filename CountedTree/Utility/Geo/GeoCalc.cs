
using GeoCoordinatePortable;
using System;

namespace Utility
{
    public static class BoundingBox
    {
        // Semi-axes of WGS-84 geoidal reference
        private const double WGS84_a = 6378137.0; // Major semiaxis [m]
        private const double WGS84_b = 6356752.3; // Minor semiaxis [m]

        // 'halfSideInKm' is the half length of the bounding box you want in kilometers.
        public static LatLonRectangle GetBoundingBox(LatLonPoint point, double halfSideInMiles)
        {
            // if the box is greater than the circumference of the earth, return a full bounding box. Otherwise, our bounding box will wrap around.
            if (halfSideInMiles > 12450)
                return new LatLonRectangle(new LatLonPoint(90F, -180F), new LatLonPoint(-90F, 180F));

            double halfSideInKm = halfSideInMiles * 1.60934;

            // Bounding box surrounding the point at given coordinates,
            // assuming local approximation of Earth surface as a sphere
            // of radius given by WGS84
            var lat = Deg2rad(point.Latitude);
            var lon = Deg2rad(point.Longitude);
            var halfSide = 1000 * halfSideInKm;

            // Radius of Earth at given latitude
            var radius = WGS84EarthRadius(lat);
            // Radius of the parallel at given latitude
            var pradius = radius * Math.Cos(lat);

            var latMin = lat - halfSide / radius;
            var latMax = lat + halfSide / radius;
            var lonMin = lon - halfSide / pradius;
            var lonMax = lon + halfSide / pradius;

            // Note: If we cross the prime meridian, our result will have Right < Left (e.g., Left = 179, Right = -179). Our LatLonRectangle class recognizes this situation.

            // Check for poles. If a pole is in the distance, then we must set the pole as the max (north) or min (south), plus allow all longitude values. 
            if (GeoCodeCalc.CalcDistance(point.Latitude, point.Longitude, 90.0, 0) < halfSideInMiles)
            {
                latMax = Deg2rad(90.0);
                lonMin = Deg2rad(-180.0F);
                lonMax = Deg2rad(180.0F);
            }
            else if (GeoCodeCalc.CalcDistance(point.Latitude, point.Longitude, -90.0, 0) < halfSideInMiles)
            {
                latMin = Deg2rad(-90.0);
                lonMin = Deg2rad(-180.0F);
                lonMax = Deg2rad(180.0F);
            }

            var latMaxDeg = (float)Rad2deg(latMax);
            var latMinDeg = (float)Rad2deg(latMin);
            var lonMinDeg = (float)Rad2deg(lonMin);
            var lonMaxDeg = (float)Rad2deg(lonMax);
            if (latMaxDeg > 90F)
                latMaxDeg = 90F; // adjust for rounding error
            if (latMinDeg < -90F)
                latMinDeg = -90F;
            return new LatLonRectangle(
                new LatLonPoint(latMaxDeg, lonMinDeg),
                new LatLonPoint(latMinDeg, lonMaxDeg)
                );
        }

        // degrees to radians
        private static double Deg2rad(double degrees)
        {
            return Math.PI * degrees / 180.0;
        }

        // radians to degrees
        private static double Rad2deg(double radians)
        {
            return 180.0 * radians / Math.PI;
        }

        // Earth radius at a given latitude, according to the WGS-84 ellipsoid [m]
        private static double WGS84EarthRadius(double lat)
        {
            // http://en.wikipedia.org/wiki/Earth_radius
            var An = WGS84_a * WGS84_a * Math.Cos(lat);
            var Bn = WGS84_b * WGS84_b * Math.Sin(lat);
            var Ad = WGS84_a * Math.Cos(lat);
            var Bd = WGS84_b * Math.Sin(lat);
            return Math.Sqrt((An * An + Bn * Bn) / (Ad * Ad + Bd * Bd));
        }
    }

    public static class GeoCodeCalc
    {
        public const double EarthRadiusInMiles = 3956.0;
        public const double EarthRadiusInKilometers = 6367.0;
        public static double ToRadian(double val) { return val * (Math.PI / 180); }
        public static double DiffRadian(double val1, double val2) { return ToRadian(val2) - ToRadian(val1); }

        public static double CalcDistance(double lat1, double lng1, double lat2, double lng2)
        {
            GeoCoordinate coordinate1 = new GeoCoordinate(lat1, lng1);
            GeoCoordinate coordinate2 = new GeoCoordinate(lat2, lng2);
            return coordinate1.GetDistanceTo(coordinate2) * 0.000621371 /* miles per meter */;
        }

        /* Keeping the following code for now because it produces a different result. Unfortunately, neither is exactly consistent with the bounding box code. */

        /// <summary>
        /// Calculate the distance between two geocodes. Defaults to using Miles.
        /// </summary>
        public static double CalcDistance_Alt(double lat1, double lng1, double lat2, double lng2)
        {
            return CalcDistance_Alt(lat1, lng1, lat2, lng2, GeoCodeCalcMeasurement.Miles);
        }
        /// <summary>
        /// Calculate the distance between two geocodes.
        /// </summary>
        public static double CalcDistance_Alt(double lat1, double lng1, double lat2, double lng2, GeoCodeCalcMeasurement m)
        {
            double radius = EarthRadiusInMiles;
            if (m == GeoCodeCalcMeasurement.Kilometers) { radius = EarthRadiusInKilometers; }
            return radius * 2 * Math.Asin(Math.Min(1, Math.Sqrt((Math.Pow(Math.Sin((DiffRadian(lat1, lat2)) / 2.0), 2.0) + Math.Cos(ToRadian(lat1)) * Math.Cos(ToRadian(lat2)) * Math.Pow(Math.Sin((DiffRadian(lng1, lng2)) / 2.0), 2.0)))));
        }
    }
    public enum GeoCodeCalcMeasurement : int
    {
        Miles = 0,
        Kilometers = 1
    }
}