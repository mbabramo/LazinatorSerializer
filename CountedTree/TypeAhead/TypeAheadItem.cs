using Lazinator.Core;
using System;
using System.Diagnostics.CodeAnalysis;

namespace CountedTree.TypeAhead
{
    public partial class TypeAheadItem<R> : ITypeAheadItem<R>, IComparable, IComparable<ITypeAheadItem<R>> where R : ILazinator,
              IComparable,
              IComparable<R>,
              IEquatable<R>
    {

        public TypeAheadItem(R searchResult, float popularity)
        {
            SearchResult = searchResult;
            Popularity = popularity;
        }

        public override bool Equals(object obj)
        {
            return CompareTo(obj) == 0;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 19;
                hash = hash * 23 + SearchResult.GetHashCode();
                hash = hash * 29 + Popularity.GetHashCode();
                return hash;
            }
        }

        public int CompareTo(object obj)
        {
            TypeAheadItem<R> other = obj as TypeAheadItem<R>;
            return CompareTo(other);
        }

        public int CompareTo([AllowNull] ITypeAheadItem<R> other)
        {
            if (other == null)
                return 1;
            int popularityCompare = Popularity.CompareTo(other.Popularity);
            if (popularityCompare != 0)
                return 0 - popularityCompare; // more popular items come first
            int resultCompare = SearchResult.CompareTo(other.SearchResult);
            return resultCompare;
        }

        public static int Compare(TypeAheadItem<R> left, TypeAheadItem<R> right)
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
            return $"<{SearchResult.ToString()}, {Popularity}>";
        }
    }
}
