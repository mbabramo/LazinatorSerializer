/*Location342*//*Location327*///16489e75-a39b-442e-80f4-a58ffe961edc
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by the Lazinator tool, version 0.1.0.432, on 2024/01/01 12:00:00.000 AM.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

#nullable enable
namespace FuzzTests.n0
{
    #pragma warning disable 8019//Location328
    using Lazinator.Attributes;/*Location329*/
    using Lazinator.Buffers;/*Location330*/
    using Lazinator.Core;/*Location331*/
    using Lazinator.Exceptions;/*Location332*/
    using Lazinator.Support;/*Location333*/
    using static Lazinator.Buffers.WriteUncompressedPrimitives;/*Location334*/
    using System;/*Location335*/
    using System.Buffers;/*Location336*/
    using System.Collections.Generic;/*Location337*/
    using System.Diagnostics;/*Location338*/
    using System.IO;/*Location339*/
    using System.Linq;/*Location340*/
    using System.Runtime.InteropServices;/*Location341*/
    using static Lazinator.Core.LazinatorUtilities;
    #pragma warning restore 8019
    
    [Autogenerated]
    public partial class RegularReasonableClass : ILazinator
    {
        /*Location343*/[DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public bool IsStruct => false;
        
        /*Location344*//* Property definitions */
        
        
        /*Location345*/// DEBUG5
        /*Location346*/        /* Abstract declarations */
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
        
        public abstract ILazinator? CloneLazinator(IncludeChildrenMode includeChildrenMode = IncludeChildrenMode.IncludeAllChildren, CloneBufferOptions cloneBufferOptions = CloneBufferOptions.IndependentBuffers);
        
        protected abstract ILazinator? AssignCloneProperties(ILazinator clone, IncludeChildrenMode includeChildrenMode);
        
        
        public abstract IEnumerable<ILazinator?> EnumerateLazinatorNodes(Func<ILazinator?, bool>? matchCriterion, bool stopExploringBelowMatch, Func<ILazinator?, bool>? exploreCriterion, bool exploreOnlyDeserializedChildren, bool enumerateNulls);
        public abstract IEnumerable<(string propertyName, ILazinator? descendant)> EnumerateLazinatorDescendants(Func<ILazinator?, bool>? matchCriterion, bool stopExploringBelowMatch, Func<ILazinator?, bool>? exploreCriterion, bool exploreOnlyDeserializedChildren, bool enumerateNulls);
        public abstract IEnumerable<(string propertyName, object descendant)> EnumerateNonLazinatorProperties();
        public abstract ILazinator? ForEachLazinator(Func<ILazinator?, ILazinator?>? changeFunc, bool exploreOnlyDeserializedChildren, bool changeThisLevel);
        
        public abstract void UpdateStoredBuffer(ref BufferWriter writer, long startPosition, long length, IncludeChildrenMode includeChildrenMode, bool updateDeserializedChildren);
        public abstract void FreeInMemoryObjects();
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public abstract int LazinatorUniqueID { get; }
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        protected virtual bool ContainsOpenGenericParameters => false;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public abstract LazinatorGenericIDType LazinatorGenericID { get; }
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
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
        
        
        
        /*Location347*/
    }
}
#nullable restore
