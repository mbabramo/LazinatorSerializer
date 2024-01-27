//2896bc9c-46e0-f08d-e497-143ce635b9c7
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by the Lazinator tool, version 0.1.0.405, on 2024/01/27 02:13:58.907 PM.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

#nullable disable
namespace Lazinator.Persistence
{
    #pragma warning disable 8019
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
    #pragma warning restore 8019
    
    [Autogenerated]
    public partial class PersistentIndex : Lazinator.Buffers.MemoryRangeCollection, ILazinator
    {
        public override bool IsNullableContext => false;
        /* Property definitions */
        
        protected int _ForkInformation_ByteIndex;
        protected int _MemoryBlockStatus_ByteIndex;
        protected virtual int _ForkInformation_ByteLength => _MemoryBlockStatus_ByteIndex - _ForkInformation_ByteIndex;
        private int _PersistentIndex_EndByteIndex;
        protected virtual  int _MemoryBlockStatus_ByteLength => _PersistentIndex_EndByteIndex - _MemoryBlockStatus_ByteIndex;
        protected override int _OverallEndByteIndex => _PersistentIndex_EndByteIndex;
        
        
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        protected int _IndexVersion;
        public int IndexVersion
        {
            [DebuggerStepThrough]
            get
            {
                return _IndexVersion;
            }
            [DebuggerStepThrough]
            private set
            {
                IsDirty = true;
                _IndexVersion = value;
            }
        }
        
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        protected List<(Int32 lastMemoryBlockIDBeforeFork, Int32 forkNumber)> _ForkInformation;
        public List<(Int32 lastMemoryBlockIDBeforeFork, Int32 forkNumber)> ForkInformation
        {
            [DebuggerStepThrough]
            get
            {
                if (!_ForkInformation_Accessed)
                {
                    LazinateForkInformation();
                }
                IsDirty = true; 
                return _ForkInformation;
            }
            [DebuggerStepThrough]
            set
            {
                IsDirty = true;
                DescendantIsDirty = true;
                _ForkInformation = value;
                _ForkInformation_Accessed = true;
            }
        }
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        protected bool _ForkInformation_Accessed;
        private void LazinateForkInformation()
        {
            if (LazinatorMemoryStorage.Length == 0)
            {
                _ForkInformation = default(List<(Int32 lastMemoryBlockIDBeforeFork, Int32 forkNumber)>);
            }
            else
            {
                LazinatorMemory childData = GetChildSlice(LazinatorMemoryStorage, _ForkInformation_ByteIndex, _ForkInformation_ByteLength, null);_ForkInformation = ConvertFromBytes_List_G_Pint_C32lastMemoryBlockIDBeforeFork_c_C32int_C32forkNumber_p_g(childData);
            }
            _ForkInformation_Accessed = true;
        }
        
        
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        protected Memory<Byte> _MemoryBlockStatus;
        public Memory<Byte> MemoryBlockStatus
        {
            [DebuggerStepThrough]
            get
            {
                if (!_MemoryBlockStatus_Accessed)
                {
                    LazinateMemoryBlockStatus();
                }
                IsDirty = true; 
                return _MemoryBlockStatus;
            }
            [DebuggerStepThrough]
            set
            {
                IsDirty = true;
                DescendantIsDirty = true;
                _MemoryBlockStatus = value;
                _MemoryBlockStatus_Accessed = true;
            }
        }
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        protected bool _MemoryBlockStatus_Accessed;
        private void LazinateMemoryBlockStatus()
        {
            if (LazinatorMemoryStorage.Length == 0)
            {
                _MemoryBlockStatus = default(Memory<Byte>);
            }
            else
            {
                LazinatorMemory childData = GetChildSlice(LazinatorMemoryStorage, _MemoryBlockStatus_ByteIndex, _MemoryBlockStatus_ByteLength, null);_MemoryBlockStatus = ConvertFromBytes_Memory_Gbyte_g(childData);
            }
            _MemoryBlockStatus_Accessed = true;
        }
        
        /* Clone overrides */
        
