using System;

namespace Utility
{
    public partial class LatLonPoint : ILatLonPoint
    {
        public LatLonPoint(float lat, float lon)
        {
            Latitude = lat;
            Longitude = lon;
        }

        public override string ToString()
        {
            return $"({Latitude}, {Longitude})";
        }

        public LatLonPoint CorrectRange()
        {
            if (IsWithinRange())
                return this;
            return new LatLonPoint(CorrectedLatitude(), CorrectedLongitude());
        }

        public bool IsWithinRange()
        {
            return -90F <= Latitude && Latitude <= 90F && -180F <= Longitude && Longitude <= 180F;
        }

        private float CorrectedLatitude()
        {
            if (Latitude < -90F)
                return Latitude + 180F;
            if (Latitude > 90F)
                return Latitude - 180F;
            return Latitude;
        }

        private float CorrectedLongitude()
        {
            if (Longitude < -180F)
                return Longitude + 360F;
            if (Longitude > 180F)
                return Longitude - 360F;
            return Longitude;
        }

        public float ApproximateMilesTo(LatLonPoint other)
        {
            return (float)GeoCodeCalc.CalcDistance(Latitude, Longitude, other.Latitude, other.Longitude);
        }

        public override bool Equals(object obj)
        {
            LatLonPoint other = obj as LatLonPoint;
            if (other == null)
                return false;
            return Latitude == other.Latitude && Longitude == other.Longitude;
        }

        public override int GetHashCode()
        {
            return Tuple.Create(Latitude, Longitude).GetHashCode();
        }
    }
}
