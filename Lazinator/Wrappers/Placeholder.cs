using System;

namespace Lazinator.Wrappers
{
    /// <summary>
    /// A struct containing no information. This is useful when a collection class requires a Lazinator type as a key, but you plan to use the same key for each item in the collection.
    /// </summary>
    public partial struct Placeholder : IPlaceholder, IComparable
    {
        public override int GetHashCode()
        {
            return 0;
        }

        public override bool Equals(object obj)
        {
            return obj is Placeholder;
        }

        public int CompareTo(object obj)
        {
            if (obj == null)
                return 1;
            if (obj is Placeholder other)
                return 0;
            throw new NotImplementedException();
        }

        public int CompareTo(Placeholder other)
        {
            return 0;
        }

        public bool Equals(Placeholder other)
        {
            return true;
        }
    }
}
