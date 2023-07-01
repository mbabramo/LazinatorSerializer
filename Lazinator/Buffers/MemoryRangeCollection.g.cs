//130c40eb-3e37-f154-7325-698d028411f0
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by the Lazinator tool, version 0.1.0.395
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

#nullable disable
namespace Lazinator.Buffers
{
    using Lazinator.Attributes;
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
    public partial class MemoryRangeCollection : MemoryBlockCollection, ILazinator
    {
        /* Property definitions */
        
        protected int _Ranges_ByteIndex;
        private int _MemoryRangeCollection_EndByteIndex;
        protected virtual  int _Ranges_ByteLength => _MemoryRangeCollection_EndByteIndex - _Ranges_ByteIndex;
        protected override int _OverallEndByteIndex => _MemoryRangeCollection_EndByteIndex;
        
        
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        protected List<MemoryRangeByBlockID> _Ranges;
        public List<MemoryRangeByBlockID> Ranges
        {
            [DebuggerStepThrough]
            get
            {
                if (!_Ranges_Accessed)
                {
                    LazinateRanges();
                }
                IsDirty = true; 
                return _Ranges;
            }
            [DebuggerStepThrough]
            set
            {
                IsDirty = true;
                DescendantIsDirty = true;
                _Ranges = value;
                _Ranges_Accessed = true;
            }
        }
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        protected bool _Ranges_Accessed;
        private void LazinateRanges()
        {
            if (LazinatorMemoryStorage.Length == 0)
            {
                _Ranges = default(List<MemoryRangeByBlockID>);
            }
            else
            {
                LazinatorMemory childData = GetChildSlice(LazinatorMemoryStorage, _Ranges_ByteIndex, _Ranges_ByteLength, null);_Ranges = ConvertFromBytes_List_GMemoryRangeByBlockID_g(childData);
            }
            _Ranges_Accessed = true;
        }
        
        /* Clone overrides */
        
        public MemoryRangeCollection(IncludeChildrenMode originalIncludeChildrenMode = IncludeChildrenMode.IncludeAllChildren) : base(originalIncludeChildrenMode)
        {
        }
        
        public MemoryRangeCollection(LazinatorMemory serializedBytes, ILazinator parent = null, IncludeChildrenMode originalIncludeChildrenMode = IncludeChildrenMode.IncludeAllChildren, int? lazinatorObjectVersion = null) : base(serializedBytes, parent, originalIncludeChildrenMode, lazinatorObjectVersion)
        {
        }
        
        public override ILazinator CloneLazinator(IncludeChildrenMode includeChildrenMode = IncludeChildrenMode.IncludeAllChildren, CloneBufferOptions cloneBufferOptions = CloneBufferOptions.IndependentBuffers)
        {
            MemoryRangeCollection clone;
            if (cloneBufferOptions == CloneBufferOptions.NoBuffer)
            {
                clone = new MemoryRangeCollection(includeChildrenMode);
                clone.LazinatorObjectVersion = LazinatorObjectVersion;
                clone = (MemoryRangeCollection)AssignCloneProperties(clone, includeChildrenMode);
            }
            else
            {
                LazinatorMemory bytes = EncodeOrRecycleToNewBuffer(includeChildrenMode, OriginalIncludeChildrenMode, IsDirty, DescendantIsDirty, false, LazinatorMemoryStorage, this);
                clone = new MemoryRangeCollection(bytes);
            }
            return clone;
        }
        
        protected override ILazinator AssignCloneProperties(ILazinator clone, IncludeChildrenMode includeChildrenMode)
        {
            base.AssignCloneProperties(clone, includeChildrenMode);
            MemoryRangeCollection typedClone = (MemoryRangeCollection) clone;
            typedClone.Ranges = CloneOrChange_List_GMemoryRangeByBlockID_g(Ranges, l => l?.CloneLazinator(includeChildrenMode, CloneBufferOptions.NoBuffer), false);
            
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
            yield return ("Ranges", (object)Ranges);
            yield break;
        }
        
        public override ILazinator ForEachLazinator(Func<ILazinator, ILazinator> changeFunc, bool exploreOnlyDeserializedChildren, bool changeThisLevel)
        {
            base.ForEachLazinator(changeFunc, exploreOnlyDeserializedChildren, false);
            if ((!exploreOnlyDeserializedChildren && Ranges != null) || (_Ranges_Accessed && _Ranges != null))
            {
                _Ranges = (List<MemoryRangeByBlockID>) CloneOrChange_List_GMemoryRangeByBlockID_g(_Ranges, l => l?.ForEachLazinator(changeFunc, exploreOnlyDeserializedChildren, true), true);
            }
            if (changeThisLevel && changeFunc != null)
            {
                return changeFunc(this);
            }
            return this;
        }
        
