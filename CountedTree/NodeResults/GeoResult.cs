using CountedTree.Core;
using Lazinator.Wrappers;
using R8RUtilities;
using System;
using Utility;

namespace CountedTree.NodeResults
{
    public partial class GeoResult : IGeoResult
    {
        public LatLonPoint Location => MortonEncoding.morton2latlonpoint(KeyAndID.Key);

        public GeoResult(KeyAndID<WUInt64> keyAndID, float distance)
        {
            KeyAndID = keyAndID;
            Distance = distance;
        }

        public override bool Equals(object obj)
        {
            GeoResult other = obj as GeoResult;
            if (other == null)
                return false;
            return Distance == other.Distance && KeyAndID.Equals(other.KeyAndID);
        }

        public override int GetHashCode()
        {
            return Tuple.Create(Distance, KeyAndID).GetHashCode();
        }
    }
}
