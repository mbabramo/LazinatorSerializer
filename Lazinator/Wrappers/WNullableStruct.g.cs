//487a4b41-13c3-a9d7-2fee-8cb0180b633d
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by the Lazinator tool, version 0.1.0.231
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
    public partial struct WNullableStruct<T> : ILazinator
    {
        public bool IsStruct => true;
        
        /* Serialization, deserialization, and object relationships */
        
        public LazinatorParentsCollection LazinatorParents { get; set; }
        
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
            
            LazinatorGenericID = GetGenericIDIfApplicable(ContainsOpenGenericParameters, LazinatorUniqueID, span, ref bytesSoFar);
            
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
            var clone = new WNullableStruct<T>()
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
        
        void AssignCloneProperties(ref WNullableStruct<T> clone, IncludeChildrenMode includeChildrenMode)
        {
            clone.FreeInMemoryObjects();
            clone.HasValue = HasValue;
            if (includeChildrenMode != IncludeChildrenMode.ExcludeAllChildren && includeChildrenMode != IncludeChildrenMode.IncludeOnlyIncludableChildren) 
            {
                clone.NonNullValue = (System.Collections.Generic.EqualityComparer<T>.Default.Equals(NonNullValue, default(T))) ? default(T) : (T) NonNullValue.CloneLazinator(includeChildrenMode, CloneBufferOptions.NoBuffer);
            }
        }
        
        public bool HasChanged { get; set; }
        
        bool _IsDirty;
        public bool IsDirty
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
        
        bool _DescendantHasChanged;
        public bool DescendantHasChanged
        {
            [DebuggerStepThrough]
            get => _DescendantHasChanged || (_NonNullValue_Accessed && !System.Collections.Generic.EqualityComparer<T>.Default.Equals(_NonNullValue, default(T)) && (NonNullValue.HasChanged || NonNullValue.DescendantHasChanged));
            [DebuggerStepThrough]
            set
            {
                _DescendantHasChanged = value;
            }
        }
        
        bool _DescendantIsDirty;
        public bool DescendantIsDirty
        {
            [DebuggerStepThrough]
            get => _DescendantIsDirty || (_NonNullValue_Accessed && !System.Collections.Generic.EqualityComparer<T>.Default.Equals(_NonNullValue, default(T)) && (NonNullValue.IsDirty || NonNullValue.DescendantIsDirty));
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
        }
        
        LazinatorMemory _LazinatorMemoryStorage;
        public LazinatorMemory LazinatorMemoryStorage
        {
            get => _LazinatorMemoryStorage;
            set
            {
                _LazinatorMemoryStorage = value;
                int length = Deserialize();
                if (length != _LazinatorMemoryStorage.Length)
                {
                    _LazinatorMemoryStorage = _LazinatorMemoryStorage.Slice(0, length);
                }
            }
        }
        ReadOnlyMemory<byte> LazinatorObjectBytes => LazinatorMemoryStorage?.Memory ?? LazinatorUtilities.EmptyReadOnlyMemory;
        
        public void EnsureLazinatorMemoryUpToDate()
        {
            if (_LazinatorMemoryStorage == null)
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
            if (_LazinatorMemoryStorage == null)
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
            if (_LazinatorMemoryStorage == null)
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
            if (_LazinatorMemoryStorage == null)
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
        
        int _NonNullValue_ByteIndex;
        private int _WNullableStruct_T_EndByteIndex;
        int _NonNullValue_ByteLength => _WNullableStruct_T_EndByteIndex - _NonNullValue_ByteIndex;
        
        bool _HasValue;
        public bool HasValue
        {
            [DebuggerStepThrough]
            get
            {
                return _HasValue;
            }
            [DebuggerStepThrough]
            set
            {
                IsDirty = true;
                _HasValue = value;
            }
        }
        T _NonNullValue;
        public T NonNullValue
        {
            [DebuggerStepThrough]
            get
            {
                if (!_NonNullValue_Accessed)
                {
                    if (LazinatorObjectBytes.Length == 0)
                    {
                        _NonNullValue = default(T);
                    }
                    else
                    {
                        LazinatorMemory childData = GetChildSlice(LazinatorMemoryStorage, _NonNullValue_ByteIndex, _NonNullValue_ByteLength, true, false, null);
                        
                        _NonNullValue = DeserializationFactory.Instance.CreateBasedOnType<T>(childData); 
                    }
                    _NonNullValue_Accessed = true;
                } 
                return _NonNullValue;
            }
            [DebuggerStepThrough]
            set
            {
                
                IsDirty = true;
                DescendantIsDirty = true;
                _NonNullValue = value;
                _NonNullValue_Accessed = true;
            }
        }
        bool _NonNullValue_Accessed;
        
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
            if (enumerateNulls && (!exploreOnlyDeserializedChildren || _NonNullValue_Accessed) && (System.Collections.Generic.EqualityComparer<T>.Default.Equals(NonNullValue, default(T))))
            {
                yield return ("NonNullValue", default);
            }
            else if ((!exploreOnlyDeserializedChildren && !System.Collections.Generic.EqualityComparer<T>.Default.Equals(NonNullValue, default(T))) || (_NonNullValue_Accessed && !System.Collections.Generic.EqualityComparer<T>.Default.Equals(_NonNullValue, default(T))))
            {
                bool isMatch = matchCriterion == null || matchCriterion(NonNullValue);
                bool shouldExplore = exploreCriterion == null || exploreCriterion(NonNullValue);
                if (isMatch)
                {
                    yield return ("NonNullValue", NonNullValue);
                }
                if ((!stopExploringBelowMatch || !isMatch) && shouldExplore)
                {
                    foreach (var toYield in NonNullValue.EnumerateLazinatorDescendants(matchCriterion, stopExploringBelowMatch, exploreCriterion, exploreOnlyDeserializedChildren, enumerateNulls))
                    {
                        yield return ("NonNullValue" + "." + toYield.propertyName, toYield.descendant);
                    }
                }
            }
            yield break;
        }
        
        
        public IEnumerable<(string propertyName, object descendant)> EnumerateNonLazinatorProperties()
        {
            yield return ("HasValue", (object)HasValue);
            yield break;
        }
        
        public void FreeInMemoryObjects()
        {
            
            IsDirty = false;
            DescendantIsDirty = false;
            HasChanged = false;
            DescendantHasChanged = false;
        }
        
        /* Conversion */
        
        public int LazinatorUniqueID => 88;
        
        bool ContainsOpenGenericParameters => true;
        LazinatorGenericIDType _LazinatorGenericID { get; set; }
        public LazinatorGenericIDType LazinatorGenericID
        {
            get
            {
                if (_LazinatorGenericID.IsEmpty)
                {
                    _LazinatorGenericID = DeserializationFactory.Instance.GetUniqueIDListForGenericType(88, new Type[] { typeof(T) });
                }
                return _LazinatorGenericID;
            }
            set
            {
                _LazinatorGenericID = value;
            }
        }
        
        public int LazinatorObjectVersion
        {
            get => -1;
            set => throw new LazinatorSerializationException("Lazinator versioning disabled for WNullableStruct<T>.");
        }
        
        
        public void ConvertFromBytesAfterHeader(IncludeChildrenMode includeChildrenMode, int serializedVersionNumber, ref int bytesSoFar)
        {
            ReadOnlySpan<byte> span = LazinatorObjectBytes.Span;
            _HasValue = span.ToBoolean(ref bytesSoFar);
            _NonNullValue_ByteIndex = bytesSoFar;
            bytesSoFar = span.Length;
            _WNullableStruct_T_EndByteIndex = bytesSoFar;
        }
        
        public void SerializeExistingBuffer(ref BinaryBufferWriter writer, IncludeChildrenMode includeChildrenMode, bool verifyCleanness, bool updateStoredBuffer)
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
                    if (_NonNullValue_Accessed && _NonNullValue.IsStruct && (_NonNullValue.IsDirty || _NonNullValue.DescendantIsDirty))
                    {
                        _NonNullValue_Accessed = false;
                    }
                }
                else
                {
                    throw new Exception("Cannot update stored buffer when serializing only some children.");
                }
                
                var newBuffer = writer.Slice(startPosition);
                if (_LazinatorMemoryStorage != null)
                {
                    _LazinatorMemoryStorage.DisposeWithThis(newBuffer);
                }
                _LazinatorMemoryStorage = newBuffer;
            }
        }
        void WritePropertiesIntoBuffer(ref BinaryBufferWriter writer, IncludeChildrenMode includeChildrenMode, bool verifyCleanness, bool updateStoredBuffer, bool includeUniqueID)
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
            writer.Write((byte)includeChildrenMode);
            // write properties
            WriteUncompressedPrimitives.WriteBool(ref writer, _HasValue);
            startOfObjectPosition = writer.Position;
            if (includeChildrenMode != IncludeChildrenMode.ExcludeAllChildren && includeChildrenMode != IncludeChildrenMode.IncludeOnlyIncludableChildren)  
            {
                if (includeChildrenMode != IncludeChildrenMode.IncludeAllChildren || includeChildrenMode != OriginalIncludeChildrenMode && !_NonNullValue_Accessed)
                {
                    var deserialized = NonNullValue;
                }
                var serializedBytesCopy = LazinatorMemoryStorage;
                var byteIndexCopy = _NonNullValue_ByteIndex;
                var byteLengthCopy = _NonNullValue_ByteLength;
                WriteChild(ref writer, _NonNullValue, includeChildrenMode, _NonNullValue_Accessed, () => GetChildSlice(serializedBytesCopy, byteIndexCopy, byteLengthCopy, true, false, null), verifyCleanness, updateStoredBuffer, false, true, null);
            }
            if (updateStoredBuffer)
            {
                _NonNullValue_ByteIndex = startOfObjectPosition - startPosition;
            }
            if (updateStoredBuffer)
            {
                _WNullableStruct_T_EndByteIndex = writer.Position - startPosition;
            }
        }
        
    }
}
