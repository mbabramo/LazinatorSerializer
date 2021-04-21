//25941316-03fd-0f22-eba2-c9651d9ea30d
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by the Lazinator tool, version 0.1.0.393
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

#nullable disable
namespace LazinatorTests.Examples.Subclasses
{
    using Lazinator.Attributes;
    using Lazinator.Buffers;
    using Lazinator.Core;
    using Lazinator.Exceptions;
    using Lazinator.Support;
    using static Lazinator.Buffers.WriteUncompressedPrimitives;
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
            
            public SubclassWithinClass(IncludeChildrenMode originalIncludeChildrenMode = IncludeChildrenMode.IncludeAllChildren)
            {
                OriginalIncludeChildrenMode = originalIncludeChildrenMode;
            }
            
            public SubclassWithinClass(LazinatorMemory serializedBytes, ILazinator parent = null, IncludeChildrenMode originalIncludeChildrenMode = IncludeChildrenMode.IncludeAllChildren, int? lazinatorObjectVersion = null)
            {
                if (lazinatorObjectVersion != null)
                {
                    LazinatorObjectVersion = (int) lazinatorObjectVersion;
                }
                OriginalIncludeChildrenMode = originalIncludeChildrenMode;
                LazinatorParents = new LazinatorParentsCollection(parent);
                DeserializeLazinator(serializedBytes);
                HasChanged = false;
                DescendantHasChanged = false;
            }
            
            public virtual LazinatorParentsCollection LazinatorParents { get; set; }
            
            public virtual LazinatorMemory LazinatorMemoryStorage
            {
                get;
                set;
            }
            
            public virtual IncludeChildrenMode OriginalIncludeChildrenMode { get; set; }
            
            public virtual bool HasChanged { get; set; }
            
