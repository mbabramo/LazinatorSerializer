using Lazinator.Core;
using System;

namespace CountedTree.Core
{
    public partial class RankKeyAndID<TKey> : IComparable, IRankKeyAndID<TKey> where TKey : struct, ILazinator,
          IComparable,
          IComparable<TKey>,
          IEquatable<TKey>
    {

        public RankKeyAndID()
        {
        }

        public RankKeyAndID(uint rank, TKey key, uint id)
        {
            Rank = rank;
            Key = key;
            ID = id;
        }

        public override bool Equals(object obj)
        {
            return CompareTo(obj) == 0;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 13;
                hash = hash * 37 + Rank.GetHashCode();
                hash = hash * 73 + Key.GetHashCode();
                hash = hash * 91 + ID.GetHashCode();
                return hash;
            }
        }

        internal RankAndID GetRankAndID()
        {
            return new RankAndID(Rank, ID);
        }

        public KeyAndID<TKey> GetKeyAndID()
        {
            return new KeyAndID<TKey>(Key, ID);
        }

        public int CompareTo(object obj)
        {
            RankKeyAndID<TKey> other = obj as RankKeyAndID<TKey>;
            if (other == null)
                return 1;
            int rankCompare = Rank.CompareTo(other.Rank);
            if (rankCompare != 0)
                return rankCompare;
            int keyCompare = Key.CompareTo(other.Key);
            if (keyCompare != 0)
                return keyCompare;
            int idCompare = ID.CompareTo(other.ID);
            return idCompare;
        }

        public static int Compare(RankKeyAndID<TKey> left, RankKeyAndID<TKey> right)
        {
            if (object.ReferenceEquals(left, right))
            {
                return 0;
            }
            if (object.ReferenceEquals(left, null))
            {
                return -1;
            }
            return left.CompareTo(right);
        }

        public override string ToString()
        {
            return $"<{Rank.ToString()}, {Key.ToString()}, {ID}>";
        }

        public string ToString(string format, IFormatProvider formatProvider)
        {
            return $"<{Rank.ToString(formatProvider)}, {Key.ToString()}, {ID}>";
        }

        public RankKeyAndID<TKey> Clone()
        {
            return new RankKeyAndID<TKey>(Rank, Key, ID);
        }

        public static bool operator ==(RankKeyAndID<TKey> left, RankKeyAndID<TKey> right)
        {
            if (object.ReferenceEquals(left, null))
            {
                return object.ReferenceEquals(right, null);
            }
            return left.Equals(right);
        }
        public static bool operator !=(RankKeyAndID<TKey> left, RankKeyAndID<TKey> right)
        {
            return !(left == right);
        }
        public static bool operator <(RankKeyAndID<TKey> left, RankKeyAndID<TKey> right)
        {
            return (Compare(left, right) < 0);
        }
        public static bool operator >(RankKeyAndID<TKey> left, RankKeyAndID<TKey> right)
        {
            return (Compare(left, right) > 0);
        }
        public static bool operator <=(RankKeyAndID<TKey> left, RankKeyAndID<TKey> right)
        {
            return (Compare(left, right) <= 0);
        }
        public static bool operator >=(RankKeyAndID<TKey> left, RankKeyAndID<TKey> right)
        {
            return (Compare(left, right) >= 0);
        }


        public const uint AnomalyPlaceholder = (uint)4000000003; // if there is an anomaly and more results are expected than we are returning, we will return extra items with this ID.
        public static RankKeyAndID<TKey> GetAnomalyPlaceholder()
        {
            return new RankKeyAndID<TKey>(0 /* really n/a */ , default(TKey), AnomalyPlaceholder);
        }
    }
}
