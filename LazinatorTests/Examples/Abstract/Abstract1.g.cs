//50e242e7-d78a-49f1-035b-f6e3db834a8d
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by the Lazinator tool, version 0.1.0.106
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace LazinatorTests.Examples.Abstract
{
    using Lazinator.Attributes;
    using Lazinator.Buffers;
    using Lazinator.Core;
    using Lazinator.Exceptions;
    using Lazinator.Support;
    using LazinatorTests.Examples;
    using System;
    using System.Buffers;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Runtime.InteropServices;
    using static Lazinator.Core.LazinatorUtilities;
    
    [Autogenerated]
    public partial class Abstract1 : ILazinator
    {
        /* Abstract declarations */
        public abstract ILazinator LazinatorParentClass { get; set; }
        
        public abstract int Deserialize();
        
        public abstract MemoryInBuffer SerializeNewBuffer(IncludeChildrenMode includeChildrenMode, bool verifyCleanness);
        
        protected abstract MemoryInBuffer EncodeToNewBuffer(IncludeChildrenMode includeChildrenMode, bool verifyCleanness);
        
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
        
        public abstract MemoryInBuffer HierarchyBytes
        {
            set;
        }
        
        public abstract ReadOnlyMemory<byte> LazinatorObjectBytes
        {
            get;
            set;
        }
        
        public abstract void LazinatorConvertToBytes();
        public abstract int GetByteLength();
        public abstract uint GetBinaryHashCode32();
        public abstract ulong GetBinaryHashCode64();
        public abstract Guid GetBinaryHashCode128();
        
        /* Property definitions */
        
        protected int _Example3_ByteIndex;
        protected int _IntList1_ByteIndex;
        protected virtual int _Example3_ByteLength => _IntList1_ByteIndex - _Example3_ByteIndex;
        protected virtual int _IntList1_ByteLength { get; }
        
        protected bool _String1_Accessed = false;
        public abstract string String1
        {
            get;
            set;
        }
        protected bool _Example3_Accessed = false;
        public abstract Example Example3
        {
            get;
            set;
        }
        protected bool _IntList1_Accessed = false;
        public abstract List<int> IntList1
        {
            get;
            set;
        }
        public abstract int LazinatorUniqueID { get; }
        protected abstract System.Collections.Generic.List<int> _LazinatorGenericID { get; set; }
        protected virtual bool ContainsOpenGenericParameters => false;
        public abstract System.Collections.Generic.List<int> LazinatorGenericID { get; set; }
        public abstract int LazinatorObjectVersion { get; set; }
        public abstract void ConvertFromBytesAfterHeader(IncludeChildrenMode includeChildrenMode, int serializedVersionNumber, ref int bytesSoFar);
        public abstract void SerializeExistingBuffer(BinaryBufferWriter writer, IncludeChildrenMode includeChildrenMode, bool verifyCleanness, bool updateStoredBuffer);
        protected abstract void WritePropertiesIntoBuffer(BinaryBufferWriter writer, IncludeChildrenMode includeChildrenMode, bool verifyCleanness, bool updateStoredBuffer, bool includeUniqueID);
        protected abstract void ResetAccessedProperties();
        
    }
}
