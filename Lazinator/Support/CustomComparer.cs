using System;
using System.Collections.Generic;
using System.Text;

namespace Lazinator.Support
{
    [Serializable]
    public class CustomComparer<T> : IComparer<T>
    {
        private readonly Func<T, T, int> _comparer;

        public CustomComparer(Func<T, T, int> comparer)
        {
            _comparer = comparer ?? throw new ArgumentNullException("comparer");
        }

        public int Compare(T x, T y)
        {
            return _comparer(x, y);
        }
    }
}
