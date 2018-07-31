//6ac94757-0c26-522d-2e45-6476a7c64ca8
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by the Lazinator tool, version 0.1.0.207
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace LazinatorTests.Examples.Abstract
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
    public partial class Concrete3 : ILazinator
    {
        /* Serialization, deserialization, and object relationships */
        
        public override LazinatorParentsCollection LazinatorParents { get; set; }
        
        protected IncludeChildrenMode OriginalIncludeChildrenMode;
        
        public override int Deserialize()
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
        
        public override LazinatorMemory SerializeNewBuffer(IncludeChildrenMode includeChildrenMode, bool verifyCleanness, bool updateStoredBuffer)
        {
            return EncodeOrRecycleToNewBuffer(includeChildrenMode, OriginalIncludeChildrenMode, verifyCleanness, IsDirty, DescendantIsDirty, false, LazinatorObjectBytes, (EncodeManuallyDelegate) EncodeToNewBuffer, updateStoredBuffer);
        }
        
        protected override LazinatorMemory EncodeToNewBuffer(IncludeChildrenMode includeChildrenMode, bool verifyCleanness, bool updateStoredBuffer) => LazinatorUtilities.EncodeToNewBinaryBufferWriter(this, includeChildrenMode, verifyCleanness, updateStoredBuffer);
        
        public override ILazinator CloneLazinator()
        {
            return CloneLazinator(OriginalIncludeChildrenMode);
        }
        
        public override ILazinator CloneLazinator(IncludeChildrenMode includeChildrenMode, bool updateStoredBuffer = false)
        {
            LazinatorMemory bytes = EncodeOrRecycleToNewBuffer(includeChildrenMode, OriginalIncludeChildrenMode, false, IsDirty, DescendantIsDirty, false, LazinatorObjectBytes, (EncodeManuallyDelegate)EncodeToNewBuffer, updateStoredBuffer);
            var clone = new Concrete3()
            {
                LazinatorParents = LazinatorParents,
                OriginalIncludeChildrenMode = includeChildrenMode,
                HierarchyBytes = bytes,
            };
            clone.LazinatorParents = default;
            return clone;
        }
        
        public override bool HasChanged { get; set; }
        
        protected bool _IsDirty;
        public override bool IsDirty
        {
            [DebuggerStepThrough]
            get => _IsDirty || LazinatorObjectBytes.Length == 0;
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
        public override bool DescendantHasChanged
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
        public override bool DescendantIsDirty
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
        
        private LazinatorMemory _HierarchyBytes;
        public override LazinatorMemory HierarchyBytes
        {
            set
            {
                _HierarchyBytes = value;
                LazinatorMemoryStorage = value;
            }
        }
        
        protected LazinatorMemory _LazinatorMemoryStorage; // TODO -- use only one memory storage
        public override LazinatorMemory LazinatorMemoryStorage
        {
            get => _LazinatorMemoryStorage;
            set
            {
                _LazinatorMemoryStorage = value;
                int length = Deserialize();
            }
        }
        
        public override ReadOnlyMemory<byte> LazinatorObjectBytes
        {
            get => LazinatorMemoryStorage?.Memory ?? LazinatorUtilities.EmptyReadOnlyMemory;
        }
        
        public override void EnsureLazinatorMemoryUpToDate()
        {
            if (!IsDirty && !DescendantIsDirty && LazinatorObjectBytes.Length > 0)
            {
                return;
            }
            EncodeOrRecycleToNewBuffer(IncludeChildrenMode.IncludeAllChildren, OriginalIncludeChildrenMode, false, IsDirty, DescendantIsDirty, false, LazinatorObjectBytes, (EncodeManuallyDelegate)EncodeToNewBuffer, true);
        }
        
        public override int GetByteLength()
        {
            EnsureLazinatorMemoryUpToDate();
            return LazinatorObjectBytes.Length;
        }
        
        public override uint GetBinaryHashCode32()
        {
            EnsureLazinatorMemoryUpToDate();
            return FarmhashByteSpans.Hash32(LazinatorObjectBytes.Span);
        }
        
        public override ulong GetBinaryHashCode64()
        {
            EnsureLazinatorMemoryUpToDate();
            return FarmhashByteSpans.Hash64(LazinatorObjectBytes.Span);
        }
        
        public override Guid GetBinaryHashCode128()
        {
            EnsureLazinatorMemoryUpToDate();
            return FarmhashByteSpans.Hash128(LazinatorObjectBytes.Span);
        }
        
        /* Property definitions */
        
        protected int _IntList3_ByteIndex;
        protected override int _Example2_ByteLength => _Example3_ByteIndex - _Example2_ByteIndex;
        protected override int _Example3_ByteLength => _IntList1_ByteIndex - _Example3_ByteIndex;
        protected override int _IntList1_ByteLength => _IntList2_ByteIndex - _IntList1_ByteIndex;
        protected override int _IntList2_ByteLength => _IntList3_ByteIndex - _IntList2_ByteIndex;
        private int _Concrete3_EndByteIndex;
        protected virtual int _IntList3_ByteLength => _Concrete3_EndByteIndex - _IntList3_ByteIndex;
        
        protected string _String1;
        public override string String1
        {
            get
            {
                return _String1;
            }
            set
            {
                IsDirty = true;
                _String1 = value;
            }
        }
        protected string _String2;
        public override string String2
        {
            get
            {
                return _String2;
            }
            set
            {
                IsDirty = true;
                _String2 = value;
            }
        }
        protected string _String3;
        public string String3
        {
            get
            {
                return _String3;
            }
            set
            {
                IsDirty = true;
                _String3 = value;
            }
        }
        protected Example _Example2;
        public override Example Example2
        {
            get
            {
                if (!_Example2_Accessed)
                {
                    if (LazinatorObjectBytes.Length == 0)
                    {
                        _Example2 = default(Example);
                    }
                    else
                    {
                        LazinatorMemory childData = GetChildSlice(LazinatorMemoryStorage, _Example2_ByteIndex, _Example2_ByteLength, false, false, null);
                        
                        _Example2 = DeserializationFactory.Instance.CreateBaseOrDerivedType(212, () => new Example(), childData, this); 
                    }
                    _Example2_Accessed = true;
                } 
                return _Example2;
            }
            set
            {
                if (_Example2 != null)
                {
                    _Example2.LazinatorParents = _Example2.LazinatorParents.WithRemoved(this);
                }
                if (value != null)
                {
                    value.LazinatorParents = value.LazinatorParents.WithAdded(this);
                }
                
                IsDirty = true;
                DescendantIsDirty = true;
                _Example2 = value;
                _Example2_Accessed = true;
            }
        }
        protected Example _Example3;
        public override Example Example3
        {
            get
            {
                if (!_Example3_Accessed)
                {
                    if (LazinatorObjectBytes.Length == 0)
                    {
                        _Example3 = default(Example);
                    }
                    else
                    {
                        LazinatorMemory childData = GetChildSlice(LazinatorMemoryStorage, _Example3_ByteIndex, _Example3_ByteLength, false, false, null);
                        
                        _Example3 = DeserializationFactory.Instance.CreateBaseOrDerivedType(212, () => new Example(), childData, this); 
                    }
                    _Example3_Accessed = true;
                } 
                return _Example3;
            }
            set
            {
                if (_Example3 != null)
                {
                    _Example3.LazinatorParents = _Example3.LazinatorParents.WithRemoved(this);
                }
                if (value != null)
                {
                    value.LazinatorParents = value.LazinatorParents.WithAdded(this);
                }
                
                IsDirty = true;
                DescendantIsDirty = true;
                _Example3 = value;
                _Example3_Accessed = true;
            }
        }
        protected List<int> _IntList1;
        public override List<int> IntList1
        {
            get
            {
                if (!_IntList1_Accessed)
                {
                    if (LazinatorObjectBytes.Length == 0)
                    {
                        _IntList1 = default(List<int>);
                    }
                    else
                    {
                        LazinatorMemory childData = GetChildSlice(LazinatorMemoryStorage, _IntList1_ByteIndex, _IntList1_ByteLength, false, false, null);
                        _IntList1 = ConvertFromBytes_List_Gint_g(childData);
                    }
                    _IntList1_Accessed = true;
                }
                IsDirty = true; 
                return _IntList1;
            }
            set
            {
                IsDirty = true;
                DescendantIsDirty = true;
                _IntList1 = value;
                _IntList1_Accessed = true;
            }
        }
        protected List<int> _IntList2;
        public override List<int> IntList2
        {
            get
            {
                if (!_IntList2_Accessed)
                {
                    if (LazinatorObjectBytes.Length == 0)
                    {
                        _IntList2 = default(List<int>);
                    }
                    else
                    {
                        LazinatorMemory childData = GetChildSlice(LazinatorMemoryStorage, _IntList2_ByteIndex, _IntList2_ByteLength, false, false, null);
                        _IntList2 = ConvertFromBytes_List_Gint_g(childData);
                    }
                    _IntList2_Accessed = true;
                }
                IsDirty = true; 
                return _IntList2;
            }
            set
            {
                IsDirty = true;
                DescendantIsDirty = true;
                _IntList2 = value;
                _IntList2_Accessed = true;
            }
        }
        protected List<int> _IntList3;
        public List<int> IntList3
        {
            get
            {
                if (!_IntList3_Accessed)
                {
                    if (LazinatorObjectBytes.Length == 0)
                    {
                        _IntList3 = default(List<int>);
                    }
                    else
                    {
                        LazinatorMemory childData = GetChildSlice(LazinatorMemoryStorage, _IntList3_ByteIndex, _IntList3_ByteLength, false, false, null);
                        _IntList3 = ConvertFromBytes_List_Gint_g(childData);
                    }
                    _IntList3_Accessed = true;
                }
                IsDirty = true; 
                return _IntList3;
            }
            set
            {
                IsDirty = true;
                DescendantIsDirty = true;
                _IntList3 = value;
                _IntList3_Accessed = true;
            }
        }
        protected bool _IntList3_Accessed;
        
        public override IEnumerable<ILazinator> EnumerateLazinatorNodes(Func<ILazinator, bool> matchCriterion, bool stopExploringBelowMatch, Func<ILazinator, bool> exploreCriterion, bool exploreOnlyDeserializedChildren, bool enumerateNulls)
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
        
        public override IEnumerable<(string propertyName, ILazinator descendant)> EnumerateLazinatorDescendants(Func<ILazinator, bool> matchCriterion, bool stopExploringBelowMatch, Func<ILazinator, bool> exploreCriterion, bool exploreOnlyDeserializedChildren, bool enumerateNulls)
        {
            if (enumerateNulls && (Example2 == null))
            {
                yield return ("Example2", default);
            }
            else if ((!exploreOnlyDeserializedChildren && Example2 != null) || (_Example2_Accessed && _Example2 != null))
            {
                foreach (ILazinator toYield in Example2.EnumerateLazinatorNodes(matchCriterion, stopExploringBelowMatch, exploreCriterion, exploreOnlyDeserializedChildren, enumerateNulls))
                {
                    yield return ("Example2", toYield);
                }
            }
            if (enumerateNulls && (Example3 == null))
            {
                yield return ("Example3", default);
            }
            else if ((!exploreOnlyDeserializedChildren && Example3 != null) || (_Example3_Accessed && _Example3 != null))
            {
                foreach (ILazinator toYield in Example3.EnumerateLazinatorNodes(matchCriterion, stopExploringBelowMatch, exploreCriterion, exploreOnlyDeserializedChildren, enumerateNulls))
                {
                    yield return ("Example3", toYield);
                }
            }
            yield break;
        }
        
        
        public override IEnumerable<(string propertyName, object descendant)> EnumerateNonLazinatorProperties()
        {
            yield return ("String1", (object)String1);
            yield return ("String2", (object)String2);
            yield return ("String3", (object)String3);
            yield return ("IntList1", (object)IntList1);
            yield return ("IntList2", (object)IntList2);
            yield return ("IntList3", (object)IntList3);
            yield break;
        }
        
        protected override void ResetAccessedProperties()
        {
            _Example2_Accessed = _Example3_Accessed = _IntList1_Accessed = _IntList2_Accessed = _IntList3_Accessed = false;
            IsDirty = false;
            DescendantIsDirty = false;
            HasChanged = false;
            DescendantHasChanged = false;
        }
        
        /* Conversion */
        
        public override int LazinatorUniqueID => 237;
        
        protected override bool ContainsOpenGenericParameters => false;
        protected override LazinatorGenericIDType _LazinatorGenericID { get; set; }
        public override LazinatorGenericIDType LazinatorGenericID
        {
            get => default;
            set { }
        }
        
        public override int LazinatorObjectVersion { get; set; } = 0;
        
        
        public override void ConvertFromBytesAfterHeader(IncludeChildrenMode includeChildrenMode, int serializedVersionNumber, ref int bytesSoFar)
        {
            ReadOnlySpan<byte> span = LazinatorObjectBytes.Span;
            _String1 = span.ToString_VarIntLengthUtf8(ref bytesSoFar);
            _String2 = span.ToString_VarIntLengthUtf8(ref bytesSoFar);
            _String3 = span.ToString_VarIntLengthUtf8(ref bytesSoFar);
            _Example2_ByteIndex = bytesSoFar;
            if (includeChildrenMode != IncludeChildrenMode.ExcludeAllChildren && includeChildrenMode != IncludeChildrenMode.IncludeOnlyIncludableChildren) 
            {
                bytesSoFar = span.ToInt32(ref bytesSoFar) + bytesSoFar;
            }
            _Example3_ByteIndex = bytesSoFar;
            if (includeChildrenMode != IncludeChildrenMode.ExcludeAllChildren && includeChildrenMode != IncludeChildrenMode.IncludeOnlyIncludableChildren) 
            {
                bytesSoFar = span.ToInt32(ref bytesSoFar) + bytesSoFar;
            }
            _IntList1_ByteIndex = bytesSoFar;
            bytesSoFar = span.ToInt32(ref bytesSoFar) + bytesSoFar;
            _IntList2_ByteIndex = bytesSoFar;
            bytesSoFar = span.ToInt32(ref bytesSoFar) + bytesSoFar;
            _IntList3_ByteIndex = bytesSoFar;
            bytesSoFar = span.ToInt32(ref bytesSoFar) + bytesSoFar;
            _Concrete3_EndByteIndex = bytesSoFar;
        }
        
        public override void SerializeExistingBuffer(ref BinaryBufferWriter writer, IncludeChildrenMode includeChildrenMode, bool verifyCleanness, bool updateStoredBuffer)
        {
            if (includeChildrenMode != IncludeChildrenMode.IncludeAllChildren)
            {
                updateStoredBuffer = false;
            }
            int startPosition = writer.Position;
            WritePropertiesIntoBuffer(ref writer, includeChildrenMode, verifyCleanness, updateStoredBuffer, true);
            if (updateStoredBuffer)
            {
                
                _IsDirty = false;
                if (includeChildrenMode == IncludeChildrenMode.IncludeAllChildren)
                {
                    _DescendantIsDirty = false;
                }
                else
                {
                    throw new Exception("Cannot update stored buffer when serializing only some children.");
                }
                
                _LazinatorMemoryStorage = writer.LazinatorMemorySlice(startPosition);
            }
        }
        protected override void WritePropertiesIntoBuffer(ref BinaryBufferWriter writer, IncludeChildrenMode includeChildrenMode, bool verifyCleanness, bool updateStoredBuffer, bool includeUniqueID)
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
            EncodeCharAndString.WriteStringUtf8WithVarIntPrefix(ref writer, _String1);
            EncodeCharAndString.WriteStringUtf8WithVarIntPrefix(ref writer, _String2);
            EncodeCharAndString.WriteStringUtf8WithVarIntPrefix(ref writer, _String3);
            startOfObjectPosition = writer.Position;
            if (includeChildrenMode != IncludeChildrenMode.ExcludeAllChildren && includeChildrenMode != IncludeChildrenMode.IncludeOnlyIncludableChildren) 
            {
                if (includeChildrenMode != IncludeChildrenMode.IncludeAllChildren && !_Example2_Accessed)
                {
                    var deserialized = Example2;
                }
                WriteChild(ref writer, _Example2, includeChildrenMode, _Example2_Accessed, () => GetChildSlice(LazinatorMemoryStorage, _Example2_ByteIndex, _Example2_ByteLength, false, false, null), verifyCleanness, updateStoredBuffer, false, false, this);
            }
            if (updateStoredBuffer)
            {
                _Example2_ByteIndex = startOfObjectPosition - startPosition;
            }
            startOfObjectPosition = writer.Position;
            if (includeChildrenMode != IncludeChildrenMode.ExcludeAllChildren && includeChildrenMode != IncludeChildrenMode.IncludeOnlyIncludableChildren) 
            {
                if (includeChildrenMode != IncludeChildrenMode.IncludeAllChildren && !_Example3_Accessed)
                {
                    var deserialized = Example3;
                }
                WriteChild(ref writer, _Example3, includeChildrenMode, _Example3_Accessed, () => GetChildSlice(LazinatorMemoryStorage, _Example3_ByteIndex, _Example3_ByteLength, false, false, null), verifyCleanness, updateStoredBuffer, false, false, this);
            }
            if (updateStoredBuffer)
            {
                _Example3_ByteIndex = startOfObjectPosition - startPosition;
            }
            startOfObjectPosition = writer.Position;
            if (includeChildrenMode != IncludeChildrenMode.IncludeAllChildren && !_IntList1_Accessed)
            {
                var deserialized = IntList1;
            }
            WriteNonLazinatorObject(
            nonLazinatorObject: _IntList1, isBelievedDirty: _IntList1_Accessed,
            isAccessed: _IntList1_Accessed, writer: ref writer,
            getChildSliceForFieldFn: () => GetChildSlice(LazinatorMemoryStorage, _IntList1_ByteIndex, _IntList1_ByteLength, false, false, null),
            verifyCleanness: false,
            binaryWriterAction: (ref BinaryBufferWriter w, bool v) =>
            ConvertToBytes_List_Gint_g(ref w, _IntList1,
            includeChildrenMode, v, updateStoredBuffer));
            if (updateStoredBuffer)
            {
                _IntList1_ByteIndex = startOfObjectPosition - startPosition;
            }
            startOfObjectPosition = writer.Position;
            if (includeChildrenMode != IncludeChildrenMode.IncludeAllChildren && !_IntList2_Accessed)
            {
                var deserialized = IntList2;
            }
            WriteNonLazinatorObject(
            nonLazinatorObject: _IntList2, isBelievedDirty: _IntList2_Accessed,
            isAccessed: _IntList2_Accessed, writer: ref writer,
            getChildSliceForFieldFn: () => GetChildSlice(LazinatorMemoryStorage, _IntList2_ByteIndex, _IntList2_ByteLength, false, false, null),
            verifyCleanness: false,
            binaryWriterAction: (ref BinaryBufferWriter w, bool v) =>
            ConvertToBytes_List_Gint_g(ref w, _IntList2,
            includeChildrenMode, v, updateStoredBuffer));
            if (updateStoredBuffer)
            {
                _IntList2_ByteIndex = startOfObjectPosition - startPosition;
            }
            startOfObjectPosition = writer.Position;
            if (includeChildrenMode != IncludeChildrenMode.IncludeAllChildren && !_IntList3_Accessed)
            {
                var deserialized = IntList3;
            }
            WriteNonLazinatorObject(
            nonLazinatorObject: _IntList3, isBelievedDirty: _IntList3_Accessed,
            isAccessed: _IntList3_Accessed, writer: ref writer,
            getChildSliceForFieldFn: () => GetChildSlice(LazinatorMemoryStorage, _IntList3_ByteIndex, _IntList3_ByteLength, false, false, null),
            verifyCleanness: false,
            binaryWriterAction: (ref BinaryBufferWriter w, bool v) =>
            ConvertToBytes_List_Gint_g(ref w, _IntList3,
            includeChildrenMode, v, updateStoredBuffer));
            if (updateStoredBuffer)
            {
                _IntList3_ByteIndex = startOfObjectPosition - startPosition;
            }
            if (updateStoredBuffer)
            {
                _Concrete3_EndByteIndex = writer.Position - startPosition;
            }
        }
        
        /* Conversion of supported collections and tuples */
        
        private static List<int> ConvertFromBytes_List_Gint_g(LazinatorMemory storage)
        {
            if (storage.Length == 0)
            {
                return default(List<int>);
            }
            ReadOnlySpan<byte> span = storage.Span;
            
            int bytesSoFar = 0;
            int collectionLength = span.ToDecompressedInt(ref bytesSoFar);
            
            List<int> collection = new List<int>(collectionLength);
            for (int i = 0; i < collectionLength; i++)
            {
                int item = span.ToDecompressedInt(ref bytesSoFar);
                collection.Add(item);
            }
            
            return collection;
        }
        
        private static void ConvertToBytes_List_Gint_g(ref BinaryBufferWriter writer, List<int> itemToConvert, IncludeChildrenMode includeChildrenMode, bool verifyCleanness, bool updateStoredBuffer)
        {
            if (itemToConvert == default(List<int>))
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
        
    }
}
