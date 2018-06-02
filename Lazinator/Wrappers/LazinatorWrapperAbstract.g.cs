//a5f927ab-aa24-98e9-50c2-025488df2b2e
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by the Lazinator tool, version 0.1.0.72
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Lazinator.Wrappers
{
    using Lazinator.Attributes;
    using Lazinator.Buffers;
    using Lazinator.Core;
    using Lazinator.Exceptions;
    using Lazinator.Support;
    using System;
    using System.Buffers;
    using System.Diagnostics;
    using System.IO;
    using System.Runtime.InteropServices;
    using static Lazinator.Core.LazinatorUtilities;
    
    [Autogenerated]
    public partial class LazinatorWrapperAbstract<T> : ILazinator
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
        
        public abstract void MarkHierarchyClean();
        
        public abstract DeserializationFactory DeserializationFactory { get; set; }
        
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
        
        /* Field definitions */
        
        protected int _Wrapped_ByteIndex;
        protected virtual int _Wrapped_ByteLength { get; }
        
        protected bool _Wrapped_Accessed = false;
        public virtual T Wrapped
        {
            get;
            set;
        }
        public abstract int LazinatorUniqueID { get; }
        public abstract int LazinatorObjectVersion { get; set; }
        public abstract void ConvertFromBytesAfterHeader(IncludeChildrenMode includeChildrenMode, int serializedVersionNumber, ref int bytesSoFar);
        public abstract void SerializeExistingBuffer(BinaryBufferWriter writer, IncludeChildrenMode includeChildrenMode, bool verifyCleanness);
        protected abstract void WritePropertiesIntoBuffer(BinaryBufferWriter writer, IncludeChildrenMode includeChildrenMode, bool verifyCleanness, bool includeUniqueID);
        protected abstract void ResetAccessedProperties();
    }
}
