//27ab395d-0a47-3f53-d253-20f9b037aeb5
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by the Lazinator tool, version 0.1.0.275
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Lazinator.Wrappers
{
    using Lazinator.Attributes;
    using Lazinator.Buffers;
    using Lazinator.Core;
    using Lazinator.Exceptions;
    using Lazinator.Support;
    using System;
    using System.Buffers;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Runtime.InteropServices;
    using static Lazinator.Core.LazinatorUtilities;
    
    [Autogenerated]
    public partial struct WReadOnlySpanChar : ILazinator
    {
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public bool IsStruct => true;
        
        /* Serialization, deserialization, and object relationships */
        
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public LazinatorParentsCollection LazinatorParents { get; set; }
        
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public IncludeChildrenMode OriginalIncludeChildrenMode { get; set; }
        
        public int Deserialize()
        {
            FreeInMemoryObjects();
            int bytesSoFar = 0;
            ReadOnlySpan<byte> span = LazinatorObjectBytes.Span;
            if (span.Length == 0)
            {
                return 0;
            }
            
            int lazinatorLibraryVersion = span.ToDecompressedInt(ref bytesSoFar);
            
            int serializedVersionNumber = -1; /* versioning disabled */
            
            OriginalIncludeChildrenMode = (IncludeChildrenMode)span.ToByte(ref bytesSoFar);
            
            ConvertFromBytesAfterHeader(OriginalIncludeChildrenMode, serializedVersionNumber, ref bytesSoFar);
            return bytesSoFar;
        }
        
        public LazinatorMemory SerializeLazinator(IncludeChildrenMode includeChildrenMode, bool verifyCleanness, bool updateStoredBuffer)
        {
            return EncodeOrRecycleToNewBuffer(includeChildrenMode, OriginalIncludeChildrenMode, verifyCleanness, IsDirty, DescendantIsDirty, false, LazinatorMemoryStorage, (EncodeManuallyDelegate) EncodeToNewBuffer, updateStoredBuffer);
        }
        
        LazinatorMemory EncodeToNewBuffer(IncludeChildrenMode includeChildrenMode, bool verifyCleanness, bool updateStoredBuffer) => LazinatorUtilities.EncodeToNewBinaryBufferWriter(this, includeChildrenMode, verifyCleanness, updateStoredBuffer);
        
        public ILazinator CloneLazinator(IncludeChildrenMode includeChildrenMode = IncludeChildrenMode.IncludeAllChildren, CloneBufferOptions cloneBufferOptions = CloneBufferOptions.LinkedBuffer)
        {
            var clone = new WReadOnlySpanChar()
            {
                OriginalIncludeChildrenMode = includeChildrenMode
            };
            if (clone.LazinatorObjectVersion != LazinatorObjectVersion)
            {
                clone.LazinatorObjectVersion = LazinatorObjectVersion;
            }
            
            if (cloneBufferOptions == CloneBufferOptions.NoBuffer)
            {
                AssignCloneProperties(ref clone, includeChildrenMode);
            }
            else
            {
                LazinatorMemory bytes = EncodeOrRecycleToNewBuffer(includeChildrenMode, OriginalIncludeChildrenMode, false, IsDirty, DescendantIsDirty, false, LazinatorMemoryStorage, (EncodeManuallyDelegate)EncodeToNewBuffer, cloneBufferOptions == CloneBufferOptions.SharedBuffer);
                clone.DeserializeLazinator(bytes);
                if (cloneBufferOptions == CloneBufferOptions.IndependentBuffers)
                {
                    clone.LazinatorMemoryStorage.DisposeIndependently();
                }
            }
            clone.LazinatorParents = default;
            return clone;
        }
        
        void AssignCloneProperties(ref WReadOnlySpanChar clone, IncludeChildrenMode includeChildrenMode)
        {
            clone.FreeInMemoryObjects();
            clone.Value = Clone_ReadOnlySpan_Gchar_g(Value, includeChildrenMode);
            
            clone.IsDirty = false;}
            
            [DebuggerBrowsable(DebuggerBrowsableState.Never)]
            public bool HasChanged { get; set; }
            
            [DebuggerBrowsable(DebuggerBrowsableState.Never)]
            bool _IsDirty;
            [DebuggerBrowsable(DebuggerBrowsableState.Never)]
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
                            LazinatorParents.InformParentsOfDirtiness();
                            HasChanged = true;
                        }
                    }
                }
            }
            
            [DebuggerBrowsable(DebuggerBrowsableState.Never)]
            bool _DescendantHasChanged;
            [DebuggerBrowsable(DebuggerBrowsableState.Never)]
            public bool DescendantHasChanged
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
            bool _DescendantIsDirty;
            [DebuggerBrowsable(DebuggerBrowsableState.Never)]
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
                        if (_DescendantIsDirty)
                        {
                            LazinatorParents.InformParentsOfDirtiness();
                            _DescendantHasChanged = true;
                        }
                    }
                }
            }
            
            public void DeserializeLazinator(LazinatorMemory serializedBytes)
            {
                LazinatorMemoryStorage = serializedBytes;
                int length = Deserialize();
                if (length != LazinatorMemoryStorage.Length)
                {
                    LazinatorMemoryStorage = LazinatorMemoryStorage.Slice(0, length);
                }
            }
            
            [DebuggerBrowsable(DebuggerBrowsableState.Never)]
            public LazinatorMemory LazinatorMemoryStorage
            {
                get;
                set;
            }
            [DebuggerBrowsable(DebuggerBrowsableState.Never)]
            ReadOnlyMemory<byte> LazinatorObjectBytes => LazinatorMemoryStorage?.Memory ?? LazinatorUtilities.EmptyReadOnlyMemory;
            
            public void EnsureLazinatorMemoryUpToDate()
            {
                if (LazinatorMemoryStorage == null)
                {
                    throw new NotSupportedException("Cannot use EnsureLazinatorMemoryUpToDate on a struct that has not been deserialized. Clone the struct instead."); 
                }
                if (!IsDirty && !DescendantIsDirty && LazinatorObjectBytes.Length > 0 && OriginalIncludeChildrenMode == IncludeChildrenMode.IncludeAllChildren)
                {
                    return;
                }
                LazinatorMemoryStorage = EncodeOrRecycleToNewBuffer(IncludeChildrenMode.IncludeAllChildren, OriginalIncludeChildrenMode, false, IsDirty, DescendantIsDirty, false, LazinatorMemoryStorage, (EncodeManuallyDelegate)EncodeToNewBuffer, true);
                OriginalIncludeChildrenMode = IncludeChildrenMode.IncludeAllChildren;
            }
            
            public int GetByteLength()
            {
                EnsureLazinatorMemoryUpToDate();
                return LazinatorObjectBytes.Length;
            }
            
            public uint GetBinaryHashCode32()
            {
                if (LazinatorMemoryStorage == null)
                {
                    var result = SerializeLazinator(IncludeChildrenMode.IncludeAllChildren, false, false);
                    return FarmhashByteSpans.Hash32(result.Span);
                }
                else
                {
                    EnsureLazinatorMemoryUpToDate();
                    return FarmhashByteSpans.Hash32(LazinatorObjectBytes.Span);
                }
            }
            
            public ulong GetBinaryHashCode64()
            {
                if (LazinatorMemoryStorage == null)
                {
                    var result = SerializeLazinator(IncludeChildrenMode.IncludeAllChildren, false, false);
                    return FarmhashByteSpans.Hash64(result.Span);
                }
                else
                {
                    EnsureLazinatorMemoryUpToDate();
                    return FarmhashByteSpans.Hash64(LazinatorObjectBytes.Span);
                }
            }
            
            public Guid GetBinaryHashCode128()
            {
                if (LazinatorMemoryStorage == null)
                {
                    var result = SerializeLazinator(IncludeChildrenMode.IncludeAllChildren, false, false);
                    return FarmhashByteSpans.Hash128(result.Span);
                }
                else
                {
                    EnsureLazinatorMemoryUpToDate();
                    return FarmhashByteSpans.Hash128(LazinatorObjectBytes.Span);
                }
            }
            
            /* Property definitions */
            
            int _Value_ByteIndex;
            private int _WReadOnlySpanChar_EndByteIndex;
            int _Value_ByteLength => _WReadOnlySpanChar_EndByteIndex - _Value_ByteIndex;
            
            [DebuggerBrowsable(DebuggerBrowsableState.Never)]
            private ReadOnlyMemory<byte> _Value;
            public ReadOnlySpan<char> Value
            {
                [DebuggerStepThrough]
                get
                {
                    if (!_Value_Accessed)
                    {
                        LazinatorMemory childData = GetChildSlice(LazinatorMemoryStorage, _Value_ByteIndex, _Value_ByteLength, true, false, null);
                        _Value = childData.ReadOnlyMemory;
                        _Value_Accessed = true;
                    }
                    return MemoryMarshal.Cast<byte, char>(_Value.Span);
                }
                [DebuggerStepThrough]
                set
                {
                    IsDirty = true;
                    _Value = new ReadOnlyMemory<byte>(MemoryMarshal.Cast<char, byte>(value).ToArray());
                    _Value_Accessed = true;
                }
            }
            bool _Value_Accessed;
            
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
            
            public IEnumerable<(string propertyName, ILazinator descendant)> EnumerateLazinatorDescendants(Func<ILazinator, bool> matchCriterion, bool stopExploringBelowMatch, Func<ILazinator, bool> exploreCriterion, bool exploreOnlyDeserializedChildren, bool enumerateNulls)
            {
                yield break;
            }
            
            
            public IEnumerable<(string propertyName, object descendant)> EnumerateNonLazinatorProperties()
            {
                yield return ("Value", (object)Value.ToString());
                yield break;
            }
            
            public void FreeInMemoryObjects()
            {
                _Value = default;
                _Value_Accessed = false;
                IsDirty = false;
                DescendantIsDirty = false;
                HasChanged = false;
                DescendantHasChanged = false;
            }
            
            /* Conversion */
            
            [DebuggerBrowsable(DebuggerBrowsableState.Never)]
            public int LazinatorUniqueID => 74;
            
            [DebuggerBrowsable(DebuggerBrowsableState.Never)]
            bool ContainsOpenGenericParameters => false;
            [DebuggerBrowsable(DebuggerBrowsableState.Never)]
            public LazinatorGenericIDType LazinatorGenericID
            {
                get => default;
                set { }
            }
            
            [DebuggerBrowsable(DebuggerBrowsableState.Never)]
            public int LazinatorObjectVersion
            {
                get => -1;
                set => throw new LazinatorSerializationException("Lazinator versioning disabled for WReadOnlySpanChar.");
            }
            
            
            public void ConvertFromBytesAfterHeader(IncludeChildrenMode includeChildrenMode, int serializedVersionNumber, ref int bytesSoFar)
            {
                ReadOnlySpan<byte> span = LazinatorObjectBytes.Span;
                _Value_ByteIndex = bytesSoFar;
                bytesSoFar = span.Length;
                _WReadOnlySpanChar_EndByteIndex = bytesSoFar;
            }
            
            public void SerializeExistingBuffer(ref BinaryBufferWriter writer, IncludeChildrenMode includeChildrenMode, bool verifyCleanness, bool updateStoredBuffer)
            {
                if (includeChildrenMode != IncludeChildrenMode.IncludeAllChildren)
                {
                    updateStoredBuffer = false;
                }
                int startPosition = writer.Position;
                WritePropertiesIntoBuffer(ref writer, includeChildrenMode, verifyCleanness, updateStoredBuffer, false);
                if (updateStoredBuffer)
                {
                    UpdateStoredBuffer(ref writer, startPosition, includeChildrenMode, false);
                }
            }
            
            public void UpdateStoredBuffer(ref BinaryBufferWriter writer, int startPosition, IncludeChildrenMode includeChildrenMode, bool updateDeserializedChildren)
            {
                _IsDirty = false;
                if (includeChildrenMode == IncludeChildrenMode.IncludeAllChildren)
                {
                    _DescendantIsDirty = false;
                    if (updateDeserializedChildren)
                    {
                    }
                    
                }
                else
                {
                    throw new Exception("Cannot update stored buffer when serializing only some children.");
                }
                
                var newBuffer = writer.Slice(startPosition);
                LazinatorMemoryStorage = ReplaceBuffer(LazinatorMemoryStorage, newBuffer, LazinatorParents, startPosition == 0, IsStruct);
            }
            
            void WritePropertiesIntoBuffer(ref BinaryBufferWriter writer, IncludeChildrenMode includeChildrenMode, bool verifyCleanness, bool updateStoredBuffer, bool includeUniqueID)
            {
                int startPosition = writer.Position;
                int startOfObjectPosition = 0;
                // header information
                if (includeUniqueID)
                {
                    CompressedIntegralTypes.WriteCompressedInt(ref writer, LazinatorUniqueID);
                }
                
                CompressedIntegralTypes.WriteCompressedInt(ref writer, Lazinator.Support.LazinatorVersionInfo.LazinatorIntVersion);
                writer.Write((byte)includeChildrenMode);
                // write properties
                startOfObjectPosition = writer.Position;
                if (includeChildrenMode != IncludeChildrenMode.IncludeAllChildren || includeChildrenMode != OriginalIncludeChildrenMode && !_Value_Accessed)
                {
                    var deserialized = Value;
                }
                var serializedBytesCopy_Value = LazinatorMemoryStorage;
                var byteIndexCopy_Value = _Value_ByteIndex;
                var byteLengthCopy_Value = _Value_ByteLength;
                var copy_Value = _Value;
                WriteNonLazinatorObject_WithoutLengthPrefix(
                nonLazinatorObject: _Value, isBelievedDirty: _Value_Accessed || (includeChildrenMode != OriginalIncludeChildrenMode),
                isAccessed: _Value_Accessed, writer: ref writer,
                getChildSliceForFieldFn: () => GetChildSlice(serializedBytesCopy_Value, byteIndexCopy_Value, byteLengthCopy_Value, true, false, null),
                verifyCleanness: false,
                binaryWriterAction: (ref BinaryBufferWriter w, bool v) =>
                copy_Value.Write(ref w));
                if (updateStoredBuffer)
                {
                    _Value_ByteIndex = startOfObjectPosition - startPosition;
                }
                if (updateStoredBuffer)
                {
                    _WReadOnlySpanChar_EndByteIndex = writer.Position - startPosition;
                }
            }
            
            /* Conversion of supported collections and tuples */
            private static ReadOnlySpan<char> Clone_ReadOnlySpan_Gchar_g(ReadOnlySpan<char> itemToClone, IncludeChildrenMode includeChildrenMode)
            {
                var clone = new Span<byte>(new byte[itemToClone.Length * sizeof(char)]);
                MemoryMarshal.Cast<char, byte>(itemToClone).CopyTo(clone);
                return MemoryMarshal.Cast<byte, char>(clone);
            }
            
        }
    }
