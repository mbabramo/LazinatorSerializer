//adfa3152-087c-bbb7-407f-8ac357f02d94
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by the Lazinator tool, version 0.1.0.393
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

#nullable disable
namespace LazinatorTests.Examples.NonAbstractGenerics
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
    public partial class ConstrainedGeneric<T, U> : ILazinator
    {
        public bool IsStruct => false;
        
        /* Property definitions */
        
        protected int _MyNullableT_ByteIndex;
        protected int _MyT_ByteIndex;
        protected int _MyU_ByteIndex;
        protected virtual int _MyNullableT_ByteLength => _MyT_ByteIndex - _MyNullableT_ByteIndex;
        protected virtual int _MyT_ByteLength => _MyU_ByteIndex - _MyT_ByteIndex;
        private int _ConstrainedGeneric_T_U_EndByteIndex = 0;
        protected virtual int _MyU_ByteLength => _ConstrainedGeneric_T_U_EndByteIndex - _MyU_ByteIndex;
        
        
        protected T? _MyNullableT;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public virtual T? MyNullableT
        {
            get
            {
                if (!_MyNullableT_Accessed)
                {
                    LazinateMyNullableT();
                } 
                return _MyNullableT;
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
                _MyNullableT = value;
                _MyNullableT_Accessed = true;
            }
        }
        protected bool _MyNullableT_Accessed;
        private void LazinateMyNullableT()
        {
            if (LazinatorMemoryStorage.Length == 0)
            {
                _MyNullableT = default(T?);
            }
            else
            {
                LazinatorMemory childData = GetChildSlice(LazinatorMemoryStorage, _MyNullableT_ByteIndex, _MyNullableT_ByteLength, true, false, null);
                
                _MyNullableT = DeserializationFactory.Instance.CreateBasedOnType<T?>(childData, this); 
            }
            
            _MyNullableT_Accessed = true;
        }
        
        
        protected T _MyT;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public virtual T MyT
        {
            get
            {
                if (!_MyT_Accessed)
                {
                    LazinateMyT();
                } 
                return _MyT;
            }
            set
            {
                value.LazinatorParents = new LazinatorParentsCollection(this);
                
                IsDirty = true;
                DescendantIsDirty = true;
                _MyT = value;
                _MyT_Accessed = true;
            }
        }
        protected bool _MyT_Accessed;
        private void LazinateMyT()
        {
            if (LazinatorMemoryStorage.Length == 0)
            {
                _MyT = default(T);
                _MyT.LazinatorParents = new LazinatorParentsCollection(this);
            }
            else
            {
                LazinatorMemory childData = GetChildSlice(LazinatorMemoryStorage, _MyT_ByteIndex, _MyT_ByteLength, true, false, null);
                
                _MyT = DeserializationFactory.Instance.CreateBasedOnType<T>(childData, this); 
            }
            
            _MyT_Accessed = true;
        }
        
        
        protected U _MyU;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public virtual U MyU
        {
            get
            {
                if (!_MyU_Accessed)
                {
                    LazinateMyU();
                } 
                return _MyU;
            }
            set
            {
                if (value != null && value.IsStruct)
                {
                    value.LazinatorParents = new LazinatorParentsCollection(this);
                }
                else
                {
                    if (_MyU != null)
                    {
                        _MyU.LazinatorParents = _MyU.LazinatorParents.WithRemoved(this);
                    }
                    if (value != null)
                    {
                        value.LazinatorParents = value.LazinatorParents.WithAdded(this);
                    }
                }
                
                IsDirty = true;
                DescendantIsDirty = true;
                _MyU = value;
                _MyU_Accessed = true;
            }
        }
        protected bool _MyU_Accessed;
        private void LazinateMyU()
        {
            if (LazinatorMemoryStorage.Length == 0)
            {
                _MyU = default(U);
                if (_MyU != null)
                { // MyU is a struct
                    _MyU.LazinatorParents = new LazinatorParentsCollection(this);
                }
            }
            else
            {
                LazinatorMemory childData = GetChildSlice(LazinatorMemoryStorage, _MyU_ByteIndex, _MyU_ByteLength, true, false, null);
                
                _MyU = DeserializationFactory.Instance.CreateBasedOnType<U>(childData, this); 
            }
            
            _MyU_Accessed = true;
        }
        
        
        /* Serialization, deserialization, and object relationships */
        
        public ConstrainedGeneric(IncludeChildrenMode originalIncludeChildrenMode = IncludeChildrenMode.IncludeAllChildren)
        {
            OriginalIncludeChildrenMode = originalIncludeChildrenMode;
        }
        
        public ConstrainedGeneric(LazinatorMemory serializedBytes, ILazinator parent = null, IncludeChildrenMode originalIncludeChildrenMode = IncludeChildrenMode.IncludeAllChildren, int? lazinatorObjectVersion = null)
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
            return bytesSoFar;
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
            ConstrainedGeneric<T, U> clone;
            if (cloneBufferOptions == CloneBufferOptions.NoBuffer)
            {
                clone = new ConstrainedGeneric<T, U>(includeChildrenMode);
                clone.LazinatorObjectVersion = LazinatorObjectVersion;
                clone = (ConstrainedGeneric<T, U>)AssignCloneProperties(clone, includeChildrenMode);
            }
            else
            {
                LazinatorMemory bytes = EncodeOrRecycleToNewBuffer(includeChildrenMode, OriginalIncludeChildrenMode, false, IsDirty, DescendantIsDirty, false, LazinatorMemoryStorage, false, this);
                clone = new ConstrainedGeneric<T, U>(bytes);
            }
            return clone;
        }
        
        protected virtual ILazinator AssignCloneProperties(ILazinator clone, IncludeChildrenMode includeChildrenMode)
        {
            clone.FreeInMemoryObjects();
            ConstrainedGeneric<T, U> typedClone = (ConstrainedGeneric<T, U>) clone;
            if (includeChildrenMode != IncludeChildrenMode.ExcludeAllChildren && includeChildrenMode != IncludeChildrenMode.IncludeOnlyIncludableChildren)
            {
                typedClone.MyNullableT = (T?) MyNullableT.Value.CloneLazinator(includeChildrenMode, CloneBufferOptions.NoBuffer);
            }
            
            if (includeChildrenMode != IncludeChildrenMode.ExcludeAllChildren && includeChildrenMode != IncludeChildrenMode.IncludeOnlyIncludableChildren)
            {
                typedClone.MyT = (T) MyT.CloneLazinator(includeChildrenMode, CloneBufferOptions.NoBuffer);
            }
            
            if (includeChildrenMode != IncludeChildrenMode.ExcludeAllChildren && includeChildrenMode != IncludeChildrenMode.IncludeOnlyIncludableChildren)
            {
                if (MyU == null)
                {
                    typedClone.MyU = default(U);
                }
                else
                {
                    typedClone.MyU = (U) MyU.CloneLazinator(includeChildrenMode, CloneBufferOptions.NoBuffer);
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
        
        public virtual void UpdateStoredBuffer()
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
            bool isMatch_MyNullableT = matchCriterion == null || matchCriterion(MyNullableT);
            bool shouldExplore_MyNullableT = exploreCriterion == null || exploreCriterion(MyNullableT);
            if (isMatch_MyNullableT)
            {
                yield return ("MyNullableT", MyNullableT);
            }
            if ((!stopExploringBelowMatch || !isMatch_MyNullableT) && shouldExplore_MyNullableT)
            {
                foreach (var toYield in MyNullableT.Value.EnumerateLazinatorDescendants(matchCriterion, stopExploringBelowMatch, exploreCriterion, exploreOnlyDeserializedChildren, enumerateNulls))
                {
                    yield return ("MyNullableT" + "." + toYield.propertyName, toYield.descendant);
                }
            }
            bool isMatch_MyT = matchCriterion == null || matchCriterion(MyT);
            bool shouldExplore_MyT = exploreCriterion == null || exploreCriterion(MyT);
            if (isMatch_MyT)
            {
                yield return ("MyT", MyT);
            }
            if ((!stopExploringBelowMatch || !isMatch_MyT) && shouldExplore_MyT)
            {
                foreach (var toYield in MyT.EnumerateLazinatorDescendants(matchCriterion, stopExploringBelowMatch, exploreCriterion, exploreOnlyDeserializedChildren, enumerateNulls))
                {
                    yield return ("MyT" + "." + toYield.propertyName, toYield.descendant);
                }
            }
            if (enumerateNulls && (!exploreOnlyDeserializedChildren || _MyU_Accessed) && MyU == null)
            {
                yield return ("MyU", default);
            }
            else
            {
                if ((!exploreOnlyDeserializedChildren && MyU != null) || (_MyU_Accessed && _MyU != null))
                {
                    bool isMatch_MyU = matchCriterion == null || matchCriterion(MyU);
                    bool shouldExplore_MyU = exploreCriterion == null || exploreCriterion(MyU);
                    if (isMatch_MyU)
                    {
                        yield return ("MyU", MyU);
                    }
                    if ((!stopExploringBelowMatch || !isMatch_MyU) && shouldExplore_MyU)
                    {
                        foreach (var toYield in MyU.EnumerateLazinatorDescendants(matchCriterion, stopExploringBelowMatch, exploreCriterion, exploreOnlyDeserializedChildren, enumerateNulls))
                        {
                            yield return ("MyU" + "." + toYield.propertyName, toYield.descendant);
                        }
                    }
                }
                
            }
            
            yield break;
        }
        
        
        public virtual IEnumerable<(string propertyName, object descendant)> EnumerateNonLazinatorProperties()
        {
            yield break;
        }
        
        public virtual ILazinator ForEachLazinator(Func<ILazinator, ILazinator> changeFunc, bool exploreOnlyDeserializedChildren, bool changeThisLevel)
        {
            var deserialized_MyNullableT = MyNullableT;
            _MyNullableT = (T?) _MyNullableT.Value.ForEachLazinator(changeFunc, exploreOnlyDeserializedChildren, true);var deserialized_MyT = MyT;
            _MyT = (T) _MyT.ForEachLazinator(changeFunc, exploreOnlyDeserializedChildren, true);if ((!exploreOnlyDeserializedChildren && MyU != null) || (_MyU_Accessed && _MyU != null))
            {
                _MyU = (U) _MyU.ForEachLazinator(changeFunc, exploreOnlyDeserializedChildren, true);
            }
            if (changeThisLevel && changeFunc != null)
            {
                return changeFunc(this);
            }
            return this;
        }
        
        public virtual void FreeInMemoryObjects()
        {
            _MyNullableT = default;
            _MyT = default;
            _MyU = default;
            _MyNullableT_Accessed = _MyT_Accessed = _MyU_Accessed = false;
            IsDirty = false;
            DescendantIsDirty = false;
            HasChanged = false;
            DescendantHasChanged = false;
        }
        
        /* Conversion */
        
        public virtual int LazinatorUniqueID => 1085;
        
        protected virtual bool ContainsOpenGenericParameters => true;
        public virtual LazinatorGenericIDType LazinatorGenericID => LazinatorGenericIDType.GetCachedForType<ConstrainedGeneric<T, U>>(() => DeserializationFactory.Instance.GetUniqueIDListForGenericType(1085, new Type[] { typeof(T), typeof(U) }));
        
        
        public virtual int LazinatorObjectVersion { get; set; } = 0;
        
        
        protected virtual void ConvertFromBytesAfterHeader(IncludeChildrenMode includeChildrenMode, int serializedVersionNumber, ref int bytesSoFar)
        {
            ReadOnlySpan<byte> span = LazinatorMemoryStorage.InitialMemory.Span;
            ConvertFromBytesForPrimitiveProperties(span, includeChildrenMode, serializedVersionNumber, ref bytesSoFar);
            ConvertFromBytesForChildProperties(span, includeChildrenMode, serializedVersionNumber, bytesSoFar + 12, ref bytesSoFar);
        }
        
        protected virtual void ConvertFromBytesForPrimitiveProperties(ReadOnlySpan<byte> span, IncludeChildrenMode includeChildrenMode, int serializedVersionNumber, ref int bytesSoFar)
        {
        }
        
        protected virtual int ConvertFromBytesForChildProperties(ReadOnlySpan<byte> span, IncludeChildrenMode includeChildrenMode, int serializedVersionNumber, int indexOfFirstChild, ref int bytesSoFar)
        {
            int totalChildrenBytes = 0;
            _MyNullableT_ByteIndex = indexOfFirstChild + totalChildrenBytes;
            if (includeChildrenMode != IncludeChildrenMode.ExcludeAllChildren && includeChildrenMode != IncludeChildrenMode.IncludeOnlyIncludableChildren)
            {
                totalChildrenBytes += span.ToInt32(ref bytesSoFar);
            }
            
            _MyT_ByteIndex = indexOfFirstChild + totalChildrenBytes;
            if (includeChildrenMode != IncludeChildrenMode.ExcludeAllChildren && includeChildrenMode != IncludeChildrenMode.IncludeOnlyIncludableChildren)
            {
                totalChildrenBytes += span.ToInt32(ref bytesSoFar);
            }
            
            _MyU_ByteIndex = indexOfFirstChild + totalChildrenBytes;
            if (includeChildrenMode != IncludeChildrenMode.ExcludeAllChildren && includeChildrenMode != IncludeChildrenMode.IncludeOnlyIncludableChildren)
            {
                totalChildrenBytes += span.ToInt32(ref bytesSoFar);
            }
            
            _ConstrainedGeneric_T_U_EndByteIndex = indexOfFirstChild + totalChildrenBytes;
            return totalChildrenBytes;
        }
        
        public virtual void SerializeToExistingBuffer(ref BinaryBufferWriter writer, IncludeChildrenMode includeChildrenMode, bool verifyCleanness, bool updateStoredBuffer)
        {
            TabbedText.WriteLine($"Initiating serialization of LazinatorTests.Examples.NonAbstractGenerics.ConstrainedGeneric<T, U> ");
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
                
                if (_MyNullableT_Accessed && _MyNullableT.Value.IsStruct && (_MyNullableT.Value.IsDirty || _MyNullableT.Value.DescendantIsDirty))
                {
                    _MyNullableT_Accessed = false;
                }
                if (_MyT_Accessed && _MyT.IsStruct && (_MyT.IsDirty || _MyT.DescendantIsDirty))
                {
                    _MyT_Accessed = false;
                }
                if (_MyU_Accessed && _MyU != null && _MyU.IsStruct && (_MyU.IsDirty || _MyU.DescendantIsDirty))
                {
                    _MyU_Accessed = false;
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
            MyNullableT.Value.UpdateStoredBuffer(ref writer, startPosition + _MyNullableT_ByteIndex + sizeof(int), _MyNullableT_ByteLength - sizeof(int), IncludeChildrenMode.IncludeAllChildren, true);
            MyT.UpdateStoredBuffer(ref writer, startPosition + _MyT_ByteIndex + sizeof(int), _MyT_ByteLength - sizeof(int), IncludeChildrenMode.IncludeAllChildren, true);
            if (_MyU_Accessed && _MyU != null)
            {
                MyU.UpdateStoredBuffer(ref writer, startPosition + _MyU_ByteIndex + sizeof(int), _MyU_ByteLength - sizeof(int), IncludeChildrenMode.IncludeAllChildren, true);
            }
            
        }
        
        
        protected virtual void WritePropertiesIntoBuffer(ref BinaryBufferWriter writer, IncludeChildrenMode includeChildrenMode, bool verifyCleanness, bool updateStoredBuffer, bool includeUniqueID)
        {
            int startPosition = writer.Position;
            TabbedText.WriteLine($"Writing properties for LazinatorTests.Examples.NonAbstractGenerics.ConstrainedGeneric<T, U> starting at {writer.Position}.");
            TabbedText.WriteLine($"Includes? uniqueID {(LazinatorGenericID.IsEmpty ? LazinatorUniqueID.ToString() : String.Join("","",LazinatorGenericID.TypeAndInnerTypeIDs.ToArray()))} {includeUniqueID}, Lazinator version {Lazinator.Support.LazinatorVersionInfo.LazinatorIntVersion} True, Object version {LazinatorObjectVersion} True, IncludeChildrenMode {includeChildrenMode} True");
            TabbedText.WriteLine($"IsDirty {IsDirty} DescendantIsDirty {DescendantIsDirty} HasParentClass {LazinatorParents.Any()}");
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
            
            int startOfObjectPosition = writer.Position;
            Span<byte> lengthsSpan = writer.FreeSpan.Slice(0, 12);
            writer.Skip(12);
            WriteChildrenPropertiesIntoBuffer(ref writer, includeChildrenMode, verifyCleanness, updateStoredBuffer, includeUniqueID, startOfObjectPosition, lengthsSpan);
            TabbedText.WriteLine($"Byte {writer.Position} (end of ConstrainedGeneric<T, U>) ");
        }
        
        protected virtual void WritePrimitivePropertiesIntoBuffer(ref BinaryBufferWriter writer, IncludeChildrenMode includeChildrenMode, bool verifyCleanness, bool updateStoredBuffer, bool includeUniqueID)
        {
        }
        
        protected virtual void WriteChildrenPropertiesIntoBuffer(ref BinaryBufferWriter writer, IncludeChildrenMode includeChildrenMode, bool verifyCleanness, bool updateStoredBuffer, bool includeUniqueID, int startOfObjectPosition, Span<byte> lengthsSpan)
        {
            int startOfChildPosition = 0;
            int lengthValue = 0;
            TabbedText.WriteLine($"Byte {writer.Position}, MyNullableT value {_MyNullableT}");
            TabbedText.Tabs++;
            startOfChildPosition = writer.Position;
            if (includeChildrenMode != IncludeChildrenMode.ExcludeAllChildren && includeChildrenMode != IncludeChildrenMode.IncludeOnlyIncludableChildren)
            {
                if ((includeChildrenMode != IncludeChildrenMode.IncludeAllChildren || includeChildrenMode != OriginalIncludeChildrenMode) && !_MyNullableT_Accessed)
                {
                    var deserialized = MyNullableT;
                }
                var copy = _MyNullableT.Value;
                WriteChild(ref writer, ref copy, includeChildrenMode, _MyNullableT_Accessed, () => GetChildSlice(LazinatorMemoryStorage, _MyNullableT_ByteIndex, _MyNullableT_ByteLength, true, false, null), verifyCleanness, updateStoredBuffer, false, true, this);
            }
            lengthValue = writer.Position - startOfChildPosition;
            WriteInt(lengthsSpan, lengthValue);
            lengthsSpan = lengthsSpan.Slice(sizeof(int));
            if (updateStoredBuffer)
            {
                _MyNullableT_ByteIndex = writer.Position - startOfObjectPosition;
            }
            TabbedText.Tabs--;
            TabbedText.WriteLine($"Byte {writer.Position}, MyT value {_MyT}");
            TabbedText.Tabs++;
            startOfChildPosition = writer.Position;
            if (includeChildrenMode != IncludeChildrenMode.ExcludeAllChildren && includeChildrenMode != IncludeChildrenMode.IncludeOnlyIncludableChildren)
            {
                if ((includeChildrenMode != IncludeChildrenMode.IncludeAllChildren || includeChildrenMode != OriginalIncludeChildrenMode) && !_MyT_Accessed)
                {
                    var deserialized = MyT;
                }
                WriteChild(ref writer, ref _MyT, includeChildrenMode, _MyT_Accessed, () => GetChildSlice(LazinatorMemoryStorage, _MyT_ByteIndex, _MyT_ByteLength, true, false, null), verifyCleanness, updateStoredBuffer, false, true, this);
            }
            lengthValue = writer.Position - startOfChildPosition;
            WriteInt(lengthsSpan, lengthValue);
            lengthsSpan = lengthsSpan.Slice(sizeof(int));
            if (updateStoredBuffer)
            {
                _MyT_ByteIndex = writer.Position - startOfObjectPosition;
            }
            TabbedText.Tabs--;
            TabbedText.WriteLine($"Byte {writer.Position}, MyU value {_MyU}");
            TabbedText.Tabs++;
            startOfChildPosition = writer.Position;
            if (includeChildrenMode != IncludeChildrenMode.ExcludeAllChildren && includeChildrenMode != IncludeChildrenMode.IncludeOnlyIncludableChildren)
            {
                if ((includeChildrenMode != IncludeChildrenMode.IncludeAllChildren || includeChildrenMode != OriginalIncludeChildrenMode) && !_MyU_Accessed)
                {
                    var deserialized = MyU;
                }
                WriteChild(ref writer, ref _MyU, includeChildrenMode, _MyU_Accessed, () => GetChildSlice(LazinatorMemoryStorage, _MyU_ByteIndex, _MyU_ByteLength, true, false, null), verifyCleanness, updateStoredBuffer, false, true, this);
            }
            lengthValue = writer.Position - startOfChildPosition;
            WriteInt(lengthsSpan, lengthValue);
            lengthsSpan = lengthsSpan.Slice(sizeof(int));
            if (updateStoredBuffer)
            {
                _MyU_ByteIndex = writer.Position - startOfObjectPosition;
            }
            TabbedText.Tabs--;
            if (updateStoredBuffer)
            {
                _ConstrainedGeneric_T_U_EndByteIndex = writer.Position - startOfObjectPosition;
            }
        }
        
    }
}
