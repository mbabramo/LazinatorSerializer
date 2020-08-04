namespace Lazinator.Wrappers
{
    /// <summary>
    /// An abstract class for a Lazinator wrapper
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract partial class WAbstract<T> : IWAbstract<T>
    {
        public override string ToString()
        {
            return Wrapped?.ToString() ?? "";
        }

        public override int GetHashCode()
        {
            return Wrapped.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            var other = (WAbstract<T>)obj;
            if (Wrapped == null)
                return other.Wrapped == null;
            return Wrapped.Equals(other.Wrapped);
        }

        public bool Equals(T other)
        {
            if (Wrapped == null)
                return other == null;
            return Wrapped.Equals(other);
        }

        public bool Equals(WAbstract<T> other)
        {
            if (Wrapped == null)
                return other.Wrapped == null;
            return Wrapped.Equals(other.Wrapped);
        }

    }
}
