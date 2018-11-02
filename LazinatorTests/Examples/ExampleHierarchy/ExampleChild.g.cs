//264863a1-62f4-eded-647b-7e47adf03c9e
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by the Lazinator tool, version 0.1.0.276
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace LazinatorTests.Examples
{
    using Lazinator.Attributes;
    using Lazinator.Buffers;
    using Lazinator.Core;
    using Lazinator.Exceptions;
    using Lazinator.Support;
    using LazinatorTests.Examples.Structs;
    using System;
    using System.Buffers;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Runtime.InteropServices;
    using static Lazinator.Core.LazinatorUtilities;
    
    [Autogenerated]
    public partial class ExampleChild : ILazinator
    {
        public bool IsStruct => false;
        
        /* Serialization, deserialization, and object relationships */
        
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
            return EncodeOrRecycleToNewBuffer(includeChildrenMode, OriginalIncludeChildrenMode, verifyCleanness, IsDirty, DescendantIsDirty, false, LazinatorMemoryStorage, (EncodeManuallyDelegate) EncodeToNewBuffer, updateStoredBuffer);
        }
        
        protected virtual LazinatorMemory EncodeToNewBuffer(IncludeChildrenMode includeChildrenMode, bool verifyCleanness, bool updateStoredBuffer) => LazinatorUtilities.EncodeToNewBinaryBufferWriter(this, includeChildrenMode, verifyCleanness, updateStoredBuffer);
        
        public virtual ILazinator CloneLazinator(IncludeChildrenMode includeChildrenMode = IncludeChildrenMode.IncludeAllChildren, CloneBufferOptions cloneBufferOptions = CloneBufferOptions.LinkedBuffer)
        {
            var clone = new ExampleChild()
            {
                OriginalIncludeChildrenMode = includeChildrenMode
            };
            if (clone.LazinatorObjectVersion != LazinatorObjectVersion)
            {
                clone.LazinatorObjectVersion = LazinatorObjectVersion;
            }
            
            if (cloneBufferOptions == CloneBufferOptions.NoBuffer)
            {
                AssignCloneProperties(clone, includeChildrenMode);
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
        
        protected virtual void AssignCloneProperties(ILazinator clone, IncludeChildrenMode includeChildrenMode)
        {
            clone.FreeInMemoryObjects();
            ExampleChild typedClone = (ExampleChild) clone;
            typedClone.MyLong = MyLong;
            typedClone.MyShort = MyShort;
            typedClone.ByteSpan = Clone_ReadOnlySpan_Gbyte_g(ByteSpan, includeChildrenMode);
            if (includeChildrenMode != IncludeChildrenMode.ExcludeAllChildren && includeChildrenMode != IncludeChildrenMode.IncludeOnlyIncludableChildren) 
            {
                typedClone.MyExampleGrandchild = (MyExampleGrandchild == null) ? default(ExampleGrandchild) : (ExampleGrandchild) MyExampleGrandchild.CloneLazinator(includeChildrenMode, CloneBufferOptions.NoBuffer);
            }
            if (includeChildrenMode != IncludeChildrenMode.ExcludeAllChildren && includeChildrenMode != IncludeChildrenMode.IncludeOnlyIncludableChildren) 
            {
                typedClone.MyWrapperContainer = (MyWrapperContainer == null) ? default(WrapperContainer) : (WrapperContainer) MyWrapperContainer.CloneLazinator(includeChildrenMode, CloneBufferOptions.NoBuffer);
            }
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
        protected virtual ReadOnlyMemory<byte> LazinatorObjectBytes => LazinatorMemoryStorage?.Memory ?? LazinatorUtilities.EmptyReadOnlyMemory;
        
        public virtual void EnsureLazinatorMemoryUpToDate()
        {
            if (!IsDirty && !DescendantIsDirty && LazinatorObjectBytes.Length > 0 && OriginalIncludeChildrenMode == IncludeChildrenMode.IncludeAllChildren)
            {
                return;
            }
            EncodeOrRecycleToNewBuffer(IncludeChildrenMode.IncludeAllChildren, OriginalIncludeChildrenMode, false, IsDirty, DescendantIsDirty, false, LazinatorMemoryStorage, (EncodeManuallyDelegate)EncodeToNewBuffer, true);
            OriginalIncludeChildrenMode = IncludeChildrenMode.IncludeAllChildren;
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
        
        protected int _ByteSpan_ByteIndex;
        protected int _MyExampleGrandchild_ByteIndex;
        protected int _MyWrapperContainer_ByteIndex;
        protected virtual int _ByteSpan_ByteLength => _MyExampleGrandchild_ByteIndex - _ByteSpan_ByteIndex;
        protected virtual int _MyExampleGrandchild_ByteLength => _MyWrapperContainer_ByteIndex - _MyExampleGrandchild_ByteIndex;
        private int _ExampleChild_EndByteIndex;
        protected virtual int _MyWrapperContainer_ByteLength => _ExampleChild_EndByteIndex - _MyWrapperContainer_ByteIndex;
        
        
        protected long _MyLong;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public long MyLong
        {
            get
            {
                return _MyLong;
            }
            set
            {
                IsDirty = true;
                _MyLong = value;
            }
        }
        
        protected short _MyShort;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public short MyShort
        {
            get
            {
                return _MyShort;
            }
            set
            {
                IsDirty = true;
                _MyShort = value;
            }
        }
        private ReadOnlyMemory<byte> _ByteSpan;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public ReadOnlySpan<byte> ByteSpan
        {
            get
            {
                if (!_ByteSpan_Accessed)
                {
                    LazinatorMemory childData = GetChildSlice(LazinatorMemoryStorage, _ByteSpan_ByteIndex, _ByteSpan_ByteLength, false, false, null);
                    _ByteSpan = childData.ReadOnlyMemory;
                    _ByteSpan_Accessed = true;
                }
                return _ByteSpan.Span;
            }
            set
            {
                IsDirty = true;
                _ByteSpan = new ReadOnlyMemory<byte>(MemoryMarshal.Cast<byte, byte>(value).ToArray());
                _ByteSpan_Accessed = true;
            }
        }
        protected bool _ByteSpan_Accessed;
        
        protected ExampleGrandchild _MyExampleGrandchild;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public ExampleGrandchild MyExampleGrandchild
        {
            get
            {
                if (!_MyExampleGrandchild_Accessed)
                {
                    if (LazinatorObjectBytes.Length == 0)
                    {
                        _MyExampleGrandchild = default(ExampleGrandchild);
                    }
                    else
                    {
                        LazinatorMemory childData = GetChildSlice(LazinatorMemoryStorage, _MyExampleGrandchild_ByteIndex, _MyExampleGrandchild_ByteLength, false, false, null);
                        
                        _MyExampleGrandchild = DeserializationFactory.Instance.CreateBaseOrDerivedType(279, () => new ExampleGrandchild(), childData, this); 
                    }
                    _MyExampleGrandchild_Accessed = true;
                } 
                return _MyExampleGrandchild;
            }
            set
            {
                if (_MyExampleGrandchild != null)
                {
                    _MyExampleGrandchild.LazinatorParents = _MyExampleGrandchild.LazinatorParents.WithRemoved(this);
                }
                if (value != null)
                {
                    value.LazinatorParents = value.LazinatorParents.WithAdded(this);
                }
                
                IsDirty = true;
                DescendantIsDirty = true;
                _MyExampleGrandchild = value;
                _MyExampleGrandchild_Accessed = true;
            }
        }
        protected bool _MyExampleGrandchild_Accessed;
        
        protected WrapperContainer _MyWrapperContainer;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public WrapperContainer MyWrapperContainer
        {
            get
            {
                if (!_MyWrapperContainer_Accessed)
                {
                    if (LazinatorObjectBytes.Length == 0)
                    {
                        _MyWrapperContainer = default(WrapperContainer);
                    }
                    else
                    {
                        LazinatorMemory childData = GetChildSlice(LazinatorMemoryStorage, _MyWrapperContainer_ByteIndex, _MyWrapperContainer_ByteLength, false, false, null);
                        
                        _MyWrapperContainer = DeserializationFactory.Instance.CreateBaseOrDerivedType(248, () => new WrapperContainer(), childData, this); 
                    }
                    _MyWrapperContainer_Accessed = true;
                } 
                return _MyWrapperContainer;
            }
            set
            {
                if (_MyWrapperContainer != null)
                {
                    _MyWrapperContainer.LazinatorParents = _MyWrapperContainer.LazinatorParents.WithRemoved(this);
                }
                if (value != null)
                {
                    value.LazinatorParents = value.LazinatorParents.WithAdded(this);
                }
                
                IsDirty = true;
                DescendantIsDirty = true;
                _MyWrapperContainer = value;
                _MyWrapperContainer_Accessed = true;
            }
        }
        protected bool _MyWrapperContainer_Accessed;
        
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
            if (enumerateNulls && (!exploreOnlyDeserializedChildren || _MyExampleGrandchild_Accessed) && (MyExampleGrandchild == null))
            {
                yield return ("MyExampleGrandchild", default);
            }
            else if ((!exploreOnlyDeserializedChildren && MyExampleGrandchild != null) || (_MyExampleGrandchild_Accessed && _MyExampleGrandchild != null))
            {
                bool isMatch = matchCriterion == null || matchCriterion(MyExampleGrandchild);
                bool shouldExplore = exploreCriterion == null || exploreCriterion(MyExampleGrandchild);
                if (isMatch)
                {
                    yield return ("MyExampleGrandchild", MyExampleGrandchild);
                }
                if ((!stopExploringBelowMatch || !isMatch) && shouldExplore)
                {
                    foreach (var toYield in MyExampleGrandchild.EnumerateLazinatorDescendants(matchCriterion, stopExploringBelowMatch, exploreCriterion, exploreOnlyDeserializedChildren, enumerateNulls))
                    {
                        yield return ("MyExampleGrandchild" + "." + toYield.propertyName, toYield.descendant);
                    }
                }
            }
            if (enumerateNulls && (!exploreOnlyDeserializedChildren || _MyWrapperContainer_Accessed) && (MyWrapperContainer == null))
            {
                yield return ("MyWrapperContainer", default);
            }
            else if ((!exploreOnlyDeserializedChildren && MyWrapperContainer != null) || (_MyWrapperContainer_Accessed && _MyWrapperContainer != null))
            {
                bool isMatch = matchCriterion == null || matchCriterion(MyWrapperContainer);
                bool shouldExplore = exploreCriterion == null || exploreCriterion(MyWrapperContainer);
                if (isMatch)
                {
                    yield return ("MyWrapperContainer", MyWrapperContainer);
                }
                if ((!stopExploringBelowMatch || !isMatch) && shouldExplore)
                {
                    foreach (var toYield in MyWrapperContainer.EnumerateLazinatorDescendants(matchCriterion, stopExploringBelowMatch, exploreCriterion, exploreOnlyDeserializedChildren, enumerateNulls))
                    {
                        yield return ("MyWrapperContainer" + "." + toYield.propertyName, toYield.descendant);
                    }
                }
            }
            yield break;
        }
        
        
        public virtual IEnumerable<(string propertyName, object descendant)> EnumerateNonLazinatorProperties()
        {
            yield return ("MyLong", (object)MyLong);
            yield return ("MyShort", (object)MyShort);
            yield return ("ByteSpan", (object)ByteSpan.ToString());
            yield break;
        }
        
        public virtual void FreeInMemoryObjects()
        {
            _ByteSpan = default;
            _MyExampleGrandchild = default;
            _MyWrapperContainer = default;
            _ByteSpan_Accessed = _MyExampleGrandchild_Accessed = _MyWrapperContainer_Accessed = false;
            IsDirty = false;
            DescendantIsDirty = false;
            HasChanged = false;
            DescendantHasChanged = false;
        }
        
        /* Conversion */
        
        public virtual int LazinatorUniqueID => 213;
        
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
            _MyLong = span.ToDecompressedLong(ref bytesSoFar);
            _MyShort = span.ToDecompressedShort(ref bytesSoFar);
            _ByteSpan_ByteIndex = bytesSoFar;
            bytesSoFar = span.ToInt32(ref bytesSoFar) + bytesSoFar;
            _MyExampleGrandchild_ByteIndex = bytesSoFar;
            if (includeChildrenMode != IncludeChildrenMode.ExcludeAllChildren && includeChildrenMode != IncludeChildrenMode.IncludeOnlyIncludableChildren) 
            {
                bytesSoFar = span.ToInt32(ref bytesSoFar) + bytesSoFar;
            }
            _MyWrapperContainer_ByteIndex = bytesSoFar;
            if (includeChildrenMode != IncludeChildrenMode.ExcludeAllChildren && includeChildrenMode != IncludeChildrenMode.IncludeOnlyIncludableChildren) 
            {
                bytesSoFar = span.ToInt32(ref bytesSoFar) + bytesSoFar;
            }
            _ExampleChild_EndByteIndex = bytesSoFar;
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
                    if (_MyExampleGrandchild_Accessed && _MyExampleGrandchild != null)
                    {
                        _MyExampleGrandchild.UpdateStoredBuffer(ref writer, startPosition + _MyExampleGrandchild_ByteIndex + sizeof(int), _MyExampleGrandchild_ByteLength - sizeof(int), IncludeChildrenMode.IncludeAllChildren, true);
                    }
                    if (_MyWrapperContainer_Accessed && _MyWrapperContainer != null)
                    {
                        _MyWrapperContainer.UpdateStoredBuffer(ref writer, startPosition + _MyWrapperContainer_ByteIndex + sizeof(int), _MyWrapperContainer_ByteLength - sizeof(int), IncludeChildrenMode.IncludeAllChildren, true);
                    }
                }
                
            }
            else
            {
                throw new Exception("Cannot update stored buffer when serializing only some children.");
            }
            
            var newBuffer = writer.Slice(startPosition, length);
            LazinatorMemoryStorage = ReplaceBuffer(LazinatorMemoryStorage, newBuffer, LazinatorParents, startPosition == 0, IsStruct);
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
            CompressedIntegralTypes.WriteCompressedLong(ref writer, _MyLong);
            CompressedIntegralTypes.WriteCompressedShort(ref writer, _MyShort);
            startOfObjectPosition = writer.Position;
            if (includeChildrenMode != IncludeChildrenMode.IncludeAllChildren || includeChildrenMode != OriginalIncludeChildrenMode && !_ByteSpan_Accessed)
            {
                var deserialized = ByteSpan;
            }
            WriteNonLazinatorObject(
            nonLazinatorObject: _ByteSpan, isBelievedDirty: _ByteSpan_Accessed || (includeChildrenMode != OriginalIncludeChildrenMode),
            isAccessed: _ByteSpan_Accessed, writer: ref writer,
            getChildSliceForFieldFn: () => GetChildSlice(LazinatorMemoryStorage, _ByteSpan_ByteIndex, _ByteSpan_ByteLength, false, false, null),
            verifyCleanness: false,
            binaryWriterAction: (ref BinaryBufferWriter w, bool v) =>
            ConvertToBytes_ReadOnlySpan_Gbyte_g(ref w, _ByteSpan.Span,
            includeChildrenMode, v, updateStoredBuffer));
            if (updateStoredBuffer)
            {
                _ByteSpan_ByteIndex = startOfObjectPosition - startPosition;
            }
            startOfObjectPosition = writer.Position;
            if (includeChildrenMode != IncludeChildrenMode.ExcludeAllChildren && includeChildrenMode != IncludeChildrenMode.IncludeOnlyIncludableChildren) 
            {
                if (includeChildrenMode != IncludeChildrenMode.IncludeAllChildren || includeChildrenMode != OriginalIncludeChildrenMode && !_MyExampleGrandchild_Accessed)
                {
                    var deserialized = MyExampleGrandchild;
                }
                WriteChild(ref writer, ref _MyExampleGrandchild, includeChildrenMode, _MyExampleGrandchild_Accessed, () => GetChildSlice(LazinatorMemoryStorage, _MyExampleGrandchild_ByteIndex, _MyExampleGrandchild_ByteLength, false, false, null), verifyCleanness, updateStoredBuffer, false, false, this);
            }
            if (updateStoredBuffer)
            {
                _MyExampleGrandchild_ByteIndex = startOfObjectPosition - startPosition;
            }
            startOfObjectPosition = writer.Position;
            if (includeChildrenMode != IncludeChildrenMode.ExcludeAllChildren && includeChildrenMode != IncludeChildrenMode.IncludeOnlyIncludableChildren) 
            {
                if (includeChildrenMode != IncludeChildrenMode.IncludeAllChildren || includeChildrenMode != OriginalIncludeChildrenMode && !_MyWrapperContainer_Accessed)
                {
                    var deserialized = MyWrapperContainer;
                }
                WriteChild(ref writer, ref _MyWrapperContainer, includeChildrenMode, _MyWrapperContainer_Accessed, () => GetChildSlice(LazinatorMemoryStorage, _MyWrapperContainer_ByteIndex, _MyWrapperContainer_ByteLength, false, false, null), verifyCleanness, updateStoredBuffer, false, false, this);
            }
            if (updateStoredBuffer)
            {
                _MyWrapperContainer_ByteIndex = startOfObjectPosition - startPosition;
            }
            if (updateStoredBuffer)
            {
                _ExampleChild_EndByteIndex = writer.Position - startPosition;
            }
        }
        
        /* Conversion of supported collections and tuples */
        
        private static void ConvertToBytes_ReadOnlySpan_Gbyte_g(ref BinaryBufferWriter writer, ReadOnlySpan<byte> itemToConvert, IncludeChildrenMode includeChildrenMode, bool verifyCleanness, bool updateStoredBuffer)
        {
            ReadOnlySpan<byte> toConvert = MemoryMarshal.Cast<byte, byte>(itemToConvert);
            for (int i = 0; i < toConvert.Length; i++)
            {
                writer.Write(toConvert[i]);
            }
        }
        private static ReadOnlySpan<byte> Clone_ReadOnlySpan_Gbyte_g(ReadOnlySpan<byte> itemToClone, IncludeChildrenMode includeChildrenMode)
        {
            var clone = new Span<byte>(new byte[itemToClone.Length * sizeof(byte)]);
            itemToClone.CopyTo(clone);
            return clone;
        }
        
    }
}
