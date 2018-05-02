using System;
using System.Buffers;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using Lazinator.Collections;
using Lazinator.Support;
using Lazinator.Buffers; 
using Lazinator.Core; 
using static Lazinator.Core.LazinatorUtilities;

namespace Lazinator.Collections
{
    public partial class LazinatorList<T> : ILazinator 
    {
        /* Boilerplate for every base class implementing ILazinator */

        public ILazinator LazinatorParentClass { get; set; }

        public ReadOnlyMemory<byte> GetChildSlice(int byteOffset, int byteLength)
        {
            if (byteLength <= sizeof(int))
                return new ReadOnlyMemory<byte>();
            return LazinatorObjectBytes.Slice(byteOffset + sizeof(int), byteLength - sizeof(int));
        }

        protected internal IncludeChildrenMode OriginalIncludeChildrenMode;

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

        public MemoryInBuffer SerializeNewBuffer(IncludeChildrenMode includeChildrenMode, bool verifyCleanness) => LazinatorUtilities.EncodeOrRecycleToNewBuffer(includeChildrenMode, OriginalIncludeChildrenMode, true, verifyCleanness, IsDirty, DescendantIsDirty, false, LazinatorObjectBytes, (IncludeChildrenMode a, bool b) => EncodeToNewBuffer(a, b));
        
        public virtual ILazinator CloneLazinator()
        {
            return CloneLazinator(OriginalIncludeChildrenMode);
        }

        public virtual ILazinator CloneLazinator(IncludeChildrenMode includeChildrenMode)
        {
            MemoryInBuffer bytes = SerializeNewBuffer(includeChildrenMode, false);
            var clone = new LazinatorList<T>()
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
                        LazinatorParentClass.DescendantIsDirty = true;
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

        /* Field boilerplate */
        internal int _Offsets_ByteIndex;
        internal int _Offsets_ByteLength => LazinatorObjectBytes.Length - _Offsets_ByteIndex;

        private LazinatorOffsetList _Offsets;
        public LazinatorOffsetList Offsets
        {
            [DebuggerStepThrough]
            get
            {
                if (!_Offsets_Accessed)
                {
                    ReadOnlyMemory<byte> childData = GetChildSlice(_Offsets_ByteIndex, _Offsets_ByteLength);
                    _Offsets = new LazinatorOffsetList()
                    {
                        DeserializationFactory = DeserializationFactory,
                        LazinatorObjectBytes = childData,
                        LazinatorParentClass = this
                    };
                    _Offsets_Accessed = true;
                }
                return _Offsets;
            }
            [DebuggerStepThrough]
            set
            {
                IsDirty = true;
                _Offsets = value;
                _Offsets_Accessed = true;
            }
        }
        internal bool _Offsets_Accessed;

        /* Conversion */

        public virtual int LazinatorUniqueID => (int)LazinatorCollectionUniqueIDs.LazinatorList;

        public virtual int LazinatorObjectVersion { get; set; } = 0;

        public void LazinatorObjectVersionUpgrade(int oldFormatVersion)
        {
        }
    }
}
