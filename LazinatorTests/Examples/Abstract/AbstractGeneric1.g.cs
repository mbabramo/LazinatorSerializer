//aad2b4ed-7bc6-e4ff-cb4e-1470dcc8b769
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by the Lazinator tool, version 0.1.0.384
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

#nullable disable
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
        
        /* Property definitions */
        
        protected int _MyT_ByteIndex;
        protected virtual int _MyT_ByteLength { get; }
        
        
        protected bool _MyEnumWithinAbstractGeneric_Accessed = false;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public abstract global::LazinatorTests.Examples.Abstract.AbstractGeneric1<T>.EnumWithinAbstractGeneric MyEnumWithinAbstractGeneric
        {
            get;
            set;
        }
        
        protected bool _MyEnumWithinAbstractGeneric2_Accessed = false;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public abstract global::LazinatorTests.Examples.Abstract.AbstractGeneric1<global::System.Int32>.EnumWithinAbstractGeneric MyEnumWithinAbstractGeneric2
        {
            get;
            set;
        }
        
        protected bool _MyUnofficialInt_Accessed = false;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public abstract int MyUnofficialInt
        {
            get;
            set;
        }
        
        protected bool _MyT_Accessed = false;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public virtual T MyT
        {
            get;
            set;
        }
        /* Abstract declarations */
        public abstract LazinatorParentsCollection LazinatorParents { get; set; }
        
        public abstract int Deserialize();
        
        public abstract LazinatorMemory SerializeLazinator(IncludeChildrenMode includeChildrenMode, bool verifyCleanness, bool updateStoredBuffer);
        
        public abstract ILazinator CloneLazinator(IncludeChildrenMode includeChildrenMode = IncludeChildrenMode.IncludeAllChildren, CloneBufferOptions cloneBufferOptions = CloneBufferOptions.IndependentBuffers);
        
        public abstract ILazinator AssignCloneProperties(ILazinator clone, IncludeChildrenMode includeChildrenMode);
        
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
        
        public abstract bool NonBinaryHash32
        {
            get;
        }
        public abstract IEnumerable<ILazinator> EnumerateLazinatorNodes(Func<ILazinator, bool> matchCriterion, bool stopExploringBelowMatch, Func<ILazinator, bool> exploreCriterion, bool exploreOnlyDeserializedChildren, bool enumerateNulls);
        public abstract IEnumerable<(string propertyName, ILazinator descendant)> EnumerateLazinatorDescendants(Func<ILazinator, bool> matchCriterion, bool stopExploringBelowMatch, Func<ILazinator, bool> exploreCriterion, bool exploreOnlyDeserializedChildren, bool enumerateNulls);
        public abstract IEnumerable<(string propertyName, object descendant)> EnumerateNonLazinatorProperties();
        public abstract ILazinator ForEachLazinator(Func<ILazinator, ILazinator> changeFunc, bool exploreOnlyDeserializedChildren, bool changeThisLevel);
        
        public abstract void DeserializeLazinator(LazinatorMemory serializedBytes);
        
        public abstract LazinatorMemory LazinatorMemoryStorage
        {
            get;
            set;
        }
        
        public abstract IncludeChildrenMode OriginalIncludeChildrenMode
        {
            get;
            set;
        }
        
        protected abstract ReadOnlyMemory<byte> LazinatorObjectBytes
        {
            get;
        }
        
        public abstract void UpdateStoredBuffer(ref BinaryBufferWriter writer, int startPosition, int length, IncludeChildrenMode includeChildrenMode, bool updateDeserializedChildren);
        public abstract void UpdateStoredBuffer();
        public abstract void FreeInMemoryObjects();
        public abstract int GetByteLength();
        
        public abstract int LazinatorUniqueID { get; }
        protected virtual bool ContainsOpenGenericParameters => true;
        public abstract LazinatorGenericIDType LazinatorGenericID { get; }
        public abstract int LazinatorObjectVersion { get; set; }
        public abstract void ConvertFromBytesAfterHeader(IncludeChildrenMode includeChildrenMode, int serializedVersionNumber, ref int bytesSoFar);
        public abstract void SerializeExistingBuffer(ref BinaryBufferWriter writer, IncludeChildrenMode includeChildrenMode, bool verifyCleanness, bool updateStoredBuffer);
        protected abstract LazinatorMemory EncodeToNewBuffer(IncludeChildrenMode includeChildrenMode, bool verifyCleanness, bool updateStoredBuffer);
        protected abstract void UpdateDeserializedChildren(ref BinaryBufferWriter writer, int startPosition);
        protected abstract void WritePropertiesIntoBuffer(ref BinaryBufferWriter writer, IncludeChildrenMode includeChildrenMode, bool verifyCleanness, bool updateStoredBuffer, bool includeUniqueID);
        
    }
}
