//d10c8bc6-fd2f-6004-9f04-558b1d67f7de
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by the Lazinator tool, version 0.1.0.397
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
    using static Lazinator.Buffers.WriteUncompressedPrimitives;
    using System;
    using System.Buffers;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Runtime.InteropServices;
    using static Lazinator.Core.LazinatorUtilities;
    
    [Autogenerated]
    public partial class AbstractGenericContainer<T> : ILazinator
    {
        public bool IsStruct => false;
        
        /* Property definitions */
        
        protected int _Item_ByteIndex;
        protected virtual int _Item_ByteLength { get; }
        
        
        protected bool _Item_Accessed = false;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public virtual IAbstractGeneric1<T> Item
        {
            get;
            set;
        }
        /* Abstract declarations */
        public abstract LazinatorParentsCollection LazinatorParents { get; set; }
        
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
        
        protected abstract void DeserializeLazinator(LazinatorMemory serializedBytes);
        
        protected abstract int Deserialize();
        
        public abstract void SerializeLazinator();
        public abstract LazinatorMemory SerializeLazinator(in LazinatorSerializationOptions options);
        
        public abstract ILazinator CloneLazinator(IncludeChildrenMode includeChildrenMode = IncludeChildrenMode.IncludeAllChildren, CloneBufferOptions cloneBufferOptions = CloneBufferOptions.IndependentBuffers);
        
        protected abstract ILazinator AssignCloneProperties(ILazinator clone, IncludeChildrenMode includeChildrenMode);
        
        
        public abstract IEnumerable<ILazinator> EnumerateLazinatorNodes(Func<ILazinator, bool> matchCriterion, bool stopExploringBelowMatch, Func<ILazinator, bool> exploreCriterion, bool exploreOnlyDeserializedChildren, bool enumerateNulls);
        public abstract IEnumerable<(string propertyName, ILazinator descendant)> EnumerateLazinatorDescendants(Func<ILazinator, bool> matchCriterion, bool stopExploringBelowMatch, Func<ILazinator, bool> exploreCriterion, bool exploreOnlyDeserializedChildren, bool enumerateNulls);
        public abstract IEnumerable<(string propertyName, object descendant)> EnumerateNonLazinatorProperties();
        public abstract ILazinator ForEachLazinator(Func<ILazinator, ILazinator> changeFunc, bool exploreOnlyDeserializedChildren, bool changeThisLevel);
        
        public abstract void UpdateStoredBuffer(ref BufferWriter writer, long startPosition, long length, IncludeChildrenMode includeChildrenMode, bool updateDeserializedChildren);
        public abstract void FreeInMemoryObjects();
        public abstract int LazinatorUniqueID { get; }
        protected virtual bool ContainsOpenGenericParameters => true;
        public abstract LazinatorGenericIDType LazinatorGenericID { get; }
        public abstract int LazinatorObjectVersion { get; set; }
        protected abstract int ConvertFromBytesAfterHeader(IncludeChildrenMode includeChildrenMode, int serializedVersionNumber, ref int bytesSoFar);
        protected abstract void ConvertFromBytesForPrimitiveProperties(ReadOnlySpan<byte> span, IncludeChildrenMode includeChildrenMode, int serializedVersionNumber, ref int bytesSoFar);
        protected abstract int ConvertFromBytesForChildLengths(ReadOnlySpan<byte> span, IncludeChildrenMode includeChildrenMode, int serializedVersionNumber, int indexOfFirstChild, ref int bytesSoFar);
        public abstract void SerializeToExistingBuffer(ref BufferWriter writer, in LazinatorSerializationOptions options);
        protected abstract LazinatorMemory EncodeToNewBuffer(in LazinatorSerializationOptions options);
        protected abstract void UpdateDeserializedChildren(ref BufferWriter writer, long startPosition);
        protected abstract void WritePropertiesIntoBuffer(ref BufferWriter writer, in LazinatorSerializationOptions options, bool includeUniqueID);
        protected abstract void WritePrimitivePropertiesIntoBuffer(ref BufferWriter writer, in LazinatorSerializationOptions options, bool includeUniqueID);
        protected abstract void WriteChildrenPropertiesIntoBuffer(ref BufferWriter writer, LazinatorSerializationOptions options, bool includeUniqueID, int startOfObjectPosition);
        
        
        
        
    }
}
