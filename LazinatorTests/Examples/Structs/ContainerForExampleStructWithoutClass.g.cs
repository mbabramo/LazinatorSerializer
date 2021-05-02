//fb849050-3d9f-8440-97b5-d63dd8534c00
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by the Lazinator tool, version 0.1.0.393
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
    using static Lazinator.Buffers.WriteUncompressedPrimitives;
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
        protected virtual  int _ExampleStructWithoutClass_ByteLength => _ContainerForExampleStructWithoutClass_EndByteIndex - _ExampleStructWithoutClass_ByteIndex;
        protected virtual int _OverallEndByteIndex => _ContainerForExampleStructWithoutClass_EndByteIndex;
        
        
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
                    LazinateExampleNullableStruct();
                } 
                return _ExampleNullableStruct;
            }
            set
            {
                if (value.HasValue)
                {
                    var copy = value.Value;
                    copy.LazinatorParents = new LazinatorParentsCollection(this);
                    value = copy;
                }
                
                
                IsDirty = true;
                DescendantIsDirty = true;
                _ExampleNullableStruct = value;
                _ExampleNullableStruct_Accessed = true;
            }
        }
        protected bool _ExampleNullableStruct_Accessed;
        private void LazinateExampleNullableStruct()
        {
            if (LazinatorMemoryStorage.Length == 0)
            {
                _ExampleNullableStruct = default(ExampleStructWithoutClass?);
            }
            else
            {
                LazinatorMemory childData = GetChildSlice(LazinatorMemoryStorage, _ExampleNullableStruct_ByteIndex, _ExampleNullableStruct_ByteLength, null);if (childData.Length == 0)
                {
                    _ExampleNullableStruct = default;
                }
                else 
                {
                    _ExampleNullableStruct = new ExampleStructWithoutClass(childData)
                    {
                        LazinatorParents = new LazinatorParentsCollection(this)
                    };
                    
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
                    if (LazinatorMemoryStorage.Length == 0)
                    {
                        return null;
                    }
                    else
                    {
                        LazinatorMemory childData = GetChildSlice(LazinatorMemoryStorage, _ExampleNullableStruct_ByteIndex, _ExampleNullableStruct_ByteLength, null);
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
                    LazinateExampleStructWithoutClass();
                } 
                return _ExampleStructWithoutClass;
            }
            set
            {
                value.LazinatorParents = new LazinatorParentsCollection(this);
                
                IsDirty = true;
                DescendantIsDirty = true;
                _ExampleStructWithoutClass = value;
                _ExampleStructWithoutClass_Accessed = true;
            }
        }
        protected bool _ExampleStructWithoutClass_Accessed;
        private void LazinateExampleStructWithoutClass()
        {
            if (LazinatorMemoryStorage.Length == 0)
            {
                _ExampleStructWithoutClass = default(ExampleStructWithoutClass);
                _ExampleStructWithoutClass.LazinatorParents = new LazinatorParentsCollection(this);
            }
            else
            {
                LazinatorMemory childData = GetChildSlice(LazinatorMemoryStorage, _ExampleStructWithoutClass_ByteIndex, _ExampleStructWithoutClass_ByteLength, null);_ExampleStructWithoutClass = new ExampleStructWithoutClass(childData)
                {
                    LazinatorParents = new LazinatorParentsCollection(this)
                };
                
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
                    if (LazinatorMemoryStorage.Length == 0)
                    {
                        return default(ExampleStructWithoutClass);
                    }
                    else
                    {
                        LazinatorMemory childData = GetChildSlice(LazinatorMemoryStorage, _ExampleStructWithoutClass_ByteIndex, _ExampleStructWithoutClass_ByteLength, null);
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
        
        public ContainerForExampleStructWithoutClass(LazinatorMemory serializedBytes, ILazinator parent = null, IncludeChildrenMode originalIncludeChildrenMode = IncludeChildrenMode.IncludeAllChildren, int? lazinatorObjectVersion = null)
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
            ReadOnlySpan<byte> span = LazinatorMemoryStorage.InitialReadOnlyMemory.Span;
            if (span.Length == 0)
            {
                return 0;
            }
            
            ReadGenericIDIfApplicable(ContainsOpenGenericParameters, LazinatorUniqueID, span, ref bytesSoFar);
            
            int lazinatorLibraryVersion = span.ToDecompressedInt32(ref bytesSoFar);
            
            int serializedVersionNumber = span.ToDecompressedInt32(ref bytesSoFar);
            
            OriginalIncludeChildrenMode = (IncludeChildrenMode)span.ToByte(ref bytesSoFar);
            
            int totalBytes = ConvertFromBytesAfterHeader(OriginalIncludeChildrenMode, serializedVersionNumber, ref bytesSoFar);
            return _OverallEndByteIndex;
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
            ContainerForExampleStructWithoutClass clone;
            if (cloneBufferOptions == CloneBufferOptions.NoBuffer)
            {
                clone = new ContainerForExampleStructWithoutClass(includeChildrenMode);
                clone.LazinatorObjectVersion = LazinatorObjectVersion;
                clone = (ContainerForExampleStructWithoutClass)AssignCloneProperties(clone, includeChildrenMode);
            }
            else
            {
                LazinatorMemory bytes = EncodeOrRecycleToNewBuffer(includeChildrenMode, OriginalIncludeChildrenMode, IsDirty, DescendantIsDirty, false, LazinatorMemoryStorage, this);
                clone = new ContainerForExampleStructWithoutClass(bytes);
            }
            return clone;
        }
        
        protected virtual ILazinator AssignCloneProperties(ILazinator clone, IncludeChildrenMode includeChildrenMode)
        {
            clone.FreeInMemoryObjects();
            ContainerForExampleStructWithoutClass typedClone = (ContainerForExampleStructWithoutClass) clone;
            typedClone.MyInt = MyInt;
            if (includeChildrenMode != IncludeChildrenMode.ExcludeAllChildren && includeChildrenMode != IncludeChildrenMode.IncludeOnlyIncludableChildren)
            {
                if (ExampleNullableStruct == null)
                {
                    typedClone.ExampleNullableStruct = null;
                }
                else
                {
                    typedClone.ExampleNullableStruct = (ExampleStructWithoutClass?) ExampleNullableStruct.Value.CloneLazinator(includeChildrenMode, CloneBufferOptions.NoBuffer);
                }
            }
            if (includeChildrenMode != IncludeChildrenMode.ExcludeAllChildren && includeChildrenMode != IncludeChildrenMode.IncludeOnlyIncludableChildren)
            {
                typedClone.ExampleStructWithoutClass = (ExampleStructWithoutClass) ExampleStructWithoutClass.CloneLazinator(includeChildrenMode, CloneBufferOptions.NoBuffer);
            }
            
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
            _ExampleStructWithoutClass = (ExampleStructWithoutClass) _ExampleStructWithoutClass.ForEachLazinator(changeFunc, exploreOnlyDeserializedChildren, true);
            if (changeThisLevel && changeFunc != null)
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
        
        
        protected virtual int ConvertFromBytesAfterHeader(IncludeChildrenMode includeChildrenMode, int serializedVersionNumber, ref int bytesSoFar)
        {
            ReadOnlySpan<byte> span = LazinatorMemoryStorage.InitialReadOnlyMemory.Span;
            ConvertFromBytesForPrimitiveProperties(span, includeChildrenMode, serializedVersionNumber, ref bytesSoFar);
            int lengthForLengths = 0;
            if (includeChildrenMode != IncludeChildrenMode.ExcludeAllChildren && includeChildrenMode != IncludeChildrenMode.IncludeOnlyIncludableChildren)
            {
                lengthForLengths += 8;
            }
            int totalChildrenSize = ConvertFromBytesForChildProperties(span, includeChildrenMode, serializedVersionNumber, bytesSoFar + lengthForLengths, ref bytesSoFar);
            return bytesSoFar + totalChildrenSize;
        }
        
        protected virtual void ConvertFromBytesForPrimitiveProperties(ReadOnlySpan<byte> span, IncludeChildrenMode includeChildrenMode, int serializedVersionNumber, ref int bytesSoFar)
        {
            _MyInt = span.ToDecompressedInt32(ref bytesSoFar);
        }
        
        protected virtual int ConvertFromBytesForChildProperties(ReadOnlySpan<byte> span, IncludeChildrenMode includeChildrenMode, int serializedVersionNumber, int indexOfFirstChild, ref int bytesSoFar)
        {
            int totalChildrenBytes = 0;
            _ExampleNullableStruct_ByteIndex = indexOfFirstChild + totalChildrenBytes;
            if (includeChildrenMode != IncludeChildrenMode.ExcludeAllChildren && includeChildrenMode != IncludeChildrenMode.IncludeOnlyIncludableChildren)
            {
                totalChildrenBytes += span.ToInt32(ref bytesSoFar);
            }
            _ExampleStructWithoutClass_ByteIndex = indexOfFirstChild + totalChildrenBytes;
            if (includeChildrenMode != IncludeChildrenMode.ExcludeAllChildren && includeChildrenMode != IncludeChildrenMode.IncludeOnlyIncludableChildren)
            {
                totalChildrenBytes += span.ToInt32(ref bytesSoFar);
            }
            _ContainerForExampleStructWithoutClass_EndByteIndex = indexOfFirstChild + totalChildrenBytes;
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
        
        protected virtual void UpdateDeserializedChildren(ref BufferWriter writer, long startPosition)
        {
            if (_ExampleNullableStruct_Accessed && _ExampleNullableStruct != null)
            {
                ExampleNullableStruct.Value.UpdateStoredBuffer(ref writer, startPosition + _ExampleNullableStruct_ByteIndex, _ExampleNullableStruct_ByteLength, IncludeChildrenMode.IncludeAllChildren, true);
            }
            ExampleStructWithoutClass.UpdateStoredBuffer(ref writer, startPosition + _ExampleStructWithoutClass_ByteIndex, _ExampleStructWithoutClass_ByteLength, IncludeChildrenMode.IncludeAllChildren, true);
            
        }
        
        
        protected virtual void WritePropertiesIntoBuffer(ref BufferWriter writer, in LazinatorSerializationOptions options, bool includeUniqueID)
        {
            int startPosition = writer.ActiveMemoryPosition;
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
            int lengthForLengths = 0;
            if (options.IncludeChildrenMode != IncludeChildrenMode.ExcludeAllChildren && options.IncludeChildrenMode != IncludeChildrenMode.IncludeOnlyIncludableChildren)
            {
                lengthForLengths += 8;
            }
            
            var previousLengthsPosition = writer.SetLengthsPosition(lengthForLengths);
            WriteChildrenPropertiesIntoBuffer(ref writer, options, includeUniqueID, startPosition);
            writer.ResetLengthsPosition(previousLengthsPosition);
            
        }
        
        protected virtual void WritePrimitivePropertiesIntoBuffer(ref BufferWriter writer, in LazinatorSerializationOptions options, bool includeUniqueID)
        {
            CompressedIntegralTypes.WriteCompressedInt(ref writer, _MyInt);
        }
        protected virtual void WriteChildrenPropertiesIntoBuffer(ref BufferWriter writer, LazinatorSerializationOptions options, bool includeUniqueID, int startOfObjectPosition)
        {
            if (options.SplittingPossible)
            {
                options = options.WithoutSplittingPossible();
            }
            int startOfChildPosition = 0;
            int lengthValue = 0;
            startOfChildPosition = writer.ActiveMemoryPosition;
            if (options.IncludeChildrenMode != IncludeChildrenMode.ExcludeAllChildren && options.IncludeChildrenMode != IncludeChildrenMode.IncludeOnlyIncludableChildren)
            {
                if ((options.IncludeChildrenMode != IncludeChildrenMode.IncludeAllChildren || options.IncludeChildrenMode != OriginalIncludeChildrenMode) && !_ExampleNullableStruct_Accessed)
                {
                    var deserialized = ExampleNullableStruct;
                }
                if (_ExampleNullableStruct == null)
                {
                    WriteNullChild_LengthsSeparate(ref writer, false);
                }
                else
                {
                    var copy = _ExampleNullableStruct.Value;
                    WriteChild(ref writer, ref copy, options, _ExampleNullableStruct_Accessed, () => GetChildSlice(LazinatorMemoryStorage, _ExampleNullableStruct_ByteIndex, _ExampleNullableStruct_ByteLength, null), this);
                    _ExampleNullableStruct = copy;
                    lengthValue = writer.ActiveMemoryPosition - startOfChildPosition;
                    if (lengthValue > int.MaxValue)
                    {
                        ThrowHelper.ThrowTooLargeException(int.MaxValue);
                    }
                    writer.RecordLength((int) lengthValue);
                }
            }
            if (options.UpdateStoredBuffer)
            {
                _ExampleNullableStruct_ByteIndex = startOfChildPosition - startOfObjectPosition;
                
            }
            startOfChildPosition = writer.ActiveMemoryPosition;
            if (options.IncludeChildrenMode != IncludeChildrenMode.ExcludeAllChildren && options.IncludeChildrenMode != IncludeChildrenMode.IncludeOnlyIncludableChildren)
            {
                if ((options.IncludeChildrenMode != IncludeChildrenMode.IncludeAllChildren || options.IncludeChildrenMode != OriginalIncludeChildrenMode) && !_ExampleStructWithoutClass_Accessed)
                {
                    var deserialized = ExampleStructWithoutClass;
                }
                WriteChild(ref writer, ref _ExampleStructWithoutClass, options, _ExampleStructWithoutClass_Accessed, () => GetChildSlice(LazinatorMemoryStorage, _ExampleStructWithoutClass_ByteIndex, _ExampleStructWithoutClass_ByteLength, null), this);
                lengthValue = writer.ActiveMemoryPosition - startOfChildPosition;
                if (lengthValue > int.MaxValue)
                {
                    ThrowHelper.ThrowTooLargeException(int.MaxValue);
                }
                writer.RecordLength((int) lengthValue);
            }
            if (options.UpdateStoredBuffer)
            {
                _ExampleStructWithoutClass_ByteIndex = startOfChildPosition - startOfObjectPosition;
                
            }
            if (options.UpdateStoredBuffer)
            {
                _ContainerForExampleStructWithoutClass_EndByteIndex = writer.ActiveMemoryPosition - startOfObjectPosition;
            }
            
        }
    }
}
