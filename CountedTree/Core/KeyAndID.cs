using System;
using System.Linq;
using CountedTree.ByteUtilities;
using Lazinator.Core;
using static CountedTree.ByteUtilities.ByteArrayConversions;

namespace CountedTree.Core
{

    public partial struct KeyAndID<TKey> : IComparable, IKeyAndID<TKey> where TKey : struct, ILazinator,
          IComparable,
          IComparable<TKey>,
          IEquatable<TKey>
    {

        public KeyAndID(TKey key, uint id) : this()
        {
            Key = key;
            ID = id;
            string x = null; // DEBUG
            x = x ?? "hello";
        }

        public KeyAndID<TKey> WithKey(TKey key)
        {
            return new KeyAndID<TKey>(key, ID);
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
                hash = hash * 17 + Key.GetHashCode();
                hash = hash * 31 + ID.GetHashCode();
                return hash;
            }
        }

        public int CompareTo(object obj)
        {
            KeyAndID<TKey> other = (KeyAndID<TKey>) obj;
            if (other == null)
                return 1;
            int keyCompare = Key.CompareTo(other.Key);
            if (keyCompare != 0)
                return keyCompare;
            int idCompare = ID.CompareTo(other.ID);
            return idCompare;
        }

        public static int Compare(KeyAndID<TKey> left, KeyAndID<TKey> right)
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
            return $"<{Key.ToString()}, {ID}>";
        }

        public KeyAndID<TKey> Clone()
        {
            return new KeyAndID<TKey>(Key, ID);
        }

        public static bool operator ==(KeyAndID<TKey> left, KeyAndID<TKey> right)
        {
            if (object.ReferenceEquals(left, null))
            {
                return object.ReferenceEquals(right, null);
            }
            return left.Equals(right);
        }
        public static bool operator !=(KeyAndID<TKey> left, KeyAndID<TKey> right)
        {
            return !(left == right);
        }
        public static bool operator <(KeyAndID<TKey> left, KeyAndID<TKey> right)
        {
            return (Compare(left, right) < 0);
        }
        public static bool operator >(KeyAndID<TKey> left, KeyAndID<TKey> right)
        {
            return (Compare(left, right) > 0);
        }
        public static bool operator <=(KeyAndID<TKey> left, KeyAndID<TKey> right)
        {
            return (Compare(left, right) <= 0);
        }
        public static bool operator >=(KeyAndID<TKey> left, KeyAndID<TKey> right)
        {
            return (Compare(left, right) >= 0);
        }
        public bool IsInRange(KeyAndID<TKey>? firstExclusive, KeyAndID<TKey>? lastInclusive)
        {
            if (firstExclusive == null && lastInclusive == null)
                return true;
            if (firstExclusive == null)
                return this <= lastInclusive;
            if (lastInclusive == null)
                return this > firstExclusive;
            return this > firstExclusive && this <= lastInclusive;
        }
    }
}