        public PersistentIndex(IncludeChildrenMode originalIncludeChildrenMode = IncludeChildrenMode.IncludeAllChildren) : base(originalIncludeChildrenMode)
        {
        }
        
        public PersistentIndex(LazinatorMemory serializedBytes, ILazinator parent = null, IncludeChildrenMode originalIncludeChildrenMode = IncludeChildrenMode.IncludeAllChildren, int? lazinatorObjectVersion = null) : base(serializedBytes, parent, originalIncludeChildrenMode, lazinatorObjectVersion)
        {
        }
        
        public override ILazinator CloneLazinator(IncludeChildrenMode includeChildrenMode = IncludeChildrenMode.IncludeAllChildren, CloneBufferOptions cloneBufferOptions = CloneBufferOptions.IndependentBuffers)
        {
            PersistentIndex clone;
            if (cloneBufferOptions == CloneBufferOptions.NoBuffer)
            {
                clone = new PersistentIndex(includeChildrenMode);
                clone.LazinatorObjectVersion = LazinatorObjectVersion;
                clone = (PersistentIndex)AssignCloneProperties(clone, includeChildrenMode);
            }
            else
            {
                LazinatorMemory bytes = EncodeOrRecycleToNewBuffer(includeChildrenMode, OriginalIncludeChildrenMode, IsDirty, DescendantIsDirty, false, LazinatorMemoryStorage, this);
                clone = new PersistentIndex(bytes);
            }
            return clone;
        }
        
        protected override ILazinator AssignCloneProperties(ILazinator clone, IncludeChildrenMode includeChildrenMode)
        {
            base.AssignCloneProperties(clone, includeChildrenMode);
            PersistentIndex typedClone = (PersistentIndex) clone;
            typedClone.IndexVersion = IndexVersion;
            typedClone.ForkInformation = CloneOrChange_List_G_Pint_C32lastMemoryBlockIDBeforeFork_c_C32int_C32forkNumber_p_g(ForkInformation, l => l?.CloneLazinator(includeChildrenMode, CloneBufferOptions.NoBuffer), false);
            typedClone.MemoryBlockStatus = CloneOrChange_Memory_Gbyte_g(MemoryBlockStatus, l => l?.CloneLazinator(includeChildrenMode, CloneBufferOptions.NoBuffer), false);
            
            return typedClone;
        }
        
        /* Properties */
        
        public override IEnumerable<(string propertyName, ILazinator descendant)> EnumerateLazinatorDescendants(Func<ILazinator, bool> matchCriterion, bool stopExploringBelowMatch, Func<ILazinator, bool> exploreCriterion, bool exploreOnlyDeserializedChildren, bool enumerateNulls)
        {
            foreach (var inheritedYield in base.EnumerateLazinatorDescendants(matchCriterion, stopExploringBelowMatch, exploreCriterion, exploreOnlyDeserializedChildren, enumerateNulls))
            {
                yield return inheritedYield;
            }
            yield break;
        }
        
        
        public override IEnumerable<(string propertyName, object descendant)> EnumerateNonLazinatorProperties()
        {
            foreach (var inheritedYield in base.EnumerateNonLazinatorProperties())
            {
                yield return inheritedYield;
            }
            yield return ("IndexVersion", (object)IndexVersion);
            yield return ("ForkInformation", (object)ForkInformation);
            yield return ("MemoryBlockStatus", (object)MemoryBlockStatus);
            yield break;
        }
        
        public override ILazinator ForEachLazinator(Func<ILazinator, ILazinator> changeFunc, bool exploreOnlyDeserializedChildren, bool changeThisLevel)
        {
            base.ForEachLazinator(changeFunc, exploreOnlyDeserializedChildren, false);
            if ((!exploreOnlyDeserializedChildren && ForkInformation != null) || (_ForkInformation_Accessed && _ForkInformation != null))
            {
                _ForkInformation = (List<(Int32 lastMemoryBlockIDBeforeFork, Int32 forkNumber)>) CloneOrChange_List_G_Pint_C32lastMemoryBlockIDBeforeFork_c_C32int_C32forkNumber_p_g(_ForkInformation, l => l?.ForEachLazinator(changeFunc, exploreOnlyDeserializedChildren, true), true);
            }
            if (!exploreOnlyDeserializedChildren)
            {
                var deserialized_MemoryBlockStatus = MemoryBlockStatus;
            }if (changeThisLevel && changeFunc != null)
            {
                return changeFunc(this);
            }
            return this;
        }
        
