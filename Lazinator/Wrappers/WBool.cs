namespace Lazinator.Wrappers
{
    /// <summary>
    /// A Lazinator wrapper for a bool. A wrapper can be used as a generic type where the unwrapped object cannot be.
    /// </summary>
    public partial struct WBool : IWBool
    {
        public bool HasValue => true;

        public WBool(bool x) : this()
        {
            WrappedValue = x;
        }

        [System.Diagnostics.DebuggerStepThrough]
        public static implicit operator WBool(bool x)
        {
            return new WBool(x);
        }

        [System.Diagnostics.DebuggerStepThrough]
        public static implicit operator bool(WBool x)
        {
            return x.WrappedValue;
        }

        public override string ToString()
        {
            return WrappedValue.ToString();
        }

        public override int GetHashCode()
        {
            return WrappedValue.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (obj is bool v)
                return WrappedValue == v;
            else if (obj is WBool w)
                return WrappedValue == w.WrappedValue;
            return false;
        }
    }
}
