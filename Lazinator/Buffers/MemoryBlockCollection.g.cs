//137228aa-d211-5b28-f31c-70defcc89ef9
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by the Lazinator tool, version 0.1.0.421, on 2024/02/11 12:18:18.391 PM.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

#nullable disable
namespace Lazinator.Buffers
{
    #pragma warning disable 8019
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
    #pragma warning restore 8019
    
    [Autogenerated]
    public partial class MemoryBlockCollection : ILazinator
    {
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public bool IsStruct => false;
        
        /* Property definitions */
        
        protected int _HighestMemoryBlockID_ByteIndex;
        protected int _MemoryBlocksLoadingInfo_ByteIndex;
        protected virtual int _HighestMemoryBlockID_ByteLength => _MemoryBlocksLoadingInfo_ByteIndex - _HighestMemoryBlockID_ByteIndex;
        private int _MemoryBlockCollection_EndByteIndex;
        protected virtual  int _MemoryBlocksLoadingInfo_ByteLength => _MemoryBlockCollection_EndByteIndex - _MemoryBlocksLoadingInfo_ByteIndex;
        protected virtual int _OverallEndByteIndex => _MemoryBlockCollection_EndByteIndex;
        
        
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        protected string _BaseBlobPath;
        public string BaseBlobPath
        {
            [DebuggerStepThrough]
            get
            {
                return _BaseBlobPath;
            }
            [DebuggerStepThrough]
            set
            {
                IsDirty = true;
                _BaseBlobPath = value;
            }
        }
        
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        protected bool _ContainedInSingleBlob;
        public bool ContainedInSingleBlob
        {
            [DebuggerStepThrough]
            get
            {
                return _ContainedInSingleBlob;
            }
            [DebuggerStepThrough]
            set
            {
                IsDirty = true;
                _ContainedInSingleBlob = value;
            }
        }
        
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        protected bool _IsPersisted;
        public bool IsPersisted
        {
            [DebuggerStepThrough]
            get
            {
                return _IsPersisted;
            }
            [DebuggerStepThrough]
            set
            {
                IsDirty = true;
                _IsPersisted = value;
            }
        }
        
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        protected long _LengthOfMemoryBlocks;
        public long LengthOfMemoryBlocks
        {
            [DebuggerStepThrough]
            get
            {
                return _LengthOfMemoryBlocks;
            }
            [DebuggerStepThrough]
            set
            {
                IsDirty = true;
                _LengthOfMemoryBlocks = value;
            }
        }
        
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        protected MemoryBlockID _HighestMemoryBlockID;
        public MemoryBlockID HighestMemoryBlockID
        {
            [DebuggerStepThrough]
            get
            {
                if (!_HighestMemoryBlockID_Accessed)
                {
                    LazinateHighestMemoryBlockID();
                } 
                return _HighestMemoryBlockID;
            }
            [DebuggerStepThrough]
            set
            {
                IsDirty = true;
                DescendantIsDirty = true;
                _HighestMemoryBlockID = value;
                _HighestMemoryBlockID_Accessed = true;
            }
        }
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        protected bool _HighestMemoryBlockID_Accessed;
        private void LazinateHighestMemoryBlockID()
        {
            if (LazinatorMemoryStorage.Length == 0)
            {
                _HighestMemoryBlockID = default(MemoryBlockID);
            }
            else
            {
                LazinatorMemory childData = GetChildSlice(LazinatorMemoryStorage, _HighestMemoryBlockID_ByteIndex, _HighestMemoryBlockID_ByteLength, null);_HighestMemoryBlockID = ConvertFromBytes_MemoryBlockID(childData);
            }
            _HighestMemoryBlockID_Accessed = true;
        }
        
        
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        protected List<MemoryBlockLoadingInfo> _MemoryBlocksLoadingInfo;
        public List<MemoryBlockLoadingInfo> MemoryBlocksLoadingInfo
        {
            [DebuggerStepThrough]
            get
            {
                if (!_MemoryBlocksLoadingInfo_Accessed)
                {
                    LazinateMemoryBlocksLoadingInfo();
                }
                IsDirty = true; 
                return _MemoryBlocksLoadingInfo;
            }
            [DebuggerStepThrough]
            set
            {
                IsDirty = true;
                DescendantIsDirty = true;
                _MemoryBlocksLoadingInfo = value;
                _MemoryBlocksLoadingInfo_Accessed = true;
            }
        }
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        protected bool _MemoryBlocksLoadingInfo_Accessed;
        private void LazinateMemoryBlocksLoadingInfo()
        {
            if (LazinatorMemoryStorage.Length == 0)
            {
                _MemoryBlocksLoadingInfo = default(List<MemoryBlockLoadingInfo>);
            }
            else
            {
                LazinatorMemory childData = GetChildSlice(LazinatorMemoryStorage, _MemoryBlocksLoadingInfo_ByteIndex, _MemoryBlocksLoadingInfo_ByteLength, null);_MemoryBlocksLoadingInfo = ConvertFromBytes_List_GMemoryBlockLoadingInfo_g(childData);
            }
            _MemoryBlocksLoadingInfo_Accessed = true;
        }
        
        
        /* Serialization, deserialization, and object relationships */
        
