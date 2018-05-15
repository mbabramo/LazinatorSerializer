//73dec52c-0003-0ded-b091-4ffe9e1d41fb
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by the Lazinator tool, version 0.1.0.25
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

namespace LazinatorTests.Examples.Abstract
{
    public partial class Abstract1 : ILazinator
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
        
        internal int _IntList1_ByteIndex;
        internal virtual int _IntList1_ByteLength { get; }
        
        internal bool _String1_Accessed = false;
        public abstract string String1
        {
            get;
            set;
        }
        internal bool _IntList1_Accessed = false;
        public abstract System.Collections.Generic.List<int> IntList1
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
