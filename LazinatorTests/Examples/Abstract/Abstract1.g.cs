//460c96da-1b22-dd28-b674-134cb42fe60e
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by the Lazinator tool, version 0.1.0.36
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
        
        public abstract void Deserialize();
        
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
        
        public abstract void MarkHierarchyClean();
        
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
        
        protected int _IntList1_ByteIndex;
        protected virtual int _IntList1_ByteLength { get; }
        
        protected bool _String1_Accessed = false;
        public abstract string String1
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
        public abstract int LazinatorObjectVersion { get; set; }
        public abstract void ConvertFromBytesAfterHeader(IncludeChildrenMode includeChildrenMode, int serializedVersionNumber, ref int bytesSoFar);
        public abstract void SerializeExistingBuffer(BinaryBufferWriter writer, IncludeChildrenMode includeChildrenMode, bool verifyCleanness);
    }
}
