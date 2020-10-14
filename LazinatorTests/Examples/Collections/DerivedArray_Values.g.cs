//978a7a5b-55bc-6938-57a0-d5881cbd81a5
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by the Lazinator tool, version 0.1.0.393
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

#nullable disable
namespace LazinatorTests.Examples.Collections
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
    public partial class DerivedArray_Values : Array_Values, ILazinator
    {
        /* Property definitions */
        
        protected int _MyArrayInt_DerivedLevel_ByteIndex;
        private int _DerivedArray_Values_EndByteIndex;
        protected int _MyArrayInt_DerivedLevel_ByteLength => _DerivedArray_Values_EndByteIndex - _MyArrayInt_DerivedLevel_ByteIndex;
        protected override int _OverallEndByteIndex => _DerivedArray_Values_EndByteIndex;
        
        
        protected Int32[] _MyArrayInt_DerivedLevel;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public Int32[] MyArrayInt_DerivedLevel
        {
            get
            {
                if (!_MyArrayInt_DerivedLevel_Accessed)
                {
                    LazinateMyArrayInt_DerivedLevel();
                } 
                return _MyArrayInt_DerivedLevel;
            }
            set
            {
                IsDirty = true;
                DescendantIsDirty = true;
                _MyArrayInt_DerivedLevel = value;
                _MyArrayInt_DerivedLevel_Dirty = true;
                _MyArrayInt_DerivedLevel_Accessed = true;
            }
        }
        protected bool _MyArrayInt_DerivedLevel_Accessed;
        private void LazinateMyArrayInt_DerivedLevel()
        {
            if (LazinatorMemoryStorage.Length == 0)
            {
                _MyArrayInt_DerivedLevel = default(Int32[]);
                _MyArrayInt_DerivedLevel_Dirty = true; 
            }else
            {
                LazinatorMemory childData = GetChildSlice(LazinatorMemoryStorage, _MyArrayInt_DerivedLevel_ByteIndex, _MyArrayInt_DerivedLevel_ByteLength, true, false, null);
                _MyArrayInt_DerivedLevel = ConvertFromBytes_int_B_b(childData);
            }
            _MyArrayInt_DerivedLevel_Accessed = true;
        }
        
        
        private bool _MyArrayInt_DerivedLevel_Dirty;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public bool MyArrayInt_DerivedLevel_Dirty
        {
            get => _MyArrayInt_DerivedLevel_Dirty;
            set
            {
                if (_MyArrayInt_DerivedLevel_Dirty != value)
                {
                    _MyArrayInt_DerivedLevel_Dirty = value;
                }
                if (value && !IsDirty)
                {
                    IsDirty = true;
                }
            }
        }
        /* Clone overrides */
        
        public DerivedArray_Values(IncludeChildrenMode originalIncludeChildrenMode = IncludeChildrenMode.IncludeAllChildren) : base(originalIncludeChildrenMode)
        {
        }
        
        public DerivedArray_Values(LazinatorMemory serializedBytes, ILazinator parent = null, IncludeChildrenMode originalIncludeChildrenMode = IncludeChildrenMode.IncludeAllChildren, int? lazinatorObjectVersion = null) : base(serializedBytes, parent, originalIncludeChildrenMode, lazinatorObjectVersion)
        {
        }
        
        public override ILazinator CloneLazinator(IncludeChildrenMode includeChildrenMode = IncludeChildrenMode.IncludeAllChildren, CloneBufferOptions cloneBufferOptions = CloneBufferOptions.IndependentBuffers)
        {
            DerivedArray_Values clone;
            if (cloneBufferOptions == CloneBufferOptions.NoBuffer)
            {
                clone = new DerivedArray_Values(includeChildrenMode);
                clone.LazinatorObjectVersion = LazinatorObjectVersion;
                clone = (DerivedArray_Values)AssignCloneProperties(clone, includeChildrenMode);
            }
            else
            {
                LazinatorMemory bytes = EncodeOrRecycleToNewBuffer(includeChildrenMode, OriginalIncludeChildrenMode, false, IsDirty, DescendantIsDirty, false, LazinatorMemoryStorage, false, this);
                clone = new DerivedArray_Values(bytes);
            }
            return clone;
        }
        
        protected override ILazinator AssignCloneProperties(ILazinator clone, IncludeChildrenMode includeChildrenMode)
        {
            base.AssignCloneProperties(clone, includeChildrenMode);
            DerivedArray_Values typedClone = (DerivedArray_Values) clone;
            typedClone.MyArrayInt_DerivedLevel = CloneOrChange_int_B_b(MyArrayInt_DerivedLevel, l => l?.CloneLazinator(includeChildrenMode, CloneBufferOptions.NoBuffer), false);
            
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
            yield return ("MyArrayInt_DerivedLevel", (object)MyArrayInt_DerivedLevel);
            yield break;
        }
        
        public override ILazinator ForEachLazinator(Func<ILazinator, ILazinator> changeFunc, bool exploreOnlyDeserializedChildren, bool changeThisLevel)
        {
            base.ForEachLazinator(changeFunc, exploreOnlyDeserializedChildren, false);
            if ((!exploreOnlyDeserializedChildren && MyArrayInt_DerivedLevel != null) || (_MyArrayInt_DerivedLevel_Accessed && _MyArrayInt_DerivedLevel != null))
            {
                _MyArrayInt_DerivedLevel = (Int32[]) CloneOrChange_int_B_b(_MyArrayInt_DerivedLevel, l => l?.ForEachLazinator(changeFunc, exploreOnlyDeserializedChildren, true), true);
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
            _MyArrayInt_DerivedLevel = default;
            _MyArrayInt_DerivedLevel_Accessed = false;
            IsDirty = false;
            DescendantIsDirty = false;
            HasChanged = false;
            DescendantHasChanged = false;
        }
        
        /* Conversion */
        
        public override int LazinatorUniqueID => 1061;
        
        protected override bool ContainsOpenGenericParameters => false;
        public override LazinatorGenericIDType LazinatorGenericID => default;
        
        
        public override int LazinatorObjectVersion { get; set; } = 0;
        
        
        protected override void ConvertFromBytesAfterHeader(IncludeChildrenMode includeChildrenMode, int serializedVersionNumber, ref int bytesSoFar)
        {
            ReadOnlySpan<byte> span = LazinatorMemoryStorage.InitialMemory.Span;
            ConvertFromBytesForPrimitiveProperties(span, includeChildrenMode, serializedVersionNumber, ref bytesSoFar);
            int lengthForLengths = 16;
            ConvertFromBytesForChildProperties(span, includeChildrenMode, serializedVersionNumber, bytesSoFar + lengthForLengths, ref bytesSoFar);
        }
        
        protected override void ConvertFromBytesForPrimitiveProperties(ReadOnlySpan<byte> span, IncludeChildrenMode includeChildrenMode, int serializedVersionNumber, ref int bytesSoFar)
        {
            base.ConvertFromBytesForPrimitiveProperties(span, OriginalIncludeChildrenMode, serializedVersionNumber, ref bytesSoFar);
        }
        
        protected override int ConvertFromBytesForChildProperties(ReadOnlySpan<byte> span, IncludeChildrenMode includeChildrenMode, int serializedVersionNumber, int indexOfFirstChild, ref int bytesSoFar)
        {
            int totalChildrenBytes = base.ConvertFromBytesForChildProperties(span, OriginalIncludeChildrenMode, serializedVersionNumber, indexOfFirstChild, ref bytesSoFar);
            TabbedText.WriteLine($"Reading length of MyArrayInt_DerivedLevel at byte location {bytesSoFar} to determine location: {indexOfFirstChild + totalChildrenBytes}"); 
            _MyArrayInt_DerivedLevel_ByteIndex = indexOfFirstChild + totalChildrenBytes;
            totalChildrenBytes += span.ToInt32(ref bytesSoFar);
            _DerivedArray_Values_EndByteIndex = indexOfFirstChild + totalChildrenBytes;
            return totalChildrenBytes;
        }
        
        public override void SerializeToExistingBuffer(ref BinaryBufferWriter writer, IncludeChildrenMode includeChildrenMode, bool verifyCleanness, bool updateStoredBuffer)
        {
            TabbedText.WriteLine($"Initiating serialization of LazinatorTests.Examples.Collections.DerivedArray_Values ");
            if (includeChildrenMode != IncludeChildrenMode.IncludeAllChildren)
            {
                updateStoredBuffer = false;
            }
            int startPosition = writer.Position;
            WritePropertiesIntoBuffer(ref writer, includeChildrenMode, verifyCleanness, updateStoredBuffer, true);
            if (updateStoredBuffer)
            {
                UpdateStoredBuffer(ref writer, startPosition, writer.Position - startPosition, includeChildrenMode, false);
            }
        }
        
        public override void UpdateStoredBuffer(ref BinaryBufferWriter writer, int startPosition, int length, IncludeChildrenMode includeChildrenMode, bool updateDeserializedChildren)
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
        
        protected override void UpdateDeserializedChildren(ref BinaryBufferWriter writer, int startPosition)
        {
            base.UpdateDeserializedChildren(ref writer, startPosition);
            if (_MyArrayInt_DerivedLevel_Accessed && _MyArrayInt_DerivedLevel != null)
            {
                _MyArrayInt_DerivedLevel = (Int32[]) CloneOrChange_int_B_b(_MyArrayInt_DerivedLevel, l => l.RemoveBufferInHierarchy(), true);
            }
        }
        
        
        protected override void WritePropertiesIntoBuffer(ref BinaryBufferWriter writer, IncludeChildrenMode includeChildrenMode, bool verifyCleanness, bool updateStoredBuffer, bool includeUniqueID)
        {
            int startPosition = writer.Position;
            TabbedText.WriteLine($"Writing properties for LazinatorTests.Examples.Collections.DerivedArray_Values starting at {writer.Position}.");
            TabbedText.WriteLine($"Includes? uniqueID {(LazinatorGenericID.IsEmpty ? LazinatorUniqueID.ToString() : String.Join("","",LazinatorGenericID.TypeAndInnerTypeIDs.ToArray()))} {includeUniqueID}, Lazinator version {Lazinator.Support.LazinatorVersionInfo.LazinatorIntVersion} True, Object version {LazinatorObjectVersion} True, IncludeChildrenMode {includeChildrenMode} True");
            TabbedText.WriteLine($"IsDirty {IsDirty} DescendantIsDirty {DescendantIsDirty} HasParentClass {LazinatorParents.Any()}");
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
            writer.Write((byte)includeChildrenMode);
            // write properties
            
            WritePrimitivePropertiesIntoBuffer(ref writer, includeChildrenMode, verifyCleanness, updateStoredBuffer, includeUniqueID);
            int lengthForLengths = 16;
            Span<byte> lengthsSpan = writer.FreeSpan.Slice(0, lengthForLengths);
            writer.Skip(lengthForLengths);TabbedText.WriteLine($"Byte {writer.Position}, Leaving {lengthForLengths} bytes to store lengths of child objects");
            WriteChildrenPropertiesIntoBuffer(ref writer, includeChildrenMode, verifyCleanness, updateStoredBuffer, includeUniqueID, startPosition, lengthsSpan);
            TabbedText.WriteLine($"Byte {writer.Position} (end of DerivedArray_Values) ");
        }
        
        protected override void WritePrimitivePropertiesIntoBuffer(ref BinaryBufferWriter writer, IncludeChildrenMode includeChildrenMode, bool verifyCleanness, bool updateStoredBuffer, bool includeUniqueID)
        {
            base.WritePrimitivePropertiesIntoBuffer(ref writer, includeChildrenMode, verifyCleanness, updateStoredBuffer, includeUniqueID);
        }
        
        protected override void WriteChildrenPropertiesIntoBuffer(ref BinaryBufferWriter writer, IncludeChildrenMode includeChildrenMode, bool verifyCleanness, bool updateStoredBuffer, bool includeUniqueID, int startOfObjectPosition, Span<byte> lengthsSpan)
        {
            base.WriteChildrenPropertiesIntoBuffer(ref writer, includeChildrenMode, verifyCleanness, updateStoredBuffer, includeUniqueID, startOfObjectPosition, lengthsSpan);
            int startOfChildPosition = 0;
            TabbedText.WriteLine($"Byte {writer.Position}, MyArrayInt_DerivedLevel (accessed? {_MyArrayInt_DerivedLevel_Accessed}) (dirty? {_MyArrayInt_DerivedLevel_Dirty})");
            TabbedText.Tabs++;
            startOfChildPosition = writer.Position;
            if ((includeChildrenMode != IncludeChildrenMode.IncludeAllChildren || includeChildrenMode != OriginalIncludeChildrenMode) && !_MyArrayInt_DerivedLevel_Accessed)
            {
                var deserialized = MyArrayInt_DerivedLevel;
            }
            WriteNonLazinatorObject(
            nonLazinatorObject: _MyArrayInt_DerivedLevel, isBelievedDirty: MyArrayInt_DerivedLevel_Dirty || (includeChildrenMode != OriginalIncludeChildrenMode),
            isAccessed: _MyArrayInt_DerivedLevel_Accessed, writer: ref writer,
            getChildSliceForFieldFn: () => GetChildSlice(LazinatorMemoryStorage, _MyArrayInt_DerivedLevel_ByteIndex, _MyArrayInt_DerivedLevel_ByteLength, true, false, null),
            verifyCleanness: verifyCleanness,
            binaryWriterAction: (ref BinaryBufferWriter w, bool v) =>
            ConvertToBytes_int_B_b(ref w, _MyArrayInt_DerivedLevel,
            includeChildrenMode, v, updateStoredBuffer),
            lengthsSpan: ref lengthsSpan);
            if (updateStoredBuffer)
            {
                _MyArrayInt_DerivedLevel_ByteIndex = startOfChildPosition - startOfObjectPosition;
                
            }
            TabbedText.Tabs--;
            if (updateStoredBuffer)
            {
                _DerivedArray_Values_EndByteIndex = writer.Position - startOfObjectPosition;
            }
        }
        
        /* Conversion of supported collections and tuples */
        
        private static Int32[] ConvertFromBytes_int_B_b(LazinatorMemory storage)
        {
            if (storage.Length == 0)
            {
                return default(Int32[]);
            }
            ReadOnlySpan<byte> span = storage.InitialMemory.Span;
            int bytesSoFar = 0;
            int collectionLength = span.ToDecompressedInt32(ref bytesSoFar);
            
            Int32[] collection = new int[collectionLength];
            for (int itemIndex = 0; itemIndex < collectionLength; itemIndex++)
            {
                int item = span.ToDecompressedInt32(ref bytesSoFar);
                collection[itemIndex] = item;
            }
            
            return collection;
        }
        
        private static void ConvertToBytes_int_B_b(ref BinaryBufferWriter writer, Int32[] itemToConvert, IncludeChildrenMode includeChildrenMode, bool verifyCleanness, bool updateStoredBuffer)
        {
            if (itemToConvert == default(Int32[]))
            {
                return;
            }
            CompressedIntegralTypes.WriteCompressedInt(ref writer, itemToConvert.Length);
            int itemToConvertCount = itemToConvert.Length;
            for (int itemIndex = 0; itemIndex < itemToConvertCount; itemIndex++)
            {
                CompressedIntegralTypes.WriteCompressedInt(ref writer, itemToConvert[itemIndex]);
            }
        }
        
        private static Int32[] CloneOrChange_int_B_b(Int32[] itemToClone, Func<ILazinator, ILazinator> cloneOrChangeFunc, bool avoidCloningIfPossible)
        {
            if (itemToClone == null)
            {
                return default;
            }
            int collectionLength = itemToClone.Length;
            Int32[] collection = new int[collectionLength];
            int itemToCloneCount = itemToClone.Length;
            for (int itemIndex = 0; itemIndex < itemToCloneCount; itemIndex++)
            {
                var itemCopied = (int) itemToClone[itemIndex];
                collection[itemIndex] = itemCopied;
            }
            return collection;
        }
        
    }
}
