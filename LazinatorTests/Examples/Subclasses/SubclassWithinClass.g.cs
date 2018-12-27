//fe6557f2-e39e-d933-02f4-e058f7d03bf6
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by the Lazinator tool, version 0.1.0.357
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
            
            /* Property definitions */
            
            
            
            protected string _StringWithinSubclass;
            [DebuggerBrowsable(DebuggerBrowsableState.Never)]
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
            
            /* Serialization, deserialization, and object relationships */
            
            public SubclassWithinClass() : base()
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
            
            public virtual LazinatorMemory SerializeLazinator(IncludeChildrenMode includeChildrenMode, bool verifyCleanness, bool updateStoredBuffer) 
            {
                if (LazinatorMemoryStorage.IsEmpty || includeChildrenMode != OriginalIncludeChildrenMode || (verifyCleanness || IsDirty || (includeChildrenMode != IncludeChildrenMode.ExcludeAllChildren && DescendantIsDirty)))
                {
                    return EncodeToNewBuffer(includeChildrenMode, verifyCleanness, updateStoredBuffer);
                }
                BinaryBufferWriter writer = new BinaryBufferWriter(LazinatorMemoryStorage.Length);
                writer.Write(LazinatorMemoryStorage.Span);
                return writer.LazinatorMemory;
            }
            
            protected virtual LazinatorMemory EncodeToNewBuffer(IncludeChildrenMode includeChildrenMode, bool verifyCleanness, bool updateStoredBuffer) 
            {
                int bufferSize = LazinatorMemoryStorage.Length == 0 ? ExpandableBytes.DefaultMinBufferSize : LazinatorMemoryStorage.Length;
                BinaryBufferWriter writer = new BinaryBufferWriter(bufferSize);
                SerializeExistingBuffer(ref writer, includeChildrenMode, verifyCleanness, updateStoredBuffer);
                return writer.LazinatorMemory;
            }
            
            public virtual ILazinator CloneLazinator(IncludeChildrenMode includeChildrenMode = IncludeChildrenMode.IncludeAllChildren, CloneBufferOptions cloneBufferOptions = CloneBufferOptions.IndependentBuffers)
            {
                var clone = new SubclassWithinClass()
                {
                    OriginalIncludeChildrenMode = includeChildrenMode
                };
                clone = CompleteClone(this, clone, includeChildrenMode, cloneBufferOptions);
                return clone;
            }
            
            public virtual ILazinator AssignCloneProperties(ILazinator clone, IncludeChildrenMode includeChildrenMode)
            {
                clone.FreeInMemoryObjects();
                SubclassWithinClass typedClone = (SubclassWithinClass) clone;
                typedClone.StringWithinSubclass = StringWithinSubclass;
                
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
            protected virtual ReadOnlyMemory<byte> LazinatorObjectBytes => LazinatorMemoryStorage.IsEmpty ? LazinatorMemory.EmptyReadOnlyMemory : LazinatorMemoryStorage.Memory;
            
            public virtual void UpdateStoredBuffer()
            {
                if (!IsDirty && !DescendantIsDirty && LazinatorObjectBytes.Length > 0 && OriginalIncludeChildrenMode == IncludeChildrenMode.IncludeAllChildren)
                {
                    return;
                }
                var previousBuffer = LazinatorMemoryStorage;
                if (LazinatorMemoryStorage.IsEmpty || IncludeChildrenMode.IncludeAllChildren != OriginalIncludeChildrenMode || (IsDirty || DescendantIsDirty))
                {
                    LazinatorMemoryStorage = EncodeToNewBuffer(IncludeChildrenMode.IncludeAllChildren, false, true);
                }
                else
                {
                    BinaryBufferWriter writer = new BinaryBufferWriter(LazinatorMemoryStorage.Length);
                    writer.Write(LazinatorMemoryStorage.Span);
                    LazinatorMemoryStorage = writer.LazinatorMemory;
                }
                OriginalIncludeChildrenMode = IncludeChildrenMode.IncludeAllChildren;
                if (!LazinatorParents.Any())
                {
                    previousBuffer.Dispose();
                }
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
                yield return ("StringWithinSubclass", (object)StringWithinSubclass);
                yield break;
            }
            
            public virtual ILazinator ForEachLazinator(Func<ILazinator, ILazinator> changeFunc, bool exploreOnlyDeserializedChildren, bool changeThisLevel)
            {
                if (changeThisLevel)
                {
                    return changeFunc(this);
                }
                return this;
            }
            
            public virtual void FreeInMemoryObjects()
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
                        UpdateDeserializedChildren(ref writer, startPosition);
                    }
                    
                }
                else
                {
                    throw new LazinatorSerializationException("Cannot update stored buffer when serializing only some children.");
                }
                
                var newBuffer = writer.Slice(startPosition, length);
                LazinatorMemoryStorage = newBuffer;
            }
            
            protected virtual void UpdateDeserializedChildren(ref BinaryBufferWriter writer, int startPosition)
            {
            }
            
            
            protected virtual void WritePropertiesIntoBuffer(ref BinaryBufferWriter writer, IncludeChildrenMode includeChildrenMode, bool verifyCleanness, bool updateStoredBuffer, bool includeUniqueID)
            {
                // header information
                if (includeUniqueID)
                {
                    if (!ContainsOpenGenericParameters)
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
