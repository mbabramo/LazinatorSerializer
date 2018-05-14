using System;
using Lazinator.Attributes;

namespace LazinatorTests.Examples
{
    public partial class ExampleChild : IExampleChild
    {
        public ExampleChild()
        {
        }

        protected bool Equals(ExampleChild other)
        {
            return _MyLong == other._MyLong && _MyShort == other._MyShort;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((ExampleChild) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (_MyLong.GetHashCode() * 397) ^ _MyShort.GetHashCode();
            }
        }
    }
}
