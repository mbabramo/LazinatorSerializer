using System;
using CountedTree.Core;
using Lazinator.Core;

namespace CountedTree.PendingChanges
{
    public partial class PendingChange<TKey> : IComparable, IPendingChange<TKey> where TKey : struct, ILazinator,
          IComparable,
          IComparable<TKey>,
          IEquatable<TKey>
    {
        public bool Delete => Operation == PendingChangeOperation.Deletion || Operation == PendingChangeOperation.DeletionValueUnknown;

        public PendingChange()
        {
        }

        public PendingChange(KeyAndID<TKey> item, bool delete)
        {
            Item = item;
            Operation = delete ? PendingChangeOperation.Deletion : PendingChangeOperation.Addition;
        }

        public override bool Equals(object obj)
        {
            PendingChange<TKey> other = obj as PendingChange<TKey>;
            if (other == null)
                return false;
            return Item.Equals(other.Item) && Delete == other.Delete;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 73;
                hash = hash * 177 + Item.GetHashCode();
                if (Delete)
                    hash = hash * 31;
                return hash;
            }
        }

        public PendingChange<TKey> Clone()
        {
            return new PendingChange<TKey>(Item.Clone(), Delete);
        }

        public override string ToString()
        {
            return $"{(Delete ? "X" : "")}{Item}";
        }

        public int CompareTo(object obj)
        {
            PendingChange<TKey> other = obj as PendingChange<TKey>;
            if (other == null)
                return 1;
            int itemCompare = Item.CompareTo(other.Item);
            if (itemCompare != 0)
                return itemCompare;
            int deleteCompare = Delete.CompareTo(other.Delete);
            return deleteCompare;
        }

        public static int Compare(PendingChange<TKey> left, PendingChange<TKey> right)
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
    }
}
