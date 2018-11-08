//b81d85ae-afd9-a3a8-5635-4a9587321306
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by the Lazinator tool, version 0.1.0.283
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace LazinatorTests.Examples.Hierarchy
{
    using Lazinator.Attributes;
    using Lazinator.Buffers;
    using Lazinator.Core;
    using Lazinator.Exceptions;
    using Lazinator.Support;
    using LazinatorTests.Examples;
    using System;
    using System.Buffers;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Runtime.InteropServices;
    using static Lazinator.Core.LazinatorUtilities;
    
    [Autogenerated]
    public partial class ExampleInterfaceContainer : ILazinator
    {
        public bool IsStruct => false;
        
        /* Serialization, deserialization, and object relationships */
        
        public ExampleInterfaceContainer() : base()
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
            return EncodeOrRecycleToNewBuffer(includeChildrenMode, OriginalIncludeChildrenMode, verifyCleanness, IsDirty, DescendantIsDirty, false, LazinatorMemoryStorage, (EncodeManuallyDelegate) EncodeToNewBuffer, updateStoredBuffer);
        }
        
        protected virtual LazinatorMemory EncodeToNewBuffer(IncludeChildrenMode includeChildrenMode, bool verifyCleanness, bool updateStoredBuffer) => LazinatorUtilities.EncodeToNewBinaryBufferWriter(this, includeChildrenMode, verifyCleanness, updateStoredBuffer);
        
        public virtual ILazinator CloneLazinator(IncludeChildrenMode includeChildrenMode = IncludeChildrenMode.IncludeAllChildren, CloneBufferOptions cloneBufferOptions = CloneBufferOptions.LinkedBuffer)
        {
            var clone = new ExampleInterfaceContainer()
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
            ExampleInterfaceContainer typedClone = (ExampleInterfaceContainer) clone;
            if (includeChildrenMode != IncludeChildrenMode.ExcludeAllChildren) 
            {
                typedClone.ExampleByInterface = (ExampleByInterface == null) ? default(IExample) : (IExample) ExampleByInterface.CloneLazinator(includeChildrenMode, CloneBufferOptions.NoBuffer);
            }
            typedClone.ExampleListByInterface = Clone_List_GIExample_g(ExampleListByInterface, l => l?.CloneLazinator(includeChildrenMode, CloneBufferOptions.NoBuffer));
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
        
        protected int _ExampleByInterface_ByteIndex;
        protected int _ExampleListByInterface_ByteIndex;
        protected virtual int _ExampleByInterface_ByteLength => _ExampleListByInterface_ByteIndex - _ExampleByInterface_ByteIndex;
        private int _ExampleInterfaceContainer_EndByteIndex;
        protected virtual int _ExampleListByInterface_ByteLength => _ExampleInterfaceContainer_EndByteIndex - _ExampleListByInterface_ByteIndex;
        
        
        protected IExample _ExampleByInterface;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public IExample ExampleByInterface
        {
            get
            {
                if (!_ExampleByInterface_Accessed)
                {
                    if (LazinatorObjectBytes.Length == 0)
                    {
                        _ExampleByInterface = default(IExample);
                    }
                    else
                    {
                        LazinatorMemory childData = GetChildSlice(LazinatorMemoryStorage, _ExampleByInterface_ByteIndex, _ExampleByInterface_ByteLength, false, false, null);
                        
                        _ExampleByInterface = DeserializationFactory.Instance.CreateBasedOnType<IExample>(childData, this); 
                    }
                    _ExampleByInterface_Accessed = true;
                } 
                return _ExampleByInterface;
            }
            set
            {
                if (_ExampleByInterface != null)
                {
                    _ExampleByInterface.LazinatorParents = _ExampleByInterface.LazinatorParents.WithRemoved(this);
                }
                if (value != null)
                {
                    value.LazinatorParents = value.LazinatorParents.WithAdded(this);
                }
                
                IsDirty = true;
                DescendantIsDirty = true;
                _ExampleByInterface = value;
                _ExampleByInterface_Accessed = true;
            }
        }
        protected bool _ExampleByInterface_Accessed;
        
        protected List<IExample> _ExampleListByInterface;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public List<IExample> ExampleListByInterface
        {
            get
            {
                if (!_ExampleListByInterface_Accessed)
                {
                    if (LazinatorObjectBytes.Length == 0)
                    {
                        _ExampleListByInterface = default(List<IExample>);
                    }
                    else
                    {
                        LazinatorMemory childData = GetChildSlice(LazinatorMemoryStorage, _ExampleListByInterface_ByteIndex, _ExampleListByInterface_ByteLength, false, false, null);
                        _ExampleListByInterface = ConvertFromBytes_List_GIExample_g(childData);
                    }
                    _ExampleListByInterface_Accessed = true;
                }
                IsDirty = true; 
                return _ExampleListByInterface;
            }
            set
            {
                IsDirty = true;
                DescendantIsDirty = true;
                _ExampleListByInterface = value;
                _ExampleListByInterface_Accessed = true;
            }
        }
        protected bool _ExampleListByInterface_Accessed;
        
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
            if (enumerateNulls && (!exploreOnlyDeserializedChildren || _ExampleByInterface_Accessed) && (ExampleByInterface == null))
            {
                yield return ("ExampleByInterface", default);
            }
            else if ((!exploreOnlyDeserializedChildren && ExampleByInterface != null) || (_ExampleByInterface_Accessed && _ExampleByInterface != null))
            {
                bool isMatch = matchCriterion == null || matchCriterion(ExampleByInterface);
                bool shouldExplore = exploreCriterion == null || exploreCriterion(ExampleByInterface);
                if (isMatch)
                {
                    yield return ("ExampleByInterface", ExampleByInterface);
                }
                if ((!stopExploringBelowMatch || !isMatch) && shouldExplore)
                {
                    foreach (var toYield in ExampleByInterface.EnumerateLazinatorDescendants(matchCriterion, stopExploringBelowMatch, exploreCriterion, exploreOnlyDeserializedChildren, enumerateNulls))
                    {
                        yield return ("ExampleByInterface" + "." + toYield.propertyName, toYield.descendant);
                    }
                }
            }
            yield break;
        }
        
        
        public virtual IEnumerable<(string propertyName, object descendant)> EnumerateNonLazinatorProperties()
        {
            yield return ("ExampleListByInterface", (object)ExampleListByInterface);
            yield break;
        }
        
        public virtual ILazinator ForEachLazinator(Func<ILazinator, ILazinator> changeFunc, bool exploreOnlyDeserializedChildren)
        {
            if ((!exploreOnlyDeserializedChildren && ExampleByInterface != null) || (_ExampleByInterface_Accessed && _ExampleByInterface != null))
            {
                _ExampleByInterface = (IExample) _ExampleByInterface.ForEachLazinator(changeFunc, exploreOnlyDeserializedChildren);
            }
            if ((!exploreOnlyDeserializedChildren && ExampleListByInterface != null) || (_ExampleListByInterface_Accessed && _ExampleListByInterface != null))
            {
                _ExampleListByInterface = (List<IExample>) Clone_List_GIExample_g(_ExampleListByInterface, l => l.ForEachLazinator(changeFunc, exploreOnlyDeserializedChildren));
            }
            return changeFunc(this);
        }
        
        public virtual void FreeInMemoryObjects()
        {
            _ExampleByInterface = default;
            _ExampleListByInterface = default;
            _ExampleByInterface_Accessed = _ExampleListByInterface_Accessed = false;
            IsDirty = false;
            DescendantIsDirty = false;
            HasChanged = false;
            DescendantHasChanged = false;
        }
        
        /* Conversion */
        
        public virtual int LazinatorUniqueID => 246;
        
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
            _ExampleByInterface_ByteIndex = bytesSoFar;
            if (includeChildrenMode != IncludeChildrenMode.ExcludeAllChildren) 
            {
                bytesSoFar = span.ToInt32(ref bytesSoFar) + bytesSoFar;
            }
            _ExampleListByInterface_ByteIndex = bytesSoFar;
            bytesSoFar = span.ToInt32(ref bytesSoFar) + bytesSoFar;
            _ExampleInterfaceContainer_EndByteIndex = bytesSoFar;
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
                    if (_ExampleByInterface_Accessed && _ExampleByInterface != null)
                    {
                        _ExampleByInterface.UpdateStoredBuffer(ref writer, startPosition + _ExampleByInterface_ByteIndex + sizeof(int), _ExampleByInterface_ByteLength - sizeof(int), IncludeChildrenMode.IncludeAllChildren, true);
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
            startOfObjectPosition = writer.Position;
            if (includeChildrenMode != IncludeChildrenMode.ExcludeAllChildren) 
            {
                if (includeChildrenMode != IncludeChildrenMode.IncludeAllChildren || includeChildrenMode != OriginalIncludeChildrenMode && !_ExampleByInterface_Accessed)
                {
                    var deserialized = ExampleByInterface;
                }
                WriteChild(ref writer, ref _ExampleByInterface, includeChildrenMode, _ExampleByInterface_Accessed, () => GetChildSlice(LazinatorMemoryStorage, _ExampleByInterface_ByteIndex, _ExampleByInterface_ByteLength, false, false, null), verifyCleanness, updateStoredBuffer, false, false, this);
            }
            if (updateStoredBuffer)
            {
                _ExampleByInterface_ByteIndex = startOfObjectPosition - startPosition;
            }
            startOfObjectPosition = writer.Position;
            if (includeChildrenMode != IncludeChildrenMode.IncludeAllChildren || includeChildrenMode != OriginalIncludeChildrenMode && !_ExampleListByInterface_Accessed)
            {
                var deserialized = ExampleListByInterface;
            }
            WriteNonLazinatorObject(
            nonLazinatorObject: _ExampleListByInterface, isBelievedDirty: _ExampleListByInterface_Accessed || (includeChildrenMode != OriginalIncludeChildrenMode),
            isAccessed: _ExampleListByInterface_Accessed, writer: ref writer,
            getChildSliceForFieldFn: () => GetChildSlice(LazinatorMemoryStorage, _ExampleListByInterface_ByteIndex, _ExampleListByInterface_ByteLength, false, false, null),
            verifyCleanness: false,
            binaryWriterAction: (ref BinaryBufferWriter w, bool v) =>
            ConvertToBytes_List_GIExample_g(ref w, _ExampleListByInterface,
            includeChildrenMode, v, updateStoredBuffer));
            if (updateStoredBuffer)
            {
                _ExampleListByInterface_ByteIndex = startOfObjectPosition - startPosition;
            }
            if (updateStoredBuffer)
            {
                _ExampleInterfaceContainer_EndByteIndex = writer.Position - startPosition;
            }
        }
        
        /* Conversion of supported collections and tuples */
        
        private static List<IExample> ConvertFromBytes_List_GIExample_g(LazinatorMemory storage)
        {
            if (storage.Length == 0)
            {
                return default(List<IExample>);
            }
            storage.LazinatorShouldNotReturnToPool();
            ReadOnlySpan<byte> span = storage.Span;
            
            int bytesSoFar = 0;
            int collectionLength = span.ToDecompressedInt(ref bytesSoFar);
            
            List<IExample> collection = new List<IExample>(collectionLength);
            for (int itemIndex = 0; itemIndex < collectionLength; itemIndex++)
            {
                int lengthCollectionMember = span.ToInt32(ref bytesSoFar);
                if (lengthCollectionMember == 0)
                {
                    collection.Add(default(IExample));
                }
                else
                {
                    LazinatorMemory childData = storage.Slice(bytesSoFar, lengthCollectionMember);
                    var item = DeserializationFactory.Instance.CreateBasedOnType<IExample>(childData);
                    collection.Add(item);
                }
                bytesSoFar += lengthCollectionMember;
            }
            
            return collection;
        }
        
        private static void ConvertToBytes_List_GIExample_g(ref BinaryBufferWriter writer, List<IExample> itemToConvert, IncludeChildrenMode includeChildrenMode, bool verifyCleanness, bool updateStoredBuffer)
        {
            if (itemToConvert == default(List<IExample>))
            {
                return;
            }
            CompressedIntegralTypes.WriteCompressedInt(ref writer, itemToConvert.Count);
            int itemToConvertCount = itemToConvert.Count;
            for (int itemIndex = 0; itemIndex < itemToConvertCount; itemIndex++)
            {
                if (itemToConvert[itemIndex] == default(IExample))
                {
                    writer.Write((uint)0);
                }
                else 
                {
                    
                    void action(ref BinaryBufferWriter w) => itemToConvert[itemIndex].SerializeExistingBuffer(ref w, includeChildrenMode, verifyCleanness, updateStoredBuffer);
                    WriteToBinaryWithIntLengthPrefix(ref writer, action);
                }
                
            }
        }
        
        private static List<IExample> Clone_List_GIExample_g(List<IExample> itemToClone, Func<ILazinator, ILazinator> cloneOrChangeFunc)
        {
            if (itemToClone == null)
            {
                return default;
            }
            
            int collectionLength = itemToClone.Count;
            List<IExample> collection = new List<IExample>(collectionLength);
            int itemToCloneCount = itemToClone.Count;
            for (int itemIndex = 0; itemIndex < itemToCloneCount; itemIndex++)
            {
                if (itemToClone[itemIndex] == null)
                {
                    collection.Add(default(IExample));
                }
                else
                {
                    var itemCopied = (IExample) cloneOrChangeFunc(itemToClone[itemIndex]);
                    collection.Add(itemCopied);
                }
            }
            return collection;
        }
        
    }
}