        public override void FreeInMemoryObjects()
        {
            base.FreeInMemoryObjects();
            _ForkInformation = default;
            _MemoryBlockStatus = default;
            _ForkInformation_Accessed = _MemoryBlockStatus_Accessed = false;
            IsDirty = false;
            DescendantIsDirty = false;
            HasChanged = false;
            DescendantHasChanged = false;
        }
        
        /* Conversion */
        
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public override int LazinatorUniqueID => 46;
        
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        protected override bool ContainsOpenGenericParameters => false;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public override LazinatorGenericIDType LazinatorGenericID => default;
        
        
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public override int LazinatorObjectVersion { get; set; } = 0;
        
        
        protected override int ConvertFromBytesAfterHeader(IncludeChildrenMode includeChildrenMode, int serializedVersionNumber, ref int bytesSoFar)
        {
            ReadOnlySpan<byte> span = LazinatorMemoryStorage.InitialReadOnlyMemory.Span;
            ConvertFromBytesForPrimitiveProperties(span, includeChildrenMode, serializedVersionNumber, ref bytesSoFar);
            int lengthForLengths = 16;
            int totalChildrenSize = ConvertFromBytesForChildLengths(span, includeChildrenMode, serializedVersionNumber, bytesSoFar + lengthForLengths, ref bytesSoFar);;
            return bytesSoFar + totalChildrenSize;
        }
        
        protected override void ConvertFromBytesForPrimitiveProperties(ReadOnlySpan<byte> span, IncludeChildrenMode includeChildrenMode, int serializedVersionNumber, ref int bytesSoFar)
        {
            base.ConvertFromBytesForPrimitiveProperties(span, OriginalIncludeChildrenMode, serializedVersionNumber, ref bytesSoFar);
            _IndexVersion = span.ToDecompressedInt32(ref bytesSoFar);
        }
        
        protected override int ConvertFromBytesForChildLengths(ReadOnlySpan<byte> span, IncludeChildrenMode includeChildrenMode, int serializedVersionNumber, int indexOfFirstChild, ref int bytesSoFar)
        {
            int totalChildrenBytes = 0;
            totalChildrenBytes = base.ConvertFromBytesForChildLengths(span, OriginalIncludeChildrenMode, serializedVersionNumber, indexOfFirstChild, ref bytesSoFar);
            _ForkInformation_ByteIndex = indexOfFirstChild + totalChildrenBytes;
            totalChildrenBytes += span.ToInt32(ref bytesSoFar);
            _MemoryBlockStatus_ByteIndex = indexOfFirstChild + totalChildrenBytes;
            totalChildrenBytes += span.ToInt32(ref bytesSoFar);
            _PersistentIndex_EndByteIndex = indexOfFirstChild + totalChildrenBytes;
            return totalChildrenBytes;
        }
        
        public override void SerializeToExistingBuffer(ref BufferWriter writer, in LazinatorSerializationOptions options)
        {
            int startPosition = writer.ActiveMemoryPosition;
            WritePropertiesIntoBuffer(ref writer, options, true);
            if (options.UpdateStoredBuffer)
            {
                UpdateStoredBuffer(ref writer, startPosition, writer.ActiveMemoryPosition - startPosition, options.IncludeChildrenMode, false);
            }
        }
        
        public override void UpdateStoredBuffer(ref BufferWriter writer, long startPosition, long length, IncludeChildrenMode includeChildrenMode, bool updateDeserializedChildren)
        {
            _IsDirty = false;
            if (includeChildrenMode == IncludeChildrenMode.IncludeAllChildren)
            {
                _DescendantIsDirty = false;
                if (updateDeserializedChildren)
                {
                    UpdateDeserializedChildren(ref writer, startPosition);
                }
                
            }
            else
            {
                ThrowHelper.ThrowCannotUpdateStoredBuffer();
            }
            
            var newBuffer = writer.Slice(startPosition, length);
            LazinatorMemoryStorage = newBuffer;
        }
        
