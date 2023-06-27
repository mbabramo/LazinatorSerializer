using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lazinator.Buffers
{
    public struct MemoryBlockID
    {
        private int _MemoryBlockID;
        
        public MemoryBlockID(int id)
        {
            _MemoryBlockID = id;
        }

        public int GetIntID() => _MemoryBlockID;

        public MemoryBlockID Next() => new MemoryBlockID(_MemoryBlockID + 1);

        public static bool operator <(MemoryBlockID l, MemoryBlockID f) => l._MemoryBlockID < f._MemoryBlockID;
        public static bool operator >(MemoryBlockID l, MemoryBlockID f) => l._MemoryBlockID > f._MemoryBlockID;
        public static bool operator <=(MemoryBlockID l, MemoryBlockID f) => l._MemoryBlockID <= f._MemoryBlockID;
        public static bool operator >=(MemoryBlockID l, MemoryBlockID f) => l._MemoryBlockID >= f._MemoryBlockID;

        public static bool operator ==(MemoryBlockID l, MemoryBlockID f) => l._MemoryBlockID == f._MemoryBlockID;
        public static bool operator !=(MemoryBlockID l, MemoryBlockID f) => l._MemoryBlockID != f._MemoryBlockID;

        public override string ToString()
        {
            return _MemoryBlockID.ToString();
        }

        public override bool Equals([NotNullWhen(true)] object obj)
        {
            return ((MemoryBlockID)obj)._MemoryBlockID == _MemoryBlockID;
        }
        public override int GetHashCode() => _MemoryBlockID.GetHashCode();
    }
}
