using System;
using System.Buffers;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using Lazinator.Exceptions;
using Lazinator.Support;
using Lazinator.Buffers; 
using Lazinator.Core;
using static Lazinator.Core.LazinatorUtilities;

namespace Lazinator.Collections
{
    public partial class LazinatorFastReadList<T> : ILazinator where T : struct
    {
        /* Boilerplate for every base class implementing ILazinator */
        
        public ILazinator LazinatorParentClass { get; set; }

        internal IncludeChildrenMode OriginalIncludeChildrenMode;

        public void Deserialize()
        {
            int bytesSoFar = 0;
            ReadOnlySpan<byte> span = LazinatorObjectBytes.Span;
            if (span.Length == 0)
            {
                return;
            }

            int uniqueID = span.ToDecompressedInt(ref bytesSoFar);
            if (uniqueID != LazinatorUniqueID)
            {
                throw new FormatException("Wrong self-serialized type initialized.");
            }

            int serializedVersionNumber = span.ToDecompressedInt(ref bytesSoFar);

            OriginalIncludeChildrenMode = (IncludeChildrenMode)span.ToByte(ref bytesSoFar);

            ConvertFromBytesAfterHeader(OriginalIncludeChildrenMode, serializedVersionNumber, ref bytesSoFar);
        }

        public MemoryInBuffer SerializeNewBuffer(IncludeChildrenMode includeChildrenMode, bool verifyCleanness)
        {
            return EncodeOrRecycleToNewBuffer(includeChildrenMode, OriginalIncludeChildrenMode, true, verifyCleanness, IsDirty, DescendantIsDirty, false, LazinatorObjectBytes, (StreamManuallyDelegate) EncodeToNewBuffer);
        }

        public ILazinator CloneLazinator()
        {
            return CloneLazinator(OriginalIncludeChildrenMode);
        }

        public ILazinator CloneLazinator(IncludeChildrenMode includeChildrenMode)
        {
            MemoryInBuffer bytes = SerializeNewBuffer(includeChildrenMode, false);
            var clone = new LazinatorFastReadList<T>()
            {
                DeserializationFactory = DeserializationFactory,
                LazinatorParentClass = LazinatorParentClass,
                InformParentOfDirtinessDelegate = InformParentOfDirtinessDelegate,
                OriginalIncludeChildrenMode = includeChildrenMode,
                HierarchyBytes = bytes
            };
            return clone;
        }

        internal MemoryInBuffer EncodeToNewBuffer(IncludeChildrenMode includeChildrenMode, bool verifyCleanness) => LazinatorUtilities.EncodeToNewBinaryBufferWriter(this, includeChildrenMode, verifyCleanness);
        
        private bool _IsDirty;
        public bool IsDirty
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
        
        public InformParentOfDirtinessDelegate InformParentOfDirtinessDelegate { get; set; }
        public void InformParentOfDirtiness()
        {
            if (InformParentOfDirtinessDelegate == null)
            {
                if (LazinatorParentClass != null)
                {
                    LazinatorParentClass.DescendantIsDirty = true;
                }
            }
            else
            InformParentOfDirtinessDelegate();
        }
        
        private bool _DescendantIsDirty;
        public bool DescendantIsDirty
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
        
        public DeserializationFactory DeserializationFactory { get; set; }

        private MemoryInBuffer _HierarchyBytes;
        public MemoryInBuffer HierarchyBytes
        {
            get => _HierarchyBytes;
            set
            {
                _HierarchyBytes = value;
                LazinatorObjectBytes = value.FilledMemory;
            }
        }

        private ReadOnlyMemory<byte> _LazinatorObjectBytes;
        public ReadOnlyMemory<byte> LazinatorObjectBytes
        {
            get => _LazinatorObjectBytes;
            set
            {
                _LazinatorObjectBytes = value;
                Deserialize();
            }
        }

