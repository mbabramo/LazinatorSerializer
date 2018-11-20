//d78281db-9d17-59e2-fb2d-1d7e0a06b827
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by the Lazinator tool, version 0.1.0.318
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace LazinatorTests.Examples.Tuples
{
    using Lazinator.Attributes;
    using Lazinator.Buffers;
    using Lazinator.Core;
    using Lazinator.Exceptions;
    using Lazinator.Support;
    using LazinatorTests.Examples;
    using System;
    using System.Buffers;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Runtime.InteropServices;
    using static Lazinator.Core.LazinatorUtilities;
    
    [Autogenerated]
    public partial class RecordLikeContainer : ILazinator
    {
        public bool IsStruct => false;
        
        /* Property definitions */
        
        protected int _MyMismatchedRecordLikeType_ByteIndex;
        protected int _MyRecordLikeClass_ByteIndex;
        protected int _MyRecordLikeType_ByteIndex;
        protected int _MyRecordLikeTypeWithLazinator_ByteIndex;
        protected virtual int _MyMismatchedRecordLikeType_ByteLength => _MyRecordLikeClass_ByteIndex - _MyMismatchedRecordLikeType_ByteIndex;
        protected virtual int _MyRecordLikeClass_ByteLength => _MyRecordLikeType_ByteIndex - _MyRecordLikeClass_ByteIndex;
        protected virtual int _MyRecordLikeType_ByteLength => _MyRecordLikeTypeWithLazinator_ByteIndex - _MyRecordLikeType_ByteIndex;
        private int _RecordLikeContainer_EndByteIndex;
        protected virtual int _MyRecordLikeTypeWithLazinator_ByteLength => _RecordLikeContainer_EndByteIndex - _MyRecordLikeTypeWithLazinator_ByteIndex;
        
        
        protected MismatchedRecordLikeType _MyMismatchedRecordLikeType;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public MismatchedRecordLikeType MyMismatchedRecordLikeType
        {
            get
            {
                if (!_MyMismatchedRecordLikeType_Accessed)
                {
                    if (LazinatorObjectBytes.Length == 0)
                    {
                        _MyMismatchedRecordLikeType = default(MismatchedRecordLikeType);
                    }
                    else
                    {
                        LazinatorMemory childData = GetChildSlice(LazinatorMemoryStorage, _MyMismatchedRecordLikeType_ByteIndex, _MyMismatchedRecordLikeType_ByteLength, false, false, null);
                        _MyMismatchedRecordLikeType = ConvertFromBytes_MismatchedRecordLikeType(childData);
                    }
                    _MyMismatchedRecordLikeType_Accessed = true;
                } 
                return _MyMismatchedRecordLikeType;
            }
            set
            {
                IsDirty = true;
                DescendantIsDirty = true;
                _MyMismatchedRecordLikeType = value;
                _MyMismatchedRecordLikeType_Accessed = true;
            }
        }
        protected bool _MyMismatchedRecordLikeType_Accessed;
        
        protected RecordLikeClass _MyRecordLikeClass;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public RecordLikeClass MyRecordLikeClass
        {
            get
            {
                if (!_MyRecordLikeClass_Accessed)
                {
                    if (LazinatorObjectBytes.Length == 0)
                    {
                        _MyRecordLikeClass = default(RecordLikeClass);
                    }
                    else
                    {
                        LazinatorMemory childData = GetChildSlice(LazinatorMemoryStorage, _MyRecordLikeClass_ByteIndex, _MyRecordLikeClass_ByteLength, false, false, null);
                        _MyRecordLikeClass = ConvertFromBytes_RecordLikeClass(childData);
                    }
                    _MyRecordLikeClass_Accessed = true;
                }
                IsDirty = true; 
                return _MyRecordLikeClass;
            }
            set
            {
                IsDirty = true;
                DescendantIsDirty = true;
                _MyRecordLikeClass = value;
                _MyRecordLikeClass_Accessed = true;
            }
        }
        protected bool _MyRecordLikeClass_Accessed;
        
        protected RecordLikeType _MyRecordLikeType;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public RecordLikeType MyRecordLikeType
        {
            get
            {
                if (!_MyRecordLikeType_Accessed)
                {
                    if (LazinatorObjectBytes.Length == 0)
                    {
                        _MyRecordLikeType = default(RecordLikeType);
                    }
                    else
                    {
                        LazinatorMemory childData = GetChildSlice(LazinatorMemoryStorage, _MyRecordLikeType_ByteIndex, _MyRecordLikeType_ByteLength, false, false, null);
                        _MyRecordLikeType = ConvertFromBytes_RecordLikeType(childData);
                    }
                    _MyRecordLikeType_Accessed = true;
                } 
                return _MyRecordLikeType;
            }
            set
            {
                IsDirty = true;
                DescendantIsDirty = true;
                _MyRecordLikeType = value;
                _MyRecordLikeType_Accessed = true;
            }
        }
        protected bool _MyRecordLikeType_Accessed;
        
        protected RecordLikeTypeWithLazinator _MyRecordLikeTypeWithLazinator;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public RecordLikeTypeWithLazinator MyRecordLikeTypeWithLazinator
        {
            get
            {
                if (!_MyRecordLikeTypeWithLazinator_Accessed)
                {
                    if (LazinatorObjectBytes.Length == 0)
                    {
                        _MyRecordLikeTypeWithLazinator = default(RecordLikeTypeWithLazinator);
                    }
                    else
                    {
                        LazinatorMemory childData = GetChildSlice(LazinatorMemoryStorage, _MyRecordLikeTypeWithLazinator_ByteIndex, _MyRecordLikeTypeWithLazinator_ByteLength, false, false, null);
                        _MyRecordLikeTypeWithLazinator = ConvertFromBytes_RecordLikeTypeWithLazinator(childData);
                    }
                    _MyRecordLikeTypeWithLazinator_Accessed = true;
                }
                IsDirty = true; 
                return _MyRecordLikeTypeWithLazinator;
            }
            set
            {
                IsDirty = true;
                DescendantIsDirty = true;
                _MyRecordLikeTypeWithLazinator = value;
                _MyRecordLikeTypeWithLazinator_Accessed = true;
            }
        }
        protected bool _MyRecordLikeTypeWithLazinator_Accessed;
        
        /* Serialization, deserialization, and object relationships */
        
        public RecordLikeContainer() : base()
        {
        }
        
        public virtual LazinatorParentsCollection LazinatorParents { get; set; }
        
        public virtual IncludeChildrenMode OriginalIncludeChildrenMode { get; set; }
        
        public virtual int Deserialize()
        {
            FreeInMemoryObjects();
            int bytesSoFar = 0;
            ReadOnlySpan<byte> span = LazinatorObjectBytes.Span;
            if (span.Length == 0)
            {
                return 0;
            }
            
            LazinatorGenericID = GetGenericIDIfApplicable(ContainsOpenGenericParameters, LazinatorUniqueID, span, ref bytesSoFar);
            
            int lazinatorLibraryVersion = span.ToDecompressedInt(ref bytesSoFar);
            
            int serializedVersionNumber = span.ToDecompressedInt(ref bytesSoFar);
            
            OriginalIncludeChildrenMode = (IncludeChildrenMode)span.ToByte(ref bytesSoFar);
            
            ConvertFromBytesAfterHeader(OriginalIncludeChildrenMode, serializedVersionNumber, ref bytesSoFar);
            return bytesSoFar;
        }
        
        public virtual LazinatorMemory SerializeLazinator(IncludeChildrenMode includeChildrenMode, bool verifyCleanness, bool updateStoredBuffer) => EncodeOrRecycleToNewBuffer(includeChildrenMode, OriginalIncludeChildrenMode, verifyCleanness, IsDirty, DescendantIsDirty, false, LazinatorMemoryStorage, updateStoredBuffer, this);
        
        public virtual ILazinator CloneLazinator(IncludeChildrenMode includeChildrenMode = IncludeChildrenMode.IncludeAllChildren, CloneBufferOptions cloneBufferOptions = CloneBufferOptions.IndependentBuffers)
        {
            var clone = new RecordLikeContainer()
            {
                OriginalIncludeChildrenMode = includeChildrenMode
            };
            clone = CompleteClone(this, clone, includeChildrenMode, cloneBufferOptions);
            return clone;
        }
        
        public virtual ILazinator AssignCloneProperties(ILazinator clone, IncludeChildrenMode includeChildrenMode)
        {
            clone.FreeInMemoryObjects();
            RecordLikeContainer typedClone = (RecordLikeContainer) clone;
            typedClone.MyMismatchedRecordLikeType = CloneOrChange_MismatchedRecordLikeType(MyMismatchedRecordLikeType, l => l?.CloneLazinator(includeChildrenMode, CloneBufferOptions.NoBuffer), false);
            typedClone.MyRecordLikeClass = CloneOrChange_RecordLikeClass(MyRecordLikeClass, l => l?.CloneLazinator(includeChildrenMode, CloneBufferOptions.NoBuffer), false);
            typedClone.MyRecordLikeType = CloneOrChange_RecordLikeType(MyRecordLikeType, l => l?.CloneLazinator(includeChildrenMode, CloneBufferOptions.NoBuffer), false);
            typedClone.MyRecordLikeTypeWithLazinator = CloneOrChange_RecordLikeTypeWithLazinator(MyRecordLikeTypeWithLazinator, l => l?.CloneLazinator(includeChildrenMode, CloneBufferOptions.NoBuffer), false);
            
            return typedClone;
        }
        
        public virtual bool HasChanged { get; set; }
        
        protected bool _IsDirty;
        public virtual bool IsDirty
        {
            [DebuggerStepThrough]
            get => _IsDirty|| LazinatorObjectBytes.Length == 0;
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
        
        public virtual void DeserializeLazinator(LazinatorMemory serializedBytes)
        {
            LazinatorMemoryStorage = serializedBytes;
            int length = Deserialize();
            if (length != LazinatorMemoryStorage.Length)
            {
                LazinatorMemoryStorage = LazinatorMemoryStorage.Slice(0, length);
            }
        }
        
        public virtual LazinatorMemory LazinatorMemoryStorage
        {
            get;
            set;
        }
        protected virtual ReadOnlyMemory<byte> LazinatorObjectBytes => LazinatorMemoryStorage?.Memory ?? LazinatorUtilities.EmptyReadOnlyMemory;
        
        public virtual void UpdateStoredBuffer()
        {
            if (!IsDirty && !DescendantIsDirty && LazinatorObjectBytes.Length > 0 && OriginalIncludeChildrenMode == IncludeChildrenMode.IncludeAllChildren)
            {
                return;
            }
            EncodeOrRecycleToNewBuffer(IncludeChildrenMode.IncludeAllChildren, OriginalIncludeChildrenMode, false, IsDirty, DescendantIsDirty, false, LazinatorMemoryStorage, true, this);
            OriginalIncludeChildrenMode = IncludeChildrenMode.IncludeAllChildren;
        }
        
        public virtual int GetByteLength()
        {
            UpdateStoredBuffer();
            return LazinatorObjectBytes.Length;
        }
        
        public virtual bool NonBinaryHash32 => false;
        
        
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
            yield return ("MyMismatchedRecordLikeType", (object)MyMismatchedRecordLikeType);
            yield return ("MyRecordLikeClass", (object)MyRecordLikeClass);
            yield return ("MyRecordLikeType", (object)MyRecordLikeType);
            yield return ("MyRecordLikeTypeWithLazinator", (object)MyRecordLikeTypeWithLazinator);
            yield break;
        }
        
        public virtual ILazinator ForEachLazinator(Func<ILazinator, ILazinator> changeFunc, bool exploreOnlyDeserializedChildren)
        {
            if ((!exploreOnlyDeserializedChildren && !System.Collections.Generic.EqualityComparer<MismatchedRecordLikeType>.Default.Equals(MyMismatchedRecordLikeType, default(MismatchedRecordLikeType))) || (_MyMismatchedRecordLikeType_Accessed && !System.Collections.Generic.EqualityComparer<MismatchedRecordLikeType>.Default.Equals(_MyMismatchedRecordLikeType, default(MismatchedRecordLikeType))))
            {
                _MyMismatchedRecordLikeType = (MismatchedRecordLikeType) CloneOrChange_MismatchedRecordLikeType(_MyMismatchedRecordLikeType, l => l?.ForEachLazinator(changeFunc, exploreOnlyDeserializedChildren), true);
            }
            if ((!exploreOnlyDeserializedChildren && MyRecordLikeClass != null) || (_MyRecordLikeClass_Accessed && _MyRecordLikeClass != null))
            {
                _MyRecordLikeClass = (RecordLikeClass) CloneOrChange_RecordLikeClass(_MyRecordLikeClass, l => l?.ForEachLazinator(changeFunc, exploreOnlyDeserializedChildren), true);
            }
            if ((!exploreOnlyDeserializedChildren && !System.Collections.Generic.EqualityComparer<RecordLikeType>.Default.Equals(MyRecordLikeType, default(RecordLikeType))) || (_MyRecordLikeType_Accessed && !System.Collections.Generic.EqualityComparer<RecordLikeType>.Default.Equals(_MyRecordLikeType, default(RecordLikeType))))
            {
                _MyRecordLikeType = (RecordLikeType) CloneOrChange_RecordLikeType(_MyRecordLikeType, l => l?.ForEachLazinator(changeFunc, exploreOnlyDeserializedChildren), true);
            }
            if ((!exploreOnlyDeserializedChildren && !System.Collections.Generic.EqualityComparer<RecordLikeTypeWithLazinator>.Default.Equals(MyRecordLikeTypeWithLazinator, default(RecordLikeTypeWithLazinator))) || (_MyRecordLikeTypeWithLazinator_Accessed && !System.Collections.Generic.EqualityComparer<RecordLikeTypeWithLazinator>.Default.Equals(_MyRecordLikeTypeWithLazinator, default(RecordLikeTypeWithLazinator))))
            {
                _MyRecordLikeTypeWithLazinator = (RecordLikeTypeWithLazinator) CloneOrChange_RecordLikeTypeWithLazinator(_MyRecordLikeTypeWithLazinator, l => l?.ForEachLazinator(changeFunc, exploreOnlyDeserializedChildren), true);
            }
            return changeFunc(this);
        }
        
        public virtual void FreeInMemoryObjects()
        {
            _MyMismatchedRecordLikeType = default;
            _MyRecordLikeClass = default;
            _MyRecordLikeType = default;
            _MyRecordLikeTypeWithLazinator = default;
            _MyMismatchedRecordLikeType_Accessed = _MyRecordLikeClass_Accessed = _MyRecordLikeType_Accessed = _MyRecordLikeTypeWithLazinator_Accessed = false;
            IsDirty = false;
            DescendantIsDirty = false;
            HasChanged = false;
            DescendantHasChanged = false;
        }
        
        /* Conversion */
        
        public virtual int LazinatorUniqueID => 226;
        
        protected virtual bool ContainsOpenGenericParameters => false;
        protected virtual LazinatorGenericIDType _LazinatorGenericID { get; set; }
        public virtual LazinatorGenericIDType LazinatorGenericID
        {
            get => default;
            set { }
        }
        
        public virtual int LazinatorObjectVersion { get; set; } = 0;
        
        
        public virtual void ConvertFromBytesAfterHeader(IncludeChildrenMode includeChildrenMode, int serializedVersionNumber, ref int bytesSoFar)
        {
            ReadOnlySpan<byte> span = LazinatorObjectBytes.Span;
            _MyMismatchedRecordLikeType_ByteIndex = bytesSoFar;
            bytesSoFar = span.ToInt32(ref bytesSoFar) + bytesSoFar;
            _MyRecordLikeClass_ByteIndex = bytesSoFar;
            bytesSoFar = span.ToInt32(ref bytesSoFar) + bytesSoFar;
            _MyRecordLikeType_ByteIndex = bytesSoFar;
            bytesSoFar = span.ToInt32(ref bytesSoFar) + bytesSoFar;
            _MyRecordLikeTypeWithLazinator_ByteIndex = bytesSoFar;
            bytesSoFar = span.ToInt32(ref bytesSoFar) + bytesSoFar;
            _RecordLikeContainer_EndByteIndex = bytesSoFar;
        }
        
        public virtual void SerializeExistingBuffer(ref BinaryBufferWriter writer, IncludeChildrenMode includeChildrenMode, bool verifyCleanness, bool updateStoredBuffer)
        {
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
        
        public virtual void UpdateStoredBuffer(ref BinaryBufferWriter writer, int startPosition, int length, IncludeChildrenMode includeChildrenMode, bool updateDeserializedChildren)
        {
            _IsDirty = false;
            if (includeChildrenMode == IncludeChildrenMode.IncludeAllChildren)
            {
                _DescendantIsDirty = false;
                if (updateDeserializedChildren)
                {
                    if (_MyMismatchedRecordLikeType_Accessed && !System.Collections.Generic.EqualityComparer<MismatchedRecordLikeType>.Default.Equals(_MyMismatchedRecordLikeType, default(MismatchedRecordLikeType)))
                    {
                        _MyMismatchedRecordLikeType = (MismatchedRecordLikeType) CloneOrChange_MismatchedRecordLikeType(_MyMismatchedRecordLikeType, l => l.RemoveBufferInHierarchy(), true);
                    }
                    if (_MyRecordLikeClass_Accessed && _MyRecordLikeClass != null)
                    {
                        _MyRecordLikeClass = (RecordLikeClass) CloneOrChange_RecordLikeClass(_MyRecordLikeClass, l => l.RemoveBufferInHierarchy(), true);
                    }
                    if (_MyRecordLikeType_Accessed && !System.Collections.Generic.EqualityComparer<RecordLikeType>.Default.Equals(_MyRecordLikeType, default(RecordLikeType)))
                    {
                        _MyRecordLikeType = (RecordLikeType) CloneOrChange_RecordLikeType(_MyRecordLikeType, l => l.RemoveBufferInHierarchy(), true);
                    }
                    if (_MyRecordLikeTypeWithLazinator_Accessed && !System.Collections.Generic.EqualityComparer<RecordLikeTypeWithLazinator>.Default.Equals(_MyRecordLikeTypeWithLazinator, default(RecordLikeTypeWithLazinator)))
                    {
                        _MyRecordLikeTypeWithLazinator = (RecordLikeTypeWithLazinator) CloneOrChange_RecordLikeTypeWithLazinator(_MyRecordLikeTypeWithLazinator, l => l.RemoveBufferInHierarchy(), true);
                    }
                }
                
            }
            else
            {
                throw new LazinatorSerializationException("Cannot update stored buffer when serializing only some children.");
            }
            
            var newBuffer = writer.Slice(startPosition, length);
            LazinatorMemoryStorage = newBuffer;
        }
        
        protected virtual void WritePropertiesIntoBuffer(ref BinaryBufferWriter writer, IncludeChildrenMode includeChildrenMode, bool verifyCleanness, bool updateStoredBuffer, bool includeUniqueID)
        {
            int startPosition = writer.Position;
            int startOfObjectPosition = 0;
            // header information
            if (includeUniqueID)
            {
                if (LazinatorGenericID.IsEmpty)
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
            startOfObjectPosition = writer.Position;
            if (includeChildrenMode != IncludeChildrenMode.IncludeAllChildren || includeChildrenMode != OriginalIncludeChildrenMode && !_MyMismatchedRecordLikeType_Accessed)
            {
                var deserialized = MyMismatchedRecordLikeType;
            }
            WriteNonLazinatorObject(
            nonLazinatorObject: _MyMismatchedRecordLikeType, isBelievedDirty: _MyMismatchedRecordLikeType_Accessed || (includeChildrenMode != OriginalIncludeChildrenMode),
            isAccessed: _MyMismatchedRecordLikeType_Accessed, writer: ref writer,
            getChildSliceForFieldFn: () => GetChildSlice(LazinatorMemoryStorage, _MyMismatchedRecordLikeType_ByteIndex, _MyMismatchedRecordLikeType_ByteLength, false, false, null),
            verifyCleanness: false,
            binaryWriterAction: (ref BinaryBufferWriter w, bool v) =>
            ConvertToBytes_MismatchedRecordLikeType(ref w, _MyMismatchedRecordLikeType,
            includeChildrenMode, v, updateStoredBuffer));
            if (updateStoredBuffer)
            {
                _MyMismatchedRecordLikeType_ByteIndex = startOfObjectPosition - startPosition;
            }
            startOfObjectPosition = writer.Position;
            if (includeChildrenMode != IncludeChildrenMode.IncludeAllChildren || includeChildrenMode != OriginalIncludeChildrenMode && !_MyRecordLikeClass_Accessed)
            {
                var deserialized = MyRecordLikeClass;
            }
            WriteNonLazinatorObject(
            nonLazinatorObject: _MyRecordLikeClass, isBelievedDirty: _MyRecordLikeClass_Accessed || (includeChildrenMode != OriginalIncludeChildrenMode),
            isAccessed: _MyRecordLikeClass_Accessed, writer: ref writer,
            getChildSliceForFieldFn: () => GetChildSlice(LazinatorMemoryStorage, _MyRecordLikeClass_ByteIndex, _MyRecordLikeClass_ByteLength, false, false, null),
            verifyCleanness: false,
            binaryWriterAction: (ref BinaryBufferWriter w, bool v) =>
            ConvertToBytes_RecordLikeClass(ref w, _MyRecordLikeClass,
            includeChildrenMode, v, updateStoredBuffer));
            if (updateStoredBuffer)
            {
                _MyRecordLikeClass_ByteIndex = startOfObjectPosition - startPosition;
            }
            startOfObjectPosition = writer.Position;
            if (includeChildrenMode != IncludeChildrenMode.IncludeAllChildren || includeChildrenMode != OriginalIncludeChildrenMode && !_MyRecordLikeType_Accessed)
            {
                var deserialized = MyRecordLikeType;
            }
            WriteNonLazinatorObject(
            nonLazinatorObject: _MyRecordLikeType, isBelievedDirty: _MyRecordLikeType_Accessed || (includeChildrenMode != OriginalIncludeChildrenMode),
            isAccessed: _MyRecordLikeType_Accessed, writer: ref writer,
            getChildSliceForFieldFn: () => GetChildSlice(LazinatorMemoryStorage, _MyRecordLikeType_ByteIndex, _MyRecordLikeType_ByteLength, false, false, null),
            verifyCleanness: false,
            binaryWriterAction: (ref BinaryBufferWriter w, bool v) =>
            ConvertToBytes_RecordLikeType(ref w, _MyRecordLikeType,
            includeChildrenMode, v, updateStoredBuffer));
            if (updateStoredBuffer)
            {
                _MyRecordLikeType_ByteIndex = startOfObjectPosition - startPosition;
            }
            startOfObjectPosition = writer.Position;
            if (includeChildrenMode != IncludeChildrenMode.IncludeAllChildren || includeChildrenMode != OriginalIncludeChildrenMode && !_MyRecordLikeTypeWithLazinator_Accessed)
            {
                var deserialized = MyRecordLikeTypeWithLazinator;
            }
            WriteNonLazinatorObject(
            nonLazinatorObject: _MyRecordLikeTypeWithLazinator, isBelievedDirty: _MyRecordLikeTypeWithLazinator_Accessed || (includeChildrenMode != OriginalIncludeChildrenMode),
            isAccessed: _MyRecordLikeTypeWithLazinator_Accessed, writer: ref writer,
            getChildSliceForFieldFn: () => GetChildSlice(LazinatorMemoryStorage, _MyRecordLikeTypeWithLazinator_ByteIndex, _MyRecordLikeTypeWithLazinator_ByteLength, false, false, null),
            verifyCleanness: false,
            binaryWriterAction: (ref BinaryBufferWriter w, bool v) =>
            ConvertToBytes_RecordLikeTypeWithLazinator(ref w, _MyRecordLikeTypeWithLazinator,
            includeChildrenMode, v, updateStoredBuffer));
            if (updateStoredBuffer)
            {
                _MyRecordLikeTypeWithLazinator_ByteIndex = startOfObjectPosition - startPosition;
            }
            if (updateStoredBuffer)
            {
                _RecordLikeContainer_EndByteIndex = writer.Position - startPosition;
            }
        }
        
        /* Conversion of supported collections and tuples */
        
        private static MismatchedRecordLikeType ConvertFromBytes_MismatchedRecordLikeType(LazinatorMemory storage)
        {
            if (storage.Length == 0)
            {
                return default;
            }
            storage.LazinatorShouldNotReturnToPool();
            ReadOnlySpan<byte> span = storage.ReadOnlySpan;
            
            int bytesSoFar = 0;
            
            int item1 = span.ToDecompressedInt(ref bytesSoFar);
            
            string item2 = span.ToString_VarIntLengthUtf8(ref bytesSoFar);
            
            var tupleType = new MismatchedRecordLikeType(item1, item2);
            
            return tupleType;
        }
        
        private static void ConvertToBytes_MismatchedRecordLikeType(ref BinaryBufferWriter writer, MismatchedRecordLikeType itemToConvert, IncludeChildrenMode includeChildrenMode, bool verifyCleanness, bool updateStoredBuffer)
        {
            writer.LazinatorMemory.LazinatorShouldNotReturnToPool();
            
            CompressedIntegralTypes.WriteCompressedInt(ref writer, itemToConvert.Age);
            
            EncodeCharAndString.WriteStringUtf8WithVarIntPrefix(ref writer, itemToConvert.Name);
        }
        
        private static MismatchedRecordLikeType CloneOrChange_MismatchedRecordLikeType(MismatchedRecordLikeType itemToConvert, Func<ILazinator, ILazinator> cloneOrChangeFunc, bool avoidCloningIfPossible)
        {
            return new MismatchedRecordLikeType((int) (itemToConvert.Age),(string) (itemToConvert.Name));
        }
        
        private static RecordLikeClass ConvertFromBytes_RecordLikeClass(LazinatorMemory storage)
        {
            if (storage.Length == 0)
            {
                return default;
            }
            storage.LazinatorShouldNotReturnToPool();
            ReadOnlySpan<byte> span = storage.ReadOnlySpan;
            
            int bytesSoFar = 0;
            
            int item1 = span.ToDecompressedInt(ref bytesSoFar);
            
            Example item2 = default;
            int lengthCollectionMember_item2 = span.ToInt32(ref bytesSoFar);
            if (lengthCollectionMember_item2 != 0)
            {
                LazinatorMemory childData = storage.Slice(bytesSoFar, lengthCollectionMember_item2);
                item2 = DeserializationFactory.Instance.CreateBasedOnType<Example>(childData);
            }
            bytesSoFar += lengthCollectionMember_item2;
            
            var tupleType = new RecordLikeClass(item1, item2);
            
            return tupleType;
        }
        
        private static void ConvertToBytes_RecordLikeClass(ref BinaryBufferWriter writer, RecordLikeClass itemToConvert, IncludeChildrenMode includeChildrenMode, bool verifyCleanness, bool updateStoredBuffer)
        {
            writer.LazinatorMemory.LazinatorShouldNotReturnToPool();
            
            CompressedIntegralTypes.WriteCompressedInt(ref writer, itemToConvert.Age);
            
            if (itemToConvert.Example == null)
            {
                writer.Write((uint)0);
            }
            else
            {
                void actionExample(ref BinaryBufferWriter w) => itemToConvert.Example.SerializeExistingBuffer(ref w, includeChildrenMode, verifyCleanness, updateStoredBuffer);
                WriteToBinaryWithIntLengthPrefix(ref writer, actionExample);
            };
        }
        
        private static RecordLikeClass CloneOrChange_RecordLikeClass(RecordLikeClass itemToConvert, Func<ILazinator, ILazinator> cloneOrChangeFunc, bool avoidCloningIfPossible)
        {
            return new RecordLikeClass((int) (itemToConvert.Age),(Example) cloneOrChangeFunc((itemToConvert.Example)));
        }
        
        private static RecordLikeType ConvertFromBytes_RecordLikeType(LazinatorMemory storage)
        {
            if (storage.Length == 0)
            {
                return default;
            }
            storage.LazinatorShouldNotReturnToPool();
            ReadOnlySpan<byte> span = storage.ReadOnlySpan;
            
            int bytesSoFar = 0;
            
            int item1 = span.ToDecompressedInt(ref bytesSoFar);
            
            string item2 = span.ToString_VarIntLengthUtf8(ref bytesSoFar);
            
            var tupleType = new RecordLikeType(item1, item2);
            
            return tupleType;
        }
        
        private static void ConvertToBytes_RecordLikeType(ref BinaryBufferWriter writer, RecordLikeType itemToConvert, IncludeChildrenMode includeChildrenMode, bool verifyCleanness, bool updateStoredBuffer)
        {
            writer.LazinatorMemory.LazinatorShouldNotReturnToPool();
            
            CompressedIntegralTypes.WriteCompressedInt(ref writer, itemToConvert.Age);
            
            EncodeCharAndString.WriteStringUtf8WithVarIntPrefix(ref writer, itemToConvert.Name);
        }
        
        private static RecordLikeType CloneOrChange_RecordLikeType(RecordLikeType itemToConvert, Func<ILazinator, ILazinator> cloneOrChangeFunc, bool avoidCloningIfPossible)
        {
            return new RecordLikeType((int) (itemToConvert.Age),(string) (itemToConvert.Name));
        }
        
        private static RecordLikeTypeWithLazinator ConvertFromBytes_RecordLikeTypeWithLazinator(LazinatorMemory storage)
        {
            if (storage.Length == 0)
            {
                return default;
            }
            storage.LazinatorShouldNotReturnToPool();
            ReadOnlySpan<byte> span = storage.ReadOnlySpan;
            
            int bytesSoFar = 0;
            
            int item1 = span.ToDecompressedInt(ref bytesSoFar);
            
            string item2 = span.ToString_VarIntLengthUtf8(ref bytesSoFar);
            
            Example item3 = default;
            int lengthCollectionMember_item3 = span.ToInt32(ref bytesSoFar);
            if (lengthCollectionMember_item3 != 0)
            {
                LazinatorMemory childData = storage.Slice(bytesSoFar, lengthCollectionMember_item3);
                item3 = DeserializationFactory.Instance.CreateBasedOnType<Example>(childData);
            }
            bytesSoFar += lengthCollectionMember_item3;
            
            var tupleType = new RecordLikeTypeWithLazinator(item1, item2, item3);
            
            return tupleType;
        }
        
        private static void ConvertToBytes_RecordLikeTypeWithLazinator(ref BinaryBufferWriter writer, RecordLikeTypeWithLazinator itemToConvert, IncludeChildrenMode includeChildrenMode, bool verifyCleanness, bool updateStoredBuffer)
        {
            writer.LazinatorMemory.LazinatorShouldNotReturnToPool();
            
            CompressedIntegralTypes.WriteCompressedInt(ref writer, itemToConvert.Age);
            
            EncodeCharAndString.WriteStringUtf8WithVarIntPrefix(ref writer, itemToConvert.Name);
            
            if (itemToConvert.Example == null)
            {
                writer.Write((uint)0);
            }
            else
            {
                void actionExample(ref BinaryBufferWriter w) => itemToConvert.Example.SerializeExistingBuffer(ref w, includeChildrenMode, verifyCleanness, updateStoredBuffer);
                WriteToBinaryWithIntLengthPrefix(ref writer, actionExample);
            };
        }
        
        private static RecordLikeTypeWithLazinator CloneOrChange_RecordLikeTypeWithLazinator(RecordLikeTypeWithLazinator itemToConvert, Func<ILazinator, ILazinator> cloneOrChangeFunc, bool avoidCloningIfPossible)
        {
            return new RecordLikeTypeWithLazinator((int) (itemToConvert.Age),(string) (itemToConvert.Name),(Example) cloneOrChangeFunc((itemToConvert.Example)));
        }
        
    }
}
