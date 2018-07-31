//ce41425f-c519-8796-00dd-26fad720ed33
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by the Lazinator tool, version 0.1.0.207
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace LazinatorTests.Examples.Subclasses
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
    
    public partial class ClassWithSubclass
    {
        [Autogenerated]
        public partial class SubclassWithinClass : ILazinator
        {
            public bool IsStruct => false;
            
            /* Serialization, deserialization, and object relationships */
            
            public SubclassWithinClass() : base()
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
            
            public virtual LazinatorMemory SerializeNewBuffer(IncludeChildrenMode includeChildrenMode, bool verifyCleanness, bool updateStoredBuffer)
            {
                return EncodeOrRecycleToNewBuffer(includeChildrenMode, OriginalIncludeChildrenMode, verifyCleanness, IsDirty, DescendantIsDirty, false, LazinatorObjectBytes, (EncodeManuallyDelegate) EncodeToNewBuffer, updateStoredBuffer);
            }
            
            protected virtual LazinatorMemory EncodeToNewBuffer(IncludeChildrenMode includeChildrenMode, bool verifyCleanness, bool updateStoredBuffer) => LazinatorUtilities.EncodeToNewBinaryBufferWriter(this, includeChildrenMode, verifyCleanness, updateStoredBuffer);
            
            public virtual ILazinator CloneLazinator()
            {
                return CloneLazinator(OriginalIncludeChildrenMode);
            }
            
            public virtual ILazinator CloneLazinator(IncludeChildrenMode includeChildrenMode, bool updateStoredBuffer = false)
            {
                LazinatorMemory bytes = EncodeOrRecycleToNewBuffer(includeChildrenMode, OriginalIncludeChildrenMode, false, IsDirty, DescendantIsDirty, false, LazinatorObjectBytes, (EncodeManuallyDelegate)EncodeToNewBuffer, updateStoredBuffer);
                var clone = new SubclassWithinClass()
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
            
            private LazinatorMemory _HierarchyBytes;
            public virtual LazinatorMemory HierarchyBytes
            {
                set
                {
                    _HierarchyBytes = value;
                    LazinatorMemoryStorage = value;
                }
            }
            
            protected LazinatorMemory _LazinatorMemoryStorage; // DEBUG -- use only one memory storage
            public virtual LazinatorMemory LazinatorMemoryStorage
            {
                get => _LazinatorMemoryStorage;
                set
                {
                    _LazinatorMemoryStorage = value;
                    int length = Deserialize();
                }
            }
            
            public virtual ReadOnlyMemory<byte> LazinatorObjectBytes
            {
                get => LazinatorMemoryStorage.Memory;
            }
            
            public virtual void EnsureLazinatorMemoryUpToDate()
            {
                if (!IsDirty && !DescendantIsDirty && LazinatorObjectBytes.Length > 0)
                {
                    return;
                }
                EncodeOrRecycleToNewBuffer(IncludeChildrenMode.IncludeAllChildren, OriginalIncludeChildrenMode, false, IsDirty, DescendantIsDirty, false, LazinatorObjectBytes, (EncodeManuallyDelegate)EncodeToNewBuffer, true);
            }
            
            public virtual int GetByteLength()
            {
                EnsureLazinatorMemoryUpToDate();
                return LazinatorObjectBytes.Length;
            }
            
            public virtual uint GetBinaryHashCode32()
            {
                EnsureLazinatorMemoryUpToDate();
                return FarmhashByteSpans.Hash32(LazinatorObjectBytes.Span);
            }
            
            public virtual ulong GetBinaryHashCode64()
            {
                EnsureLazinatorMemoryUpToDate();
                return FarmhashByteSpans.Hash64(LazinatorObjectBytes.Span);
            }
            
            public virtual Guid GetBinaryHashCode128()
            {
                EnsureLazinatorMemoryUpToDate();
                return FarmhashByteSpans.Hash128(LazinatorObjectBytes.Span);
            }
            
            /* Property definitions */
            
            
            protected string _StringWithinSubclass;
            public string StringWithinSubclass
            {
                get
                {
                    return _StringWithinSubclass;
                }
                set
                {
                    IsDirty = true;
                    _StringWithinSubclass = value;
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
            
            public virtual IEnumerable<(string propertyName, ILazinator descendant)> EnumerateLazinatorDescendants(Func<ILazinator, bool> matchCriterion, bool stopExploringBelowMatch, Func<ILazinator, bool> exploreCriterion, bool exploreOnlyDeserializedChildren, bool enumerateNulls)
            {
                yield break;
            }
            
            
            public virtual IEnumerable<(string propertyName, object descendant)> EnumerateNonLazinatorProperties()
            {
                yield return ("StringWithinSubclass", (object)StringWithinSubclass);
                yield break;
            }
            
            protected virtual void ResetAccessedProperties()
            {
                
                IsDirty = false;
                DescendantIsDirty = false;
                HasChanged = false;
                DescendantHasChanged = false;
            }
            
            /* Conversion */
            
            public virtual int LazinatorUniqueID => 258;
            
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
                _StringWithinSubclass = span.ToString_VarIntLengthUtf8(ref bytesSoFar);
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
                    
                    _IsDirty = false;
                    if (includeChildrenMode == IncludeChildrenMode.IncludeAllChildren)
                    {
                        _DescendantIsDirty = false;
                    }
                    else
                    {
                        throw new Exception("Cannot update stored buffer when serializing only some children.");
                    }
                    
                    LazinatorMemoryStorage = writer.LazinatorMemorySlice(startPosition);
                }
            }
            protected virtual void WritePropertiesIntoBuffer(ref BinaryBufferWriter writer, IncludeChildrenMode includeChildrenMode, bool verifyCleanness, bool updateStoredBuffer, bool includeUniqueID)
            {
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
                EncodeCharAndString.WriteStringUtf8WithVarIntPrefix(ref writer, _StringWithinSubclass);
            }
            
        }
        
    }
}
