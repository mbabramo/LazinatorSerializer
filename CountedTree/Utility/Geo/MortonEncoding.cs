using System;
using System.Linq;
using Utility;

namespace Utility
{

    public static class MortonEncoding
    {
        public static string ConvertToBinaryString(ulong value)
        {
            if (value == 0) return "0";
            System.Text.StringBuilder b = new System.Text.StringBuilder();
            while (value != 0)
            {
                b.Insert(0, ((value & 1) == 1) ? '1' : '0');
                value >>= 1;
            }
            var resultString = b.ToString();
            if (resultString.Length < 64)
                resultString = new string(Enumerable.Range(0, 64 - resultString.Length).Select(x => '0').ToArray()) + resultString;
            return resultString;
        }


        public static UInt64 yx2morton(UInt32 y, UInt32 x)
        {
            UInt64 y2 = MoveToEvenBits(y);
            UInt64 x2 = MoveToEvenBits(x);
            UInt64 result = x2 | (y2 << 1);
            return result;
        }

        public static void morton2yx(UInt64 d, out UInt32 y, out UInt32 x)
        {
            y = ExtractEvenBits(d >> 1);
            x = ExtractEvenBits(d);
        }

        private static ulong MoveToEvenBits(UInt32 b)
        {
            UInt64 a = b;
            a = (a | (a << 16)) & 0x0000FFFF0000FFFF;
            a = (a | (a << 8)) & 0x00FF00FF00FF00FF;
            a = (a | (a << 4)) & 0x0F0F0F0F0F0F0F0F;
            a = (a | (a << 2)) & 0x3333333333333333;
            a = (a | (a << 1)) & 0x5555555555555555;
            return a;
        }

        // morton_1 - extract even bits

        public static UInt32 ExtractEvenBits(UInt64 a)
        {
            a = a & 0x5555555555555555;
            a = (a | (a >> 1)) & 0x3333333333333333;
            a = (a | (a >> 2)) & 0x0F0F0F0F0F0F0F0F;
            a = (a | (a >> 4)) & 0x00FF00FF00FF00FF;
            a = (a | (a >> 8)) & 0x0000FFFF0000FFFF;
            a = (a | (a >> 16)) & 0x00000000FFFFFFFF;
            return (UInt32) a; // returns low 32 bits
        }


        // Originally, we used the same scale for latitude and longitude, thus not using half of the Morton range. The advantage of this is that the Morton range was a square, since one latitude point is approximately equal to one longitude point. But then we end up with a nonsensical point in our Morton range, which causes problems.



        public static double ConvertLatTo0To1(float val)
        {
            return (((double)val) / 180.0 + 0.5);
        }

        public static float Convert0To1ToLat(double val)
        {
            return (float)(-90.0 + 180.0 * (double)val);
        }

        public static double ConvertLonTo0To1(float val)
        {
            return (((double)val) / 360.0 + 0.5);
        }

        public static float Convert0To1ToLon(double val)
        {
            return (float)(-180.0 + 360.0 * (double)val);
        }

        public static UInt32 Convert0To1ToUInt32(double val)
        {
            return (UInt32) (val * UInt32.MaxValue);
        }

        public static double ConvertUInt32To0To1(UInt32 val)
        {
            return (val / (double)UInt32.MaxValue);
        }

        public static UInt32 ConvertLatToUInt32(float val)
        {
            return Convert0To1ToUInt32(ConvertLatTo0To1(val));
        }

        public static UInt32 ConvertLonToUInt32(float val)
        {
            return Convert0To1ToUInt32(ConvertLonTo0To1(val));
        }

        public static float ConvertUInt32ToLat(UInt32 val)
        {
            return Convert0To1ToLat(ConvertUInt32To0To1(val));
        }

        public static float ConvertUInt32ToLon(UInt32 val)
        {
            return Convert0To1ToLon(ConvertUInt32To0To1(val));
        }
        
        public static void yx2latlon(UInt32 y, UInt32 x, out float lat, out float lon)
        {
            // NOTE: Longitude is across, so we assign it to x.
            lon = ConvertUInt32ToLon(x);
            lat = ConvertUInt32ToLat(y);
        }

        public static void latlon2yx(float lat, float lon, out UInt32 y, out UInt32 x)
        {
            y = ConvertLatToUInt32(lat);
            x = ConvertLonToUInt32(lon);
        }

        public static ulong latlon2morton(float lat, float lon)
        {
            UInt32 x, y;
            latlon2yx(lat, lon, out y, out x);
            return yx2morton(y, x);
        }

        public static void morton2latlon(ulong m, out float lat, out float lon)
        {
            UInt32 x, y;
            morton2yx(m, out y, out x);
            yx2latlon(y, x, out lat, out lon);
        }

        public static ulong latlonpoint2morton(LatLonPoint point)
        {
            UInt32 x, y;
            latlon2yx(point.Latitude, point.Longitude, out y, out x);
            return yx2morton(y, x);
        }

        public static LatLonPoint morton2latlonpoint(ulong m)
        {
            UInt32 x, y;
            morton2yx(m, out y, out x);
            float lat, lon;
            yx2latlon(y, x, out lat, out lon);
            return new LatLonPoint(lat, lon);
        }

        public static ProperMortonRange GetProperMortonRangeFollowingValue(ulong valuePrecedingRange, byte treeDepth, int numChildrenPerInternalNode)
        {
            byte mortonDepth = GetMortonDepth(treeDepth, numChildrenPerInternalNode);
            ulong startValue = valuePrecedingRange;
            if (startValue != 0)
                startValue++; // this is the last value included in the previous node, since the first value is exclusive.
            ProperMortonRange mr = new ProperMortonRange(startValue, mortonDepth);
            return mr;
        }

        public static byte GetMortonDepth(byte treeDepth, int numChildrenPerInternalNode)
        {
            int mortonDepthPerTreeDepth = MortonTreeGenerationsToSkip(numChildrenPerInternalNode) + 1;
            byte mortonDepth = (byte)(treeDepth * mortonDepthPerTreeDepth);
            return mortonDepth;
        }

        public static int MortonTreeGenerationsToSkip(int numChildrenPerInternalNode)
        {
            switch (numChildrenPerInternalNode)
            {
                case 4:
                    return 0;
                case 16:
                    return 1;
                case 64:
                    return 2;
                case 256:
                    return 3;
                default:
                    throw new NotImplementedException();
            }
        }
    }
}
