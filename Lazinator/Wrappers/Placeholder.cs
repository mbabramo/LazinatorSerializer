using System;

namespace Lazinator.Wrappers
{
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
