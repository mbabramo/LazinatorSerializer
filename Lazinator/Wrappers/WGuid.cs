using System;

namespace Lazinator.Wrappers
{
    public partial struct WGuid : IWGuid
    {
        public bool HasValue => true;

        public WGuid(Guid x) : this()
        {
            WrappedValue = x;
        }

        [System.Diagnostics.DebuggerStepThrough]
        public static implicit operator WGuid(Guid x)
        {
            return new WGuid(x);
        }

        [System.Diagnostics.DebuggerStepThrough]
        public static implicit operator Guid(WGuid x)
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
            if (obj is Guid v)
                return WrappedValue == v;
            else if (obj is WGuid w)
                return WrappedValue == w.WrappedValue;
            return false;
        }
    }
}