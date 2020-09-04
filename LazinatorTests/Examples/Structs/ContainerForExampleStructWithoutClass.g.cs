//395eda8c-81ee-38b9-232a-1e78f479264b
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by the Lazinator tool, version 0.1.0.390
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

#nullable disable
namespace LazinatorTests.Examples.Structs
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
    public partial class ContainerForExampleStructWithoutClass : ILazinator
    {
        public bool IsStruct => false;
        
        /* Property definitions */
        
        protected int _ExampleNullableStruct_ByteIndex;
        protected int _ExampleStructWithoutClass_ByteIndex;
        protected virtual int _ExampleNullableStruct_ByteLength => _ExampleStructWithoutClass_ByteIndex - _ExampleNullableStruct_ByteIndex;
        private int _ContainerForExampleStructWithoutClass_EndByteIndex;
        protected virtual int _ExampleStructWithoutClass_ByteLength => _ContainerForExampleStructWithoutClass_EndByteIndex - _ExampleStructWithoutClass_ByteIndex;
        
        
        protected int _MyInt;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public int MyInt
        {
            get
            {
                return _MyInt;
            }
            set
            {
                IsDirty = true;
                _MyInt = value;
            }
        }
        
        protected ExampleStructWithoutClass? _ExampleNullableStruct;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public ExampleStructWithoutClass? ExampleNullableStruct
        {
            get
            {
                if (!_ExampleNullableStruct_Accessed)
                {
                    Lazinate_ExampleNullableStruct();
                } 
                return _ExampleNullableStruct;
            }
            set
            {
                if (value.HasValue)
                {
                    var copy = value.Value;
                    copy.LazinatorParents = new LazinatorParentsCollection(this);/*Location88*/
                    value = copy;
                }
                
                
                IsDirty = true;
                DescendantIsDirty = true;
                _ExampleNullableStruct = value;
                _ExampleNullableStruct_Accessed = true;
            }
        }
        protected bool _ExampleNullableStruct_Accessed;
        private void Lazinate_ExampleNullableStruct()
        {
            if (LazinatorObjectBytes.Length == 0)
            {
                _ExampleNullableStruct = default(ExampleStructWithoutClass?);/*Location86*/
            }
            else
            {
                LazinatorMemory childData = GetChildSlice(LazinatorMemoryStorage, _ExampleNullableStruct_ByteIndex, _ExampleNullableStruct_ByteLength, false, false, null);
                if (childData.Length == 0)
                {
                    _ExampleNullableStruct = default;
                }
                else 
                {
                    _ExampleNullableStruct = new ExampleStructWithoutClass()
                    {
                        LazinatorParents = new LazinatorParentsCollection(this)/*Location87*/
                    };
                    var copy = _ExampleNullableStruct.Value;
                    copy.DeserializeLazinator(childData);
                    _ExampleNullableStruct = copy;
                }
            }
            
            _ExampleNullableStruct_Accessed = true;
        }
        
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public ExampleStructWithoutClass? ExampleNullableStruct_Copy
        {
            get
            {
                if (!_ExampleNullableStruct_Accessed)
                {
                    if (LazinatorObjectBytes.Length == 0)
                    {
                        return null;
                    }
                    else
                    {
                        LazinatorMemory childData = GetChildSlice(LazinatorMemoryStorage, _ExampleNullableStruct_ByteIndex, _ExampleNullableStruct_ByteLength, false, false, null);
                        var toReturn = new ExampleStructWithoutClass(childData);
                        toReturn.IsDirty = false;
                        return toReturn;
                    }
                }
                
                if (_ExampleNullableStruct == null)
                {
                    return null;
                }
                var cleanCopy = _ExampleNullableStruct.Value;
                cleanCopy.IsDirty = false;
                cleanCopy.DescendantIsDirty = false;
                return cleanCopy;
            }
        }
        
        protected ExampleStructWithoutClass _ExampleStructWithoutClass;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public ExampleStructWithoutClass ExampleStructWithoutClass
        {
            get
            {
                if (!_ExampleStructWithoutClass_Accessed)
                {
                    Lazinate_ExampleStructWithoutClass();
                } 
                return _ExampleStructWithoutClass;
            }
            set
            {
                value.LazinatorParents = new LazinatorParentsCollection(this);/*Location91*/
                
                IsDirty = true;
                DescendantIsDirty = true;
                _ExampleStructWithoutClass = value;
                _ExampleStructWithoutClass_Accessed = true;
            }
        }
        protected bool _ExampleStructWithoutClass_Accessed;
        private void Lazinate_ExampleStructWithoutClass()
        {
            if (LazinatorObjectBytes.Length == 0)
            {
                _ExampleStructWithoutClass = default(ExampleStructWithoutClass);
                _ExampleStructWithoutClass.LazinatorParents = new LazinatorParentsCollection(this);/*Location89*/
            }
            else
            {
                LazinatorMemory childData = GetChildSlice(LazinatorMemoryStorage, _ExampleStructWithoutClass_ByteIndex, _ExampleStructWithoutClass_ByteLength, false, false, null);
                _ExampleStructWithoutClass = new ExampleStructWithoutClass()
                {
                    LazinatorParents = new LazinatorParentsCollection(this)/*Location90*/
                };
                _ExampleStructWithoutClass.DeserializeLazinator(childData);
            }
            
            _ExampleStructWithoutClass_Accessed = true;
        }
        
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public ExampleStructWithoutClass ExampleStructWithoutClass_Copy
        {
            get
            {
                if (!_ExampleStructWithoutClass_Accessed)
                {
                    if (LazinatorObjectBytes.Length == 0)
                    {
                        return default(ExampleStructWithoutClass);
                    }
                    else
                    {
                        LazinatorMemory childData = GetChildSlice(LazinatorMemoryStorage, _ExampleStructWithoutClass_ByteIndex, _ExampleStructWithoutClass_ByteLength, false, false, null);
                        var toReturn = new ExampleStructWithoutClass(childData);
                        toReturn.IsDirty = false;
                        return toReturn;
                    }
                }
                
                var cleanCopy = _ExampleStructWithoutClass;
                cleanCopy.IsDirty = false;
                cleanCopy.DescendantIsDirty = false;
                return cleanCopy;
            }
        }
        
        /* Serialization, deserialization, and object relationships */
        
        public ContainerForExampleStructWithoutClass(IncludeChildrenMode originalIncludeChildrenMode = IncludeChildrenMode.IncludeAllChildren)
        {
            OriginalIncludeChildrenMode = originalIncludeChildrenMode;
        }
        
        public ContainerForExampleStructWithoutClass(LazinatorMemory serializedBytes, ILazinator parent = null)
        {
            LazinatorParents = new LazinatorParentsCollection(parent);
            DeserializeLazinator(serializedBytes);
            HasChanged = false;
            DescendantHasChanged = false;
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
            
            ReadGenericIDIfApplicable(ContainsOpenGenericParameters, LazinatorUniqueID, span, ref bytesSoFar);
            
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
            ContainerForExampleStructWithoutClass clone;
            if (cloneBufferOptions == CloneBufferOptions.NoBuffer)
            {
                clone = new ContainerForExampleStructWithoutClass(includeChildrenMode);
                clone.LazinatorObjectVersion = LazinatorObjectVersion;
                clone = (ContainerForExampleStructWithoutClass)AssignCloneProperties(clone, includeChildrenMode);
            }
            else
            {
                LazinatorMemory bytes = EncodeOrRecycleToNewBuffer(includeChildrenMode, OriginalIncludeChildrenMode, false, IsDirty, DescendantIsDirty, false, LazinatorMemoryStorage, false, this);
                clone = new ContainerForExampleStructWithoutClass(bytes);
            }
            return clone;
        }
        
        public virtual ILazinator AssignCloneProperties(ILazinator clone, IncludeChildrenMode includeChildrenMode)
        {
            clone.FreeInMemoryObjects();
            ContainerForExampleStructWithoutClass typedClone = (ContainerForExampleStructWithoutClass) clone;
            typedClone.MyInt = MyInt;/*Location92*/
            if (includeChildrenMode != IncludeChildrenMode.ExcludeAllChildren && includeChildrenMode != IncludeChildrenMode.IncludeOnlyIncludableChildren)
            {
                if (ExampleNullableStruct == null)
                {
                    typedClone.ExampleNullableStruct = null;/*Location94*//*Location95*/
                }
                else
                {
                    typedClone.ExampleNullableStruct = (ExampleStructWithoutClass?) ExampleNullableStruct.Value.CloneLazinator(includeChildrenMode, CloneBufferOptions.NoBuffer);/*Location93*/
                }
                
            }
            
            if (includeChildrenMode != IncludeChildrenMode.ExcludeAllChildren && includeChildrenMode != IncludeChildrenMode.IncludeOnlyIncludableChildren)
            {
                typedClone.ExampleStructWithoutClass = (ExampleStructWithoutClass) ExampleStructWithoutClass.CloneLazinator(includeChildrenMode, CloneBufferOptions.NoBuffer);/*Location96*/
            }
            
            
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
            if (enumerateNulls && (!exploreOnlyDeserializedChildren || _ExampleNullableStruct_Accessed) && ExampleNullableStruct == null)
            {
                yield return ("ExampleNullableStruct", default);
            }
            else
            {
                if ((!exploreOnlyDeserializedChildren && ExampleNullableStruct != null) || (_ExampleNullableStruct_Accessed && _ExampleNullableStruct != null))
                {
                    bool isMatch_ExampleNullableStruct = matchCriterion == null || matchCriterion(ExampleNullableStruct);
                    bool shouldExplore_ExampleNullableStruct = exploreCriterion == null || exploreCriterion(ExampleNullableStruct);
                    if (isMatch_ExampleNullableStruct)
                    {
                        yield return ("ExampleNullableStruct", ExampleNullableStruct);
                    }
                    if ((!stopExploringBelowMatch || !isMatch_ExampleNullableStruct) && shouldExplore_ExampleNullableStruct)
                    {
                        foreach (var toYield in ExampleNullableStruct.Value.EnumerateLazinatorDescendants(matchCriterion, stopExploringBelowMatch, exploreCriterion, exploreOnlyDeserializedChildren, enumerateNulls))
                        {
                            yield return ("ExampleNullableStruct" + "." + toYield.propertyName, toYield.descendant);
                        }
                    }
                }
                
            }
            
            bool isMatch_ExampleStructWithoutClass = matchCriterion == null || matchCriterion(ExampleStructWithoutClass);
            bool shouldExplore_ExampleStructWithoutClass = exploreCriterion == null || exploreCriterion(ExampleStructWithoutClass);
            if (isMatch_ExampleStructWithoutClass)
            {
                yield return ("ExampleStructWithoutClass", ExampleStructWithoutClass);
            }
            if ((!stopExploringBelowMatch || !isMatch_ExampleStructWithoutClass) && shouldExplore_ExampleStructWithoutClass)
            {
                foreach (var toYield in ExampleStructWithoutClass.EnumerateLazinatorDescendants(matchCriterion, stopExploringBelowMatch, exploreCriterion, exploreOnlyDeserializedChildren, enumerateNulls))
                {
                    yield return ("ExampleStructWithoutClass" + "." + toYield.propertyName, toYield.descendant);
                }
            }
            yield break;
        }
        
        
        public virtual IEnumerable<(string propertyName, object descendant)> EnumerateNonLazinatorProperties()
        {
            yield return ("MyInt", (object)MyInt);
            yield break;
        }
        
        public virtual ILazinator ForEachLazinator(Func<ILazinator, ILazinator> changeFunc, bool exploreOnlyDeserializedChildren, bool changeThisLevel)
        {
            if ((!exploreOnlyDeserializedChildren && ExampleNullableStruct != null) || (_ExampleNullableStruct_Accessed && _ExampleNullableStruct != null))
            {
                _ExampleNullableStruct = (ExampleStructWithoutClass?) _ExampleNullableStruct.Value.ForEachLazinator(changeFunc, exploreOnlyDeserializedChildren, true);
            }
            var deserialized_ExampleStructWithoutClass = ExampleStructWithoutClass;
            _ExampleStructWithoutClass = (ExampleStructWithoutClass) _ExampleStructWithoutClass.ForEachLazinator(changeFunc, exploreOnlyDeserializedChildren, true);if (changeThisLevel && changeFunc != null)
            {
                return changeFunc(this);
            }
            return this;
        }
        
        public virtual void FreeInMemoryObjects()
        {
            _ExampleNullableStruct = default;
            _ExampleStructWithoutClass = default;
            _ExampleNullableStruct_Accessed = _ExampleStructWithoutClass_Accessed = false;
            IsDirty = false;
            DescendantIsDirty = false;
            HasChanged = false;
            DescendantHasChanged = false;
        }
        
        /* Conversion */
        
        public virtual int LazinatorUniqueID => 1054;
        
        protected virtual bool ContainsOpenGenericParameters => false;
        public virtual LazinatorGenericIDType LazinatorGenericID => default;
        
        
        public virtual int LazinatorObjectVersion { get; set; } = 0;
        
        
        public virtual void ConvertFromBytesAfterHeader(IncludeChildrenMode includeChildrenMode, int serializedVersionNumber, ref int bytesSoFar)
        {
            ReadOnlySpan<byte> span = LazinatorObjectBytes.Span;
            _MyInt = span.ToDecompressedInt(ref bytesSoFar);
            _ExampleNullableStruct_ByteIndex = bytesSoFar;
            if (includeChildrenMode != IncludeChildrenMode.ExcludeAllChildren && includeChildrenMode != IncludeChildrenMode.IncludeOnlyIncludableChildren)
            {
                bytesSoFar = span.ToInt32(ref bytesSoFar) + bytesSoFar;
            }
            
            _ExampleStructWithoutClass_ByteIndex = bytesSoFar;
            if (includeChildrenMode != IncludeChildrenMode.ExcludeAllChildren && includeChildrenMode != IncludeChildrenMode.IncludeOnlyIncludableChildren)
            {
                bytesSoFar = span.ToInt32(ref bytesSoFar) + bytesSoFar;
            }
            
            _ContainerForExampleStructWithoutClass_EndByteIndex = bytesSoFar;
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
                
                _ExampleNullableStruct_Accessed = false;
                _ExampleStructWithoutClass_Accessed = false;
            }
            else
            {
                ThrowHelper.ThrowCannotUpdateStoredBuffer();
            }
            
            var newBuffer = writer.Slice(startPosition, length);
            LazinatorMemoryStorage = newBuffer;
        }
        
        protected virtual void UpdateDeserializedChildren(ref BinaryBufferWriter writer, int startPosition)
        {
            if (_ExampleNullableStruct_Accessed && _ExampleNullableStruct != null)
            {
                ExampleNullableStruct.Value.UpdateStoredBuffer(ref writer, startPosition + _ExampleNullableStruct_ByteIndex + sizeof(int), _ExampleNullableStruct_ByteLength - sizeof(int), IncludeChildrenMode.IncludeAllChildren, true);
            }
            
            ExampleStructWithoutClass.UpdateStoredBuffer(ref writer, startPosition + _ExampleStructWithoutClass_ByteIndex + sizeof(int), _ExampleStructWithoutClass_ByteLength - sizeof(int), IncludeChildrenMode.IncludeAllChildren, true);
        }
        
        
        protected virtual void WritePropertiesIntoBuffer(ref BinaryBufferWriter writer, IncludeChildrenMode includeChildrenMode, bool verifyCleanness, bool updateStoredBuffer, bool includeUniqueID)
        {
            int startPosition = writer.Position;
            int startOfObjectPosition = 0;
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
            CompressedIntegralTypes.WriteCompressedInt(ref writer, _MyInt);
            startOfObjectPosition = writer.Position;
            if (includeChildrenMode != IncludeChildrenMode.ExcludeAllChildren && includeChildrenMode != IncludeChildrenMode.IncludeOnlyIncludableChildren)
            {
                if ((includeChildrenMode != IncludeChildrenMode.IncludeAllChildren || includeChildrenMode != OriginalIncludeChildrenMode) && !_ExampleNullableStruct_Accessed)
                {
                    var deserialized = ExampleNullableStruct;
                }
                if (_ExampleNullableStruct == null)
                {
                    WriteNullChild(ref writer, false, false);
                }
                else
                {
                    var copy = _ExampleNullableStruct.Value;
                    WriteChild(ref writer, ref copy, includeChildrenMode, _ExampleNullableStruct_Accessed, () => GetChildSlice(LazinatorMemoryStorage, _ExampleNullableStruct_ByteIndex, _ExampleNullableStruct_ByteLength, false, false, null), verifyCleanness, updateStoredBuffer, false, false, this);
                    _ExampleNullableStruct = copy;
                }
                
            }
            
            if (updateStoredBuffer)
            {
                _ExampleNullableStruct_ByteIndex = startOfObjectPosition - startPosition;
            }
            startOfObjectPosition = writer.Position;
            if (includeChildrenMode != IncludeChildrenMode.ExcludeAllChildren && includeChildrenMode != IncludeChildrenMode.IncludeOnlyIncludableChildren)
            {
                if ((includeChildrenMode != IncludeChildrenMode.IncludeAllChildren || includeChildrenMode != OriginalIncludeChildrenMode) && !_ExampleStructWithoutClass_Accessed)
                {
                    var deserialized = ExampleStructWithoutClass;
                }
                WriteChild(ref writer, ref _ExampleStructWithoutClass, includeChildrenMode, _ExampleStructWithoutClass_Accessed, () => GetChildSlice(LazinatorMemoryStorage, _ExampleStructWithoutClass_ByteIndex, _ExampleStructWithoutClass_ByteLength, false, false, null), verifyCleanness, updateStoredBuffer, false, false, this);
            }
            
            if (updateStoredBuffer)
            {
                _ExampleStructWithoutClass_ByteIndex = startOfObjectPosition - startPosition;
            }
            if (updateStoredBuffer)
            {
                _ContainerForExampleStructWithoutClass_EndByteIndex = writer.Position - startPosition;
            }
        }
        
    }
}
