using Lazinator.Buffers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Lazinator.Core
{
    public readonly struct LazinatorGenericIDType
    {
        public readonly int OuterType, InnerType1, InnerType2, InnerType3, InnerType4;
        public readonly List<int> FurtherInnerTypeIDs;

        public LazinatorGenericIDType(List<int> outerAndInnerTypeIDs)
        {
            OuterType = outerAndInnerTypeIDs.First();
            int count = outerAndInnerTypeIDs.Count();
            InnerType1 = count > 1 ? outerAndInnerTypeIDs.Skip(1).First() : -1;
            InnerType2 = count > 2 ? outerAndInnerTypeIDs.Skip(2).First() : -1;
            InnerType3 = count > 3 ? outerAndInnerTypeIDs.Skip(3).First() : -1;
            InnerType4 = count > 4 ? outerAndInnerTypeIDs.Skip(4).First() : -1;
            FurtherInnerTypeIDs = count >= 5 ? outerAndInnerTypeIDs.Skip(5).ToList() : null;
        }

        public LazinatorGenericIDType(int outerType = -1, int innerType1 = -1, int innerType2 = -1, int innerType3 = -1, int innerType4 = -1, List<int> furtherInnerTypeIDs = null)
        {
            OuterType = outerType;
            InnerType1 = innerType1;
            InnerType2 = innerType2;
            InnerType3 = innerType3;
            InnerType4 = innerType4;
            FurtherInnerTypeIDs = furtherInnerTypeIDs;
            if (FurtherInnerTypeIDs != null && !FurtherInnerTypeIDs.Any())
                FurtherInnerTypeIDs = null;
            if (InnerType4 == -1 && FurtherInnerTypeIDs != null)
                throw new Exception("Invalid LazinatorGenericIDType.");
        }

        public bool IsEmpty => OuterType == -1;

        public int Count()
        {
            if (InnerType1 == -1)
                return 1;
            if (InnerType2 == -1)
                return 2;
            if (InnerType3 == -1)
                return 3;
            if (InnerType4 == -1)
                return 4;
            if (FurtherInnerTypeIDs == null)
                return 5;
            return 5 + FurtherInnerTypeIDs.Count();
        }

        public int this[int i]
        {
            get
            {
                if (i == 0)
                    return OuterType;
                if (i == 1)
                    return InnerType1;
                if (i == 2)
                    return InnerType2;
                if (i == 3)
                    return InnerType3;
                if (i == 4)
                    return InnerType4;
                return FurtherInnerTypeIDs[i - 5];
            }
        }

        public void Write(ref BinaryBufferWriter writer)
        {
            // We write the first component before the total count to be consistent with non-generic items.
            CompressedIntegralTypes.WriteCompressedInt(ref writer, OuterType);
            int numItems = Count();
            writer.Write((byte)numItems);
            if (InnerType1 == -1)
                return;
            CompressedIntegralTypes.WriteCompressedInt(ref writer, InnerType1);
            if (InnerType2 == -1)
                return;
            CompressedIntegralTypes.WriteCompressedInt(ref writer, InnerType2);
            if (InnerType3 == -1)
                return;
            CompressedIntegralTypes.WriteCompressedInt(ref writer, InnerType3);
            if (InnerType4 == -1)
                return;
            CompressedIntegralTypes.WriteCompressedInt(ref writer, InnerType4);
            if (FurtherInnerTypeIDs != null)
                for (int i = 0; i < FurtherInnerTypeIDs.Count; i++)
                {
                    CompressedIntegralTypes.WriteCompressedInt(ref writer, FurtherInnerTypeIDs[i]);
                }
        }

        public override int GetHashCode()
        {
            int res = 0x2D2816FE;
            if (OuterType != -1)
                res = res * 31 + OuterType;
            if (InnerType1 != -1)
                res = res * 31 + InnerType1;
            if (InnerType2 != -1)
                res = res * 31 + InnerType2;
            if (InnerType3 != -1)
                res = res * 31 + InnerType3;
            if (InnerType4 != -1)
                res = res * 31 + InnerType4;
            if (FurtherInnerTypeIDs != null)
                foreach (var item in FurtherInnerTypeIDs)
                {
                    res = res * 31 + (item.GetHashCode());
                }
            return res;
        }

        public override bool Equals(object obj)
        {
            if (obj is LazinatorGenericIDType other)
            {
                if (IsEmpty && other.IsEmpty)
                    return true;
                if (IsEmpty != other.IsEmpty)
                    return false;
                if (InnerType1 == -1)
                    return OuterType == other.OuterType && InnerType1 == other.InnerType1;
                if (InnerType2 == -1)
                    return OuterType == other.OuterType && InnerType1 == other.InnerType1 && InnerType2 == other.InnerType2;
                if (InnerType3 == -1)
                    return OuterType == other.OuterType && InnerType1 == other.InnerType1 && InnerType2 == other.InnerType2 && InnerType3 == other.InnerType3;
                if (InnerType4 == -1 || (FurtherInnerTypeIDs == null && other.FurtherInnerTypeIDs == null))
                    return OuterType == other.OuterType && InnerType1 == other.InnerType1 && InnerType2 == other.InnerType2 && InnerType3 == other.InnerType3 && InnerType4 == other.InnerType4;
                if ((FurtherInnerTypeIDs == null) != (other.FurtherInnerTypeIDs == null))
                    return false;
                return OuterType == other.OuterType && InnerType1 == other.InnerType1 && InnerType2 == other.InnerType2 && InnerType3 == other.InnerType3 && InnerType4 == other.InnerType4 && FurtherInnerTypeIDs.SequenceEqual(other.FurtherInnerTypeIDs);
            }
            return false;
        }

        /// <summary>
        /// A container with a cached LazinatorGenericIDType for each class. This allows us to avoid allocating a new LazinatorGenericIDType each time we access the LazinatorGenericID property.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        static class CachedPerType<T>
        {
            public static LazinatorGenericIDType? GenericIDType;
        }

        public static LazinatorGenericIDType GetCachedForType<T>(Func<LazinatorGenericIDType> func)
        {
            LazinatorGenericIDType? t = CachedPerType<T>.GenericIDType;
            if (t == null)
            {
                t = func();
                CachedPerType<T>.GenericIDType = t;
            }
            return t.Value;
        }
    }
}
