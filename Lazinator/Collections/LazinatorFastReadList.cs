using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Lazinator.Support;
using Lazinator.Core;
using System.Runtime.InteropServices;

namespace Lazinator.Collections
{
    /// <summary>
    /// A list of primitive values that can be indexed without deserialization if the list is not changed. The AsList property provides an underlying List, which can be mutated. 
    /// </summary>
    /// <typeparam name="T">A primitive value type (such as int, float, char, etc.) Note that using numeric types will work only on little endian processors. </typeparam>
    public sealed partial class LazinatorFastReadList<T> : ILazinatorFastReadList, ILazinator where T : struct 
    {
        //TODO: Do conversions if necessary in the ReadOnly property and in ConvertFromList if on a big-endian processor.
        //   [NonSerialized]
        //bool _IsBigEndian = !BitConverter.IsLittleEndian;

        public ReadOnlySpan<T> ReadOnly
        {
            get => MemoryMarshal.Cast<byte, T>(ReadOnlyBytes);
            set => ReadOnlyBytes = MemoryMarshal.Cast<T, byte>(value);
        }

        // The key feature of this list is that if no changes are made to it, we can read directly from the span.
        public T this[int index]
        {
            get { return IsDirty ? AsList[index] : ReadOnly[index]; }
        }

        public int Length
        {
            get { return IsDirty ? AsList.Count : ReadOnly.Length; }
        }
        
        private List<T> _AsList;
        public List<T> AsList
        {
            get
            {
                if (!IsDirty || _AsList == null)
                    ConvertToList();
                return _AsList;
            }
            set
            {
                if (value == null)
                    _AsList = new List<T>(); // not allowing null Lists
                else
                    _AsList = value;
                IsDirty = true;
            }
        }

        private void ConvertToList()
        {
            var span = ReadOnly;
            _AsList = new List<T>();
            for (int i = 0; i < span.Length; i++)
                _AsList.Add(span[i]);
            IsDirty = true;
        }

        public void PreSerialization(bool verifyCleanness, bool updateStoredBuffer)
        {
            if (IsDirty)
                ConvertFromList();
        }

        private void ConvertFromList()
        {
            T[] items = new T[_AsList.Count];
            Memory<T> m = new Memory<T>(items);
            Span<T> span = m.Span;
            for (int i = 0; i < _AsList.Count; i++)
                span[i] = _AsList[i];
            ReadOnly = span;
        }

        // DEBUG
        public IEnumerable<ILazinator> GetDirtyNodes(Func<ILazinator, bool> exploreCriterion, Func<ILazinator, bool> yieldCriterion, bool onlyHighestDirty)
        {
            throw new NotImplementedException();
        }
    }
}
