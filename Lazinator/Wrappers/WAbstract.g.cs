//4e9eb941-170d-0ea4-c812-340ee1dcc752
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by the Lazinator tool, version 0.1.0.393
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

#nullable disable
namespace Lazinator.Wrappers
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
    public partial class WAbstract<T> : ILazinator
    {
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public bool IsStruct => false;
        
        /* Property definitions */
        
        protected int _Wrapped_ByteIndex;
        protected virtual int _Wrapped_ByteLength { get; }
        
        
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        protected bool _Wrapped_Accessed = false;
        public virtual T Wrapped
        {
            get;
            set;
        }
        /* Abstract declarations */
        public abstract LazinatorParentsCollection LazinatorParents { get; set; }
        
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public abstract LazinatorMemory LazinatorMemoryStorage
        {
            get;
            set;
        }
        
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public abstract IncludeChildrenMode OriginalIncludeChildrenMode
        {
            get;
            set;
        }
        
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public abstract bool HasChanged
        {
            get;
            set;
        }
        
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public abstract bool IsDirty
        {
            get;
            set;
        }
        
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public abstract bool DescendantHasChanged
        {
            get;
            set;
        }
        
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
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
        
        public abstract void UpdateStoredBuffer(ref BinaryBufferWriter writer, long startPosition, long length, IncludeChildrenMode includeChildrenMode, bool updateDeserializedChildren);
        public abstract void FreeInMemoryObjects();
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public abstract int LazinatorUniqueID { get; }
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        protected virtual bool ContainsOpenGenericParameters => true;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public abstract LazinatorGenericIDType LazinatorGenericID { get; }
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public abstract int LazinatorObjectVersion { get; set; }
        protected abstract int ConvertFromBytesAfterHeader(IncludeChildrenMode includeChildrenMode, int serializedVersionNumber, ref int bytesSoFar);
        protected abstract void ConvertFromBytesForPrimitiveProperties(ReadOnlySpan<byte> span, IncludeChildrenMode includeChildrenMode, int serializedVersionNumber, ref int bytesSoFar);
        protected abstract int ConvertFromBytesForChildProperties(ReadOnlySpan<byte> span, IncludeChildrenMode includeChildrenMode, int serializedVersionNumber, int indexOfFirstChild, ref int bytesSoFar);
        public abstract void SerializeToExistingBuffer(ref BinaryBufferWriter writer, in LazinatorSerializationOptions options);
        protected abstract LazinatorMemory EncodeToNewBuffer(in LazinatorSerializationOptions options);
        protected abstract void UpdateDeserializedChildren(ref BinaryBufferWriter writer, long startPosition);
        protected abstract void WritePropertiesIntoBuffer(ref BinaryBufferWriter writer, in LazinatorSerializationOptions options, bool includeUniqueID);
        protected abstract void WritePrimitivePropertiesIntoBuffer(ref BinaryBufferWriter writer, in LazinatorSerializationOptions options, bool includeUniqueID);
        protected abstract void WriteChildrenPropertiesIntoBuffer(ref BinaryBufferWriter writer, LazinatorSerializationOptions options, bool includeUniqueID, int startOfObjectPosition);
        
        
        
        
    }
}
