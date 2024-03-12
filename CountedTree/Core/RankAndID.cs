using System;

namespace CountedTree.Core
{
    public partial class RankAndID : IComparable, IRankAndID
    {

        public RankAndID()
        {
        }

        public RankAndID(uint rank, uint id) : this()
        {
            Rank = rank;
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
                hash = hash * 91 + ID.GetHashCode();
                return hash;
            }
        }

        public int CompareTo(object obj)
        {
            RankAndID other = obj as RankAndID;
            if (other == null)
                return 1;
            int rankCompare = Rank.CompareTo(other.Rank);
            if (rankCompare != 0)
                return rankCompare;
            int idCompare = ID.CompareTo(other.ID);
            return idCompare;
        }

        public static int Compare(RankAndID left, RankAndID right)
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
            return $"<{Rank.ToString()}, {ID}>";
        }

        public string ToString(string format, IFormatProvider formatProvider)
        {
            return $"<{Rank.ToString(formatProvider)}, {ID}>";
        }

        public RankAndID Clone()
        {
            return new RankAndID(Rank, ID);
        }

        public static bool operator ==(RankAndID left, RankAndID right)
        {
            if (object.ReferenceEquals(left, null))
            {
                return object.ReferenceEquals(right, null);
            }
            return left.Equals(right);
        }
        public static bool operator !=(RankAndID left, RankAndID right)
        {
            return !(left == right);
        }
        public static bool operator <(RankAndID left, RankAndID right)
        {
            return (Compare(left, right) < 0);
        }
        public static bool operator >(RankAndID left, RankAndID right)
        {
            return (Compare(left, right) > 0);
        }
        public static bool operator <=(RankAndID left, RankAndID right)
        {
            return (Compare(left, right) <= 0);
        }
        public static bool operator >=(RankAndID left, RankAndID right)
        {
            return (Compare(left, right) >= 0);
        }
    }
}