        protected override void UpdateDeserializedChildren(ref BufferWriter writer, long startPosition)
        {
            base.UpdateDeserializedChildren(ref writer, startPosition);
            if (_ForkInformation_Accessed && _ForkInformation != null)
            {
                _ForkInformation = (List<(Int32 lastMemoryBlockIDBeforeFork, Int32 forkNumber)>) CloneOrChange_List_G_Pint_C32lastMemoryBlockIDBeforeFork_c_C32int_C32forkNumber_p_g(_ForkInformation, l => l.RemoveBufferInHierarchy(), true);
            }
            
        }
        
        
        protected override void WritePropertiesIntoBuffer(ref BufferWriter writer, in LazinatorSerializationOptions options, bool includeUniqueID)
        {
            int startPosition = writer.ActiveMemoryPosition;
            if (includeUniqueID)
            {
                if (!ContainsOpenGenericParameters)
                {
                    CompressedIntegralTypes.WriteCompressedInt(ref writer, LazinatorUniqueID);
                }
                else
                {
                    WriteLazinatorGenericID(ref writer, LazinatorGenericID);
                }
            }
            CompressedIntegralTypes.WriteCompressedInt(ref writer, Lazinator.Support.LazinatorVersionInfo.LazinatorIntVersion);
            CompressedIntegralTypes.WriteCompressedInt(ref writer, LazinatorObjectVersion);
            writer.Write((byte)options.IncludeChildrenMode);
            // write properties
            
            WritePrimitivePropertiesIntoBuffer(ref writer, options, includeUniqueID);
            int lengthForLengths = 16;
            
            var previousLengthsPosition = writer.SetLengthsPosition(lengthForLengths);
            WriteChildrenPropertiesIntoBuffer(ref writer, options, includeUniqueID, startPosition);
            writer.ResetLengthsPosition(previousLengthsPosition);
            
        }
        
        protected override void WritePrimitivePropertiesIntoBuffer(ref BufferWriter writer, in LazinatorSerializationOptions options, bool includeUniqueID)
        {
            base.WritePrimitivePropertiesIntoBuffer(ref writer, options, includeUniqueID);
            CompressedIntegralTypes.WriteCompressedInt(ref writer, _IndexVersion);
        }
        protected override void WriteChildrenPropertiesIntoBuffer(ref BufferWriter writer, LazinatorSerializationOptions options, bool includeUniqueID, int startOfObjectPosition)
        {
            base.WriteChildrenPropertiesIntoBuffer(ref writer, options, includeUniqueID, startOfObjectPosition);
            if (options.SplittingPossible)
            {
                options = options.WithoutSplittingPossible();
            }
            int startOfChildPosition = 0;
            startOfChildPosition = writer.ActiveMemoryPosition;
            if ((options.IncludeChildrenMode != IncludeChildrenMode.IncludeAllChildren || options.IncludeChildrenMode != OriginalIncludeChildrenMode) && !_ForkInformation_Accessed)
            {
                var deserialized = ForkInformation;
            }
            WriteNonLazinatorObject(
            nonLazinatorObject: _ForkInformation, isBelievedDirty: _ForkInformation_Accessed || (options.IncludeChildrenMode != OriginalIncludeChildrenMode),
            isAccessed: _ForkInformation_Accessed, writer: ref writer,
            getChildSliceForFieldFn: () => GetChildSlice(LazinatorMemoryStorage, _ForkInformation_ByteIndex, _ForkInformation_ByteLength, null),
            verifyCleanness: false,
            binaryWriterAction: (ref BufferWriter w, bool v) =>
            ConvertToBytes_List_G_Pint_C32lastMemoryBlockIDBeforeFork_c_C32int_C32forkNumber_p_g(ref w, _ForkInformation,
            options));
            if (options.UpdateStoredBuffer)
            {
                _ForkInformation_ByteIndex = startOfChildPosition - startOfObjectPosition;
                
            }
            startOfChildPosition = writer.ActiveMemoryPosition;
            if ((options.IncludeChildrenMode != IncludeChildrenMode.IncludeAllChildren || options.IncludeChildrenMode != OriginalIncludeChildrenMode) && !_MemoryBlockStatus_Accessed)
            {
                var deserialized = MemoryBlockStatus;
            }
            WriteNonLazinatorObject(
            nonLazinatorObject: _MemoryBlockStatus, isBelievedDirty: _MemoryBlockStatus_Accessed || (options.IncludeChildrenMode != OriginalIncludeChildrenMode),
            isAccessed: _MemoryBlockStatus_Accessed, writer: ref writer,
            getChildSliceForFieldFn: () => GetChildSlice(LazinatorMemoryStorage, _MemoryBlockStatus_ByteIndex, _MemoryBlockStatus_ByteLength, null),
            verifyCleanness: false,
            binaryWriterAction: (ref BufferWriter w, bool v) =>
            ConvertToBytes_Memory_Gbyte_g(ref w, _MemoryBlockStatus,
            options));
            if (options.UpdateStoredBuffer)
            {
                _MemoryBlockStatus_ByteIndex = startOfChildPosition - startOfObjectPosition;
                
            }
            if (options.UpdateStoredBuffer)
            {
                _PersistentIndex_EndByteIndex = writer.ActiveMemoryPosition - startOfObjectPosition;
            }
            
        }
        /* Conversion of supported collections and tuples */
        
