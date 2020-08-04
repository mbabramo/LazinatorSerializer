﻿using System;

namespace Lazinator.Wrappers
{
    /// <summary>
    /// A Lazinator wrapper for an unsigned long. A wrapper can be used as a generic type where the unwrapped object cannot be.
    /// </summary>
    public partial struct WUlong : IWUlong, IComparable, IComparable<ulong>, IEquatable<ulong>, IComparable<WUlong>, IEquatable<WUlong>
    {
        public bool HasValue => true;

        public WUlong(ulong x) : this()
        {
            WrappedValue = x;
        }

        [System.Diagnostics.DebuggerStepThrough]
        public static implicit operator WUlong(ulong x)
        {
            return new WUlong(x);
        }

        [System.Diagnostics.DebuggerStepThrough]
        public static implicit operator ulong(WUlong x)
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
            if (obj is ulong v)
                return WrappedValue == v;
            else if (obj is WUlong w)
                return WrappedValue == w.WrappedValue;
            return false;
        }

        public int CompareTo(object obj)
        {
            if (obj == null)
                return 1;
            if (obj is WLong other)
                return CompareTo(other);
            if (obj is long b)
                return CompareTo(b);
            throw new NotImplementedException();
        }

        public int CompareTo(ulong other)
        {
            return WrappedValue.CompareTo(other);
        }

        public bool Equals(ulong other)
        {
            return WrappedValue.Equals(other);
        }

        public int CompareTo(WUlong other)
        {
            return WrappedValue.CompareTo(other.WrappedValue);
        }

        public bool Equals(WUlong other)
        {
            return WrappedValue.Equals(other.WrappedValue);
        }
    }
}