            protected bool _IsDirty;
            public virtual bool IsDirty
            {
                [DebuggerStepThrough]
                get => _IsDirty|| LazinatorMemoryStorage.Length == 0;
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
            
            public virtual bool NonBinaryHash32 => false;
            
            protected virtual void DeserializeLazinator(LazinatorMemory serializedBytes)
            {
                LazinatorMemoryStorage = serializedBytes;
                int length = Deserialize();
                if (length != LazinatorMemoryStorage.Length)
                {
                    LazinatorMemoryStorage = LazinatorMemoryStorage.Slice(0, length);
                }
            }
            
            protected virtual int Deserialize()
            {
                FreeInMemoryObjects();
                int bytesSoFar = 0;
                ReadOnlySpan<byte> span = LazinatorMemoryStorage.ReadOnlyMemory.Span;
                if (span.Length == 0)
                {
                    return 0;
                }
                
                ReadGenericIDIfApplicable(ContainsOpenGenericParameters, LazinatorUniqueID, span, ref bytesSoFar);
                
                int lazinatorLibraryVersion = span.ToDecompressedInt32(ref bytesSoFar);
                
                int serializedVersionNumber = span.ToDecompressedInt32(ref bytesSoFar);
                
                OriginalIncludeChildrenMode = (IncludeChildrenMode)span.ToByte(ref bytesSoFar);
                
                int totalBytes = ConvertFromBytesAfterHeader(OriginalIncludeChildrenMode, serializedVersionNumber, ref bytesSoFar);
                return totalBytes;
            }
            
            public virtual void SerializeLazinator()
            {
                if (!IsDirty && !DescendantIsDirty && LazinatorMemoryStorage.Length > 0 && OriginalIncludeChildrenMode == IncludeChildrenMode.IncludeAllChildren)
                {
                    return;
                    
                }
                var previousBuffer = LazinatorMemoryStorage;
                if (LazinatorMemoryStorage.IsEmpty || IncludeChildrenMode.IncludeAllChildren != OriginalIncludeChildrenMode || (IsDirty || DescendantIsDirty))
                {
                    LazinatorMemoryStorage = EncodeToNewBuffer(LazinatorSerializationOptions.Default);
                }
                else
                {
                    BufferWriter writer = new BufferWriter(LazinatorMemoryStorage.LengthInt ?? 0);
                    LazinatorMemoryStorage.WriteToBuffer(ref writer);
                    LazinatorMemoryStorage = writer.LazinatorMemory;
                }
                OriginalIncludeChildrenMode = IncludeChildrenMode.IncludeAllChildren;
                if (!LazinatorParents.Any())
                {
                    previousBuffer.Dispose();
                }
            }
            
            public virtual LazinatorMemory SerializeLazinator(in LazinatorSerializationOptions options) 
            {
                if (LazinatorMemoryStorage.IsEmpty || options.IncludeChildrenMode != OriginalIncludeChildrenMode || (options.SerializeDiffs || options.VerifyCleanness || IsDirty || (options.IncludeChildrenMode != IncludeChildrenMode.ExcludeAllChildren && DescendantIsDirty)))
                {
                    return EncodeToNewBuffer(options);
                }
                BufferWriter writer = new BufferWriter(LazinatorMemoryStorage.LengthInt ?? 0);
                LazinatorMemoryStorage.WriteToBuffer(ref writer);
                return writer.LazinatorMemory;
            }
            
            protected virtual LazinatorMemory EncodeToNewBuffer(in LazinatorSerializationOptions options) 
            {
                int bufferSize = LazinatorMemoryStorage.Length == 0 ? ExpandableBytes.DefaultMinBufferSize : LazinatorMemoryStorage.LengthInt ?? ExpandableBytes.DefaultMinBufferSize;
                BufferWriter writer = new BufferWriter(bufferSize);
                SerializeToExistingBuffer(ref writer, options);
                return writer.LazinatorMemory;
            }
            
            public virtual ILazinator CloneLazinator(IncludeChildrenMode includeChildrenMode = IncludeChildrenMode.IncludeAllChildren, CloneBufferOptions cloneBufferOptions = CloneBufferOptions.IndependentBuffers)
            {
                SubclassWithinClass clone;
                if (cloneBufferOptions == CloneBufferOptions.NoBuffer)
                {
                    clone = new SubclassWithinClass(includeChildrenMode);
                    clone.LazinatorObjectVersion = LazinatorObjectVersion;
                    clone = (SubclassWithinClass)AssignCloneProperties(clone, includeChildrenMode);
                }
                else
                {
                    LazinatorMemory bytes = EncodeOrRecycleToNewBuffer(includeChildrenMode, OriginalIncludeChildrenMode, IsDirty, DescendantIsDirty, false, LazinatorMemoryStorage, this);
                    clone = new SubclassWithinClass(bytes);
                }
                return clone;
            }
            
            protected virtual ILazinator AssignCloneProperties(ILazinator clone, IncludeChildrenMode includeChildrenMode)
            {
                clone.FreeInMemoryObjects();
                SubclassWithinClass typedClone = (SubclassWithinClass) clone;
                typedClone.StringWithinSubclass = StringWithinSubclass;
                
                return typedClone;
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
            
            public virtual ILazinator ForEachLazinator(Func<ILazinator, ILazinator> changeFunc, bool exploreOnlyDeserializedChildren, bool changeThisLevel)
            {
                if (changeThisLevel && changeFunc != null)
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
            
            public virtual int LazinatorUniqueID => 1058;
            
            protected virtual bool ContainsOpenGenericParameters => false;
            public virtual LazinatorGenericIDType LazinatorGenericID => default;
            
            
            public virtual int LazinatorObjectVersion { get; set; } = 0;
            
            
            protected virtual int ConvertFromBytesAfterHeader(IncludeChildrenMode includeChildrenMode, int serializedVersionNumber, ref int bytesSoFar)
            {
                ReadOnlySpan<byte> span = LazinatorMemoryStorage.ReadOnlyMemory.Span;
                ConvertFromBytesForPrimitiveProperties(span, includeChildrenMode, serializedVersionNumber, ref bytesSoFar);
                int lengthForLengths = 0;
                int totalChildrenSize = ConvertFromBytesForChildProperties(span, includeChildrenMode, serializedVersionNumber, bytesSoFar + lengthForLengths, ref bytesSoFar);
                return bytesSoFar + totalChildrenSize;
            }
            
            protected virtual void ConvertFromBytesForPrimitiveProperties(ReadOnlySpan<byte> span, IncludeChildrenMode includeChildrenMode, int serializedVersionNumber, ref int bytesSoFar)
            {
                _StringWithinSubclass = span.ToString_BrotliCompressedWithLength(ref bytesSoFar);
            }
            
            protected virtual int ConvertFromBytesForChildProperties(ReadOnlySpan<byte> span, IncludeChildrenMode includeChildrenMode, int serializedVersionNumber, int indexOfFirstChild, ref int bytesSoFar)
            {
                int totalChildrenBytes = 0;
                return totalChildrenBytes;
            }
            
            public virtual void SerializeToExistingBuffer(ref BufferWriter writer, in LazinatorSerializationOptions options)
            {
                int startPosition = writer.ActiveMemoryPosition;
                WritePropertiesIntoBuffer(ref writer, options, true);
                if (options.UpdateStoredBuffer)
                {
                    UpdateStoredBuffer(ref writer, startPosition, writer.ActiveMemoryPosition - startPosition, options.IncludeChildrenMode, false);
                }
            }
            
            public virtual void UpdateStoredBuffer(ref BufferWriter writer, long startPosition, long length, IncludeChildrenMode includeChildrenMode, bool updateDeserializedChildren)
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
                    ThrowHelper.ThrowCannotUpdateStoredBuffer();
                }
                
                var newBuffer = writer.Slice(startPosition, length);
                LazinatorMemoryStorage = newBuffer;
            }
            
            protected virtual void UpdateDeserializedChildren(ref BufferWriter writer, long startPosition)
            {
                
            }
            
            
            protected virtual void WritePropertiesIntoBuffer(ref BufferWriter writer, in LazinatorSerializationOptions options, bool includeUniqueID)
            {
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
                writer.Write((byte)options.IncludeChildrenMode);
                // write properties
                
                WritePrimitivePropertiesIntoBuffer(ref writer, options, includeUniqueID);
                
            }
            
            protected virtual void WritePrimitivePropertiesIntoBuffer(ref BufferWriter writer, in LazinatorSerializationOptions options, bool includeUniqueID)
            {
                EncodeCharAndString.WriteBrotliCompressedWithIntPrefix(ref writer, _StringWithinSubclass);
            }
            protected virtual void WriteChildrenPropertiesIntoBuffer(ref BufferWriter writer, LazinatorSerializationOptions options, bool includeUniqueID, int startOfObjectPosition)
            {
                
            }
        }
        
    }
}