        public MemoryBlockCollection(IncludeChildrenMode originalIncludeChildrenMode = IncludeChildrenMode.IncludeAllChildren)
        {
            OriginalIncludeChildrenMode = originalIncludeChildrenMode;
        }
        
        public MemoryBlockCollection(LazinatorMemory serializedBytes, ILazinator parent = null, IncludeChildrenMode originalIncludeChildrenMode = IncludeChildrenMode.IncludeAllChildren, int? lazinatorObjectVersion = null)
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
        
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public virtual LazinatorParentsCollection LazinatorParents { get; set; }
        
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public virtual LazinatorMemory LazinatorMemoryStorage
        {
            get;
            set;
        }
        
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public virtual IncludeChildrenMode OriginalIncludeChildrenMode { get; set; }
        
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public virtual bool HasChanged { get; set; }
        
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        protected bool _IsDirty;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
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
        
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        protected bool _DescendantHasChanged;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
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
        
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        protected bool _DescendantIsDirty;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
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
            MemoryBlockCollection clone;
            if (cloneBufferOptions == CloneBufferOptions.NoBuffer)
            {
                clone = new MemoryBlockCollection(includeChildrenMode);
                clone.LazinatorObjectVersion = LazinatorObjectVersion;
                clone = (MemoryBlockCollection)AssignCloneProperties(clone, includeChildrenMode);
            }
            else
            {
                LazinatorMemory bytes = EncodeOrRecycleToNewBuffer(includeChildrenMode, OriginalIncludeChildrenMode, IsDirty, DescendantIsDirty, false, LazinatorMemoryStorage, this);
                clone = new MemoryBlockCollection(bytes);
            }
            return clone;
        }
        
        protected virtual ILazinator AssignCloneProperties(ILazinator clone, IncludeChildrenMode includeChildrenMode)
        {
            clone.FreeInMemoryObjects();
            MemoryBlockCollection typedClone = (MemoryBlockCollection) clone;
            typedClone.BaseBlobPath = BaseBlobPath;
            typedClone.ContainedInSingleBlob = ContainedInSingleBlob;
            typedClone.IsPersisted = IsPersisted;
            typedClone.LengthOfMemoryBlocks = LengthOfMemoryBlocks;
            typedClone.HighestMemoryBlockID = CloneOrChange_MemoryBlockID(HighestMemoryBlockID, l => l?.CloneLazinator(includeChildrenMode, CloneBufferOptions.NoBuffer), false);
            typedClone.MemoryBlocksLoadingInfo = CloneOrChange_List_GMemoryBlockLoadingInfo_g(MemoryBlocksLoadingInfo, l => l?.CloneLazinator(includeChildrenMode, CloneBufferOptions.NoBuffer), false);
            
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
            yield return ("BaseBlobPath", (object)BaseBlobPath);
            yield return ("ContainedInSingleBlob", (object)ContainedInSingleBlob);
            yield return ("IsPersisted", (object)IsPersisted);
            yield return ("LengthOfMemoryBlocks", (object)LengthOfMemoryBlocks);
            yield return ("HighestMemoryBlockID", (object)HighestMemoryBlockID);
            yield return ("MemoryBlocksLoadingInfo", (object)MemoryBlocksLoadingInfo);
            yield break;
        }
        
