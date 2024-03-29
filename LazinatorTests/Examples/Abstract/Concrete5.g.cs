//c34d0796-3bf5-2965-10b1-db33288cc648
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by the Lazinator tool, version 0.1.0.432, on 2024/03/23 04:24:48.988 PM.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

#nullable disable
namespace LazinatorTests.Examples.Abstract
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
    public partial class Concrete5 : Abstract4, ILazinator
    {
        /* Property definitions */
        
        protected int _IntList5_ByteIndex;
        protected override int _IntList4_ByteLength => _IntList5_ByteIndex - _IntList4_ByteIndex;
        private int _Concrete5_EndByteIndex;
        protected virtual  int _IntList5_ByteLength => _Concrete5_EndByteIndex - _IntList5_ByteIndex;
        protected override int _OverallEndByteIndex => _Concrete5_EndByteIndex;
        
        
        protected string _String4;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public override string String4
        {
            get
            {
                return _String4;
            }
            set
            {
                IsDirty = true;
                _String4 = value;
            }
        }
        
        protected string _String5;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public string String5
        {
            get
            {
                return _String5;
            }
            set
            {
                IsDirty = true;
                _String5 = value;
            }
        }
        
        protected List<Int32> _IntList4;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public override List<Int32> IntList4
        {
            get
            {
                if (!_IntList4_Accessed)
                {
                    LazinateIntList4();
                }
                IsDirty = true; 
                return _IntList4;
            }
            set
            {
                IsDirty = true;
                DescendantIsDirty = true;
                _IntList4 = value;
                _IntList4_Accessed = true;
            }
        }
        private void LazinateIntList4()
        {
            if (LazinatorMemoryStorage.Length == 0)
            {
                _IntList4 = default(List<Int32>);
            }
            else
            {
                LazinatorMemory childData = GetChildSlice(LazinatorMemoryStorage, _IntList4_ByteIndex, _IntList4_ByteLength, null);_IntList4 = ConvertFromBytes_List_Gint_g(childData);
            }
            _IntList4_Accessed = true;
        }
        
        
        protected List<Int32> _IntList5;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public List<Int32> IntList5
        {
            get
            {
                if (!_IntList5_Accessed)
                {
                    LazinateIntList5();
                }
                IsDirty = true; 
                return _IntList5;
            }
            set
            {
                IsDirty = true;
                DescendantIsDirty = true;
                _IntList5 = value;
                _IntList5_Accessed = true;
            }
        }
        protected bool _IntList5_Accessed;
        private void LazinateIntList5()
        {
            if (LazinatorMemoryStorage.Length == 0)
            {
                _IntList5 = default(List<Int32>);
            }
            else
            {
                LazinatorMemory childData = GetChildSlice(LazinatorMemoryStorage, _IntList5_ByteIndex, _IntList5_ByteLength, null);_IntList5 = ConvertFromBytes_List_Gint_g(childData);
            }
            _IntList5_Accessed = true;
        }
        
        /* Clone overrides */
        
        public Concrete5(IncludeChildrenMode originalIncludeChildrenMode = IncludeChildrenMode.IncludeAllChildren) : base(originalIncludeChildrenMode)
        {
        }
        
        public Concrete5(LazinatorMemory serializedBytes, ILazinator parent = null, IncludeChildrenMode originalIncludeChildrenMode = IncludeChildrenMode.IncludeAllChildren, int? lazinatorObjectVersion = null) : base(serializedBytes, parent, originalIncludeChildrenMode, lazinatorObjectVersion)
        {
        }
        
        public override ILazinator CloneLazinator(IncludeChildrenMode includeChildrenMode = IncludeChildrenMode.IncludeAllChildren, CloneBufferOptions cloneBufferOptions = CloneBufferOptions.IndependentBuffers)
        {
            Concrete5 clone;
            if (cloneBufferOptions == CloneBufferOptions.NoBuffer)
            {
                clone = new Concrete5(includeChildrenMode);
                clone.LazinatorObjectVersion = LazinatorObjectVersion;
                clone = (Concrete5)AssignCloneProperties(clone, includeChildrenMode);
            }
            else
            {
                LazinatorMemory bytes = EncodeOrRecycleToNewBuffer(includeChildrenMode, OriginalIncludeChildrenMode, IsDirty, DescendantIsDirty, false, LazinatorMemoryStorage, this);
                clone = new Concrete5(bytes);
            }
            return clone;
        }
        
        protected override ILazinator AssignCloneProperties(ILazinator clone, IncludeChildrenMode includeChildrenMode)
        {
            base.AssignCloneProperties(clone, includeChildrenMode);
            Concrete5 typedClone = (Concrete5) clone;
            typedClone.String4 = String4;
            typedClone.String5 = String5;
            typedClone.IntList4 = CloneOrChange_List_Gint_g(IntList4, l => l?.CloneLazinator(includeChildrenMode, CloneBufferOptions.NoBuffer), false);
            typedClone.IntList5 = CloneOrChange_List_Gint_g(IntList5, l => l?.CloneLazinator(includeChildrenMode, CloneBufferOptions.NoBuffer), false);
            
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
            yield return ("String4", (object)String4);
            yield return ("String5", (object)String5);
            yield return ("IntList4", (object)IntList4);
            yield return ("IntList5", (object)IntList5);
            yield break;
        }
        
        public override ILazinator ForEachLazinator(Func<ILazinator, ILazinator> changeFunc, bool exploreOnlyDeserializedChildren, bool changeThisLevel)
        {
            base.ForEachLazinator(changeFunc, exploreOnlyDeserializedChildren, false);
            if ((!exploreOnlyDeserializedChildren && IntList4 != null) || (_IntList4_Accessed && _IntList4 != null))
            {
                _IntList4 = (List<Int32>) CloneOrChange_List_Gint_g(_IntList4, l => l?.ForEachLazinator(changeFunc, exploreOnlyDeserializedChildren, true), true);
            }
            if ((!exploreOnlyDeserializedChildren && IntList5 != null) || (_IntList5_Accessed && _IntList5 != null))
            {
                _IntList5 = (List<Int32>) CloneOrChange_List_Gint_g(_IntList5, l => l?.ForEachLazinator(changeFunc, exploreOnlyDeserializedChildren, true), true);
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
            _IntList4 = default;
            _IntList5 = default;
            _IntList4_Accessed = _IntList5_Accessed = false;
            IsDirty = false;
            DescendantIsDirty = false;
            HasChanged = false;
            DescendantHasChanged = false;
        }
        
        /* Conversion */
        
        public override int LazinatorUniqueID => 1039;
        
        protected override bool ContainsOpenGenericParameters => false;
        public override LazinatorGenericIDType LazinatorGenericID => default;
        
        
        public override int LazinatorObjectVersion { get; set; } = 0;
        
        
        protected override int ConvertFromBytesAfterHeader(IncludeChildrenMode includeChildrenMode, int serializedVersionNumber, ref int bytesSoFar)
        {
            ReadOnlySpan<byte> span = LazinatorMemoryStorage.InitialReadOnlyMemory.Span;
            ConvertFromBytesForPrimitiveProperties(span, includeChildrenMode, serializedVersionNumber, ref bytesSoFar);
            int lengthForLengths = 20;
            if (includeChildrenMode != IncludeChildrenMode.ExcludeAllChildren && includeChildrenMode != IncludeChildrenMode.IncludeOnlyIncludableChildren)
            {
                lengthForLengths += 8;
            }
            int totalChildrenSize = ConvertFromBytesForChildLengths(span, includeChildrenMode, serializedVersionNumber, bytesSoFar + lengthForLengths, ref bytesSoFar);
            return bytesSoFar + totalChildrenSize;
        }
        
        protected override void ConvertFromBytesForPrimitiveProperties(ReadOnlySpan<byte> span, IncludeChildrenMode includeChildrenMode, int serializedVersionNumber, ref int bytesSoFar)
        {
            base.ConvertFromBytesForPrimitiveProperties(span, OriginalIncludeChildrenMode, serializedVersionNumber, ref bytesSoFar);
            _String4 = span.ToString_BrotliCompressedWithLength(ref bytesSoFar);
            _String5 = span.ToString_BrotliCompressedWithLength(ref bytesSoFar);
        }
        
        protected override int ConvertFromBytesForChildLengths(ReadOnlySpan<byte> span, IncludeChildrenMode includeChildrenMode, int serializedVersionNumber, int indexOfFirstChild, ref int bytesSoFar)
        {
            int totalChildrenBytes = 0;
            totalChildrenBytes = base.ConvertFromBytesForChildLengths(span, OriginalIncludeChildrenMode, serializedVersionNumber, indexOfFirstChild, ref bytesSoFar);
            _IntList4_ByteIndex = indexOfFirstChild + totalChildrenBytes;
            totalChildrenBytes += span.ToInt32(ref bytesSoFar);
            _IntList5_ByteIndex = indexOfFirstChild + totalChildrenBytes;
            totalChildrenBytes += span.ToInt32(ref bytesSoFar);
            _Concrete5_EndByteIndex = indexOfFirstChild + totalChildrenBytes;
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
            if (_IntList4_Accessed && _IntList4 != null)
            {
                _IntList4 = (List<Int32>) CloneOrChange_List_Gint_g(_IntList4, l => l.RemoveBufferInHierarchy(), true);
            }
            if (_IntList5_Accessed && _IntList5 != null)
            {
                _IntList5 = (List<Int32>) CloneOrChange_List_Gint_g(_IntList5, l => l.RemoveBufferInHierarchy(), true);
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
            int lengthForLengths = 20;
            if (options.IncludeChildrenMode != IncludeChildrenMode.ExcludeAllChildren && options.IncludeChildrenMode != IncludeChildrenMode.IncludeOnlyIncludableChildren)
            {
                lengthForLengths += 8;
            }
            
            var previousLengthsPosition = writer.SetLengthsPosition(lengthForLengths);
            WriteChildrenPropertiesIntoBuffer(ref writer, options, includeUniqueID, startPosition);
            writer.ResetLengthsPosition(previousLengthsPosition);
            
        }
        
        protected override void WritePrimitivePropertiesIntoBuffer(ref BufferWriter writer, in LazinatorSerializationOptions options, bool includeUniqueID)
        {
            base.WritePrimitivePropertiesIntoBuffer(ref writer, options, includeUniqueID);
            EncodeCharAndString.WriteBrotliCompressedWithIntPrefix(ref writer, _String4);
            EncodeCharAndString.WriteBrotliCompressedWithIntPrefix(ref writer, _String5);
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
            if ((options.IncludeChildrenMode != IncludeChildrenMode.IncludeAllChildren || options.IncludeChildrenMode != OriginalIncludeChildrenMode) && !_IntList4_Accessed)
            {
                var deserialized = IntList4;
            }
            WriteNonLazinatorObject(
            nonLazinatorObject: _IntList4, isBelievedDirty: _IntList4_Accessed || (options.IncludeChildrenMode != OriginalIncludeChildrenMode),
            isAccessed: _IntList4_Accessed, writer: ref writer,
            getChildSliceForFieldFn: () => GetChildSlice(LazinatorMemoryStorage, _IntList4_ByteIndex, _IntList4_ByteLength, null),
            verifyCleanness: false,
            binaryWriterAction: (ref BufferWriter w, bool v) =>
            ConvertToBytes_List_Gint_g(ref w, _IntList4,
            options));
            if (options.UpdateStoredBuffer)
            {
                _IntList4_ByteIndex = startOfChildPosition - startOfObjectPosition;
                
            }
            startOfChildPosition = writer.ActiveMemoryPosition;
            if ((options.IncludeChildrenMode != IncludeChildrenMode.IncludeAllChildren || options.IncludeChildrenMode != OriginalIncludeChildrenMode) && !_IntList5_Accessed)
            {
                var deserialized = IntList5;
            }
            WriteNonLazinatorObject(
            nonLazinatorObject: _IntList5, isBelievedDirty: _IntList5_Accessed || (options.IncludeChildrenMode != OriginalIncludeChildrenMode),
            isAccessed: _IntList5_Accessed, writer: ref writer,
            getChildSliceForFieldFn: () => GetChildSlice(LazinatorMemoryStorage, _IntList5_ByteIndex, _IntList5_ByteLength, null),
            verifyCleanness: false,
            binaryWriterAction: (ref BufferWriter w, bool v) =>
            ConvertToBytes_List_Gint_g(ref w, _IntList5,
            options));
            if (options.UpdateStoredBuffer)
            {
                _IntList5_ByteIndex = startOfChildPosition - startOfObjectPosition;
                
            }
            if (options.UpdateStoredBuffer)
            {
                _Concrete5_EndByteIndex = writer.ActiveMemoryPosition - startOfObjectPosition;
            }
            
        }
        /* Conversion of supported collections and tuples */
        
        private static List<Int32> ConvertFromBytes_List_Gint_g(LazinatorMemory storage)
        {
            if (storage.Length == 0)
            {
                return default(List<Int32>);
            }
            ReadOnlySpan<byte> span = storage.InitialReadOnlyMemory.Span;
            int bytesSoFar = 0;
            int collectionLength = span.ToDecompressedInt32(ref bytesSoFar);
            
            List<Int32> collection = new List<Int32>(collectionLength);
            for (int itemIndex = 0; itemIndex < collectionLength; itemIndex++)
            {
                int item = span.ToDecompressedInt32(ref bytesSoFar);
                collection.Add(item);
            }
            
            return collection;
        }
        
        private static void ConvertToBytes_List_Gint_g(ref BufferWriter writer, List<Int32> itemToConvert, LazinatorSerializationOptions options)
        {
            if (itemToConvert == default(List<Int32>))
            {
                return;
            }
            CompressedIntegralTypes.WriteCompressedInt(ref writer, itemToConvert.Count);
            int itemToConvertCount = itemToConvert.Count;
            for (int itemIndex = 0; itemIndex < itemToConvertCount; itemIndex++)
            {
                CompressedIntegralTypes.WriteCompressedInt(ref writer, itemToConvert[itemIndex]);
            }
        }
        
        private static List<Int32> CloneOrChange_List_Gint_g(List<Int32> itemToClone, Func<ILazinator, ILazinator> cloneOrChangeFunc, bool avoidCloningIfPossible)
        {
            if (itemToClone == null)
            {
                return default;
            }
            int collectionLength = itemToClone.Count;
            List<Int32> collection = new List<Int32>(collectionLength);
            int itemToCloneCount = itemToClone.Count;
            for (int itemIndex = 0; itemIndex < itemToCloneCount; itemIndex++)
            {
                var itemCopied = (int) itemToClone[itemIndex];
                collection.Add(itemCopied);
            }
            return collection;
        }
        
    }
}
#nullable restore
