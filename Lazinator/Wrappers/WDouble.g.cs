//540010dc-77e6-d3c6-3310-a84339798cd4
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by the Lazinator tool, version 0.1.0.262
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
    public partial struct WDouble : ILazinator
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
            
            int lazinatorLibraryVersion = span.ToDecompressedInt(ref bytesSoFar);
            
            int serializedVersionNumber = -1; /* versioning disabled */
            
            OriginalIncludeChildrenMode = IncludeChildrenMode.IncludeAllChildren; /* cannot have children */
            
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
            var clone = new WDouble()
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
        
        void AssignCloneProperties(ref WDouble clone, IncludeChildrenMode includeChildrenMode)
        {
            clone.FreeInMemoryObjects();
            clone.WrappedValue = WrappedValue;
            
            clone.IsDirty = false;}
            
            public bool HasChanged { get; set; }
            
            bool _IsDirty;
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
            
            bool _DescendantHasChanged;
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
            
            bool _DescendantIsDirty;
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
            
            public LazinatorMemory LazinatorMemoryStorage
            {
                get;
                set;
            }
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
                return (uint) GetHashCode();
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
            
            
            double _WrappedValue;
            public double WrappedValue
            {
                [DebuggerStepThrough]
                get
                {
                    return _WrappedValue;
                }
                [DebuggerStepThrough]
                private set
                {
                    IsDirty = true;
                    _WrappedValue = value;
                }
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
            
            public IEnumerable<(string propertyName, ILazinator descendant)> EnumerateLazinatorDescendants(Func<ILazinator, bool> matchCriterion, bool stopExploringBelowMatch, Func<ILazinator, bool> exploreCriterion, bool exploreOnlyDeserializedChildren, bool enumerateNulls)
            {
                yield break;
            }
            
            
            public IEnumerable<(string propertyName, object descendant)> EnumerateNonLazinatorProperties()
            {
                yield return ("WrappedValue", (object)WrappedValue);
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
            
            public int LazinatorUniqueID => 54;
            
            bool ContainsOpenGenericParameters => false;
            public LazinatorGenericIDType LazinatorGenericID
            {
                get => default;
                set { }
            }
            
            public int LazinatorObjectVersion
            {
                get => -1;
                set => throw new LazinatorSerializationException("Lazinator versioning disabled for WDouble.");
            }
            
            
            public void ConvertFromBytesAfterHeader(IncludeChildrenMode includeChildrenMode, int serializedVersionNumber, ref int bytesSoFar)
            {
                ReadOnlySpan<byte> span = LazinatorObjectBytes.Span;
                _WrappedValue = span.ToDouble(ref bytesSoFar);
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
                // header information
                if (includeUniqueID)
                {
                    CompressedIntegralTypes.WriteCompressedInt(ref writer, LazinatorUniqueID);
                }
                
                CompressedIntegralTypes.WriteCompressedInt(ref writer, Lazinator.Support.LazinatorVersionInfo.LazinatorIntVersion);
                
                // write properties
                WriteUncompressedPrimitives.WriteDouble(ref writer, _WrappedValue);
            }
            
        }
    }
