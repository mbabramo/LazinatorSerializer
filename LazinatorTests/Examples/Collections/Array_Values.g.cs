//005eba2e-d10b-bf1b-29f6-f675118ace74
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by the Lazinator tool, version 0.1.0.411, on 2024/02/04 12:07:38.634 PM.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

#nullable disable
namespace LazinatorTests.Examples.Collections
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
    public partial class Array_Values : ILazinator
    {
        public bool IsStruct => false;
        
        public virtual bool IsNullableContext => false;
        /* Property definitions */
        
        protected int _MyArrayInt_ByteIndex;
        protected int _MyArrayNullableInt_ByteIndex;
        protected int _MyJaggedArrayInt_ByteIndex;
        protected virtual int _MyArrayInt_ByteLength => _MyArrayNullableInt_ByteIndex - _MyArrayInt_ByteIndex;
        protected virtual int _MyArrayNullableInt_ByteLength => _MyJaggedArrayInt_ByteIndex - _MyArrayNullableInt_ByteIndex;
        private int _Array_Values_EndByteIndex;
        protected virtual  int _MyJaggedArrayInt_ByteLength => _Array_Values_EndByteIndex - _MyJaggedArrayInt_ByteIndex;
        protected virtual int _OverallEndByteIndex => _Array_Values_EndByteIndex;
        
        
        protected Int32[] _MyArrayInt;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public Int32[] MyArrayInt
        {
            get
            {
                if (!_MyArrayInt_Accessed)
                {
                    LazinateMyArrayInt();
                } 
                return _MyArrayInt;
            }
            set
            {
                IsDirty = true;
                DescendantIsDirty = true;
                _MyArrayInt = value;
                _MyArrayInt_Dirty = true;
                _MyArrayInt_Accessed = true;
            }
        }
        protected bool _MyArrayInt_Accessed;
        private void LazinateMyArrayInt()
        {
            if (LazinatorMemoryStorage.Length == 0)
            {
                _MyArrayInt = default(Int32[]);
                _MyArrayInt_Dirty = true; 
            }
            else
            {
                LazinatorMemory childData = GetChildSlice(LazinatorMemoryStorage, _MyArrayInt_ByteIndex, _MyArrayInt_ByteLength, null);_MyArrayInt = ConvertFromBytes_int_B_b(childData);
            }
            _MyArrayInt_Accessed = true;
        }
        
        
        private bool _MyArrayInt_Dirty;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public bool MyArrayInt_Dirty
        {
            get => _MyArrayInt_Dirty;
            set
            {
                if (_MyArrayInt_Dirty != value)
                {
                    _MyArrayInt_Dirty = value;
                }
                if (value && !IsDirty)
                {
                    IsDirty = true;
                }
            }
        }
        
        protected Int32?[] _MyArrayNullableInt;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public Int32?[] MyArrayNullableInt
        {
            get
            {
                if (!_MyArrayNullableInt_Accessed)
                {
                    LazinateMyArrayNullableInt();
                }
                IsDirty = true; 
                return _MyArrayNullableInt;
            }
            set
            {
                IsDirty = true;
                DescendantIsDirty = true;
                _MyArrayNullableInt = value;
                _MyArrayNullableInt_Accessed = true;
            }
        }
        protected bool _MyArrayNullableInt_Accessed;
        private void LazinateMyArrayNullableInt()
        {
            if (LazinatorMemoryStorage.Length == 0)
            {
                _MyArrayNullableInt = default(Int32?[]);
            }
            else
            {
                LazinatorMemory childData = GetChildSlice(LazinatorMemoryStorage, _MyArrayNullableInt_ByteIndex, _MyArrayNullableInt_ByteLength, null);_MyArrayNullableInt = ConvertFromBytes_int_n_B_b(childData);
            }
            _MyArrayNullableInt_Accessed = true;
        }
        
        
        protected Int32[][] _MyJaggedArrayInt;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public Int32[][] MyJaggedArrayInt
        {
            get
            {
                if (!_MyJaggedArrayInt_Accessed)
                {
                    LazinateMyJaggedArrayInt();
                }
                IsDirty = true; 
                return _MyJaggedArrayInt;
            }
            set
            {
                IsDirty = true;
                DescendantIsDirty = true;
                _MyJaggedArrayInt = value;
                _MyJaggedArrayInt_Accessed = true;
            }
        }
        protected bool _MyJaggedArrayInt_Accessed;
        private void LazinateMyJaggedArrayInt()
        {
            if (LazinatorMemoryStorage.Length == 0)
            {
                _MyJaggedArrayInt = default(Int32[][]);
            }
            else
            {
                LazinatorMemory childData = GetChildSlice(LazinatorMemoryStorage, _MyJaggedArrayInt_ByteIndex, _MyJaggedArrayInt_ByteLength, null);_MyJaggedArrayInt = ConvertFromBytes_int_B_b_B_b(childData);
            }
            _MyJaggedArrayInt_Accessed = true;
        }
        
        
        /* Serialization, deserialization, and object relationships */
        
        public Array_Values(IncludeChildrenMode originalIncludeChildrenMode = IncludeChildrenMode.IncludeAllChildren)
        {
            OriginalIncludeChildrenMode = originalIncludeChildrenMode;
        }
        
        public Array_Values(LazinatorMemory serializedBytes, ILazinator parent = null, IncludeChildrenMode originalIncludeChildrenMode = IncludeChildrenMode.IncludeAllChildren, int? lazinatorObjectVersion = null)
        {
            if (lazinatorObjectVersion != null)
            {
                LazinatorObjectVersion = (int) lazinatorObjectVersion;
            }
            OriginalIncludeChildrenMode = originalIncludeChildrenMode;
            LazinatorParents = new LazinatorParentsCollection(parent, null);
            DeserializeLazinator(serializedBytes);
            HasChanged = false;
            DescendantHasChanged = false;
        }
        
        public virtual LazinatorParentsCollection LazinatorParents { get; set; }
        
        public virtual LazinatorMemory LazinatorMemoryStorage
        {
            get;
            set;
        }
        
        public virtual IncludeChildrenMode OriginalIncludeChildrenMode { get; set; }
        
        public virtual bool HasChanged { get; set; }
        
        protected bool _IsDirty;
        public virtual bool IsDirty
        {
            [DebuggerStepThrough]
            get => _IsDirty|| LazinatorMemoryStorage.Length == 0;
            [DebuggerStepThrough]
            set
            {
                if (_IsDirty != value)
                {
                    _IsDirty = value;
                    if (_IsDirty)
                    {
                        LazinatorParents.InformParentsOfDirtiness();
                        HasChanged = true;
                    }
                }
            }
        }
        
        protected bool _DescendantHasChanged;
        public virtual bool DescendantHasChanged
        {
            [DebuggerStepThrough]
            get => _DescendantHasChanged;
            [DebuggerStepThrough]
            set
            {
                _DescendantHasChanged = value;
            }
        }
        
        protected bool _DescendantIsDirty;
        public virtual bool DescendantIsDirty
        {
            [DebuggerStepThrough]
            get => _DescendantIsDirty;
            [DebuggerStepThrough]
            set
            {
                if (_DescendantIsDirty != value)
                {
                    _DescendantIsDirty = value;
                    if (_DescendantIsDirty)
                    {
                        LazinatorParents.InformParentsOfDirtiness();
                        _DescendantHasChanged = true;
                    }
                }
            }
        }
        
        public virtual bool NonBinaryHash32 => false;
        
        protected virtual void DeserializeLazinator(LazinatorMemory serializedBytes)
        {
            LazinatorMemoryStorage = serializedBytes;
            int length = Deserialize();
            if (length != LazinatorMemoryStorage.Length)
            {
                LazinatorMemoryStorage = LazinatorMemoryStorage.Slice(0, length);
            }
        }
        
        protected virtual int Deserialize()
        {
            FreeInMemoryObjects();
            int bytesSoFar = 0;
            ReadOnlySpan<byte> span = LazinatorMemoryStorage.InitialReadOnlyMemory.Span;
            if (span.Length == 0)
            {
                return 0;
            }
            
            ReadGenericIDIfApplicable(ContainsOpenGenericParameters, LazinatorUniqueID, span, ref bytesSoFar);
            
            int lazinatorLibraryVersion = span.ToDecompressedInt32(ref bytesSoFar);
            
            int serializedVersionNumber = span.ToDecompressedInt32(ref bytesSoFar);
            
            OriginalIncludeChildrenMode = (IncludeChildrenMode)span.ToByte(ref bytesSoFar);
            
            int totalBytes = ConvertFromBytesAfterHeader(OriginalIncludeChildrenMode, serializedVersionNumber, ref bytesSoFar);
            return _OverallEndByteIndex;
        }
        
        public virtual void SerializeLazinator()
        {
            if (!IsDirty && !DescendantIsDirty && LazinatorMemoryStorage.Length > 0 && OriginalIncludeChildrenMode == IncludeChildrenMode.IncludeAllChildren)
            {
                return;
                
            }
            var previousBuffer = LazinatorMemoryStorage;
            if (LazinatorMemoryStorage.IsEmpty || IncludeChildrenMode.IncludeAllChildren != OriginalIncludeChildrenMode || (IsDirty || DescendantIsDirty))
            {
                LazinatorMemoryStorage = EncodeToNewBuffer(LazinatorSerializationOptions.Default);
            }
            else
            {
                BufferWriter writer = new BufferWriter(LazinatorMemoryStorage.LengthInt ?? 0);
                LazinatorMemoryStorage.WriteToBuffer(ref writer);
                LazinatorMemoryStorage = writer.LazinatorMemory;
            }
            OriginalIncludeChildrenMode = IncludeChildrenMode.IncludeAllChildren;
            if (!LazinatorParents.Any())
            {
                previousBuffer.Dispose();
            }
        }
        
        public virtual LazinatorMemory SerializeLazinator(in LazinatorSerializationOptions options) 
        {
            if (LazinatorMemoryStorage.IsEmpty || options.IncludeChildrenMode != OriginalIncludeChildrenMode || (options.SerializeDiffs || options.VerifyCleanness || IsDirty || (options.IncludeChildrenMode != IncludeChildrenMode.ExcludeAllChildren && DescendantIsDirty)))
            {
                return EncodeToNewBuffer(options);
            }
            BufferWriter writer = new BufferWriter(LazinatorMemoryStorage.LengthInt ?? 0);
            LazinatorMemoryStorage.WriteToBuffer(ref writer);
            return writer.LazinatorMemory;
        }
        
        protected virtual LazinatorMemory EncodeToNewBuffer(in LazinatorSerializationOptions options) 
        {
            int bufferSize = LazinatorMemoryStorage.Length == 0 ? ExpandableBytes.DefaultMinBufferSize : LazinatorMemoryStorage.LengthInt ?? ExpandableBytes.DefaultMinBufferSize;
            BufferWriter writer = new BufferWriter(bufferSize);
            SerializeToExistingBuffer(ref writer, options);
            return writer.LazinatorMemory;
        }
        
        public virtual ILazinator CloneLazinator(IncludeChildrenMode includeChildrenMode = IncludeChildrenMode.IncludeAllChildren, CloneBufferOptions cloneBufferOptions = CloneBufferOptions.IndependentBuffers)
        {
            Array_Values clone;
            if (cloneBufferOptions == CloneBufferOptions.NoBuffer)
            {
                clone = new Array_Values(includeChildrenMode);
                clone.LazinatorObjectVersion = LazinatorObjectVersion;
                clone = (Array_Values)AssignCloneProperties(clone, includeChildrenMode);
            }
            else
            {
                LazinatorMemory bytes = EncodeOrRecycleToNewBuffer(includeChildrenMode, OriginalIncludeChildrenMode, IsDirty, DescendantIsDirty, false, LazinatorMemoryStorage, this);
                clone = new Array_Values(bytes);
            }
            return clone;
        }
        
        protected virtual ILazinator AssignCloneProperties(ILazinator clone, IncludeChildrenMode includeChildrenMode)
        {
            clone.FreeInMemoryObjects();
            Array_Values typedClone = (Array_Values) clone;
            typedClone.MyArrayInt = CloneOrChange_int_B_b(MyArrayInt, l => l?.CloneLazinator(includeChildrenMode, CloneBufferOptions.NoBuffer), false);
            typedClone.MyArrayNullableInt = CloneOrChange_int_n_B_b(MyArrayNullableInt, l => l?.CloneLazinator(includeChildrenMode, CloneBufferOptions.NoBuffer), false);
            typedClone.MyJaggedArrayInt = CloneOrChange_int_B_b_B_b(MyJaggedArrayInt, l => l?.CloneLazinator(includeChildrenMode, CloneBufferOptions.NoBuffer), false);
            
            return typedClone;
        }
        
        
        public IEnumerable<ILazinator> EnumerateLazinatorNodes(Func<ILazinator, bool> matchCriterion, bool stopExploringBelowMatch, Func<ILazinator, bool> exploreCriterion, bool exploreOnlyDeserializedChildren, bool enumerateNulls)
        {
            bool match = (matchCriterion == null) ? true : matchCriterion(this);
            bool explore = (!match || !stopExploringBelowMatch) && ((exploreCriterion == null) ? true : exploreCriterion(this));
            if (match)
            {
                yield return this;
            }
            if (explore)
            {
                foreach (var item in EnumerateLazinatorDescendants(matchCriterion, stopExploringBelowMatch, exploreCriterion, exploreOnlyDeserializedChildren, enumerateNulls))
                {
                    yield return item.descendant;
                }
            }
        }
        
        public virtual IEnumerable<(string propertyName, ILazinator descendant)> EnumerateLazinatorDescendants(Func<ILazinator, bool> matchCriterion, bool stopExploringBelowMatch, Func<ILazinator, bool> exploreCriterion, bool exploreOnlyDeserializedChildren, bool enumerateNulls)
        {
            yield break;
        }
        
        
        public virtual IEnumerable<(string propertyName, object descendant)> EnumerateNonLazinatorProperties()
        {
            yield return ("MyArrayInt", (object)MyArrayInt);
            yield return ("MyArrayNullableInt", (object)MyArrayNullableInt);
            yield return ("MyJaggedArrayInt", (object)MyJaggedArrayInt);
            yield break;
        }
        
        public virtual ILazinator ForEachLazinator(Func<ILazinator, ILazinator> changeFunc, bool exploreOnlyDeserializedChildren, bool changeThisLevel)
        {
            if ((!exploreOnlyDeserializedChildren && MyArrayInt != null) || (_MyArrayInt_Accessed && _MyArrayInt != null))
            {
                _MyArrayInt = (Int32[]) CloneOrChange_int_B_b(_MyArrayInt, l => l?.ForEachLazinator(changeFunc, exploreOnlyDeserializedChildren, true), true);
            }
            if ((!exploreOnlyDeserializedChildren && MyArrayNullableInt != null) || (_MyArrayNullableInt_Accessed && _MyArrayNullableInt != null))
            {
                _MyArrayNullableInt = (Int32?[]) CloneOrChange_int_n_B_b(_MyArrayNullableInt, l => l?.ForEachLazinator(changeFunc, exploreOnlyDeserializedChildren, true), true);
            }
            if ((!exploreOnlyDeserializedChildren && MyJaggedArrayInt != null) || (_MyJaggedArrayInt_Accessed && _MyJaggedArrayInt != null))
            {
                _MyJaggedArrayInt = (Int32[][]) CloneOrChange_int_B_b_B_b(_MyJaggedArrayInt, l => l?.ForEachLazinator(changeFunc, exploreOnlyDeserializedChildren, true), true);
            }
            if (changeThisLevel && changeFunc != null)
            {
                return changeFunc(this);
            }
            return this;
        }
        
        public virtual void FreeInMemoryObjects()
        {
            _MyArrayInt = default;
            _MyArrayNullableInt = default;
            _MyJaggedArrayInt = default;
            _MyArrayInt_Accessed = _MyArrayNullableInt_Accessed = _MyJaggedArrayInt_Accessed = false;
            IsDirty = false;
            DescendantIsDirty = false;
            HasChanged = false;
            DescendantHasChanged = false;
        }
        
        /* Conversion */
        
        public virtual int LazinatorUniqueID => 1001;
        
        protected virtual bool ContainsOpenGenericParameters => false;
        public virtual LazinatorGenericIDType LazinatorGenericID => default;
        
        
        public virtual int LazinatorObjectVersion { get; set; } = 0;
        
        
        protected virtual int ConvertFromBytesAfterHeader(IncludeChildrenMode includeChildrenMode, int serializedVersionNumber, ref int bytesSoFar)
        {
            ReadOnlySpan<byte> span = LazinatorMemoryStorage.InitialReadOnlyMemory.Span;
            ConvertFromBytesForPrimitiveProperties(span, includeChildrenMode, serializedVersionNumber, ref bytesSoFar);
            int lengthForLengths = 12;
            int totalChildrenSize = ConvertFromBytesForChildLengths(span, includeChildrenMode, serializedVersionNumber, bytesSoFar + lengthForLengths, ref bytesSoFar);;
            return bytesSoFar + totalChildrenSize;
        }
        
        protected virtual void ConvertFromBytesForPrimitiveProperties(ReadOnlySpan<byte> span, IncludeChildrenMode includeChildrenMode, int serializedVersionNumber, ref int bytesSoFar)
        {
        }
        
        protected virtual int ConvertFromBytesForChildLengths(ReadOnlySpan<byte> span, IncludeChildrenMode includeChildrenMode, int serializedVersionNumber, int indexOfFirstChild, ref int bytesSoFar)
        {
            int totalChildrenBytes = 0;
            _MyArrayInt_ByteIndex = indexOfFirstChild + totalChildrenBytes;
            totalChildrenBytes += span.ToInt32(ref bytesSoFar);
            _MyArrayNullableInt_ByteIndex = indexOfFirstChild + totalChildrenBytes;
            totalChildrenBytes += span.ToInt32(ref bytesSoFar);
            _MyJaggedArrayInt_ByteIndex = indexOfFirstChild + totalChildrenBytes;
            totalChildrenBytes += span.ToInt32(ref bytesSoFar);
            _Array_Values_EndByteIndex = indexOfFirstChild + totalChildrenBytes;
            return totalChildrenBytes;
        }
        
        public virtual void SerializeToExistingBuffer(ref BufferWriter writer, in LazinatorSerializationOptions options)
        {
            int startPosition = writer.ActiveMemoryPosition;
            WritePropertiesIntoBuffer(ref writer, options, true);
            if (options.UpdateStoredBuffer)
            {
                UpdateStoredBuffer(ref writer, startPosition, writer.ActiveMemoryPosition - startPosition, options.IncludeChildrenMode, false);
            }
        }
        
        public virtual void UpdateStoredBuffer(ref BufferWriter writer, long startPosition, long length, IncludeChildrenMode includeChildrenMode, bool updateDeserializedChildren)
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
        
        protected virtual void UpdateDeserializedChildren(ref BufferWriter writer, long startPosition)
        {
            if (_MyArrayInt_Accessed && _MyArrayInt != null)
            {
                _MyArrayInt = (Int32[]) CloneOrChange_int_B_b(_MyArrayInt, l => l.RemoveBufferInHierarchy(), true);
            }
            if (_MyArrayNullableInt_Accessed && _MyArrayNullableInt != null)
            {
                _MyArrayNullableInt = (Int32?[]) CloneOrChange_int_n_B_b(_MyArrayNullableInt, l => l.RemoveBufferInHierarchy(), true);
            }
            if (_MyJaggedArrayInt_Accessed && _MyJaggedArrayInt != null)
            {
                _MyJaggedArrayInt = (Int32[][]) CloneOrChange_int_B_b_B_b(_MyJaggedArrayInt, l => l.RemoveBufferInHierarchy(), true);
            }
            
        }
        
        
        protected virtual void WritePropertiesIntoBuffer(ref BufferWriter writer, in LazinatorSerializationOptions options, bool includeUniqueID)
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
            
            
            int lengthForLengths = 12;
            
            var previousLengthsPosition = writer.SetLengthsPosition(lengthForLengths);
            WriteChildrenPropertiesIntoBuffer(ref writer, options, includeUniqueID, startPosition);
            writer.ResetLengthsPosition(previousLengthsPosition);
            
        }
        
        protected virtual void WritePrimitivePropertiesIntoBuffer(ref BufferWriter writer, in LazinatorSerializationOptions options, bool includeUniqueID)
        {
        }
        protected virtual void WriteChildrenPropertiesIntoBuffer(ref BufferWriter writer, LazinatorSerializationOptions options, bool includeUniqueID, int startOfObjectPosition)
        {
            if (options.SplittingPossible)
            {
                options = options.WithoutSplittingPossible();
            }
            int startOfChildPosition = 0;
            startOfChildPosition = writer.ActiveMemoryPosition;
            if ((options.IncludeChildrenMode != IncludeChildrenMode.IncludeAllChildren || options.IncludeChildrenMode != OriginalIncludeChildrenMode) && !_MyArrayInt_Accessed)
            {
                var deserialized = MyArrayInt;
            }
            WriteNonLazinatorObject(
            nonLazinatorObject: _MyArrayInt, isBelievedDirty: MyArrayInt_Dirty || (options.IncludeChildrenMode != OriginalIncludeChildrenMode),
            isAccessed: _MyArrayInt_Accessed, writer: ref writer,
            getChildSliceForFieldFn: () => GetChildSlice(LazinatorMemoryStorage, _MyArrayInt_ByteIndex, _MyArrayInt_ByteLength, null),
            verifyCleanness: options.VerifyCleanness,
            binaryWriterAction: (ref BufferWriter w, bool v) =>
            ConvertToBytes_int_B_b(ref w, _MyArrayInt,
            options));
            if (options.UpdateStoredBuffer)
            {
                _MyArrayInt_ByteIndex = startOfChildPosition - startOfObjectPosition;
                
            }
            startOfChildPosition = writer.ActiveMemoryPosition;
            if ((options.IncludeChildrenMode != IncludeChildrenMode.IncludeAllChildren || options.IncludeChildrenMode != OriginalIncludeChildrenMode) && !_MyArrayNullableInt_Accessed)
            {
                var deserialized = MyArrayNullableInt;
            }
            WriteNonLazinatorObject(
            nonLazinatorObject: _MyArrayNullableInt, isBelievedDirty: _MyArrayNullableInt_Accessed || (options.IncludeChildrenMode != OriginalIncludeChildrenMode),
            isAccessed: _MyArrayNullableInt_Accessed, writer: ref writer,
            getChildSliceForFieldFn: () => GetChildSlice(LazinatorMemoryStorage, _MyArrayNullableInt_ByteIndex, _MyArrayNullableInt_ByteLength, null),
            verifyCleanness: false,
            binaryWriterAction: (ref BufferWriter w, bool v) =>
            ConvertToBytes_int_n_B_b(ref w, _MyArrayNullableInt,
            options));
            if (options.UpdateStoredBuffer)
            {
                _MyArrayNullableInt_ByteIndex = startOfChildPosition - startOfObjectPosition;
                
            }
            startOfChildPosition = writer.ActiveMemoryPosition;
            if ((options.IncludeChildrenMode != IncludeChildrenMode.IncludeAllChildren || options.IncludeChildrenMode != OriginalIncludeChildrenMode) && !_MyJaggedArrayInt_Accessed)
            {
                var deserialized = MyJaggedArrayInt;
            }
            WriteNonLazinatorObject(
            nonLazinatorObject: _MyJaggedArrayInt, isBelievedDirty: _MyJaggedArrayInt_Accessed || (options.IncludeChildrenMode != OriginalIncludeChildrenMode),
            isAccessed: _MyJaggedArrayInt_Accessed, writer: ref writer,
            getChildSliceForFieldFn: () => GetChildSlice(LazinatorMemoryStorage, _MyJaggedArrayInt_ByteIndex, _MyJaggedArrayInt_ByteLength, null),
            verifyCleanness: false,
            binaryWriterAction: (ref BufferWriter w, bool v) =>
            ConvertToBytes_int_B_b_B_b(ref w, _MyJaggedArrayInt,
            options));
            if (options.UpdateStoredBuffer)
            {
                _MyJaggedArrayInt_ByteIndex = startOfChildPosition - startOfObjectPosition;
                
            }
            if (options.UpdateStoredBuffer)
            {
                _Array_Values_EndByteIndex = writer.ActiveMemoryPosition - startOfObjectPosition;
            }
            
        }
        /* Conversion of supported collections and tuples */
        
        private static Int32[] ConvertFromBytes_int_B_b(LazinatorMemory storage)
        {
            if (storage.Length == 0)
            {
                return default(Int32[]);
            }
            ReadOnlySpan<byte> span = storage.InitialReadOnlyMemory.Span;
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
        
        private static void ConvertToBytes_int_B_b(ref BufferWriter writer, Int32[] itemToConvert, LazinatorSerializationOptions options)
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
        
        private static Int32?[] ConvertFromBytes_int_n_B_b(LazinatorMemory storage)
        {
            if (storage.Length == 0)
            {
                return default(Int32?[]);
            }
            ReadOnlySpan<byte> span = storage.InitialReadOnlyMemory.Span;
            int bytesSoFar = 0;
            int collectionLength = span.ToDecompressedInt32(ref bytesSoFar);
            
            Int32?[] collection = new int?[collectionLength];
            for (int itemIndex = 0; itemIndex < collectionLength; itemIndex++)
            {
                int? item = span.ToDecompressedNullableInt32(ref bytesSoFar);
                collection[itemIndex] = item;
            }
            
            return collection;
        }
        
        private static void ConvertToBytes_int_n_B_b(ref BufferWriter writer, Int32?[] itemToConvert, LazinatorSerializationOptions options)
        {
            if (itemToConvert == default(Int32?[]))
            {
                return;
            }
            CompressedIntegralTypes.WriteCompressedInt(ref writer, itemToConvert.Length);
            int itemToConvertCount = itemToConvert.Length;
            for (int itemIndex = 0; itemIndex < itemToConvertCount; itemIndex++)
            {
                CompressedIntegralTypes.WriteCompressedNullableInt(ref writer, itemToConvert[itemIndex]);
            }
        }
        
        private static Int32?[] CloneOrChange_int_n_B_b(Int32?[] itemToClone, Func<ILazinator, ILazinator> cloneOrChangeFunc, bool avoidCloningIfPossible)
        {
            if (itemToClone == null)
            {
                return default;
            }
            int collectionLength = itemToClone.Length;
            Int32?[] collection = new int?[collectionLength];
            int itemToCloneCount = itemToClone.Length;
            for (int itemIndex = 0; itemIndex < itemToCloneCount; itemIndex++)
            {
                if (itemToClone[itemIndex] == null)
                {
                    collection[itemIndex] = default(int?);
                }
                else
                {
                    var itemCopied = (int?) itemToClone[itemIndex];
                    collection[itemIndex] = itemCopied;
                }
            }
            return collection;
        }
        
        private static Int32[][] ConvertFromBytes_int_B_b_B_b(LazinatorMemory storage)
        {
            if (storage.Length == 0)
            {
                return default(Int32[][]);
            }
            ReadOnlySpan<byte> span = storage.InitialReadOnlyMemory.Span;
            int bytesSoFar = 0;
            int collectionLength = span.ToDecompressedInt32(ref bytesSoFar);
            
            Int32[][] collection = new Int32[collectionLength][];
            for (int itemIndex = 0; itemIndex < collectionLength; itemIndex++)
            {
                int lengthCollectionMember = span.ToInt32(ref bytesSoFar);
                if (lengthCollectionMember == 0)
                {
                    collection[itemIndex] = default(Int32[]);
                }
                else
                {
                    LazinatorMemory childData = storage.Slice(bytesSoFar, lengthCollectionMember);
                    var item = ConvertFromBytes_int_B_b(childData);
                    collection[itemIndex] = item;
                }bytesSoFar += lengthCollectionMember;
            }
            
            return collection;
        }
        
        private static void ConvertToBytes_int_B_b_B_b(ref BufferWriter writer, Int32[][] itemToConvert, LazinatorSerializationOptions options)
        {
            if (itemToConvert == default(Int32[][]))
            {
                return;
            }
            CompressedIntegralTypes.WriteCompressedInt(ref writer, itemToConvert.Length);
            int itemToConvertCount = itemToConvert.Length;
            for (int itemIndex = 0; itemIndex < itemToConvertCount; itemIndex++)
            {
                if (itemToConvert[itemIndex] == default(Int32[]))
                {
                    writer.Write((int)0);
                }
                else 
                {
                    
                    void action(ref BufferWriter w) => ConvertToBytes_int_B_b(ref w, itemToConvert[itemIndex], options);
                    WriteToBinaryWithInt32LengthPrefix(ref writer, action);
                }
                
            }
        }
        
        private static Int32[][] CloneOrChange_int_B_b_B_b(Int32[][] itemToClone, Func<ILazinator, ILazinator> cloneOrChangeFunc, bool avoidCloningIfPossible)
        {
            if (itemToClone == null)
            {
                return default;
            }
            int collectionLength = itemToClone.Length;
            Int32[][] collection = new Int32[collectionLength][];
            int itemToCloneCount = itemToClone.Length;
            for (int itemIndex = 0; itemIndex < itemToCloneCount; itemIndex++)
            {
                if (itemToClone[itemIndex] == null)
                {
                    collection[itemIndex] = default(Int32[]);
                }
                else
                {
                    var itemCopied = (Int32[]) CloneOrChange_int_B_b(itemToClone[itemIndex], cloneOrChangeFunc, avoidCloningIfPossible);
                    collection[itemIndex] = itemCopied;
                }
            }
            return collection;
        }
        
    }
}
#nullable restore
