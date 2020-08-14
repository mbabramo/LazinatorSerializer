/*Location2413*//*Location2398*///47be356b-704c-8c55-fbd8-6238bd0552e0
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by the Lazinator tool, version 0.1.0.380
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

#nullable disable
namespace LazinatorTests.Examples.Abstract
{/*Location2399*/
    using Lazinator.Attributes;/*Location2400*/
    using Lazinator.Buffers;/*Location2401*/
    using Lazinator.Core;/*Location2402*/
    using Lazinator.Exceptions;/*Location2403*/
    using Lazinator.Support;/*Location2404*/
    using LazinatorTests.Examples;/*Location2405*/
    using System;/*Location2406*/
    using System.Buffers;/*Location2407*/
    using System.Collections.Generic;/*Location2408*/
    using System.Diagnostics;/*Location2409*/
    using System.IO;/*Location2410*/
    using System.Linq;/*Location2411*/
    using System.Runtime.InteropServices;/*Location2412*/
    using static Lazinator.Core.LazinatorUtilities;
    
    [Autogenerated]
    public partial class Abstract1 : ILazinator
    {
        /*Location2414*/public bool IsStruct => false;
        
        /*Location2415*//* Property definitions */
        
        /*Location2416*/        protected int _Example3_ByteIndex;
        /*Location2417*/        protected int _IntList1_ByteIndex;
        /*Location2418*/protected virtual int _Example3_ByteLength => _IntList1_ByteIndex - _Example3_ByteIndex;
        /*Location2419*/protected virtual int _IntList1_ByteLength { get; }
        
        /*Location2420*/
        protected bool _String1_Accessed = false;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public abstract string String1
        {
            get;
            set;
        }
        /*Location2421*/
        protected bool _Example3_Accessed = false;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public abstract Example Example3
        {
            get;
            set;
        }
        /*Location2422*/
        protected bool _IntList1_Accessed = false;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public abstract List<int> IntList1
        {
            get;
            set;
        }
        /*Location2426*/        /* Abstract declarations */
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
        
        /*Location2427*/public abstract int LazinatorUniqueID { get; }
        protected virtual bool ContainsOpenGenericParameters => false;
        public abstract LazinatorGenericIDType LazinatorGenericID { get; }
        public abstract int LazinatorObjectVersion { get; set; }
        public abstract void ConvertFromBytesAfterHeader(IncludeChildrenMode includeChildrenMode, int serializedVersionNumber, ref int bytesSoFar);
        public abstract void SerializeExistingBuffer(ref BinaryBufferWriter writer, IncludeChildrenMode includeChildrenMode, bool verifyCleanness, bool updateStoredBuffer);
        protected abstract LazinatorMemory EncodeToNewBuffer(IncludeChildrenMode includeChildrenMode, bool verifyCleanness, bool updateStoredBuffer);
        protected abstract void UpdateDeserializedChildren(ref BinaryBufferWriter writer, int startPosition);
        protected abstract void WritePropertiesIntoBuffer(ref BinaryBufferWriter writer, IncludeChildrenMode includeChildrenMode, bool verifyCleanness, bool updateStoredBuffer, bool includeUniqueID);
        /*Location2428*/
    }
}
