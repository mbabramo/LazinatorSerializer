//995cc6f6-4919-50bf-7f88-e3d0b62f42f3
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
    
    [Autogenerated]
    public partial class ClassWithSubclass : ILazinator
    {
        public bool IsStruct => false;
        
        /* Serialization, deserialization, and object relationships */
        
        public ClassWithSubclass() : base()
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
        
        public virtual LazinatorMemory SerializeNewBuffer(IncludeChildrenMode includeChildrenMode, bool verifyCleanness)
        {
            return EncodeOrRecycleToNewBuffer(includeChildrenMode, OriginalIncludeChildrenMode, verifyCleanness, IsDirty, DescendantIsDirty, false, LazinatorObjectBytes, (EncodeManuallyDelegate) EncodeToNewBuffer);
        }
        
        protected virtual LazinatorMemory EncodeToNewBuffer(IncludeChildrenMode includeChildrenMode, bool verifyCleanness, bool updateStoredBuffer) => LazinatorUtilities.EncodeToNewBinaryBufferWriter(this, includeChildrenMode, verifyCleanness, updateStoredBuffer);
        
        public virtual ILazinator CloneLazinator()
        {
            return CloneLazinator(OriginalIncludeChildrenMode);
        }
        
        public virtual ILazinator CloneLazinator(IncludeChildrenMode includeChildrenMode)
        {
            LazinatorMemory bytes = EncodeOrRecycleToNewBuffer(includeChildrenMode, OriginalIncludeChildrenMode, false, IsDirty, DescendantIsDirty, false, LazinatorObjectBytes, (EncodeManuallyDelegate)EncodeToNewBuffer);
            var clone = new ClassWithSubclass()
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
                LazinatorObjectBytes = value.Memory;
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
            if (!IsDirty && !DescendantIsDirty && LazinatorObjectBytes.Length > 0)
            {
                return;
            }
            LazinatorMemory bytes = EncodeOrRecycleToNewBuffer(IncludeChildrenMode.IncludeAllChildren, OriginalIncludeChildrenMode, false, IsDirty, DescendantIsDirty, false, LazinatorObjectBytes, (EncodeManuallyDelegate)EncodeToNewBuffer);
            _LazinatorObjectBytes = bytes.Memory;
        }
        
        public virtual int GetByteLength()
        {
            LazinatorConvertToBytes();
            return LazinatorObjectBytes.Length;
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
        
        protected int _SubclassInstance1_ByteIndex;
        protected int _SubclassInstance2_ByteIndex;
        protected virtual int _SubclassInstance1_ByteLength => _SubclassInstance2_ByteIndex - _SubclassInstance1_ByteIndex;
        private int _ClassWithSubclass_EndByteIndex;
        protected virtual int _SubclassInstance2_ByteLength => _ClassWithSubclass_EndByteIndex - _SubclassInstance2_ByteIndex;
        
        protected int _IntWithinSuperclass;
        public int IntWithinSuperclass
        {
            get
            {
                return _IntWithinSuperclass;
            }
            set
            {
                IsDirty = true;
                _IntWithinSuperclass = value;
            }
        }
        protected global::LazinatorTests.Examples.Subclasses.ClassWithSubclass.SubclassWithinClass _SubclassInstance1;
        public global::LazinatorTests.Examples.Subclasses.ClassWithSubclass.SubclassWithinClass SubclassInstance1
        {
            get
            {
                if (!_SubclassInstance1_Accessed)
                {
                    if (LazinatorObjectBytes.Length == 0)
                    {
                        _SubclassInstance1 = default(global::LazinatorTests.Examples.Subclasses.ClassWithSubclass.SubclassWithinClass);
                    }
                    else
                    {
                        ReadOnlyMemory<byte> childData = GetChildSlice(LazinatorObjectBytes, _SubclassInstance1_ByteIndex, _SubclassInstance1_ByteLength, false, false, null);
                        
                        _SubclassInstance1 = DeserializationFactory.Instance.CreateBaseOrDerivedType(258, () => new global::LazinatorTests.Examples.Subclasses.ClassWithSubclass.SubclassWithinClass(), childData, this); 
                    }
                    _SubclassInstance1_Accessed = true;
                } 
                return _SubclassInstance1;
            }
            set
            {
                if (_SubclassInstance1 != null)
                {
                    _SubclassInstance1.LazinatorParents = _SubclassInstance1.LazinatorParents.WithRemoved(this);
                }
                if (value != null)
                {
                    value.LazinatorParents = value.LazinatorParents.WithAdded(this);
                }
                
                IsDirty = true;
                DescendantIsDirty = true;
                _SubclassInstance1 = value;
                _SubclassInstance1_Accessed = true;
            }
        }
        protected bool _SubclassInstance1_Accessed;
        protected global::LazinatorTests.Examples.Subclasses.ClassWithSubclass.SubclassWithinClass _SubclassInstance2;
        public global::LazinatorTests.Examples.Subclasses.ClassWithSubclass.SubclassWithinClass SubclassInstance2
        {
            get
            {
                if (!_SubclassInstance2_Accessed)
                {
                    if (LazinatorObjectBytes.Length == 0)
                    {
                        _SubclassInstance2 = default(global::LazinatorTests.Examples.Subclasses.ClassWithSubclass.SubclassWithinClass);
                    }
                    else
                    {
                        ReadOnlyMemory<byte> childData = GetChildSlice(LazinatorObjectBytes, _SubclassInstance2_ByteIndex, _SubclassInstance2_ByteLength, false, false, null);
                        
                        _SubclassInstance2 = DeserializationFactory.Instance.CreateBaseOrDerivedType(258, () => new global::LazinatorTests.Examples.Subclasses.ClassWithSubclass.SubclassWithinClass(), childData, this); 
                    }
                    _SubclassInstance2_Accessed = true;
                } 
                return _SubclassInstance2;
            }
            set
            {
                if (_SubclassInstance2 != null)
                {
                    _SubclassInstance2.LazinatorParents = _SubclassInstance2.LazinatorParents.WithRemoved(this);
                }
                if (value != null)
                {
                    value.LazinatorParents = value.LazinatorParents.WithAdded(this);
                }
                
                IsDirty = true;
                DescendantIsDirty = true;
                _SubclassInstance2 = value;
                _SubclassInstance2_Accessed = true;
            }
        }
        protected bool _SubclassInstance2_Accessed;
        
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
            if (enumerateNulls && (SubclassInstance1 == null))
            {
                yield return ("SubclassInstance1", default);
            }
            else if ((!exploreOnlyDeserializedChildren && SubclassInstance1 != null) || (_SubclassInstance1_Accessed && _SubclassInstance1 != null))
            {
                foreach (ILazinator toYield in SubclassInstance1.EnumerateLazinatorNodes(matchCriterion, stopExploringBelowMatch, exploreCriterion, exploreOnlyDeserializedChildren, enumerateNulls))
                {
                    yield return ("SubclassInstance1", toYield);
                }
            }
            if (enumerateNulls && (SubclassInstance2 == null))
            {
                yield return ("SubclassInstance2", default);
            }
            else if ((!exploreOnlyDeserializedChildren && SubclassInstance2 != null) || (_SubclassInstance2_Accessed && _SubclassInstance2 != null))
            {
                foreach (ILazinator toYield in SubclassInstance2.EnumerateLazinatorNodes(matchCriterion, stopExploringBelowMatch, exploreCriterion, exploreOnlyDeserializedChildren, enumerateNulls))
                {
                    yield return ("SubclassInstance2", toYield);
                }
            }
            yield break;
        }
        
        
        public virtual IEnumerable<(string propertyName, object descendant)> EnumerateNonLazinatorProperties()
        {
            yield return ("IntWithinSuperclass", (object)IntWithinSuperclass);
            yield break;
        }
        
        protected virtual void ResetAccessedProperties()
        {
            _SubclassInstance1_Accessed = _SubclassInstance2_Accessed = false;
            IsDirty = false;
            DescendantIsDirty = false;
            HasChanged = false;
            DescendantHasChanged = false;
        }
        
        /* Conversion */
        
        public virtual int LazinatorUniqueID => 257;
        
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
            _IntWithinSuperclass = span.ToDecompressedInt(ref bytesSoFar);
            _SubclassInstance1_ByteIndex = bytesSoFar;
            if (includeChildrenMode != IncludeChildrenMode.ExcludeAllChildren && includeChildrenMode != IncludeChildrenMode.IncludeOnlyIncludableChildren) 
            {
                bytesSoFar = span.ToInt32(ref bytesSoFar) + bytesSoFar;
            }
            _SubclassInstance2_ByteIndex = bytesSoFar;
            if (includeChildrenMode != IncludeChildrenMode.ExcludeAllChildren && includeChildrenMode != IncludeChildrenMode.IncludeOnlyIncludableChildren) 
            {
                bytesSoFar = span.ToInt32(ref bytesSoFar) + bytesSoFar;
            }
            _ClassWithSubclass_EndByteIndex = bytesSoFar;
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
                
                _LazinatorObjectBytes = writer.Slice(startPosition);
            }
        }
        protected virtual void WritePropertiesIntoBuffer(ref BinaryBufferWriter writer, IncludeChildrenMode includeChildrenMode, bool verifyCleanness, bool updateStoredBuffer, bool includeUniqueID)
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
            CompressedIntegralTypes.WriteCompressedInt(ref writer, _IntWithinSuperclass);
            startOfObjectPosition = writer.Position;
            if (includeChildrenMode != IncludeChildrenMode.ExcludeAllChildren && includeChildrenMode != IncludeChildrenMode.IncludeOnlyIncludableChildren) 
            {
                if (includeChildrenMode != IncludeChildrenMode.IncludeAllChildren && !_SubclassInstance1_Accessed)
                {
                    var deserialized = SubclassInstance1;
                }
                WriteChild(ref writer, _SubclassInstance1, includeChildrenMode, _SubclassInstance1_Accessed, () => GetChildSlice(LazinatorObjectBytes, _SubclassInstance1_ByteIndex, _SubclassInstance1_ByteLength, false, false, null), verifyCleanness, updateStoredBuffer, false, false, this);
            }
            if (updateStoredBuffer)
            {
                _SubclassInstance1_ByteIndex = startOfObjectPosition - startPosition;
            }
            startOfObjectPosition = writer.Position;
            if (includeChildrenMode != IncludeChildrenMode.ExcludeAllChildren && includeChildrenMode != IncludeChildrenMode.IncludeOnlyIncludableChildren) 
            {
                if (includeChildrenMode != IncludeChildrenMode.IncludeAllChildren && !_SubclassInstance2_Accessed)
                {
                    var deserialized = SubclassInstance2;
                }
                WriteChild(ref writer, _SubclassInstance2, includeChildrenMode, _SubclassInstance2_Accessed, () => GetChildSlice(LazinatorObjectBytes, _SubclassInstance2_ByteIndex, _SubclassInstance2_ByteLength, false, false, null), verifyCleanness, updateStoredBuffer, false, false, this);
            }
            if (updateStoredBuffer)
            {
                _SubclassInstance2_ByteIndex = startOfObjectPosition - startPosition;
            }
            if (updateStoredBuffer)
            {
                _ClassWithSubclass_EndByteIndex = writer.Position - startPosition;
            }
        }
        
    }
}
