//5e74d4ba-4fce-bde1-1293-67df07d19423
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by the Lazinator tool, version 0.1.0.20
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Buffers;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using Lazinator.Buffers; 
using Lazinator.Collections;
using Lazinator.Core; 
using static Lazinator.Core.LazinatorUtilities;
using Lazinator.Exceptions;
using Lazinator.Support;
using Lazinator.Wrappers;

namespace Lazinator.Wrappers
{
    public partial class LazinatorWrapperAbstract<T> : ILazinator
    {
        /* Boilerplate for abstract ILazinator object */
        public abstract ILazinator LazinatorParentClass { get; set; }
        
        public abstract void Deserialize();
        
        public abstract MemoryInBuffer SerializeNewBuffer(IncludeChildrenMode includeChildrenMode, bool verifyCleanness);
        
        protected internal abstract MemoryInBuffer EncodeToNewBuffer(IncludeChildrenMode includeChildrenMode, bool verifyCleanness);
        
        public abstract ILazinator CloneLazinator();
        
        public abstract ILazinator CloneLazinator(IncludeChildrenMode includeChildrenMode);
        
        public abstract bool IsDirty
        {
            get;
            set;
        }
        
        public abstract InformParentOfDirtinessDelegate InformParentOfDirtinessDelegate { get; set; }
        public abstract void InformParentOfDirtiness();
        
        public abstract bool DescendantIsDirty
        {
            get;
            set;
        }
        
        public abstract DeserializationFactory DeserializationFactory { get; set; }
        
        public abstract MemoryInBuffer HierarchyBytes
        {
            get;
            set;
        }
        
        public abstract ReadOnlyMemory<byte> LazinatorObjectBytes
        {
            get;
            set;
        }
        
        public abstract void LazinatorConvertToBytes();
        public abstract uint GetBinaryHashCode32();
        public abstract ulong GetBinaryHashCode64();
        
        /* Field boilerplate */
        
        public abstract T Value
        {
            get;
            set;
        }
        public abstract int LazinatorUniqueID { get; }
        public abstract int LazinatorObjectVersion { get; set; }
        public abstract void ConvertFromBytesAfterHeader(IncludeChildrenMode includeChildrenMode, int serializedVersionNumber, ref int bytesSoFar);
        public abstract void SerializeExistingBuffer(BinaryBufferWriter writer, IncludeChildrenMode includeChildrenMode, bool verifyCleanness);
    }
}
