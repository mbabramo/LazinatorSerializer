using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lazinator.Buffers
{
    public readonly struct MemoryBlockID
    {
        public readonly int AsInt { get; }
        
        public MemoryBlockID(int asInt)
        {
            AsInt = asInt;
        }

        public MemoryBlockID Next() => new MemoryBlockID(AsInt + 1);

        public static bool operator <(MemoryBlockID l, MemoryBlockID f) => l.AsInt < f.AsInt;
        public static bool operator >(MemoryBlockID l, MemoryBlockID f) => l.AsInt > f.AsInt;
        public static bool operator <=(MemoryBlockID l, MemoryBlockID f) => l.AsInt <= f.AsInt;
        public static bool operator >=(MemoryBlockID l, MemoryBlockID f) => l.AsInt >= f.AsInt;

        public static bool operator ==(MemoryBlockID l, MemoryBlockID f) => l.AsInt == f.AsInt;
        public static bool operator !=(MemoryBlockID l, MemoryBlockID f) => l.AsInt != f.AsInt;

        public override string ToString()
        {
            return AsInt.ToString();
        }

        public override bool Equals([NotNullWhen(true)] object obj)
        {
            return ((MemoryBlockID)obj).AsInt == AsInt;
        }
        public override int GetHashCode() => AsInt.GetHashCode();
    }
}
