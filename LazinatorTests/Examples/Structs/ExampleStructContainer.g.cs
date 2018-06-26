//ddb81bc6-929c-c86f-8b3c-511a584d72d6
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by the Lazinator tool, version 0.1.0.153
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace LazinatorTests.Examples
{
    using Lazinator.Attributes;
    using Lazinator.Buffers;
    using Lazinator.Core;
    using Lazinator.Exceptions;
    using Lazinator.Support;
    using Lazinator.Wrappers;
    using System;
    using System.Buffers;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Runtime.InteropServices;
    using static Lazinator.Core.LazinatorUtilities;
    
    [Autogenerated]
    public partial class ExampleStructContainer : ILazinator
    {
        public bool IsStruct => false;
        
        /* Serialization, deserialization, and object relationships */
        
        public ExampleStructContainer() : base()
        {
        }
        
        public virtual LazinatorParentsCollection LazinatorParents { get; set; }
        
        protected IncludeChildrenMode OriginalIncludeChildrenMode;
        
        public virtual int Deserialize()
        {
            ResetAccessedProperties();
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
        
        public virtual MemoryInBuffer SerializeNewBuffer(IncludeChildrenMode includeChildrenMode, bool verifyCleanness)
        {
            return EncodeOrRecycleToNewBuffer(includeChildrenMode, OriginalIncludeChildrenMode, verifyCleanness, IsDirty, DescendantIsDirty, false, LazinatorObjectBytes, (StreamManuallyDelegate) EncodeToNewBuffer);
        }
        
        protected virtual MemoryInBuffer EncodeToNewBuffer(IncludeChildrenMode includeChildrenMode, bool verifyCleanness) => LazinatorUtilities.EncodeToNewBinaryBufferWriter(this, includeChildrenMode, verifyCleanness);
        
        public virtual ILazinator CloneLazinator()
        {
            return CloneLazinator(OriginalIncludeChildrenMode);
        }
        
        public virtual ILazinator CloneLazinator(IncludeChildrenMode includeChildrenMode)
        {
            MemoryInBuffer bytes = EncodeOrRecycleToNewBuffer(includeChildrenMode, OriginalIncludeChildrenMode, false, IsDirty, DescendantIsDirty, false, LazinatorObjectBytes, (StreamManuallyDelegate)EncodeToNewBuffer);
            var clone = new ExampleStructContainer()
            {
                LazinatorParents = LazinatorParents,
                OriginalIncludeChildrenMode = includeChildrenMode,
                HierarchyBytes = bytes,
            };
            clone.LazinatorParents = default;
            return clone;
        }
        
        public virtual bool HasChanged { get; set; }
        
        protected bool _IsDirty;
        public virtual bool IsDirty
        {
            [DebuggerStepThrough]
            get => _IsDirty;
            [DebuggerStepThrough]
            set
            {
                _IsDirty = value;
                if (_IsDirty)
                {
                    LazinatorParents.InformParentsOfDirtiness();
                    HasChanged = true;
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
                        _DescendantHasChanged = true;
                        LazinatorParents.InformParentsOfDirtiness();
                    }
                }
                if (_DescendantIsDirty)
                {
                    _DescendantHasChanged = true;
                }
            }
        }
        
        private MemoryInBuffer _HierarchyBytes;
        public virtual MemoryInBuffer HierarchyBytes
        {
            set
            {
                _HierarchyBytes = value;
                LazinatorObjectBytes = value.FilledMemory;
            }
        }
        
        protected ReadOnlyMemory<byte> _LazinatorObjectBytes;
        public virtual ReadOnlyMemory<byte> LazinatorObjectBytes
        {
            get => _LazinatorObjectBytes;
            set
            {
                _LazinatorObjectBytes = value;
                int length = Deserialize();
                _LazinatorObjectBytes = _LazinatorObjectBytes.Slice(0, length);
            }
        }
        
        public virtual void LazinatorConvertToBytes()
        {
            if (!IsDirty && !DescendantIsDirty && _LazinatorObjectBytes.Length > 0)
            {
                return;
            }
            MemoryInBuffer bytes = EncodeOrRecycleToNewBuffer(IncludeChildrenMode.IncludeAllChildren, OriginalIncludeChildrenMode, false, IsDirty, DescendantIsDirty, false, LazinatorObjectBytes, (StreamManuallyDelegate)EncodeToNewBuffer);
            _LazinatorObjectBytes = bytes.FilledMemory;
        }
        
        public virtual int GetByteLength()
        {
            LazinatorConvertToBytes();
            return _LazinatorObjectBytes.Length;
        }
        
        public virtual uint GetBinaryHashCode32()
        {
            LazinatorConvertToBytes();
            return FarmhashByteSpans.Hash32(LazinatorObjectBytes.Span);
        }
        
        public virtual ulong GetBinaryHashCode64()
        {
            LazinatorConvertToBytes();
            return FarmhashByteSpans.Hash64(LazinatorObjectBytes.Span);
        }
        
        public virtual Guid GetBinaryHashCode128()
        {
            LazinatorConvertToBytes();
            return FarmhashByteSpans.Hash128(LazinatorObjectBytes.Span);
        }
        
        /* Property definitions */
        
        protected int _IntWrapper_ByteIndex;
        protected int _MyExampleStruct_ByteIndex;
        protected int _MyListExampleStruct_ByteIndex;
        protected int _MyListNullableExampleStruct_ByteIndex;
        protected virtual int _IntWrapper_ByteLength => _MyExampleStruct_ByteIndex - _IntWrapper_ByteIndex;
        protected virtual int _MyExampleStruct_ByteLength => _MyListExampleStruct_ByteIndex - _MyExampleStruct_ByteIndex;
        protected virtual int _MyListExampleStruct_ByteLength => _MyListNullableExampleStruct_ByteIndex - _MyListExampleStruct_ByteIndex;
        private int _ExampleStructContainer_EndByteIndex;
        protected virtual int _MyListNullableExampleStruct_ByteLength => _ExampleStructContainer_EndByteIndex - _MyListNullableExampleStruct_ByteIndex;
        
        private WInt _IntWrapper;
        public WInt IntWrapper
        {
            get
            {
                if (!_IntWrapper_Accessed)
                {
                    if (LazinatorObjectBytes.Length == 0)
                    {
                        _IntWrapper = default(WInt);
                    }
                    else
                    {
                        ReadOnlyMemory<byte> childData = GetChildSlice(LazinatorObjectBytes, _IntWrapper_ByteIndex, _IntWrapper_ByteLength, false, true, null);
                        _IntWrapper = new WInt()
                        {
                            LazinatorParents = new LazinatorParentsCollection(this),
                            LazinatorObjectBytes = childData,
                        };
                    }
                    _IntWrapper_Accessed = true;
                } 
                return _IntWrapper;
            }
            set
            {
                
                value.LazinatorParents = value.LazinatorParents.WithAdded(this);
                value.IsDirty = true;
                IsDirty = true;
                _IntWrapper = value;
                _IntWrapper_Accessed = true;
            }
        }
        protected bool _IntWrapper_Accessed;
        public WInt IntWrapper_Copy
        {
            get
            {
                if (!_IntWrapper_Accessed)
                {
                    if (LazinatorObjectBytes.Length == 0)
                    {
                        return default(WInt);
                    }
                    else
                    {
                        ReadOnlyMemory<byte> childData = GetChildSlice(LazinatorObjectBytes, _IntWrapper_ByteIndex, _IntWrapper_ByteLength, false, true, null);
                        return new WInt()
                        {
                            LazinatorObjectBytes = childData,
                        };
                    }
                }
                return _IntWrapper;
            }
        }
        private ExampleStruct _MyExampleStruct;
        public ExampleStruct MyExampleStruct
        {
            get
            {
                if (!_MyExampleStruct_Accessed)
                {
                    if (LazinatorObjectBytes.Length == 0)
                    {
                        _MyExampleStruct = default(ExampleStruct);
                    }
                    else
                    {
                        ReadOnlyMemory<byte> childData = GetChildSlice(LazinatorObjectBytes, _MyExampleStruct_ByteIndex, _MyExampleStruct_ByteLength, false, false, null);
                        _MyExampleStruct = new ExampleStruct()
                        {
                            LazinatorParents = new LazinatorParentsCollection(this),
                            LazinatorObjectBytes = childData,
                        };
                    }
                    _MyExampleStruct_Accessed = true;
                } 
                return _MyExampleStruct;
            }
            set
            {
                
                value.LazinatorParents = value.LazinatorParents.WithAdded(this);
                value.IsDirty = true;
                IsDirty = true;
                _MyExampleStruct = value;
                _MyExampleStruct_Accessed = true;
            }
        }
        protected bool _MyExampleStruct_Accessed;
        public ExampleStruct MyExampleStruct_Copy
        {
            get
            {
                if (!_MyExampleStruct_Accessed)
                {
                    if (LazinatorObjectBytes.Length == 0)
                    {
                        return default(ExampleStruct);
                    }
                    else
                    {
                        ReadOnlyMemory<byte> childData = GetChildSlice(LazinatorObjectBytes, _MyExampleStruct_ByteIndex, _MyExampleStruct_ByteLength, false, false, null);
                        return new ExampleStruct()
                        {
                            LazinatorObjectBytes = childData,
                        };
                    }
                }
                return _MyExampleStruct;
            }
        }
        private List<ExampleStruct> _MyListExampleStruct;
        public List<ExampleStruct> MyListExampleStruct
        {
            get
            {
                if (!_MyListExampleStruct_Accessed)
                {
                    if (LazinatorObjectBytes.Length == 0)
                    {
                        _MyListExampleStruct = default(List<ExampleStruct>);
                    }
                    else
                    {
                        ReadOnlyMemory<byte> childData = GetChildSlice(LazinatorObjectBytes, _MyListExampleStruct_ByteIndex, _MyListExampleStruct_ByteLength, false, false, null);
                        _MyListExampleStruct = ConvertFromBytes_List_GExampleStruct_g(childData);
                    }
                    _MyListExampleStruct_Accessed = true;
                }
                IsDirty = true; 
                return _MyListExampleStruct;
            }
            set
            {
                IsDirty = true;
                _MyListExampleStruct = value;
                _MyListExampleStruct_Accessed = true;
            }
        }
        protected bool _MyListExampleStruct_Accessed;
        private List<WNullableStruct<ExampleStruct>> _MyListNullableExampleStruct;
        public List<WNullableStruct<ExampleStruct>> MyListNullableExampleStruct
        {
            get
            {
                if (!_MyListNullableExampleStruct_Accessed)
                {
                    if (LazinatorObjectBytes.Length == 0)
                    {
                        _MyListNullableExampleStruct = default(List<WNullableStruct<ExampleStruct>>);
                    }
                    else
                    {
                        ReadOnlyMemory<byte> childData = GetChildSlice(LazinatorObjectBytes, _MyListNullableExampleStruct_ByteIndex, _MyListNullableExampleStruct_ByteLength, false, false, null);
                        _MyListNullableExampleStruct = ConvertFromBytes_List_GWNullableStruct_GExampleStruct_g_g(childData);
                    }
                    _MyListNullableExampleStruct_Accessed = true;
                }
                IsDirty = true; 
                return _MyListNullableExampleStruct;
            }
            set
            {
                IsDirty = true;
                _MyListNullableExampleStruct = value;
                _MyListNullableExampleStruct_Accessed = true;
            }
        }
        protected bool _MyListNullableExampleStruct_Accessed;
        
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
            if (enumerateNulls && (System.Collections.Generic.EqualityComparer<WInt>.Default.Equals(IntWrapper, default(WInt))))
            {
                yield return ("IntWrapper", default);
            }
            else if ((!exploreOnlyDeserializedChildren && !System.Collections.Generic.EqualityComparer<WInt>.Default.Equals(IntWrapper, default(WInt))) || (_IntWrapper_Accessed && !System.Collections.Generic.EqualityComparer<WInt>.Default.Equals(_IntWrapper, default(WInt))))
            {
                foreach (ILazinator toYield in IntWrapper.EnumerateLazinatorNodes(matchCriterion, stopExploringBelowMatch, exploreCriterion, exploreOnlyDeserializedChildren, enumerateNulls))
                {
                    yield return ("IntWrapper", toYield);
                }
            }
            if (enumerateNulls && (System.Collections.Generic.EqualityComparer<ExampleStruct>.Default.Equals(MyExampleStruct, default(ExampleStruct))))
            {
                yield return ("MyExampleStruct", default);
            }
            else if ((!exploreOnlyDeserializedChildren && !System.Collections.Generic.EqualityComparer<ExampleStruct>.Default.Equals(MyExampleStruct, default(ExampleStruct))) || (_MyExampleStruct_Accessed && !System.Collections.Generic.EqualityComparer<ExampleStruct>.Default.Equals(_MyExampleStruct, default(ExampleStruct))))
            {
                foreach (ILazinator toYield in MyExampleStruct.EnumerateLazinatorNodes(matchCriterion, stopExploringBelowMatch, exploreCriterion, exploreOnlyDeserializedChildren, enumerateNulls))
                {
                    yield return ("MyExampleStruct", toYield);
                }
            }
            yield break;
        }
        
        
        public virtual IEnumerable<(string propertyName, object descendant)> EnumerateNonLazinatorProperties()
        {
            yield return ("MyListExampleStruct", (object)MyListExampleStruct);
            yield return ("MyListNullableExampleStruct", (object)MyListNullableExampleStruct);
            yield break;
        }
        
        protected virtual void ResetAccessedProperties()
        {
            _IntWrapper_Accessed = _MyExampleStruct_Accessed = _MyListExampleStruct_Accessed = _MyListNullableExampleStruct_Accessed = false;
        }
        
        /* Conversion */
        
        public virtual int LazinatorUniqueID => 217;
        
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
            _IntWrapper_ByteIndex = bytesSoFar;
            if (includeChildrenMode != IncludeChildrenMode.ExcludeAllChildren && includeChildrenMode != IncludeChildrenMode.IncludeOnlyIncludableChildren) 
            {
                bytesSoFar = span.ToByte(ref bytesSoFar) + bytesSoFar;
            }
            _MyExampleStruct_ByteIndex = bytesSoFar;
            if (includeChildrenMode != IncludeChildrenMode.ExcludeAllChildren && includeChildrenMode != IncludeChildrenMode.IncludeOnlyIncludableChildren) 
            {
                bytesSoFar = span.ToInt32(ref bytesSoFar) + bytesSoFar;
            }
            _MyListExampleStruct_ByteIndex = bytesSoFar;
            bytesSoFar = span.ToInt32(ref bytesSoFar) + bytesSoFar;
            _MyListNullableExampleStruct_ByteIndex = bytesSoFar;
            bytesSoFar = span.ToInt32(ref bytesSoFar) + bytesSoFar;
            _ExampleStructContainer_EndByteIndex = bytesSoFar;
        }
        
        public virtual void SerializeExistingBuffer(BinaryBufferWriter writer, IncludeChildrenMode includeChildrenMode, bool verifyCleanness, bool updateStoredBuffer)
        {
            if (includeChildrenMode != IncludeChildrenMode.IncludeAllChildren)
            {
                updateStoredBuffer = false;
            }
            int startPosition = writer.Position;
            WritePropertiesIntoBuffer(writer, includeChildrenMode, verifyCleanness, updateStoredBuffer, true);
            if (updateStoredBuffer)
            {
                
                _IsDirty = false;
                _DescendantIsDirty = includeChildrenMode != IncludeChildrenMode.IncludeAllChildren && ((_IntWrapper_Accessed && (IntWrapper.IsDirty || IntWrapper.DescendantIsDirty)) || (_MyExampleStruct_Accessed && (MyExampleStruct.IsDirty || MyExampleStruct.DescendantIsDirty)));
                
                _LazinatorObjectBytes = writer.Slice(startPosition);
            }
        }
        protected virtual void WritePropertiesIntoBuffer(BinaryBufferWriter writer, IncludeChildrenMode includeChildrenMode, bool verifyCleanness, bool updateStoredBuffer, bool includeUniqueID)
        {
            int startPosition = writer.Position;
            int startOfObjectPosition = 0;
            // header information
            if (includeUniqueID)
            {
                if (LazinatorGenericID.IsEmpty)
                {
                    CompressedIntegralTypes.WriteCompressedInt(writer, LazinatorUniqueID);
                }
                else
                {
                    WriteLazinatorGenericID(writer, LazinatorGenericID);
                }
            }
            CompressedIntegralTypes.WriteCompressedInt(writer, Lazinator.Support.LazinatorVersionInfo.LazinatorIntVersion);
            CompressedIntegralTypes.WriteCompressedInt(writer, LazinatorObjectVersion);
            writer.Write((byte)includeChildrenMode);
            // write properties
            startOfObjectPosition = writer.Position;
            if (includeChildrenMode != IncludeChildrenMode.ExcludeAllChildren && includeChildrenMode != IncludeChildrenMode.IncludeOnlyIncludableChildren) 
            {
                WriteChild(writer, _IntWrapper, includeChildrenMode, _IntWrapper_Accessed, () => GetChildSlice(LazinatorObjectBytes, _IntWrapper_ByteIndex, _IntWrapper_ByteLength, false, true, null), verifyCleanness, updateStoredBuffer, true, false, this);
            }
            if (updateStoredBuffer)
            {
                _IntWrapper_ByteIndex = startOfObjectPosition - startPosition;
            }
            startOfObjectPosition = writer.Position;
            if (includeChildrenMode != IncludeChildrenMode.ExcludeAllChildren && includeChildrenMode != IncludeChildrenMode.IncludeOnlyIncludableChildren) 
            {
                WriteChild(writer, _MyExampleStruct, includeChildrenMode, _MyExampleStruct_Accessed, () => GetChildSlice(LazinatorObjectBytes, _MyExampleStruct_ByteIndex, _MyExampleStruct_ByteLength, false, false, null), verifyCleanness, updateStoredBuffer, false, false, this);
            }
            if (updateStoredBuffer)
            {
                _MyExampleStruct_ByteIndex = startOfObjectPosition - startPosition;
            }
            startOfObjectPosition = writer.Position;
            WriteNonLazinatorObject(
            nonLazinatorObject: _MyListExampleStruct, isBelievedDirty: _MyListExampleStruct_Accessed,
            isAccessed: _MyListExampleStruct_Accessed, writer: writer,
            getChildSliceForFieldFn: () => GetChildSlice(LazinatorObjectBytes, _MyListExampleStruct_ByteIndex, _MyListExampleStruct_ByteLength, false, false, null),
            verifyCleanness: false,
            binaryWriterAction: (w, v) =>
            ConvertToBytes_List_GExampleStruct_g(w, _MyListExampleStruct,
            includeChildrenMode, v, updateStoredBuffer));
            if (updateStoredBuffer)
            {
                _MyListExampleStruct_ByteIndex = startOfObjectPosition - startPosition;
            }
            startOfObjectPosition = writer.Position;
            WriteNonLazinatorObject(
            nonLazinatorObject: _MyListNullableExampleStruct, isBelievedDirty: _MyListNullableExampleStruct_Accessed,
            isAccessed: _MyListNullableExampleStruct_Accessed, writer: writer,
            getChildSliceForFieldFn: () => GetChildSlice(LazinatorObjectBytes, _MyListNullableExampleStruct_ByteIndex, _MyListNullableExampleStruct_ByteLength, false, false, null),
            verifyCleanness: false,
            binaryWriterAction: (w, v) =>
            ConvertToBytes_List_GWNullableStruct_GExampleStruct_g_g(w, _MyListNullableExampleStruct,
            includeChildrenMode, v, updateStoredBuffer));
            if (updateStoredBuffer)
            {
                _MyListNullableExampleStruct_ByteIndex = startOfObjectPosition - startPosition;
            }
            if (updateStoredBuffer)
            {
                _ExampleStructContainer_EndByteIndex = writer.Position - startPosition;
            }
        }
        
        /* Conversion of supported collections and tuples */
        
        private static List<ExampleStruct> ConvertFromBytes_List_GExampleStruct_g(ReadOnlyMemory<byte> storage)
        {
            if (storage.Length == 0)
            {
                return default(List<ExampleStruct>);
            }
            ReadOnlySpan<byte> span = storage.Span;
            
            int bytesSoFar = 0;
            int collectionLength = span.ToDecompressedInt(ref bytesSoFar);
            
            List<ExampleStruct> collection = new List<ExampleStruct>(collectionLength);
            for (int i = 0; i < collectionLength; i++)
            {
                int lengthCollectionMember = span.ToInt32(ref bytesSoFar);
                ReadOnlyMemory<byte> childData = storage.Slice(bytesSoFar, lengthCollectionMember);
                var item = new ExampleStruct()
                {
                    LazinatorObjectBytes = childData,
                };
                collection.Add(item);
                bytesSoFar += lengthCollectionMember;
            }
            
            return collection;
        }
        
        private static void ConvertToBytes_List_GExampleStruct_g(BinaryBufferWriter writer, List<ExampleStruct> itemToConvert, IncludeChildrenMode includeChildrenMode, bool verifyCleanness, bool updateStoredBuffer)
        {
            if (itemToConvert == default(List<ExampleStruct>))
            {
                return;
            }
            CompressedIntegralTypes.WriteCompressedInt(writer, itemToConvert.Count);
            int itemToConvertCount = itemToConvert.Count;
            for (int itemIndex = 0; itemIndex < itemToConvertCount; itemIndex++)
            {
                void action(BinaryBufferWriter w) => itemToConvert[itemIndex].SerializeExistingBuffer(writer, includeChildrenMode, verifyCleanness, updateStoredBuffer);
                WriteToBinaryWithIntLengthPrefix(writer, action);
            }
        }
        
        private static List<WNullableStruct<ExampleStruct>> ConvertFromBytes_List_GWNullableStruct_GExampleStruct_g_g(ReadOnlyMemory<byte> storage)
        {
            if (storage.Length == 0)
            {
                return default(List<WNullableStruct<ExampleStruct>>);
            }
            ReadOnlySpan<byte> span = storage.Span;
            
            int bytesSoFar = 0;
            int collectionLength = span.ToDecompressedInt(ref bytesSoFar);
            
            List<WNullableStruct<ExampleStruct>> collection = new List<WNullableStruct<ExampleStruct>>(collectionLength);
            for (int i = 0; i < collectionLength; i++)
            {
                int lengthCollectionMember = span.ToInt32(ref bytesSoFar);
                ReadOnlyMemory<byte> childData = storage.Slice(bytesSoFar, lengthCollectionMember);
                var item = new WNullableStruct<ExampleStruct>()
                {
                    LazinatorObjectBytes = childData,
                };
                collection.Add(item);
                bytesSoFar += lengthCollectionMember;
            }
            
            return collection;
        }
        
        private static void ConvertToBytes_List_GWNullableStruct_GExampleStruct_g_g(BinaryBufferWriter writer, List<WNullableStruct<ExampleStruct>> itemToConvert, IncludeChildrenMode includeChildrenMode, bool verifyCleanness, bool updateStoredBuffer)
        {
            if (itemToConvert == default(List<WNullableStruct<ExampleStruct>>))
            {
                return;
            }
            CompressedIntegralTypes.WriteCompressedInt(writer, itemToConvert.Count);
            int itemToConvertCount = itemToConvert.Count;
            for (int itemIndex = 0; itemIndex < itemToConvertCount; itemIndex++)
            {
                void action(BinaryBufferWriter w) => itemToConvert[itemIndex].SerializeExistingBuffer(writer, includeChildrenMode, verifyCleanness, updateStoredBuffer);
                WriteToBinaryWithIntLengthPrefix(writer, action);
            }
        }
        
    }
}