        public virtual ILazinator ForEachLazinator(Func<ILazinator, ILazinator> changeFunc, bool exploreOnlyDeserializedChildren, bool changeThisLevel)
        {
            var deserialized_HighestMemoryBlockID = HighestMemoryBlockID;
            _HighestMemoryBlockID = (MemoryBlockID) CloneOrChange_MemoryBlockID(_HighestMemoryBlockID, l => l?.ForEachLazinator(changeFunc, exploreOnlyDeserializedChildren, true), true);
            if ((!exploreOnlyDeserializedChildren && MemoryBlocksLoadingInfo != null) || (_MemoryBlocksLoadingInfo_Accessed && _MemoryBlocksLoadingInfo != null))
            {
                _MemoryBlocksLoadingInfo = (List<MemoryBlockLoadingInfo>) CloneOrChange_List_GMemoryBlockLoadingInfo_g(_MemoryBlocksLoadingInfo, l => l?.ForEachLazinator(changeFunc, exploreOnlyDeserializedChildren, true), true);
            }
            if (changeThisLevel && changeFunc != null)
            {
                return changeFunc(this);
            }
            return this;
        }
        
        public virtual void FreeInMemoryObjects()
        {
            _HighestMemoryBlockID = default;
            _MemoryBlocksLoadingInfo = default;
            _HighestMemoryBlockID_Accessed = _MemoryBlocksLoadingInfo_Accessed = false;
            IsDirty = false;
            DescendantIsDirty = false;
            HasChanged = false;
            DescendantHasChanged = false;
        }
        
        /* Conversion */
        
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public virtual int LazinatorUniqueID => 44;
        
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        protected virtual bool ContainsOpenGenericParameters => false;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public virtual LazinatorGenericIDType LazinatorGenericID => default;
        
        
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public virtual int LazinatorObjectVersion { get; set; } = 0;
        
        
        protected virtual int ConvertFromBytesAfterHeader(IncludeChildrenMode includeChildrenMode, int serializedVersionNumber, ref int bytesSoFar)
        {
            ReadOnlySpan<byte> span = LazinatorMemoryStorage.InitialReadOnlyMemory.Span;
            ConvertFromBytesForPrimitiveProperties(span, includeChildrenMode, serializedVersionNumber, ref bytesSoFar);
            int lengthForLengths = 8;
            int totalChildrenSize = ConvertFromBytesForChildLengths(span, includeChildrenMode, serializedVersionNumber, bytesSoFar + lengthForLengths, ref bytesSoFar);
            return bytesSoFar + totalChildrenSize;
        }
        
        protected virtual void ConvertFromBytesForPrimitiveProperties(ReadOnlySpan<byte> span, IncludeChildrenMode includeChildrenMode, int serializedVersionNumber, ref int bytesSoFar)
        {
            _BaseBlobPath = span.ToString_VarIntLengthUtf8(ref bytesSoFar);
            _ContainedInSingleBlob = span.ToBoolean(ref bytesSoFar);
            _IsPersisted = span.ToBoolean(ref bytesSoFar);
            _LengthOfMemoryBlocks = span.ToDecompressedInt64(ref bytesSoFar);
        }
        