        private static List<(Int32 lastMemoryBlockIDBeforeFork, Int32 forkNumber)> ConvertFromBytes_List_G_Pint_C32lastMemoryBlockIDBeforeFork_c_C32int_C32forkNumber_p_g(LazinatorMemory storage)
        {
            if (storage.Length == 0)
            {
                return default(List<(Int32 lastMemoryBlockIDBeforeFork, Int32 forkNumber)>);
            }
            ReadOnlySpan<byte> span = storage.InitialReadOnlyMemory.Span;
            int bytesSoFar = 0;
            int collectionLength = span.ToDecompressedInt32(ref bytesSoFar);
            
            List<(Int32 lastMemoryBlockIDBeforeFork, Int32 forkNumber)> collection = new List<(Int32 lastMemoryBlockIDBeforeFork, Int32 forkNumber)>(collectionLength);
            for (int itemIndex = 0; itemIndex < collectionLength; itemIndex++)
            {
                int lengthCollectionMember = span.ToInt32(ref bytesSoFar);
                LazinatorMemory childData = storage.Slice(bytesSoFar, lengthCollectionMember);
                var item = ConvertFromBytes__Pint_C32lastMemoryBlockIDBeforeFork_c_C32int_C32forkNumber_p(childData);
                collection.Add(item);
                bytesSoFar += lengthCollectionMember;
            }
            
            return collection;
        }
        
        private static void ConvertToBytes_List_G_Pint_C32lastMemoryBlockIDBeforeFork_c_C32int_C32forkNumber_p_g(ref BufferWriter writer, List<(Int32 lastMemoryBlockIDBeforeFork, Int32 forkNumber)> itemToConvert, LazinatorSerializationOptions options)
        {
            if (itemToConvert == default(List<(Int32 lastMemoryBlockIDBeforeFork, Int32 forkNumber)>))
            {
                return;
            }
            CompressedIntegralTypes.WriteCompressedInt(ref writer, itemToConvert.Count);
            int itemToConvertCount = itemToConvert.Count;
            for (int itemIndex = 0; itemIndex < itemToConvertCount; itemIndex++)
            {
                void action(ref BufferWriter w) => ConvertToBytes__Pint_C32lastMemoryBlockIDBeforeFork_c_C32int_C32forkNumber_p(ref w, itemToConvert[itemIndex], options);
                WriteToBinaryWithInt32LengthPrefix(ref writer, action);
            }
        }
        
