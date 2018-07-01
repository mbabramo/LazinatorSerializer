//921786bc-b6c2-e472-89c4-13aa1beb8402
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by the Lazinator tool, version 0.1.0.181
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
    using System.Linq;
    using System.Runtime.InteropServices;
    using static Lazinator.Core.LazinatorUtilities;
    
    [Autogenerated]
    public partial class AbstractGeneric1<T> : ILazinator
    {
        public bool IsStruct => false;
        
        /* Abstract declarations */
        public abstract LazinatorParentsCollection LazinatorParents { get; set; }
        
        public abstract int Deserialize();
        
        public abstract MemoryInBuffer SerializeNewBuffer(IncludeChildrenMode includeChildrenMode, bool verifyCleanness);
        
        protected abstract MemoryInBuffer EncodeToNewBuffer(IncludeChildrenMode includeChildrenMode, bool verifyCleanness);
        
        public abstract ILazinator CloneLazinator();
        
        public abstract ILazinator CloneLazinator(IncludeChildrenMode includeChildrenMode);
        
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
        
        protected int _MyT_ByteIndex;
        protected virtual int _MyT_ByteLength { get; }
        
        protected bool _MyEnumWithinAbstractGeneric_Accessed = false;
        public abstract global::LazinatorTests.Examples.Abstract.AbstractGeneric1<T>.EnumWithinAbstractGeneric MyEnumWithinAbstractGeneric
        {
            get;
            set;
        }
        protected bool _MyEnumWithinAbstractGeneric2_Accessed = false;
        public abstract global::LazinatorTests.Examples.Abstract.AbstractGeneric1<int>.EnumWithinAbstractGeneric MyEnumWithinAbstractGeneric2
        {
            get;
            set;
        }
        protected bool _MyUnofficialInt_Accessed = false;
        public abstract int MyUnofficialInt
        {
            get;
            set;
        }
        protected bool _MyT_Accessed = false;
        public virtual T MyT
        {
            get;
            set;
        }
        public abstract int LazinatorUniqueID { get; }
        protected abstract LazinatorGenericIDType _LazinatorGenericID { get; set; }
        protected virtual bool ContainsOpenGenericParameters => true;
        public abstract LazinatorGenericIDType LazinatorGenericID { get; set; }
        public abstract int LazinatorObjectVersion { get; set; }
        public abstract void ConvertFromBytesAfterHeader(IncludeChildrenMode includeChildrenMode, int serializedVersionNumber, ref int bytesSoFar);
        public abstract void SerializeExistingBuffer(ref BinaryBufferWriter writer, IncludeChildrenMode includeChildrenMode, bool verifyCleanness, bool updateStoredBuffer);
        protected abstract void WritePropertiesIntoBuffer(ref BinaryBufferWriter writer, IncludeChildrenMode includeChildrenMode, bool verifyCleanness, bool updateStoredBuffer, bool includeUniqueID);
        protected abstract void ResetAccessedProperties();
        
    }
}