        public virtual void LazinatorConvertToBytes()
        {
            if (!IsDirty)
            {
                return;
            }
            MemoryInBuffer bytes = EncodeOrRecycleToNewBuffer(IncludeChildrenMode.IncludeAllChildren, OriginalIncludeChildrenMode, false, false, IsDirty, DescendantIsDirty, false, LazinatorObjectBytes, (StreamManuallyDelegate)EncodeToNewBuffer);
            _IsDirty = false;
            LazinatorObjectBytes = bytes.FilledMemory;
            _ReadOnly_Accessed = false;
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

        /* Field boilerplate */

        internal int _ReadOnly_ByteIndex;
        internal int _ReadOnly_ByteLength => LazinatorObjectBytes.Length - _ReadOnly_ByteIndex;
        
        private ReadOnlyMemory<byte> _ReadOnly;
        public ReadOnlySpan<T> ReadOnly
        {
            [DebuggerStepThrough]
            get
            {
                if (!_ReadOnly_Accessed)
                {
                    ReadOnlyMemory<byte> childData = GetChildSlice(LazinatorObjectBytes, _ReadOnly_ByteIndex, _ReadOnly_ByteLength);
                    _ReadOnly = childData;
                    _ReadOnly_Accessed = true;
                }
                return MemoryMarshal.Cast<byte, T>(_ReadOnly.Span);
            }
            [DebuggerStepThrough]
            set
            {
                var span = value;
                IsDirty = true;
                _ReadOnly = new ReadOnlyMemory<byte>(MemoryMarshal.Cast<T, byte>(span).ToArray());
                _ReadOnly_Accessed = true;
            }
        }
        internal bool _ReadOnly_Accessed;
        
        /* Conversion */
        
        public virtual int LazinatorUniqueID => 52;
        
        public virtual int LazinatorObjectVersion { get; set; } = 0;
        
        public virtual void ConvertFromBytesAfterHeader(IncludeChildrenMode includeChildrenMode, int serializedVersionNumber, ref int bytesSoFar)
        {
            ReadOnlySpan<byte> span = LazinatorObjectBytes.Span;
            _ReadOnly_ByteIndex = bytesSoFar;
            if (includeChildrenMode != IncludeChildrenMode.ExcludeAllChildren)  bytesSoFar = span.ToInt32(ref bytesSoFar) + bytesSoFar;
        }
        
        public virtual void SerializeExistingBuffer(BinaryBufferWriter writer, IncludeChildrenMode includeChildrenMode, bool verifyCleanness)
        {
            PreSerialization();
            // header information
            CompressedIntegralTypes.WriteCompressedInt(writer, LazinatorUniqueID);
            CompressedIntegralTypes.WriteCompressedInt(writer, LazinatorObjectVersion);
            writer.Write((byte)includeChildrenMode);
            // write properties
            WriteNonLazinatorObject(
            nonLazinatorObject: _ReadOnly, isBelievedDirty: _ReadOnly_Accessed,
            isAccessed: _ReadOnly_Accessed, writer: writer,
            getChildSliceForFieldFn: () => GetChildSlice(LazinatorObjectBytes, _ReadOnly_ByteIndex, _ReadOnly_ByteLength),
            verifyCleanness: false,
            binaryWriterAction: (w, v) =>
            ConvertToBytes_ReadOnlySpan_T(w, ReadOnly,
            includeChildrenMode, v));
        }
        
        /* Conversion of supported collections and tuples */
        
        private static void ConvertToBytes_ReadOnlySpan_T(BinaryBufferWriter writer, ReadOnlySpan<T> itemToConvert, IncludeChildrenMode includeChildrenMode, bool verifyCleanness)
        {
            ReadOnlySpan<byte> toConvert = MemoryMarshal.Cast<T, byte>(itemToConvert);
            for (int i = 0; i < toConvert.Length; i++)
            writer.Write(toConvert[i]);
        }
        
    }
}
