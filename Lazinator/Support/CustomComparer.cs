﻿using System;
using System.Collections.Generic;

namespace Lazinator.Support
{
    /// <summary>
    /// A comparer initialized by passing a function to the constructor.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [Serializable]
    public struct CustomComparer<T> : IComparer<T>
    {
        public readonly bool Initialized;
        private readonly Func<T, T, int> _comparer;

        public CustomComparer(Func<T, T, int> comparer)
        {
            _comparer = comparer ?? throw new ArgumentNullException("comparer");
            Initialized = true;
        }

        public int Compare(T x, T y)
        {
            return _comparer(x, y);
        }
    }
}
