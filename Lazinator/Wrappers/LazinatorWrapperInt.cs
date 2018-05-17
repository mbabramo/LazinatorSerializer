﻿using System;

namespace Lazinator.Wrappers
{
    public partial struct LazinatorWrapperInt : ILazinatorWrapperInt, IComparable, IComparable<int>, IEquatable<int>, IComparable<LazinatorWrapperInt>, IEquatable<LazinatorWrapperInt>
    {
        public bool HasValue => true;

        public LazinatorWrapperInt(int x)
        {
            _Value = x;
            _HierarchyBytes = default;
            _LazinatorObjectBytes = null;
            _IsDirty = true;
            OriginalIncludeChildrenMode = Core.IncludeChildrenMode.IncludeAllChildren;
            _DescendantIsDirty = false;
            LazinatorParentClass = null;
            InformParentOfDirtinessDelegate = null;
            DeserializationFactory = null;
        }

        public static implicit operator LazinatorWrapperInt(int x)
        {
            return new LazinatorWrapperInt(x);
        }

        public static implicit operator int(LazinatorWrapperInt x)
        {
            return x.Value;
        }

        public override string ToString()
        {
            return Value.ToString();
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (obj is int v)
                return Value == v;
            else if (obj is LazinatorWrapperInt w)
                return Value == w.Value;
            return false;
        }

        public int CompareTo(object obj)
        {
            return Value.CompareTo(obj);
        }

        public int CompareTo(int other)
        {
            return Value.CompareTo(other);
        }

        public bool Equals(int other)
        {
            return Value.Equals(other);
        }

        public int CompareTo(LazinatorWrapperInt other)
        {
            return Value.CompareTo(other.Value);
        }

        public bool Equals(LazinatorWrapperInt other)
        {
            return Value.Equals(other.Value);
        }
    }
}