        protected virtual int ConvertFromBytesForChildLengths(ReadOnlySpan<byte> span, IncludeChildrenMode includeChildrenMode, int serializedVersionNumber, int indexOfFirstChild, ref int bytesSoFar)
        {
            int totalChildrenBytes = 0;
            _HighestMemoryBlockID_ByteIndex = indexOfFirstChild + totalChildrenBytes;
            totalChildrenBytes += span.ToInt32(ref bytesSoFar);
            _MemoryBlocksLoadingInfo_ByteIndex = indexOfFirstChild + totalChildrenBytes;
            totalChildrenBytes += span.ToInt32(ref bytesSoFar);
            _MemoryBlockCollection_EndByteIndex = indexOfFirstChild + totalChildrenBytes;
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
            _HighestMemoryBlockID = (MemoryBlockID) CloneOrChange_MemoryBlockID(_HighestMemoryBlockID, l => l.RemoveBufferInHierarchy(), true);
            if (_MemoryBlocksLoadingInfo_Accessed && _MemoryBlocksLoadingInfo != null)
            {
                _MemoryBlocksLoadingInfo = (List<MemoryBlockLoadingInfo>) CloneOrChange_List_GMemoryBlockLoadingInfo_g(_MemoryBlocksLoadingInfo, l => l.RemoveBufferInHierarchy(), true);
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
            
            WritePrimitivePropertiesIntoBuffer(ref writer, options, includeUniqueID);
            int lengthForLengths = 8;
            
            var previousLengthsPosition = writer.SetLengthsPosition(lengthForLengths);
            WriteChildrenPropertiesIntoBuffer(ref writer, options, includeUniqueID, startPosition);
            writer.ResetLengthsPosition(previousLengthsPosition);
            
        }
        
        protected virtual void WritePrimitivePropertiesIntoBuffer(ref BufferWriter writer, in LazinatorSerializationOptions options, bool includeUniqueID)
        {
            EncodeCharAndString.WriteStringUtf8WithVarIntPrefix(ref writer, _BaseBlobPath);
            WriteUncompressedPrimitives.WriteBool(ref writer, _ContainedInSingleBlob);
            WriteUncompressedPrimitives.WriteBool(ref writer, _IsPersisted);
            CompressedIntegralTypes.WriteCompressedLong(ref writer, _LengthOfMemoryBlocks);
        }
        protected virtual void WriteChildrenPropertiesIntoBuffer(ref BufferWriter writer, LazinatorSerializationOptions options, bool includeUniqueID, int startOfObjectPosition)
        {
            if (options.SplittingPossible)
            {
                options = options.WithoutSplittingPossible();
            }
            int startOfChildPosition = 0;
            startOfChildPosition = writer.ActiveMemoryPosition;
            if ((options.IncludeChildrenMode != IncludeChildrenMode.IncludeAllChildren || options.IncludeChildrenMode != OriginalIncludeChildrenMode) && !_HighestMemoryBlockID_Accessed)
            {
                var deserialized = HighestMemoryBlockID;
            }
            WriteNonLazinatorObject(
            nonLazinatorObject: _HighestMemoryBlockID, isBelievedDirty: _HighestMemoryBlockID_Accessed || (options.IncludeChildrenMode != OriginalIncludeChildrenMode),
            isAccessed: _HighestMemoryBlockID_Accessed, writer: ref writer,
            getChildSliceForFieldFn: () => GetChildSlice(LazinatorMemoryStorage, _HighestMemoryBlockID_ByteIndex, _HighestMemoryBlockID_ByteLength, null),
            verifyCleanness: false,
            binaryWriterAction: (ref BufferWriter w, bool v) =>
            ConvertToBytes_MemoryBlockID(ref w, _HighestMemoryBlockID,
            options));
            if (options.UpdateStoredBuffer)
            {
                _HighestMemoryBlockID_ByteIndex = startOfChildPosition - startOfObjectPosition;
                
            }
            startOfChildPosition = writer.ActiveMemoryPosition;
            if ((options.IncludeChildrenMode != IncludeChildrenMode.IncludeAllChildren || options.IncludeChildrenMode != OriginalIncludeChildrenMode) && !_MemoryBlocksLoadingInfo_Accessed)
            {
                var deserialized = MemoryBlocksLoadingInfo;
            }
            WriteNonLazinatorObject(
            nonLazinatorObject: _MemoryBlocksLoadingInfo, isBelievedDirty: _MemoryBlocksLoadingInfo_Accessed || (options.IncludeChildrenMode != OriginalIncludeChildrenMode),
            isAccessed: _MemoryBlocksLoadingInfo_Accessed, writer: ref writer,
            getChildSliceForFieldFn: () => GetChildSlice(LazinatorMemoryStorage, _MemoryBlocksLoadingInfo_ByteIndex, _MemoryBlocksLoadingInfo_ByteLength, null),
            verifyCleanness: false,
            binaryWriterAction: (ref BufferWriter w, bool v) =>
            ConvertToBytes_List_GMemoryBlockLoadingInfo_g(ref w, _MemoryBlocksLoadingInfo,
            options));
            if (options.UpdateStoredBuffer)
            {
                _MemoryBlocksLoadingInfo_ByteIndex = startOfChildPosition - startOfObjectPosition;
                
            }
            if (options.UpdateStoredBuffer)
            {
                _MemoryBlockCollection_EndByteIndex = writer.ActiveMemoryPosition - startOfObjectPosition;
            }
            
        }
        /* Conversion of supported collections and tuples */
        
        private static List<MemoryBlockLoadingInfo> ConvertFromBytes_List_GMemoryBlockLoadingInfo_g(LazinatorMemory storage)
        {
            if (storage.Length == 0)
            {
                return default(List<MemoryBlockLoadingInfo>);
            }
            ReadOnlySpan<byte> span = storage.InitialReadOnlyMemory.Span;
            int bytesSoFar = 0;
            int collectionLength = span.ToDecompressedInt32(ref bytesSoFar);
            
            List<MemoryBlockLoadingInfo> collection = new List<MemoryBlockLoadingInfo>(collectionLength);
            for (int itemIndex = 0; itemIndex < collectionLength; itemIndex++)
            {
                int lengthCollectionMember = span.ToInt32(ref bytesSoFar);
                if (lengthCollectionMember == 0)
                {
                    collection.Add(null);
                }
                else
                {
                    LazinatorMemory childData = storage.Slice(bytesSoFar, lengthCollectionMember);
                    var item = DeserializationFactory.Instance.CreateBasedOnType<MemoryBlockLoadingInfo>(childData);
                    collection.Add(item);
                }bytesSoFar += lengthCollectionMember;
            }
            
            return collection;
        }
        
        private static void ConvertToBytes_List_GMemoryBlockLoadingInfo_g(ref BufferWriter writer, List<MemoryBlockLoadingInfo> itemToConvert, LazinatorSerializationOptions options)
        {
            if (itemToConvert == default(List<MemoryBlockLoadingInfo>))
            {
                return;
            }
            CompressedIntegralTypes.WriteCompressedInt(ref writer, itemToConvert.Count);
            int itemToConvertCount = itemToConvert.Count;
            for (int itemIndex = 0; itemIndex < itemToConvertCount; itemIndex++)
            {
                if (itemToConvert[itemIndex] == null)
                {
                    writer.Write((int)0);
                }
                else 
                {
                    
                    void action(ref BufferWriter w) => itemToConvert[itemIndex].SerializeToExistingBuffer(ref w, options);
                    WriteToBinaryWithInt32LengthPrefix(ref writer, action);
                }
                
            }
        }
        
        private static List<MemoryBlockLoadingInfo> CloneOrChange_List_GMemoryBlockLoadingInfo_g(List<MemoryBlockLoadingInfo> itemToClone, Func<ILazinator, ILazinator> cloneOrChangeFunc, bool avoidCloningIfPossible)
        {
            if (itemToClone == null)
            {
                return default;
            }
            int collectionLength = itemToClone.Count;
            List<MemoryBlockLoadingInfo> collection = avoidCloningIfPossible ? itemToClone : new List<MemoryBlockLoadingInfo>(collectionLength);
            int itemToCloneCount = itemToClone.Count;
            for (int itemIndex = 0; itemIndex < itemToCloneCount; itemIndex++)
            {
                if (avoidCloningIfPossible)
                {
                    if (itemToClone[itemIndex] != null)
                    {
                        itemToClone[itemIndex] = (MemoryBlockLoadingInfo) (cloneOrChangeFunc(itemToClone[itemIndex]));
                    }
                    continue;
                }
                if (itemToClone[itemIndex] == null)
                {
                    collection.Add(null);
                }
                else
                {
                    var itemCopied = (MemoryBlockLoadingInfo) (cloneOrChangeFunc(itemToClone[itemIndex]));
                    collection.Add(itemCopied);
                }
            }
            return collection;
        }
        
        private static MemoryBlockID ConvertFromBytes_MemoryBlockID(LazinatorMemory storage)
        {
            if (storage.Length == 0)
            {
                return default;
            }
            ReadOnlySpan<byte> span = storage.InitialReadOnlyMemory.Span;
            
            int bytesSoFar = 0;
            
            int item1 = span.ToDecompressedInt32(ref bytesSoFar);
            
            var itemToCreate = new MemoryBlockID(item1);
            
            return itemToCreate;
        }
        
        private static void ConvertToBytes_MemoryBlockID(ref BufferWriter writer, MemoryBlockID itemToConvert, LazinatorSerializationOptions options)
        {
            
            CompressedIntegralTypes.WriteCompressedInt(ref writer, itemToConvert.AsInt);
        }
        
        private static MemoryBlockID CloneOrChange_MemoryBlockID(MemoryBlockID itemToConvert, Func<ILazinator, ILazinator> cloneOrChangeFunc, bool avoidCloningIfPossible)
        {
            return new MemoryBlockID((int) (itemToConvert.AsInt));
        }
        
    }
}
#nullable restore
