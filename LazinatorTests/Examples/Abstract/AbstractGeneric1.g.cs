/*Location4284*//*Location4270*///f4a51311-71b9-a2bb-a572-ba1480562368
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
{/*Location4271*/
    using Lazinator.Attributes;/*Location4272*/
    using Lazinator.Buffers;/*Location4273*/
    using Lazinator.Core;/*Location4274*/
    using Lazinator.Exceptions;/*Location4275*/
    using Lazinator.Support;/*Location4276*/
    using System;/*Location4277*/
    using System.Buffers;/*Location4278*/
    using System.Collections.Generic;/*Location4279*/
    using System.Diagnostics;/*Location4280*/
    using System.IO;/*Location4281*/
    using System.Linq;/*Location4282*/
    using System.Runtime.InteropServices;/*Location4283*/
    using static Lazinator.Core.LazinatorUtilities;
    
    [Autogenerated]
    public partial class AbstractGeneric1<T> : ILazinator
    {
        /*Location4285*/public bool IsStruct => false;
        
        /*Location4286*//* Property definitions */
        
        /*Location4287*/        protected int _MyT_ByteIndex;
        /*Location4288*/protected virtual int _MyT_ByteLength { get; }
        
        /*Location4289*/
        protected bool _MyEnumWithinAbstractGeneric_Accessed = false;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public abstract global::LazinatorTests.Examples.Abstract.AbstractGeneric1<T>.EnumWithinAbstractGeneric MyEnumWithinAbstractGeneric
        {
            get;
            set;
        }
        /*Location4290*/
        protected bool _MyEnumWithinAbstractGeneric2_Accessed = false;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public abstract global::LazinatorTests.Examples.Abstract.AbstractGeneric1<int>.EnumWithinAbstractGeneric MyEnumWithinAbstractGeneric2
        {
            get;
            set;
        }
        /*Location4291*/
        protected bool _MyUnofficialInt_Accessed = false;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public abstract int MyUnofficialInt
        {
            get;
            set;
        }
        /*Location4292*/
        protected bool _MyT_Accessed = false;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public virtual T MyT
        {
            get;
            set;
        }
        /*Location4297*/        /* Abstract declarations */
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
        
        /*Location4298*/public abstract int LazinatorUniqueID { get; }
        protected virtual bool ContainsOpenGenericParameters => true;
        public abstract LazinatorGenericIDType LazinatorGenericID { get; }
        public abstract int LazinatorObjectVersion { get; set; }
        public abstract void ConvertFromBytesAfterHeader(IncludeChildrenMode includeChildrenMode, int serializedVersionNumber, ref int bytesSoFar);
        public abstract void SerializeExistingBuffer(ref BinaryBufferWriter writer, IncludeChildrenMode includeChildrenMode, bool verifyCleanness, bool updateStoredBuffer);
        protected abstract LazinatorMemory EncodeToNewBuffer(IncludeChildrenMode includeChildrenMode, bool verifyCleanness, bool updateStoredBuffer);
        protected abstract void UpdateDeserializedChildren(ref BinaryBufferWriter writer, int startPosition);
        protected abstract void WritePropertiesIntoBuffer(ref BinaryBufferWriter writer, IncludeChildrenMode includeChildrenMode, bool verifyCleanness, bool updateStoredBuffer, bool includeUniqueID);
        /*Location4299*/
    }
}
