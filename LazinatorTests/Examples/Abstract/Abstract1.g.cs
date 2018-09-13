//4e6ba57f-850c-41df-9da9-e69c06cd0d64
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by the Lazinator tool, version 0.1.0.220
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
    using System.Linq;
    using System.Runtime.InteropServices;
    using static Lazinator.Core.LazinatorUtilities;
    
    [Autogenerated]
    public partial class Abstract1 : ILazinator
    {
        public bool IsStruct => false;
        
        /* Abstract declarations */
        public abstract LazinatorParentsCollection LazinatorParents { get; set; }
        
        public abstract int Deserialize();
        
        public abstract LazinatorMemory SerializeLazinator(IncludeChildrenMode includeChildrenMode, bool verifyCleanness, bool updateStoredBuffer);
        
        protected abstract LazinatorMemory EncodeToNewBuffer(IncludeChildrenMode includeChildrenMode, bool verifyCleanness, bool updateStoredBuffer);
        
        public abstract ILazinator CloneLazinator(IncludeChildrenMode includeChildrenMode = IncludeChildrenMode.IncludeAllChildren, bool updateStoredBuffer = false);
        
        public abstract bool HasChanged
        {
            get;
            set;
        }
        
        public abstract bool IsDirty
        {
            get;
            set;
        }
        
        public abstract bool DescendantHasChanged
        {
            get;
            set;
        }
        
        public abstract bool DescendantIsDirty
        {
            get;
            set;
        }
        
        public abstract IEnumerable<ILazinator> EnumerateLazinatorNodes(Func<ILazinator, bool> matchCriterion, bool stopExploringBelowMatch, Func<ILazinator, bool> exploreCriterion, bool exploreOnlyDeserializedChildren, bool enumerateNulls);
        public abstract IEnumerable<(string propertyName, ILazinator descendant)> EnumerateLazinatorDescendants(Func<ILazinator, bool> matchCriterion, bool stopExploringBelowMatch, Func<ILazinator, bool> exploreCriterion, bool exploreOnlyDeserializedChildren, bool enumerateNulls);
        public abstract IEnumerable<(string propertyName, object descendant)> EnumerateNonLazinatorProperties();
        
        public abstract void DeserializeLazinator(LazinatorMemory serializedBytes);
        
        public abstract LazinatorMemory LazinatorMemoryStorage
        {
            get;
            set;
        }
        
        protected abstract ReadOnlyMemory<byte> LazinatorObjectBytes
        {
            get;
        }
        
        public abstract void EnsureLazinatorMemoryUpToDate();
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
        protected abstract LazinatorGenericIDType _LazinatorGenericID { get; set; }
        protected virtual bool ContainsOpenGenericParameters => false;
        public abstract LazinatorGenericIDType LazinatorGenericID { get; set; }
        public abstract int LazinatorObjectVersion { get; set; }
        public abstract void ConvertFromBytesAfterHeader(IncludeChildrenMode includeChildrenMode, int serializedVersionNumber, ref int bytesSoFar);
        public abstract void SerializeExistingBuffer(ref BinaryBufferWriter writer, IncludeChildrenMode includeChildrenMode, bool verifyCleanness, bool updateStoredBuffer);
        protected abstract void WritePropertiesIntoBuffer(ref BinaryBufferWriter writer, IncludeChildrenMode includeChildrenMode, bool verifyCleanness, bool updateStoredBuffer, bool includeUniqueID);
        protected abstract void ResetAccessedProperties();
        
    }
}
