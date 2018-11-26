//7e1e78e7-83cf-de1e-3318-68159bc6b287
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by the Lazinator tool, version 0.1.0.329
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
        
        public abstract int Deserialize();
        
        public abstract LazinatorMemory SerializeLazinator(IncludeChildrenMode includeChildrenMode, bool verifyCleanness, bool updateStoredBuffer);
        
        public abstract ILazinator CloneLazinator(IncludeChildrenMode includeChildrenMode = IncludeChildrenMode.IncludeAllChildren, CloneBufferOptions cloneBufferOptions = CloneBufferOptions.IndependentBuffers);
        
        public abstract ILazinator AssignCloneProperties(ILazinator clone, IncludeChildrenMode includeChildrenMode);
        
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
        public abstract IEnumerable<ILazinator> EnumerateLazinatorNodes(Func<ILazinator, bool> matchCriterion, bool stopExploringBelowMatch, Func<ILazinator, bool> exploreCriterion, bool exploreOnlyDeserializedChildren, bool enumerateNulls);
        public abstract IEnumerable<(string propertyName, ILazinator descendant)> EnumerateLazinatorDescendants(Func<ILazinator, bool> matchCriterion, bool stopExploringBelowMatch, Func<ILazinator, bool> exploreCriterion, bool exploreOnlyDeserializedChildren, bool enumerateNulls);
        public abstract IEnumerable<(string propertyName, object descendant)> EnumerateNonLazinatorProperties();
        public abstract ILazinator ForEachLazinator(Func<ILazinator, ILazinator> changeFunc, bool exploreOnlyDeserializedChildren);
        
        public abstract void DeserializeLazinator(LazinatorMemory serializedBytes);
        
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
        protected abstract ReadOnlyMemory<byte> LazinatorObjectBytes
        {
            get;
        }
        
        public abstract void UpdateStoredBuffer(ref BinaryBufferWriter writer, int startPosition, int length, IncludeChildrenMode includeChildrenMode, bool updateDeserializedChildren);
        public abstract void UpdateStoredBuffer(bool disposePreviousBuffer = false);
        public abstract void FreeInMemoryObjects();
        public abstract int GetByteLength();
        
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public abstract int LazinatorUniqueID { get; }
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        protected abstract LazinatorGenericIDType _LazinatorGenericID { get; set; }
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        protected virtual bool ContainsOpenGenericParameters => true;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public abstract LazinatorGenericIDType LazinatorGenericID { get; set; }
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public abstract int LazinatorObjectVersion { get; set; }
        public abstract void ConvertFromBytesAfterHeader(IncludeChildrenMode includeChildrenMode, int serializedVersionNumber, ref int bytesSoFar);
        public abstract void SerializeExistingBuffer(ref BinaryBufferWriter writer, IncludeChildrenMode includeChildrenMode, bool verifyCleanness, bool updateStoredBuffer);
        protected abstract void WritePropertiesIntoBuffer(ref BinaryBufferWriter writer, IncludeChildrenMode includeChildrenMode, bool verifyCleanness, bool updateStoredBuffer, bool includeUniqueID);
        
    }
}
