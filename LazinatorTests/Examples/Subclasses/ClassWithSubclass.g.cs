//70a32b6a-ee59-6fff-a082-1a7575b6684d
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by the Lazinator tool, version 0.1.0.418, on 2024/02/06 03:46:56.376 PM.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

#nullable disable
namespace LazinatorTests.Examples.Subclasses
{
    #pragma warning disable 8019
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
    #pragma warning restore 8019
    
    [Autogenerated]
    public partial class ClassWithSubclass : ILazinator
    {
        public bool IsStruct => false;
        
        public virtual bool IsNullableContext => false;
        /* Property definitions */
        
        protected int _SubclassInstance1_ByteIndex;
        protected int _SubclassInstance2_ByteIndex;
        protected virtual int _SubclassInstance1_ByteLength => _SubclassInstance2_ByteIndex - _SubclassInstance1_ByteIndex;
        private int _ClassWithSubclass_EndByteIndex;
        protected virtual  int _SubclassInstance2_ByteLength => _ClassWithSubclass_EndByteIndex - _SubclassInstance2_ByteIndex;
        protected virtual int _OverallEndByteIndex => _ClassWithSubclass_EndByteIndex;
        
        
        protected int _IntWithinSuperclass;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
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
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public global::LazinatorTests.Examples.Subclasses.ClassWithSubclass.SubclassWithinClass SubclassInstance1
        {
            get
            {
                if (!_SubclassInstance1_Accessed)
                {
                    LazinateSubclassInstance1();
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
        private void LazinateSubclassInstance1()
        {
            if (LazinatorMemoryStorage.Length == 0)
            {
                _SubclassInstance1 = null;
            }
            else
            {
                LazinatorMemory childData = GetChildSlice(LazinatorMemoryStorage, _SubclassInstance1_ByteIndex, _SubclassInstance1_ByteLength, null);
                _SubclassInstance1 = DeserializationFactory.Instance.CreateBaseOrDerivedType(1058, (c, p) => new global::LazinatorTests.Examples.Subclasses.ClassWithSubclass.SubclassWithinClass(c, p), childData, this); 
            }
            _SubclassInstance1_Accessed = true;
        }
        
        
        protected global::LazinatorTests.Examples.Subclasses.ClassWithSubclass.SubclassWithinClass _SubclassInstance2;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public global::LazinatorTests.Examples.Subclasses.ClassWithSubclass.SubclassWithinClass SubclassInstance2
        {
            get
            {
                if (!_SubclassInstance2_Accessed)
                {
                    LazinateSubclassInstance2();
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
        private void LazinateSubclassInstance2()
        {
            if (LazinatorMemoryStorage.Length == 0)
            {
                _SubclassInstance2 = null;
            }
            else
            {
                LazinatorMemory childData = GetChildSlice(LazinatorMemoryStorage, _SubclassInstance2_ByteIndex, _SubclassInstance2_ByteLength, null);
                _SubclassInstance2 = DeserializationFactory.Instance.CreateBaseOrDerivedType(1058, (c, p) => new global::LazinatorTests.Examples.Subclasses.ClassWithSubclass.SubclassWithinClass(c, p), childData, this); 
            }
            _SubclassInstance2_Accessed = true;
        }
        
        
        /* Serialization, deserialization, and object relationships */
        
        public ClassWithSubclass(IncludeChildrenMode originalIncludeChildrenMode = IncludeChildrenMode.IncludeAllChildren)
        {
            OriginalIncludeChildrenMode = originalIncludeChildrenMode;
        }
        
        public ClassWithSubclass(LazinatorMemory serializedBytes, ILazinator parent = null, IncludeChildrenMode originalIncludeChildrenMode = IncludeChildrenMode.IncludeAllChildren, int? lazinatorObjectVersion = null)
        {
            if (lazinatorObjectVersion != null)
            {
                LazinatorObjectVersion = (int) lazinatorObjectVersion;
            }
            OriginalIncludeChildrenMode = originalIncludeChildrenMode;
            LazinatorParents = new LazinatorParentsCollection(parent, null);
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
            ClassWithSubclass clone;
            if (cloneBufferOptions == CloneBufferOptions.NoBuffer)
            {
                clone = new ClassWithSubclass(includeChildrenMode);
                clone.LazinatorObjectVersion = LazinatorObjectVersion;
                clone = (ClassWithSubclass)AssignCloneProperties(clone, includeChildrenMode);
            }
            else
            {
                LazinatorMemory bytes = EncodeOrRecycleToNewBuffer(includeChildrenMode, OriginalIncludeChildrenMode, IsDirty, DescendantIsDirty, false, LazinatorMemoryStorage, this);
                clone = new ClassWithSubclass(bytes);
            }
            return clone;
        }
        
        protected virtual ILazinator AssignCloneProperties(ILazinator clone, IncludeChildrenMode includeChildrenMode)
        {
            clone.FreeInMemoryObjects();
            ClassWithSubclass typedClone = (ClassWithSubclass) clone;
            typedClone.IntWithinSuperclass = IntWithinSuperclass;
            if (includeChildrenMode != IncludeChildrenMode.ExcludeAllChildren && includeChildrenMode != IncludeChildrenMode.IncludeOnlyIncludableChildren)
            {
                if (SubclassInstance1 == null)
                {
                    typedClone.SubclassInstance1 = null;
                }
                else
                {
                    typedClone.SubclassInstance1 = (global::LazinatorTests.Examples.Subclasses.ClassWithSubclass.SubclassWithinClass) SubclassInstance1.CloneLazinator(includeChildrenMode, CloneBufferOptions.NoBuffer);
                }
            }
            if (includeChildrenMode != IncludeChildrenMode.ExcludeAllChildren && includeChildrenMode != IncludeChildrenMode.IncludeOnlyIncludableChildren)
            {
                if (SubclassInstance2 == null)
                {
                    typedClone.SubclassInstance2 = null;
                }
                else
                {
                    typedClone.SubclassInstance2 = (global::LazinatorTests.Examples.Subclasses.ClassWithSubclass.SubclassWithinClass) SubclassInstance2.CloneLazinator(includeChildrenMode, CloneBufferOptions.NoBuffer);
                }
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
            if (enumerateNulls && (!exploreOnlyDeserializedChildren || _SubclassInstance1_Accessed) && SubclassInstance1 == null)
            {
                yield return ("SubclassInstance1", default);
            }
            else
            {
                if ((!exploreOnlyDeserializedChildren && SubclassInstance1 != null) || (_SubclassInstance1_Accessed && _SubclassInstance1 != null))
                {
                    bool isMatch_SubclassInstance1 = matchCriterion == null || matchCriterion(SubclassInstance1);
                    bool shouldExplore_SubclassInstance1 = exploreCriterion == null || exploreCriterion(SubclassInstance1);
                    if (isMatch_SubclassInstance1)
                    {
                        yield return ("SubclassInstance1", SubclassInstance1);
                    }
                    if ((!stopExploringBelowMatch || !isMatch_SubclassInstance1) && shouldExplore_SubclassInstance1)
                    {
                        foreach (var toYield in SubclassInstance1.EnumerateLazinatorDescendants(matchCriterion, stopExploringBelowMatch, exploreCriterion, exploreOnlyDeserializedChildren, enumerateNulls))
                        {
                            yield return ("SubclassInstance1" + "." + toYield.propertyName, toYield.descendant);
                        }
                    }
                }
            }
            if (enumerateNulls && (!exploreOnlyDeserializedChildren || _SubclassInstance2_Accessed) && SubclassInstance2 == null)
            {
                yield return ("SubclassInstance2", default);
            }
            else
            {
                if ((!exploreOnlyDeserializedChildren && SubclassInstance2 != null) || (_SubclassInstance2_Accessed && _SubclassInstance2 != null))
                {
                    bool isMatch_SubclassInstance2 = matchCriterion == null || matchCriterion(SubclassInstance2);
                    bool shouldExplore_SubclassInstance2 = exploreCriterion == null || exploreCriterion(SubclassInstance2);
                    if (isMatch_SubclassInstance2)
                    {
                        yield return ("SubclassInstance2", SubclassInstance2);
                    }
                    if ((!stopExploringBelowMatch || !isMatch_SubclassInstance2) && shouldExplore_SubclassInstance2)
                    {
                        foreach (var toYield in SubclassInstance2.EnumerateLazinatorDescendants(matchCriterion, stopExploringBelowMatch, exploreCriterion, exploreOnlyDeserializedChildren, enumerateNulls))
                        {
                            yield return ("SubclassInstance2" + "." + toYield.propertyName, toYield.descendant);
                        }
                    }
                }
            }
            yield break;
        }
        
        
        public virtual IEnumerable<(string propertyName, object descendant)> EnumerateNonLazinatorProperties()
        {
            yield return ("IntWithinSuperclass", (object)IntWithinSuperclass);
            yield break;
        }
        
        public virtual ILazinator ForEachLazinator(Func<ILazinator, ILazinator> changeFunc, bool exploreOnlyDeserializedChildren, bool changeThisLevel)
        {
            if ((!exploreOnlyDeserializedChildren && SubclassInstance1 != null) || (_SubclassInstance1_Accessed && _SubclassInstance1 != null))
            {
                _SubclassInstance1 = (global::LazinatorTests.Examples.Subclasses.ClassWithSubclass.SubclassWithinClass) _SubclassInstance1.ForEachLazinator(changeFunc, exploreOnlyDeserializedChildren, true);
            }
            if ((!exploreOnlyDeserializedChildren && SubclassInstance2 != null) || (_SubclassInstance2_Accessed && _SubclassInstance2 != null))
            {
                _SubclassInstance2 = (global::LazinatorTests.Examples.Subclasses.ClassWithSubclass.SubclassWithinClass) _SubclassInstance2.ForEachLazinator(changeFunc, exploreOnlyDeserializedChildren, true);
            }
            if (changeThisLevel && changeFunc != null)
            {
                return changeFunc(this);
            }
            return this;
        }
        
        public virtual void FreeInMemoryObjects()
        {
            _SubclassInstance1 = default;
            _SubclassInstance2 = default;
            _SubclassInstance1_Accessed = _SubclassInstance2_Accessed = false;
            IsDirty = false;
            DescendantIsDirty = false;
            HasChanged = false;
            DescendantHasChanged = false;
        }
        
        /* Conversion */
        
        public virtual int LazinatorUniqueID => 1057;
        
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
            int totalChildrenSize = ConvertFromBytesForChildLengths(span, includeChildrenMode, serializedVersionNumber, bytesSoFar + lengthForLengths, ref bytesSoFar);
            return bytesSoFar + totalChildrenSize;
        }
        
        protected virtual void ConvertFromBytesForPrimitiveProperties(ReadOnlySpan<byte> span, IncludeChildrenMode includeChildrenMode, int serializedVersionNumber, ref int bytesSoFar)
        {
            _IntWithinSuperclass = span.ToDecompressedInt32(ref bytesSoFar);
        }
        
        protected virtual int ConvertFromBytesForChildLengths(ReadOnlySpan<byte> span, IncludeChildrenMode includeChildrenMode, int serializedVersionNumber, int indexOfFirstChild, ref int bytesSoFar)
        {
            int totalChildrenBytes = 0;
            _SubclassInstance1_ByteIndex = indexOfFirstChild + totalChildrenBytes;
            if (includeChildrenMode != IncludeChildrenMode.ExcludeAllChildren && includeChildrenMode != IncludeChildrenMode.IncludeOnlyIncludableChildren)
            {
                totalChildrenBytes += span.ToInt32(ref bytesSoFar);
            }
            _SubclassInstance2_ByteIndex = indexOfFirstChild + totalChildrenBytes;
            if (includeChildrenMode != IncludeChildrenMode.ExcludeAllChildren && includeChildrenMode != IncludeChildrenMode.IncludeOnlyIncludableChildren)
            {
                totalChildrenBytes += span.ToInt32(ref bytesSoFar);
            }
            _ClassWithSubclass_EndByteIndex = indexOfFirstChild + totalChildrenBytes;
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
            if (_SubclassInstance1_Accessed && _SubclassInstance1 != null)
            {
                SubclassInstance1.UpdateStoredBuffer(ref writer, startPosition + _SubclassInstance1_ByteIndex, _SubclassInstance1_ByteLength, IncludeChildrenMode.IncludeAllChildren, true);
            }
            if (_SubclassInstance2_Accessed && _SubclassInstance2 != null)
            {
                SubclassInstance2.UpdateStoredBuffer(ref writer, startPosition + _SubclassInstance2_ByteIndex, _SubclassInstance2_ByteLength, IncludeChildrenMode.IncludeAllChildren, true);
            }
            
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
            CompressedIntegralTypes.WriteCompressedInt(ref writer, _IntWithinSuperclass);
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
                if ((options.IncludeChildrenMode != IncludeChildrenMode.IncludeAllChildren || options.IncludeChildrenMode != OriginalIncludeChildrenMode) && !_SubclassInstance1_Accessed)
                {
                    var deserialized = SubclassInstance1;
                }
                WriteChild(ref writer, ref _SubclassInstance1, options, _SubclassInstance1_Accessed, () => GetChildSlice(LazinatorMemoryStorage, _SubclassInstance1_ByteIndex, _SubclassInstance1_ByteLength, null), this);
                lengthValue = writer.ActiveMemoryPosition - startOfChildPosition;
                if (lengthValue > int.MaxValue)
                {
                    ThrowHelper.ThrowTooLargeException(int.MaxValue);
                }
                writer.RecordLength((int) lengthValue);
            }
            if (options.UpdateStoredBuffer)
            {
                _SubclassInstance1_ByteIndex = startOfChildPosition - startOfObjectPosition;
                
            }
            startOfChildPosition = writer.ActiveMemoryPosition;
            if (options.IncludeChildrenMode != IncludeChildrenMode.ExcludeAllChildren && options.IncludeChildrenMode != IncludeChildrenMode.IncludeOnlyIncludableChildren)
            {
                if ((options.IncludeChildrenMode != IncludeChildrenMode.IncludeAllChildren || options.IncludeChildrenMode != OriginalIncludeChildrenMode) && !_SubclassInstance2_Accessed)
                {
                    var deserialized = SubclassInstance2;
                }
                WriteChild(ref writer, ref _SubclassInstance2, options, _SubclassInstance2_Accessed, () => GetChildSlice(LazinatorMemoryStorage, _SubclassInstance2_ByteIndex, _SubclassInstance2_ByteLength, null), this);
                lengthValue = writer.ActiveMemoryPosition - startOfChildPosition;
                if (lengthValue > int.MaxValue)
                {
                    ThrowHelper.ThrowTooLargeException(int.MaxValue);
                }
                writer.RecordLength((int) lengthValue);
            }
            if (options.UpdateStoredBuffer)
            {
                _SubclassInstance2_ByteIndex = startOfChildPosition - startOfObjectPosition;
                
            }
            if (options.UpdateStoredBuffer)
            {
                _ClassWithSubclass_EndByteIndex = writer.ActiveMemoryPosition - startOfObjectPosition;
            }
            
        }
    }
}
#nullable restore
