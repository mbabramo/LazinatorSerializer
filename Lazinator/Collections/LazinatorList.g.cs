//db56083d-2e18-5cb4-d7a5-cdec71015861
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by the Lazinator tool, version 0.1.0.107
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Lazinator.Collections
{
    using Lazinator.Attributes;
    using Lazinator.Buffers;
    using Lazinator.Core;
    using Lazinator.Exceptions;
    using Lazinator.Support;
    using System;
    using System.Buffers;
    using System.Diagnostics;
    using System.IO;
    using System.Runtime.InteropServices;
    using static Lazinator.Core.LazinatorUtilities;
    
    [Autogenerated]
    public partial class LazinatorList<T> : ILazinator
    {
        /* Serialization, deserialization, and object relationships */
        
        protected ILazinator _LazinatorParentClass;
        public virtual ILazinator LazinatorParentClass 
        { 
            get => _LazinatorParentClass;
            set
            {
                _LazinatorParentClass = value;
                if (value != null && (value.IsDirty || value.DescendantIsDirty))
                {
                    DescendantIsDirty = true;
                }
            }
        }
        
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
            return EncodeOrRecycleToNewBuffer(includeChildrenMode, OriginalIncludeChildrenMode, true, verifyCleanness, IsDirty, DescendantIsDirty, false, LazinatorObjectBytes, (StreamManuallyDelegate) EncodeToNewBuffer);
        }
        
        protected virtual MemoryInBuffer EncodeToNewBuffer(IncludeChildrenMode includeChildrenMode, bool verifyCleanness) => LazinatorUtilities.EncodeToNewBinaryBufferWriter(this, includeChildrenMode, verifyCleanness);
        
        public virtual ILazinator CloneLazinator()
        {
            return CloneLazinator(OriginalIncludeChildrenMode);
        }
        
        public virtual ILazinator CloneLazinator(IncludeChildrenMode includeChildrenMode)
        {
            MemoryInBuffer bytes = EncodeOrRecycleToNewBuffer(includeChildrenMode, OriginalIncludeChildrenMode, false, false, IsDirty, DescendantIsDirty, false, LazinatorObjectBytes, (StreamManuallyDelegate)EncodeToNewBuffer);
            var clone = new LazinatorList<T>()
            {
                LazinatorParentClass = LazinatorParentClass,
                OriginalIncludeChildrenMode = includeChildrenMode,
                HierarchyBytes = bytes,
            };
            clone.LazinatorParentClass = null;
            return clone;
        }
        
        protected bool _IsDirty;
        public virtual bool IsDirty
        {
            [DebuggerStepThrough]
            get => _IsDirty;
            [DebuggerStepThrough]
            set
            {
                if (_IsDirty != value)
                {
                    _IsDirty = value;
                    if (_IsDirty)
                    {
                        InformParentOfDirtiness();
                    }
                }
            }
        }
        
        public virtual InformParentOfDirtinessDelegate InformParentOfDirtinessDelegate { get; set; }
        public virtual void InformParentOfDirtiness()
        {
            if (InformParentOfDirtinessDelegate == null)
            {
                if (LazinatorParentClass != null)
                {
                    LazinatorParentClass.DescendantIsDirty = true;
                }
            }
            else
            {
                InformParentOfDirtinessDelegate();
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
                    if (_DescendantIsDirty && LazinatorParentClass != null)
                    {
                        LazinatorParentClass.DescendantIsDirty = true;
                    }
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
            MemoryInBuffer bytes = EncodeOrRecycleToNewBuffer(IncludeChildrenMode.IncludeAllChildren, OriginalIncludeChildrenMode, false, false, IsDirty, DescendantIsDirty, false, LazinatorObjectBytes, (StreamManuallyDelegate)EncodeToNewBuffer);
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
        
        protected int _Offsets_ByteIndex;
        protected int _SerializedMainList_ByteIndex;
        protected virtual int _Offsets_ByteLength => _SerializedMainList_ByteIndex - _Offsets_ByteIndex;
        private int _LazinatorList_T_EndByteIndex;
        protected virtual int _SerializedMainList_ByteLength => _LazinatorList_T_EndByteIndex - _SerializedMainList_ByteIndex;
        
        private LazinatorOffsetList _Offsets;
        public LazinatorOffsetList Offsets
        {
            get
            {
                if (!_Offsets_Accessed)
                {
                    if (LazinatorObjectBytes.Length == 0)
                    {
                        _Offsets = default(LazinatorOffsetList);
                    }
                    else
                    {
                        ReadOnlyMemory<byte> childData = GetChildSlice(LazinatorObjectBytes, _Offsets_ByteIndex, _Offsets_ByteLength, false, false, null);
                        
                        _Offsets = DeserializationFactory.Instance.CreateBaseOrDerivedType(50, () => new LazinatorOffsetList(), childData, this); 
                    }
                    _Offsets_Accessed = true;
                }
                return _Offsets;
            }
            set
            {
                if (value != null)
                {
                    if (value.LazinatorParentClass != null)
                    {
                        throw new MovedLazinatorException($"The property Offsets cannot be set to a Lazinator object with a defined LazinatorParentClass, because AutoChangeParent is set to false in the configuration file and no attribute providing an exception is present.");
                    }
                    value.LazinatorParentClass = this;
                }
                IsDirty = true;
                _Offsets = value;
                if (_Offsets != null)
                {
                    _Offsets.IsDirty = true;
                }
                _Offsets_Accessed = true;
                LazinatorUtilities.ConfirmDescendantDirtinessConsistency(this);
            }
        }
        protected bool _Offsets_Accessed;
        private ReadOnlyMemory<byte> _SerializedMainList;
        public ReadOnlyMemory<byte> SerializedMainList
        {
            get
            {
                if (!_SerializedMainList_Accessed)
                {
                    ReadOnlyMemory<byte> childData = GetChildSlice(LazinatorObjectBytes, _SerializedMainList_ByteIndex, _SerializedMainList_ByteLength, false, false, null);
                    _SerializedMainList = childData;
                    _SerializedMainList_Accessed = true;
                }
                return _SerializedMainList;
            }
            set
            {
                
                IsDirty = true;
                _SerializedMainList = value;
                _SerializedMainList_Accessed = true;
                LazinatorUtilities.ConfirmDescendantDirtinessConsistency(this);
            }
        }
        protected bool _SerializedMainList_Accessed;
        
        protected virtual void ResetAccessedProperties()
        {
            _Offsets_Accessed = _SerializedMainList_Accessed = false;
        }
        
        /* Conversion */
        
        public virtual int LazinatorUniqueID => 51;
        
        protected virtual bool ContainsOpenGenericParameters => true;
        protected virtual System.Collections.Generic.List<int> _LazinatorGenericID { get; set; }
        public virtual System.Collections.Generic.List<int> LazinatorGenericID
        {
            get
            {
                if (_LazinatorGenericID == null)
                {
                    _LazinatorGenericID = DeserializationFactory.Instance.GetUniqueIDListForGenericType(51, new Type[] { typeof(T) });
                }
                return _LazinatorGenericID;
            }
            set
            {
                _LazinatorGenericID = value;
            }
        }
        
        public virtual int LazinatorObjectVersion { get; set; } = 0;
        
        
        public virtual void ConvertFromBytesAfterHeader(IncludeChildrenMode includeChildrenMode, int serializedVersionNumber, ref int bytesSoFar)
        {
            ReadOnlySpan<byte> span = LazinatorObjectBytes.Span;
            _Offsets_ByteIndex = bytesSoFar;
            if (includeChildrenMode != IncludeChildrenMode.ExcludeAllChildren && includeChildrenMode != IncludeChildrenMode.IncludeOnlyIncludableChildren) 
            {
                bytesSoFar = span.ToInt32(ref bytesSoFar) + bytesSoFar;
            }
            _SerializedMainList_ByteIndex = bytesSoFar;
            bytesSoFar = span.ToInt32(ref bytesSoFar) + bytesSoFar;
            _LazinatorList_T_EndByteIndex = bytesSoFar;
        }
        
        public virtual void SerializeExistingBuffer(BinaryBufferWriter writer, IncludeChildrenMode includeChildrenMode, bool verifyCleanness, bool updateStoredBuffer)
        {
            TabbedText.WriteLine($"Initiating serialization of Lazinator.Collections.LazinatorList<T> ");
            if (includeChildrenMode != IncludeChildrenMode.IncludeAllChildren)
            {
                updateStoredBuffer = false;
            }
            PreSerialization(verifyCleanness, updateStoredBuffer);
            int startPosition = writer.Position;
            WritePropertiesIntoBuffer(writer, includeChildrenMode, verifyCleanness, updateStoredBuffer, true);
            if (updateStoredBuffer)
            {
                
                _IsDirty = false;
                _DescendantIsDirty = includeChildrenMode != IncludeChildrenMode.IncludeAllChildren && ((_Offsets_Accessed && Offsets != null && (Offsets.IsDirty || Offsets.DescendantIsDirty)));
                
                _LazinatorObjectBytes = writer.Slice(startPosition);
            }
        }
        protected virtual void WritePropertiesIntoBuffer(BinaryBufferWriter writer, IncludeChildrenMode includeChildrenMode, bool verifyCleanness, bool updateStoredBuffer, bool includeUniqueID)
        {
            int startPosition = writer.Position;
            int startOfObjectPosition = 0;
            // header information
            TabbedText.WriteLine($"Writing properties for Lazinator.Collections.LazinatorList<T> starting at {writer.Position}.");
            TabbedText.WriteLine($"Includes? uniqueID {(LazinatorGenericID == null ? LazinatorUniqueID.ToString() : String.Join("","",LazinatorGenericID.ToArray()))} {includeUniqueID}, Lazinator version {Lazinator.Support.LazinatorVersionInfo.LazinatorIntVersion} True, Object version {LazinatorObjectVersion} True, IncludeChildrenMode {includeChildrenMode} True");
            TabbedText.WriteLine($"IsDirty {IsDirty} DescendantIsDirty {DescendantIsDirty} HasParentClass {LazinatorParentClass != null}");
            if (includeUniqueID)
            {
                if (LazinatorGenericID == null)
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
            TabbedText.WriteLine($"Byte {writer.Position}, Offsets (accessed? {_Offsets_Accessed}) (backing var null? {_Offsets == null}) ");
            TabbedText.Tabs++;
            startOfObjectPosition = writer.Position;
            if (includeChildrenMode != IncludeChildrenMode.ExcludeAllChildren && includeChildrenMode != IncludeChildrenMode.IncludeOnlyIncludableChildren) 
            {
                WriteChild(writer, _Offsets, includeChildrenMode, _Offsets_Accessed, () => GetChildSlice(LazinatorObjectBytes, _Offsets_ByteIndex, _Offsets_ByteLength, false, false, null), verifyCleanness, updateStoredBuffer, false, false, this);
            }
            if (updateStoredBuffer)
            {
                _Offsets_ByteIndex = startOfObjectPosition - startPosition;
            }
            TabbedText.Tabs--;
            TabbedText.WriteLine($"Byte {writer.Position}, SerializedMainList (accessed? {_SerializedMainList_Accessed}");
            TabbedText.Tabs++;
            startOfObjectPosition = writer.Position;
            WriteNonLazinatorObject(
            nonLazinatorObject: _SerializedMainList, isBelievedDirty: _SerializedMainList_Accessed,
            isAccessed: _SerializedMainList_Accessed, writer: writer,
            getChildSliceForFieldFn: () => GetChildSlice(LazinatorObjectBytes, _SerializedMainList_ByteIndex, _SerializedMainList_ByteLength, false, false, null),
            verifyCleanness: false,
            binaryWriterAction: (w, v) =>
            ConvertToBytes_ReadOnlyMemory_Gbyte_g(w, SerializedMainList,
            includeChildrenMode, v, updateStoredBuffer));
            if (updateStoredBuffer)
            {
                _SerializedMainList_ByteIndex = startOfObjectPosition - startPosition;
            }
            TabbedText.Tabs--;
            if (updateStoredBuffer)
            {
                _LazinatorList_T_EndByteIndex = writer.Position - startPosition;
            }
            TabbedText.WriteLine($"Byte {writer.Position} (end of LazinatorList<T>) ");
        }
        
        /* Conversion of supported collections and tuples */
        
        private static void ConvertToBytes_ReadOnlyMemory_Gbyte_g(BinaryBufferWriter writer, ReadOnlyMemory<byte> itemToConvert, IncludeChildrenMode includeChildrenMode, bool verifyCleanness, bool updateStoredBuffer)
        {
            ReadOnlySpan<byte> toConvert = MemoryMarshal.Cast<byte, byte>(itemToConvert.Span);
            for (int i = 0; i < toConvert.Length; i++)
            {
                writer.Write(toConvert[i]);
            }
        }
        
    }
}
