//44a07f3e-fa3b-e7df-eb28-9462a50162e4
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by the Lazinator tool, version 0.1.0.393
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

#nullable disable
namespace LazinatorTests.Examples
{
    using Lazinator.Attributes;
    using Lazinator.Buffers;
    using Lazinator.Core;
    using Lazinator.Exceptions;
    using Lazinator.Support;
    using LazinatorTests.Examples.Structs;
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
    public partial class ExampleChild : ILazinator
    {
        public bool IsStruct => false;
        
        /* Property definitions */
        
        protected int _ByteSpan_ByteIndex;
        protected int _MyExampleGrandchild_ByteIndex;
        protected int _MyWrapperContainer_ByteIndex;
        protected virtual int _ByteSpan_ByteLength => _MyExampleGrandchild_ByteIndex - _ByteSpan_ByteIndex;
        protected virtual int _MyExampleGrandchild_ByteLength => _MyWrapperContainer_ByteIndex - _MyExampleGrandchild_ByteIndex;
        private int _ExampleChild_EndByteIndex;
        protected virtual  int _MyWrapperContainer_ByteLength => _ExampleChild_EndByteIndex - _MyWrapperContainer_ByteIndex;
        protected virtual int _OverallEndByteIndex => _ExampleChild_EndByteIndex;
        
        
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
        public ReadOnlySpan<Byte> ByteSpan
        {
            get
            {
                if (!_ByteSpan_Accessed)
                {
                    LazinatorMemory childData = GetChildSlice(LazinatorMemoryStorage, _ByteSpan_ByteIndex, _ByteSpan_ByteLength, true, false, null);
                    return childData.InitialMemory.Span;
                }
                return _ByteSpan.Span;
            }
            set
            {
                IsDirty = true;
                _ByteSpan = new ReadOnlyMemory<byte>((value).ToArray());
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
                    LazinateMyExampleGrandchild();
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
        private void LazinateMyExampleGrandchild()
        {
            if (LazinatorMemoryStorage.Length == 0)
            {
                _MyExampleGrandchild = null;
            }
            else
            {
                LazinatorMemory childData = GetChildSlice(LazinatorMemoryStorage, _MyExampleGrandchild_ByteIndex, _MyExampleGrandchild_ByteLength, true, false, null);
                
                _MyExampleGrandchild = DeserializationFactory.Instance.CreateBaseOrDerivedType(1079, (c, p) => new ExampleGrandchild(c, p), childData, this); 
            }
            _MyExampleGrandchild_Accessed = true;
        }
        
        
        protected WrapperContainer _MyWrapperContainer;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public WrapperContainer MyWrapperContainer
        {
            get
            {
                if (!_MyWrapperContainer_Accessed)
                {
                    LazinateMyWrapperContainer();
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
        private void LazinateMyWrapperContainer()
        {
            if (LazinatorMemoryStorage.Length == 0)
            {
                _MyWrapperContainer = null;
            }
            else
            {
                LazinatorMemory childData = GetChildSlice(LazinatorMemoryStorage, _MyWrapperContainer_ByteIndex, _MyWrapperContainer_ByteLength, true, false, null);
                
                _MyWrapperContainer = DeserializationFactory.Instance.CreateBaseOrDerivedType(1048, (c, p) => new WrapperContainer(c, p), childData, this); 
            }
            _MyWrapperContainer_Accessed = true;
        }
        
        
        /* Serialization, deserialization, and object relationships */
        
        public ExampleChild(IncludeChildrenMode originalIncludeChildrenMode = IncludeChildrenMode.IncludeAllChildren)
        {
            OriginalIncludeChildrenMode = originalIncludeChildrenMode;
        }
        
        public ExampleChild(LazinatorMemory serializedBytes, ILazinator parent = null, IncludeChildrenMode originalIncludeChildrenMode = IncludeChildrenMode.IncludeAllChildren, int? lazinatorObjectVersion = null)
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
        
        public virtual IncludeChildrenMode OriginalIncludeChildrenMode { get; set; }
        
        protected virtual int Deserialize()
        {
            FreeInMemoryObjects();
            int bytesSoFar = 0;
            ReadOnlySpan<byte> span = LazinatorMemoryStorage.InitialMemory.Span;
            if (span.Length == 0)
            {
                return 0;
            }
            
            ReadGenericIDIfApplicable(ContainsOpenGenericParameters, LazinatorUniqueID, span, ref bytesSoFar);
            
            int lazinatorLibraryVersion = span.ToDecompressedInt32(ref bytesSoFar);
            
            int serializedVersionNumber = span.ToDecompressedInt32(ref bytesSoFar);
            
            OriginalIncludeChildrenMode = (IncludeChildrenMode)span.ToByte(ref bytesSoFar);
            
            ConvertFromBytesAfterHeader(OriginalIncludeChildrenMode, serializedVersionNumber, ref bytesSoFar);
            return _OverallEndByteIndex;
        }
        
        public virtual LazinatorMemory SerializeLazinator(IncludeChildrenMode includeChildrenMode, bool verifyCleanness, bool updateStoredBuffer) 
        {
            if (LazinatorMemoryStorage.IsEmpty || includeChildrenMode != OriginalIncludeChildrenMode || (verifyCleanness || IsDirty || (includeChildrenMode != IncludeChildrenMode.ExcludeAllChildren && DescendantIsDirty)))
            {
                return EncodeToNewBuffer(includeChildrenMode, verifyCleanness, updateStoredBuffer);
            }
            BinaryBufferWriter writer = new BinaryBufferWriter(LazinatorMemoryStorage.Length);
            LazinatorMemoryStorage.WriteToBinaryBuffer(ref writer);
            return writer.LazinatorMemory;
        }
        
        protected virtual LazinatorMemory EncodeToNewBuffer(IncludeChildrenMode includeChildrenMode, bool verifyCleanness, bool updateStoredBuffer) 
        {
            int bufferSize = LazinatorMemoryStorage.Length == 0 ? ExpandableBytes.DefaultMinBufferSize : LazinatorMemoryStorage.Length;
            BinaryBufferWriter writer = new BinaryBufferWriter(bufferSize);
            SerializeToExistingBuffer(ref writer, includeChildrenMode, verifyCleanness, updateStoredBuffer);
            return writer.LazinatorMemory;
        }
        
        public virtual ILazinator CloneLazinator(IncludeChildrenMode includeChildrenMode = IncludeChildrenMode.IncludeAllChildren, CloneBufferOptions cloneBufferOptions = CloneBufferOptions.IndependentBuffers)
        {
            ExampleChild clone;
            if (cloneBufferOptions == CloneBufferOptions.NoBuffer)
            {
                clone = new ExampleChild(includeChildrenMode);
                clone.LazinatorObjectVersion = LazinatorObjectVersion;
                clone = (ExampleChild)AssignCloneProperties(clone, includeChildrenMode);
            }
            else
            {
                LazinatorMemory bytes = EncodeOrRecycleToNewBuffer(includeChildrenMode, OriginalIncludeChildrenMode, false, IsDirty, DescendantIsDirty, false, LazinatorMemoryStorage, false, this);
                clone = new ExampleChild(bytes);
            }
            return clone;
        }
        
        protected virtual ILazinator AssignCloneProperties(ILazinator clone, IncludeChildrenMode includeChildrenMode)
        {
            clone.FreeInMemoryObjects();
            ExampleChild typedClone = (ExampleChild) clone;
            typedClone.MyLong = MyLong;
            typedClone.MyShort = MyShort;
            typedClone.ByteSpan = CloneOrChange_ReadOnlySpan_Gbyte_g(ByteSpan, l => l?.CloneLazinator(includeChildrenMode, CloneBufferOptions.NoBuffer), false);
            if (includeChildrenMode != IncludeChildrenMode.ExcludeAllChildren && includeChildrenMode != IncludeChildrenMode.IncludeOnlyIncludableChildren)
            {
                if (MyExampleGrandchild == null)
                {
                    typedClone.MyExampleGrandchild = null;
                }
                else
                {
                    typedClone.MyExampleGrandchild = (ExampleGrandchild) MyExampleGrandchild.CloneLazinator(includeChildrenMode, CloneBufferOptions.NoBuffer);
                }
            }
            if (includeChildrenMode != IncludeChildrenMode.ExcludeAllChildren && includeChildrenMode != IncludeChildrenMode.IncludeOnlyIncludableChildren)
            {
                if (MyWrapperContainer == null)
                {
                    typedClone.MyWrapperContainer = null;
                }
                else
                {
                    typedClone.MyWrapperContainer = (WrapperContainer) MyWrapperContainer.CloneLazinator(includeChildrenMode, CloneBufferOptions.NoBuffer);
                }
            }
            
            return typedClone;
        }
        
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
        
        protected virtual void DeserializeLazinator(LazinatorMemory serializedBytes)
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
        
        public virtual void SerializeLazinator()
        {
            if (!IsDirty && !DescendantIsDirty && LazinatorMemoryStorage.Length > 0 && OriginalIncludeChildrenMode == IncludeChildrenMode.IncludeAllChildren)
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
                LazinatorMemoryStorage.WriteToBinaryBuffer(ref writer);
                LazinatorMemoryStorage = writer.LazinatorMemory;
            }
            OriginalIncludeChildrenMode = IncludeChildrenMode.IncludeAllChildren;
            if (!LazinatorParents.Any())
            {
                previousBuffer.Dispose();
            }
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
            if (enumerateNulls && (!exploreOnlyDeserializedChildren || _MyExampleGrandchild_Accessed) && MyExampleGrandchild == null)
            {
                yield return ("MyExampleGrandchild", default);
            }
            else
            {
                if ((!exploreOnlyDeserializedChildren && MyExampleGrandchild != null) || (_MyExampleGrandchild_Accessed && _MyExampleGrandchild != null))
                {
                    bool isMatch_MyExampleGrandchild = matchCriterion == null || matchCriterion(MyExampleGrandchild);
                    bool shouldExplore_MyExampleGrandchild = exploreCriterion == null || exploreCriterion(MyExampleGrandchild);
                    if (isMatch_MyExampleGrandchild)
                    {
                        yield return ("MyExampleGrandchild", MyExampleGrandchild);
                    }
                    if ((!stopExploringBelowMatch || !isMatch_MyExampleGrandchild) && shouldExplore_MyExampleGrandchild)
                    {
                        foreach (var toYield in MyExampleGrandchild.EnumerateLazinatorDescendants(matchCriterion, stopExploringBelowMatch, exploreCriterion, exploreOnlyDeserializedChildren, enumerateNulls))
                        {
                            yield return ("MyExampleGrandchild" + "." + toYield.propertyName, toYield.descendant);
                        }
                    }
                }
            }
            if (enumerateNulls && (!exploreOnlyDeserializedChildren || _MyWrapperContainer_Accessed) && MyWrapperContainer == null)
            {
                yield return ("MyWrapperContainer", default);
            }
            else
            {
                if ((!exploreOnlyDeserializedChildren && MyWrapperContainer != null) || (_MyWrapperContainer_Accessed && _MyWrapperContainer != null))
                {
                    bool isMatch_MyWrapperContainer = matchCriterion == null || matchCriterion(MyWrapperContainer);
                    bool shouldExplore_MyWrapperContainer = exploreCriterion == null || exploreCriterion(MyWrapperContainer);
                    if (isMatch_MyWrapperContainer)
                    {
                        yield return ("MyWrapperContainer", MyWrapperContainer);
                    }
                    if ((!stopExploringBelowMatch || !isMatch_MyWrapperContainer) && shouldExplore_MyWrapperContainer)
                    {
                        foreach (var toYield in MyWrapperContainer.EnumerateLazinatorDescendants(matchCriterion, stopExploringBelowMatch, exploreCriterion, exploreOnlyDeserializedChildren, enumerateNulls))
                        {
                            yield return ("MyWrapperContainer" + "." + toYield.propertyName, toYield.descendant);
                        }
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
        
        public virtual ILazinator ForEachLazinator(Func<ILazinator, ILazinator> changeFunc, bool exploreOnlyDeserializedChildren, bool changeThisLevel)
        {
            if ((!exploreOnlyDeserializedChildren && MyExampleGrandchild != null) || (_MyExampleGrandchild_Accessed && _MyExampleGrandchild != null))
            {
                _MyExampleGrandchild = (ExampleGrandchild) _MyExampleGrandchild.ForEachLazinator(changeFunc, exploreOnlyDeserializedChildren, true);
            }
            if ((!exploreOnlyDeserializedChildren && MyWrapperContainer != null) || (_MyWrapperContainer_Accessed && _MyWrapperContainer != null))
            {
                _MyWrapperContainer = (WrapperContainer) _MyWrapperContainer.ForEachLazinator(changeFunc, exploreOnlyDeserializedChildren, true);
            }
            if (!exploreOnlyDeserializedChildren)
            {
                var deserialized_ByteSpan = ByteSpan;
                if (!_ByteSpan_Accessed)
                {
                    ByteSpan = deserialized_ByteSpan;
                }
            }
            if (changeThisLevel && changeFunc != null)
            {
                return changeFunc(this);
            }
            return this;
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
        
        public virtual int LazinatorUniqueID => 1013;
        
        protected virtual bool ContainsOpenGenericParameters => false;
        public virtual LazinatorGenericIDType LazinatorGenericID => default;
        
        
        public virtual int LazinatorObjectVersion { get; set; } = 0;
        
        
        protected virtual void ConvertFromBytesAfterHeader(IncludeChildrenMode includeChildrenMode, int serializedVersionNumber, ref int bytesSoFar)
        {
            ReadOnlySpan<byte> span = LazinatorMemoryStorage.InitialMemory.Span;
            ConvertFromBytesForPrimitiveProperties(span, includeChildrenMode, serializedVersionNumber, ref bytesSoFar);
            int lengthForLengths = 4;
            if (includeChildrenMode != IncludeChildrenMode.ExcludeAllChildren && includeChildrenMode != IncludeChildrenMode.IncludeOnlyIncludableChildren)
            {
                lengthForLengths += 8;
            }
            int totalChildrenSize = ConvertFromBytesForChildProperties(span, includeChildrenMode, serializedVersionNumber, bytesSoFar + lengthForLengths, ref bytesSoFar);
            bytesSoFar += totalChildrenSize;
        }
        
        protected virtual void ConvertFromBytesForPrimitiveProperties(ReadOnlySpan<byte> span, IncludeChildrenMode includeChildrenMode, int serializedVersionNumber, ref int bytesSoFar)
        {
            _MyLong = span.ToDecompressedInt64(ref bytesSoFar);
            _MyShort = span.ToDecompressedInt16(ref bytesSoFar);
        }
        
        protected virtual int ConvertFromBytesForChildProperties(ReadOnlySpan<byte> span, IncludeChildrenMode includeChildrenMode, int serializedVersionNumber, int indexOfFirstChild, ref int bytesSoFar)
        {
            int totalChildrenBytes = 0;
            _ByteSpan_ByteIndex = indexOfFirstChild + totalChildrenBytes;
            totalChildrenBytes += span.ToInt32(ref bytesSoFar);
            _MyExampleGrandchild_ByteIndex = indexOfFirstChild + totalChildrenBytes;
            if (includeChildrenMode != IncludeChildrenMode.ExcludeAllChildren && includeChildrenMode != IncludeChildrenMode.IncludeOnlyIncludableChildren)
            {
                totalChildrenBytes += span.ToInt32(ref bytesSoFar);
            }
            _MyWrapperContainer_ByteIndex = indexOfFirstChild + totalChildrenBytes;
            if (includeChildrenMode != IncludeChildrenMode.ExcludeAllChildren && includeChildrenMode != IncludeChildrenMode.IncludeOnlyIncludableChildren)
            {
                totalChildrenBytes += span.ToInt32(ref bytesSoFar);
            }
            _ExampleChild_EndByteIndex = indexOfFirstChild + totalChildrenBytes;
            return totalChildrenBytes;
        }
        
        public virtual void SerializeToExistingBuffer(ref BinaryBufferWriter writer, IncludeChildrenMode includeChildrenMode, bool verifyCleanness, bool updateStoredBuffer)
        {
            if (includeChildrenMode != IncludeChildrenMode.IncludeAllChildren)
            {
                updateStoredBuffer = false;
            }
            int startPosition = writer.ActiveMemoryPosition;
            WritePropertiesIntoBuffer(ref writer, includeChildrenMode, verifyCleanness, updateStoredBuffer, true);
            if (updateStoredBuffer)
            {
                UpdateStoredBuffer(ref writer, startPosition, writer.ActiveMemoryPosition - startPosition, includeChildrenMode, false);
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
                ThrowHelper.ThrowCannotUpdateStoredBuffer();
            }
            
            var newBuffer = writer.Slice(startPosition, length);
            LazinatorMemoryStorage = newBuffer;
        }
        
        protected virtual void UpdateDeserializedChildren(ref BinaryBufferWriter writer, int startPosition)
        {
            if (_MyExampleGrandchild_Accessed && _MyExampleGrandchild != null)
            {
                MyExampleGrandchild.UpdateStoredBuffer(ref writer, startPosition + _MyExampleGrandchild_ByteIndex, _MyExampleGrandchild_ByteLength, IncludeChildrenMode.IncludeAllChildren, true);
            }
            if (_MyWrapperContainer_Accessed && _MyWrapperContainer != null)
            {
                MyWrapperContainer.UpdateStoredBuffer(ref writer, startPosition + _MyWrapperContainer_ByteIndex, _MyWrapperContainer_ByteLength, IncludeChildrenMode.IncludeAllChildren, true);
            }
        }
        
        
        protected virtual void WritePropertiesIntoBuffer(ref BinaryBufferWriter writer, IncludeChildrenMode includeChildrenMode, bool verifyCleanness, bool updateStoredBuffer, bool includeUniqueID)
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
            writer.Write((byte)includeChildrenMode);
            // write properties
            
            WritePrimitivePropertiesIntoBuffer(ref writer, includeChildrenMode, verifyCleanness, updateStoredBuffer, includeUniqueID);
            int lengthForLengths = 4;
            if (includeChildrenMode != IncludeChildrenMode.ExcludeAllChildren && includeChildrenMode != IncludeChildrenMode.IncludeOnlyIncludableChildren)
            {
                lengthForLengths += 8;
            }
            
            int previousLengthsPosition = writer.SetLengthsPosition(lengthForLengths);
            WriteChildrenPropertiesIntoBuffer(ref writer, includeChildrenMode, verifyCleanness, updateStoredBuffer, includeUniqueID, startPosition);
            writer.ResetLengthsPosition(previousLengthsPosition);
        }
        
        protected virtual void WritePrimitivePropertiesIntoBuffer(ref BinaryBufferWriter writer, IncludeChildrenMode includeChildrenMode, bool verifyCleanness, bool updateStoredBuffer, bool includeUniqueID)
        {
            CompressedIntegralTypes.WriteCompressedLong(ref writer, _MyLong);
            CompressedIntegralTypes.WriteCompressedShort(ref writer, _MyShort);
        }
        
        protected virtual void WriteChildrenPropertiesIntoBuffer(ref BinaryBufferWriter writer, IncludeChildrenMode includeChildrenMode, bool verifyCleanness, bool updateStoredBuffer, bool includeUniqueID, int startOfObjectPosition)
        {
            int startOfChildPosition = 0;
            int lengthValue = 0;
            startOfChildPosition = writer.ActiveMemoryPosition;
            if ((includeChildrenMode != IncludeChildrenMode.IncludeAllChildren || includeChildrenMode != OriginalIncludeChildrenMode) && !_ByteSpan_Accessed)
            {
                var deserialized = ByteSpan;
            }
            WriteNonLazinatorObject(
            nonLazinatorObject: _ByteSpan, isBelievedDirty: _ByteSpan_Accessed || (includeChildrenMode != OriginalIncludeChildrenMode),
            isAccessed: _ByteSpan_Accessed, writer: ref writer,
            getChildSliceForFieldFn: () => GetChildSlice(LazinatorMemoryStorage, _ByteSpan_ByteIndex, _ByteSpan_ByteLength, true, false, null),
            verifyCleanness: false,
            binaryWriterAction: (ref BinaryBufferWriter w, bool v) =>
            ConvertToBytes_ReadOnlySpan_Gbyte_g(ref w, _ByteSpan.Span,
            includeChildrenMode, v, updateStoredBuffer),
            writeLengthInByte: false);
            if (updateStoredBuffer)
            {
                _ByteSpan_ByteIndex = startOfChildPosition - startOfObjectPosition;
                
            }
            startOfChildPosition = writer.ActiveMemoryPosition;
            if (includeChildrenMode != IncludeChildrenMode.ExcludeAllChildren && includeChildrenMode != IncludeChildrenMode.IncludeOnlyIncludableChildren)
            {
                if ((includeChildrenMode != IncludeChildrenMode.IncludeAllChildren || includeChildrenMode != OriginalIncludeChildrenMode) && !_MyExampleGrandchild_Accessed)
                {
                    var deserialized = MyExampleGrandchild;
                }
                WriteChild(ref writer, ref _MyExampleGrandchild, includeChildrenMode, _MyExampleGrandchild_Accessed, () => GetChildSlice(LazinatorMemoryStorage, _MyExampleGrandchild_ByteIndex, _MyExampleGrandchild_ByteLength, true, false, null), verifyCleanness, updateStoredBuffer, false, true, this);
                lengthValue = writer.ActiveMemoryPosition - startOfChildPosition;
                writer.RecordLength((int) lengthValue);
            }
            if (updateStoredBuffer)
            {
                _MyExampleGrandchild_ByteIndex = startOfChildPosition - startOfObjectPosition;
                
            }
            startOfChildPosition = writer.ActiveMemoryPosition;
            if (includeChildrenMode != IncludeChildrenMode.ExcludeAllChildren && includeChildrenMode != IncludeChildrenMode.IncludeOnlyIncludableChildren)
            {
                if ((includeChildrenMode != IncludeChildrenMode.IncludeAllChildren || includeChildrenMode != OriginalIncludeChildrenMode) && !_MyWrapperContainer_Accessed)
                {
                    var deserialized = MyWrapperContainer;
                }
                WriteChild(ref writer, ref _MyWrapperContainer, includeChildrenMode, _MyWrapperContainer_Accessed, () => GetChildSlice(LazinatorMemoryStorage, _MyWrapperContainer_ByteIndex, _MyWrapperContainer_ByteLength, true, false, null), verifyCleanness, updateStoredBuffer, false, true, this);
                lengthValue = writer.ActiveMemoryPosition - startOfChildPosition;
                writer.RecordLength((int) lengthValue);
            }
            if (updateStoredBuffer)
            {
                _MyWrapperContainer_ByteIndex = startOfChildPosition - startOfObjectPosition;
                
            }
            if (updateStoredBuffer)
            {
                _ExampleChild_EndByteIndex = writer.ActiveMemoryPosition - startOfObjectPosition;
            }
        }
        
        /* Conversion of supported collections and tuples */
        
        private static void ConvertToBytes_ReadOnlySpan_Gbyte_g(ref BinaryBufferWriter writer, ReadOnlySpan<Byte> itemToConvert, IncludeChildrenMode includeChildrenMode, bool verifyCleanness, bool updateStoredBuffer)
        {
            ReadOnlySpan<byte> toConvert = (itemToConvert);
            for (int i = 0; i < toConvert.Length; i++)
            {
                writer.Write(toConvert[i]);
            }
        }
        private static ReadOnlySpan<Byte> CloneOrChange_ReadOnlySpan_Gbyte_g(ReadOnlySpan<Byte> itemToClone, Func<ILazinator, ILazinator> cloneOrChangeFunc, bool avoidCloningIfPossible)
        {
            var clone = new Span<byte>(new byte[itemToClone.Length * sizeof(byte)]);
            itemToClone.CopyTo(clone);
            return clone;
        }
        
    }
}