        private static List<(Int32 lastMemoryBlockIDBeforeFork, Int32 forkNumber)> CloneOrChange_List_G_Pint_C32lastMemoryBlockIDBeforeFork_c_C32int_C32forkNumber_p_g(List<(Int32 lastMemoryBlockIDBeforeFork, Int32 forkNumber)> itemToClone, Func<ILazinator, ILazinator> cloneOrChangeFunc, bool avoidCloningIfPossible)
        {
            if (itemToClone == null)
            {
                return default;
            }
            int collectionLength = itemToClone.Count;
            List<(Int32 lastMemoryBlockIDBeforeFork, Int32 forkNumber)> collection = new List<(Int32 lastMemoryBlockIDBeforeFork, Int32 forkNumber)>(collectionLength);
            int itemToCloneCount = itemToClone.Count;
            for (int itemIndex = 0; itemIndex < itemToCloneCount; itemIndex++)
            {
                var itemCopied = ((Int32 lastMemoryBlockIDBeforeFork, Int32 forkNumber)) CloneOrChange__Pint_C32lastMemoryBlockIDBeforeFork_c_C32int_C32forkNumber_p(itemToClone[itemIndex], cloneOrChangeFunc, avoidCloningIfPossible);
                collection.Add(itemCopied);
            }
            return collection;
        }
        
        private static (Int32 lastMemoryBlockIDBeforeFork, Int32 forkNumber) ConvertFromBytes__Pint_C32lastMemoryBlockIDBeforeFork_c_C32int_C32forkNumber_p(LazinatorMemory storage)
        {
            if (storage.Length == 0)
            {
                return default;
            }
            ReadOnlySpan<byte> span = storage.InitialReadOnlyMemory.Span;
            
            int bytesSoFar = 0;
            
            int item1 = span.ToDecompressedInt32(ref bytesSoFar);
            
            int item2 = span.ToDecompressedInt32(ref bytesSoFar);
            
            var itemToCreate = (item1, item2);
            
            return itemToCreate;
        }
        
        private static void ConvertToBytes__Pint_C32lastMemoryBlockIDBeforeFork_c_C32int_C32forkNumber_p(ref BufferWriter writer, (Int32 lastMemoryBlockIDBeforeFork, Int32 forkNumber) itemToConvert, LazinatorSerializationOptions options)
        {
            
            CompressedIntegralTypes.WriteCompressedInt(ref writer, itemToConvert.Item1);
            
            CompressedIntegralTypes.WriteCompressedInt(ref writer, itemToConvert.Item2);
        }
        
        private static (Int32 lastMemoryBlockIDBeforeFork, Int32 forkNumber) CloneOrChange__Pint_C32lastMemoryBlockIDBeforeFork_c_C32int_C32forkNumber_p((Int32 lastMemoryBlockIDBeforeFork, Int32 forkNumber) itemToConvert, Func<ILazinator, ILazinator> cloneOrChangeFunc, bool avoidCloningIfPossible)
        {
            return ((int) (itemToConvert.Item1), (int) (itemToConvert.Item2));
        }
        
        private static Memory<Byte> ConvertFromBytes_Memory_Gbyte_g(LazinatorMemory storage)
        {
            return storage.InitialReadOnlyMemory.ToArray();
        }
        
        private static void ConvertToBytes_Memory_Gbyte_g(ref BufferWriter writer, Memory<Byte> itemToConvert, LazinatorSerializationOptions options)
        {
            writer.Write(itemToConvert.Span);
        }
        
        private static Memory<Byte> CloneOrChange_Memory_Gbyte_g(Memory<Byte> itemToClone, Func<ILazinator, ILazinator> cloneOrChangeFunc, bool avoidCloningIfPossible)
        {
            int collectionLength = itemToClone.Length;
            Memory<Byte> collection = new Memory<Byte>(new byte[collectionLength]);
            var collectionAsSpan = collection.Span;
            var itemToCloneSpan = itemToClone.Span;
            int itemToCloneCount = itemToCloneSpan.Length;
            for (int itemIndex = 0; itemIndex < itemToCloneCount; itemIndex++)
            {
                var itemCopied = (byte) itemToCloneSpan[itemIndex];
                collectionAsSpan[itemIndex] = itemCopied;
            }
            return collection;
        }
        
    }
}
#nullable restore