        public override void FreeInMemoryObjects()
        {
            base.FreeInMemoryObjects();
            _Ranges = default;
            _Ranges_Accessed = false;
            IsDirty = false;
            DescendantIsDirty = false;
            HasChanged = false;
            DescendantHasChanged = false;
        }
        
        /* Conversion */
        
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public override int LazinatorUniqueID => 48;
        
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
            int lengthForLengths = 8;
            int totalChildrenSize = ConvertFromBytesForChildProperties(span, includeChildrenMode, serializedVersionNumber, bytesSoFar + lengthForLengths, ref bytesSoFar);
            return bytesSoFar + totalChildrenSize;
        }
        
        protected override void ConvertFromBytesForPrimitiveProperties(ReadOnlySpan<byte> span, IncludeChildrenMode includeChildrenMode, int serializedVersionNumber, ref int bytesSoFar)
        {
            base.ConvertFromBytesForPrimitiveProperties(span, OriginalIncludeChildrenMode, serializedVersionNumber, ref bytesSoFar);
        }
        
        protected override int ConvertFromBytesForChildProperties(ReadOnlySpan<byte> span, IncludeChildrenMode includeChildrenMode, int serializedVersionNumber, int indexOfFirstChild, ref int bytesSoFar)
        {
            int totalChildrenBytes = 0;
            totalChildrenBytes = base.ConvertFromBytesForChildProperties(span, OriginalIncludeChildrenMode, serializedVersionNumber, indexOfFirstChild, ref bytesSoFar);
            _Ranges_ByteIndex = indexOfFirstChild + totalChildrenBytes;
            totalChildrenBytes += span.ToInt32(ref bytesSoFar);
            _MemoryRangeCollection_EndByteIndex = indexOfFirstChild + totalChildrenBytes;
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
            if (_Ranges_Accessed && _Ranges != null)
            {
                _Ranges = (List<MemoryRangeByBlockID>) CloneOrChange_List_GMemoryRangeByBlockID_g(_Ranges, l => l.RemoveBufferInHierarchy(), true);
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
            int lengthForLengths = 8;
            
            var previousLengthsPosition = writer.SetLengthsPosition(lengthForLengths);
            WriteChildrenPropertiesIntoBuffer(ref writer, options, includeUniqueID, startPosition);
            writer.ResetLengthsPosition(previousLengthsPosition);
            
        }
        
        protected override void WritePrimitivePropertiesIntoBuffer(ref BufferWriter writer, in LazinatorSerializationOptions options, bool includeUniqueID)
        {
            base.WritePrimitivePropertiesIntoBuffer(ref writer, options, includeUniqueID);
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
            if ((options.IncludeChildrenMode != IncludeChildrenMode.IncludeAllChildren || options.IncludeChildrenMode != OriginalIncludeChildrenMode) && !_Ranges_Accessed)
            {
                var deserialized = Ranges;
            }
            WriteNonLazinatorObject(
            nonLazinatorObject: _Ranges, isBelievedDirty: _Ranges_Accessed || (options.IncludeChildrenMode != OriginalIncludeChildrenMode),
            isAccessed: _Ranges_Accessed, writer: ref writer,
            getChildSliceForFieldFn: () => GetChildSlice(LazinatorMemoryStorage, _Ranges_ByteIndex, _Ranges_ByteLength, null),
            verifyCleanness: false,
            binaryWriterAction: (ref BufferWriter w, bool v) =>
            ConvertToBytes_List_GMemoryRangeByBlockID_g(ref w, _Ranges,
            options));
            if (options.UpdateStoredBuffer)
            {
                _Ranges_ByteIndex = startOfChildPosition - startOfObjectPosition;
                
            }
            if (options.UpdateStoredBuffer)
            {
                _MemoryRangeCollection_EndByteIndex = writer.ActiveMemoryPosition - startOfObjectPosition;
            }
            
        }
        /* Conversion of supported collections and tuples */
        
        private static List<MemoryRangeByBlockID> ConvertFromBytes_List_GMemoryRangeByBlockID_g(LazinatorMemory storage)
        {
            if (storage.Length == 0)
            {
                return default(List<MemoryRangeByBlockID>);
            }
            ReadOnlySpan<byte> span = storage.InitialReadOnlyMemory.Span;
            int bytesSoFar = 0;
            int collectionLength = span.ToDecompressedInt32(ref bytesSoFar);
            
            List<MemoryRangeByBlockID> collection = new List<MemoryRangeByBlockID>(collectionLength);
            for (int itemIndex = 0; itemIndex < collectionLength; itemIndex++)
            {
                int lengthCollectionMember = span.ToInt32(ref bytesSoFar);
                LazinatorMemory childData = storage.Slice(bytesSoFar, lengthCollectionMember);
                var item = ConvertFromBytes_MemoryRangeByBlockID(childData);
                collection.Add(item);
                bytesSoFar += lengthCollectionMember;
            }
            
            return collection;
        }
        
        private static void ConvertToBytes_List_GMemoryRangeByBlockID_g(ref BufferWriter writer, List<MemoryRangeByBlockID> itemToConvert, LazinatorSerializationOptions options)
        {
            if (itemToConvert == default(List<MemoryRangeByBlockID>))
            {
                return;
            }
            CompressedIntegralTypes.WriteCompressedInt(ref writer, itemToConvert.Count);
            int itemToConvertCount = itemToConvert.Count;
            for (int itemIndex = 0; itemIndex < itemToConvertCount; itemIndex++)
            {
                void action(ref BufferWriter w) => ConvertToBytes_MemoryRangeByBlockID(ref w, itemToConvert[itemIndex], options);
                WriteToBinaryWithInt32LengthPrefix(ref writer, action);
            }
        }
        
        private static List<MemoryRangeByBlockID> CloneOrChange_List_GMemoryRangeByBlockID_g(List<MemoryRangeByBlockID> itemToClone, Func<ILazinator, ILazinator> cloneOrChangeFunc, bool avoidCloningIfPossible)
        {
            if (itemToClone == null)
            {
                return default;
            }
            int collectionLength = itemToClone.Count;
            List<MemoryRangeByBlockID> collection = new List<MemoryRangeByBlockID>(collectionLength);
            int itemToCloneCount = itemToClone.Count;
            for (int itemIndex = 0; itemIndex < itemToCloneCount; itemIndex++)
            {
                var itemCopied = (MemoryRangeByBlockID) CloneOrChange_MemoryRangeByBlockID(itemToClone[itemIndex], cloneOrChangeFunc, avoidCloningIfPossible);
                collection.Add(itemCopied);
            }
            return collection;
        }
        
        private static MemoryRangeByBlockID ConvertFromBytes_MemoryRangeByBlockID(LazinatorMemory storage)
        {
            if (storage.Length == 0)
            {
                return default;
            }
            ReadOnlySpan<byte> span = storage.InitialReadOnlyMemory.Span;
            
            int bytesSoFar = 0;
            
            int item1 = span.ToDecompressedInt32(ref bytesSoFar);
            
            int item2 = span.ToDecompressedInt32(ref bytesSoFar);
            
            int item3 = span.ToDecompressedInt32(ref bytesSoFar);
            
            var itemToCreate = new MemoryRangeByBlockID(item1, item2, item3);
            
            return itemToCreate;
        }
        
        private static void ConvertToBytes_MemoryRangeByBlockID(ref BufferWriter writer, MemoryRangeByBlockID itemToConvert, LazinatorSerializationOptions options)
        {
            
            CompressedIntegralTypes.WriteCompressedInt(ref writer, itemToConvert.MemoryBlockIntID);
            
            CompressedIntegralTypes.WriteCompressedInt(ref writer, itemToConvert.OffsetIntoMemoryBlock);
            
            CompressedIntegralTypes.WriteCompressedInt(ref writer, itemToConvert.Length);
        }
        
        private static MemoryRangeByBlockID CloneOrChange_MemoryRangeByBlockID(MemoryRangeByBlockID itemToConvert, Func<ILazinator, ILazinator> cloneOrChangeFunc, bool avoidCloningIfPossible)
        {
            return new MemoryRangeByBlockID((int) (itemToConvert.MemoryBlockIntID), (int) (itemToConvert.OffsetIntoMemoryBlock), (int) (itemToConvert.Length));
        }
        
    }
}
