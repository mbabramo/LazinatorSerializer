//c2bf49d1-90e9-a04c-5991-f0cd89a4e5b5
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by the Lazinator tool, version 0.1.0.392
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
    using LazinatorTests.Examples.Abstract;
    using System;
    using System.Buffers;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Runtime.InteropServices;
    using static Lazinator.Core.LazinatorUtilities;
    
    [Autogenerated]
    public partial class UnofficialInterfaceIncorporator : ILazinator
    {
        public bool IsStruct => false;
        
        /* Property definitions */
        
        protected int _MyOfficialObject_ByteIndex;
        protected int _MyUnofficialObject_ByteIndex;
        protected virtual int _MyOfficialObject_ByteLength => _MyUnofficialObject_ByteIndex - _MyOfficialObject_ByteIndex;
        private int _UnofficialInterfaceIncorporator_EndByteIndex;
        protected virtual int _MyUnofficialObject_ByteLength => _UnofficialInterfaceIncorporator_EndByteIndex - _MyUnofficialObject_ByteIndex;
        
        
        protected long _MyOfficialLong;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public long MyOfficialLong
        {
            get
            {
                return _MyOfficialLong;
            }
            set
            {
                IsDirty = true;
                _MyOfficialLong = value;
            }
        }
        
        protected int _MyUnofficialInt;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int MyUnofficialInt
        {
            get
            {
                return _MyUnofficialInt;
            }
            set
            {
                IsDirty = true;
                _MyUnofficialInt = value;
            }
        }
        
        protected Concrete5 _MyOfficialObject;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public Concrete5 MyOfficialObject
        {
            get
            {
                if (!_MyOfficialObject_Accessed)
                {
                    Lazinate_MyOfficialObject();
                } 
                return _MyOfficialObject;
            }
            set
            {
                if (_MyOfficialObject != null)
                {
                    _MyOfficialObject.LazinatorParents = _MyOfficialObject.LazinatorParents.WithRemoved(this);
                }
                if (value != null)
                {
                    value.LazinatorParents = value.LazinatorParents.WithAdded(this);
                }
                
                IsDirty = true;
                DescendantIsDirty = true;
                _MyOfficialObject = value;
                _MyOfficialObject_Accessed = true;
            }
        }
        protected bool _MyOfficialObject_Accessed;
        private void Lazinate_MyOfficialObject()
        {
            if (LazinatorObjectBytes.Length == 0)
            {
                _MyOfficialObject = null;
            }
            else
            {
                LazinatorMemory childData = GetChildSlice(LazinatorMemoryStorage, _MyOfficialObject_ByteIndex, _MyOfficialObject_ByteLength, false, false, null);
                
                _MyOfficialObject = DeserializationFactory.Instance.CreateBaseOrDerivedType(1039, (c, p) => new Concrete5(c, p), childData, this); 
            }
            
            _MyOfficialObject_Accessed = true;
        }
        
        
        protected Concrete3 _MyUnofficialObject;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private Concrete3 MyUnofficialObject
        {
            get
            {
                if (!_MyUnofficialObject_Accessed)
                {
                    Lazinate_MyUnofficialObject();
                } 
                return _MyUnofficialObject;
            }
            set
            {
                if (_MyUnofficialObject != null)
                {
                    _MyUnofficialObject.LazinatorParents = _MyUnofficialObject.LazinatorParents.WithRemoved(this);
                }
                if (value != null)
                {
                    value.LazinatorParents = value.LazinatorParents.WithAdded(this);
                }
                
                IsDirty = true;
                DescendantIsDirty = true;
                _MyUnofficialObject = value;
                _MyUnofficialObject_Accessed = true;
            }
        }
        protected bool _MyUnofficialObject_Accessed;
        private void Lazinate_MyUnofficialObject()
        {
            if (LazinatorObjectBytes.Length == 0)
            {
                _MyUnofficialObject = null;
            }
            else
            {
                LazinatorMemory childData = GetChildSlice(LazinatorMemoryStorage, _MyUnofficialObject_ByteIndex, _MyUnofficialObject_ByteLength, false, false, null);
                
                _MyUnofficialObject = DeserializationFactory.Instance.CreateBaseOrDerivedType(1037, (c, p) => new Concrete3(c, p), childData, this); 
            }
            
            _MyUnofficialObject_Accessed = true;
        }
        
        
        /* Serialization, deserialization, and object relationships */
        
        public UnofficialInterfaceIncorporator(IncludeChildrenMode originalIncludeChildrenMode = IncludeChildrenMode.IncludeAllChildren)
        {
            OriginalIncludeChildrenMode = originalIncludeChildrenMode;
        }
        
        public UnofficialInterfaceIncorporator(LazinatorMemory serializedBytes, ILazinator parent = null)
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
            UnofficialInterfaceIncorporator clone;
            if (cloneBufferOptions == CloneBufferOptions.NoBuffer)
            {
                clone = new UnofficialInterfaceIncorporator(includeChildrenMode);
                clone.LazinatorObjectVersion = LazinatorObjectVersion;
                clone = (UnofficialInterfaceIncorporator)AssignCloneProperties(clone, includeChildrenMode);
            }
            else
            {
                LazinatorMemory bytes = EncodeOrRecycleToNewBuffer(includeChildrenMode, OriginalIncludeChildrenMode, false, IsDirty, DescendantIsDirty, false, LazinatorMemoryStorage, false, this);
                clone = new UnofficialInterfaceIncorporator(bytes);
            }
            return clone;
        }
        
        public virtual ILazinator AssignCloneProperties(ILazinator clone, IncludeChildrenMode includeChildrenMode)
        {
            clone.FreeInMemoryObjects();
            UnofficialInterfaceIncorporator typedClone = (UnofficialInterfaceIncorporator) clone;
            typedClone.MyOfficialLong = MyOfficialLong;
            typedClone.MyUnofficialInt = MyUnofficialInt;
            if (includeChildrenMode != IncludeChildrenMode.ExcludeAllChildren && includeChildrenMode != IncludeChildrenMode.IncludeOnlyIncludableChildren)
            {
                if (MyOfficialObject == null)
                {
                    typedClone.MyOfficialObject = null;
                }
                else
                {
                    typedClone.MyOfficialObject = (Concrete5) MyOfficialObject.CloneLazinator(includeChildrenMode, CloneBufferOptions.NoBuffer);
                }
                
            }
            
            if (includeChildrenMode != IncludeChildrenMode.ExcludeAllChildren && includeChildrenMode != IncludeChildrenMode.IncludeOnlyIncludableChildren)
            {
                if (MyUnofficialObject == null)
                {
                    typedClone.MyUnofficialObject = null;
                }
                else
                {
                    typedClone.MyUnofficialObject = (Concrete3) MyUnofficialObject.CloneLazinator(includeChildrenMode, CloneBufferOptions.NoBuffer);
                }
                
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
            if (enumerateNulls && (!exploreOnlyDeserializedChildren || _MyOfficialObject_Accessed) && MyOfficialObject == null)
            {
                yield return ("MyOfficialObject", default);
            }
            else
            {
                if ((!exploreOnlyDeserializedChildren && MyOfficialObject != null) || (_MyOfficialObject_Accessed && _MyOfficialObject != null))
                {
                    bool isMatch_MyOfficialObject = matchCriterion == null || matchCriterion(MyOfficialObject);
                    bool shouldExplore_MyOfficialObject = exploreCriterion == null || exploreCriterion(MyOfficialObject);
                    if (isMatch_MyOfficialObject)
                    {
                        yield return ("MyOfficialObject", MyOfficialObject);
                    }
                    if ((!stopExploringBelowMatch || !isMatch_MyOfficialObject) && shouldExplore_MyOfficialObject)
                    {
                        foreach (var toYield in MyOfficialObject.EnumerateLazinatorDescendants(matchCriterion, stopExploringBelowMatch, exploreCriterion, exploreOnlyDeserializedChildren, enumerateNulls))
                        {
                            yield return ("MyOfficialObject" + "." + toYield.propertyName, toYield.descendant);
                        }
                    }
                }
                
            }
            
            if (enumerateNulls && (!exploreOnlyDeserializedChildren || _MyUnofficialObject_Accessed) && MyUnofficialObject == null)
            {
                yield return ("MyUnofficialObject", default);
            }
            else
            {
                if ((!exploreOnlyDeserializedChildren && MyUnofficialObject != null) || (_MyUnofficialObject_Accessed && _MyUnofficialObject != null))
                {
                    bool isMatch_MyUnofficialObject = matchCriterion == null || matchCriterion(MyUnofficialObject);
                    bool shouldExplore_MyUnofficialObject = exploreCriterion == null || exploreCriterion(MyUnofficialObject);
                    if (isMatch_MyUnofficialObject)
                    {
                        yield return ("MyUnofficialObject", MyUnofficialObject);
                    }
                    if ((!stopExploringBelowMatch || !isMatch_MyUnofficialObject) && shouldExplore_MyUnofficialObject)
                    {
                        foreach (var toYield in MyUnofficialObject.EnumerateLazinatorDescendants(matchCriterion, stopExploringBelowMatch, exploreCriterion, exploreOnlyDeserializedChildren, enumerateNulls))
                        {
                            yield return ("MyUnofficialObject" + "." + toYield.propertyName, toYield.descendant);
                        }
                    }
                }
                
            }
            
            yield break;
        }
        
        
        public virtual IEnumerable<(string propertyName, object descendant)> EnumerateNonLazinatorProperties()
        {
            yield return ("MyOfficialLong", (object)MyOfficialLong);
            yield return ("MyUnofficialInt", (object)MyUnofficialInt);
            yield break;
        }
        
        public virtual ILazinator ForEachLazinator(Func<ILazinator, ILazinator> changeFunc, bool exploreOnlyDeserializedChildren, bool changeThisLevel)
        {
            if ((!exploreOnlyDeserializedChildren && MyOfficialObject != null) || (_MyOfficialObject_Accessed && _MyOfficialObject != null))
            {
                _MyOfficialObject = (Concrete5) _MyOfficialObject.ForEachLazinator(changeFunc, exploreOnlyDeserializedChildren, true);
            }
            if ((!exploreOnlyDeserializedChildren && MyUnofficialObject != null) || (_MyUnofficialObject_Accessed && _MyUnofficialObject != null))
            {
                _MyUnofficialObject = (Concrete3) _MyUnofficialObject.ForEachLazinator(changeFunc, exploreOnlyDeserializedChildren, true);
            }
            if (changeThisLevel && changeFunc != null)
            {
                return changeFunc(this);
            }
            return this;
        }
        
        public virtual void FreeInMemoryObjects()
        {
            _MyOfficialObject = default;
            _MyUnofficialObject = default;
            _MyOfficialObject_Accessed = _MyUnofficialObject_Accessed = false;
            IsDirty = false;
            DescendantIsDirty = false;
            HasChanged = false;
            DescendantHasChanged = false;
        }
        
        /* Conversion */
        
        public virtual int LazinatorUniqueID => 1031;
        
        protected virtual bool ContainsOpenGenericParameters => false;
        public virtual LazinatorGenericIDType LazinatorGenericID => default;
        
        
        public virtual int LazinatorObjectVersion { get; set; } = 0;
        
        
        public virtual void ConvertFromBytesAfterHeader(IncludeChildrenMode includeChildrenMode, int serializedVersionNumber, ref int bytesSoFar)
        {
            ReadOnlySpan<byte> span = LazinatorObjectBytes.Span;
            _MyOfficialLong = span.ToDecompressedLong(ref bytesSoFar);
            _MyUnofficialInt = span.ToDecompressedInt(ref bytesSoFar);
            _MyOfficialObject_ByteIndex = bytesSoFar;
            if (includeChildrenMode != IncludeChildrenMode.ExcludeAllChildren && includeChildrenMode != IncludeChildrenMode.IncludeOnlyIncludableChildren)
            {
                bytesSoFar = span.ToInt32(ref bytesSoFar) + bytesSoFar;
            }
            
            _MyUnofficialObject_ByteIndex = bytesSoFar;
            if (includeChildrenMode != IncludeChildrenMode.ExcludeAllChildren && includeChildrenMode != IncludeChildrenMode.IncludeOnlyIncludableChildren)
            {
                bytesSoFar = span.ToInt32(ref bytesSoFar) + bytesSoFar;
            }
            
            _UnofficialInterfaceIncorporator_EndByteIndex = bytesSoFar;
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
                ThrowHelper.ThrowCannotUpdateStoredBuffer();
            }
            
            var newBuffer = writer.Slice(startPosition, length);
            LazinatorMemoryStorage = newBuffer;
        }
        
        protected virtual void UpdateDeserializedChildren(ref BinaryBufferWriter writer, int startPosition)
        {
            if (_MyOfficialObject_Accessed && _MyOfficialObject != null)
            {
                MyOfficialObject.UpdateStoredBuffer(ref writer, startPosition + _MyOfficialObject_ByteIndex + sizeof(int), _MyOfficialObject_ByteLength - sizeof(int), IncludeChildrenMode.IncludeAllChildren, true);
            }
            
            if (_MyUnofficialObject_Accessed && _MyUnofficialObject != null)
            {
                MyUnofficialObject.UpdateStoredBuffer(ref writer, startPosition + _MyUnofficialObject_ByteIndex + sizeof(int), _MyUnofficialObject_ByteLength - sizeof(int), IncludeChildrenMode.IncludeAllChildren, true);
            }
            
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
            CompressedIntegralTypes.WriteCompressedLong(ref writer, _MyOfficialLong);
            CompressedIntegralTypes.WriteCompressedInt(ref writer, _MyUnofficialInt);
            startOfObjectPosition = writer.Position;
            if (includeChildrenMode != IncludeChildrenMode.ExcludeAllChildren && includeChildrenMode != IncludeChildrenMode.IncludeOnlyIncludableChildren)
            {
                if ((includeChildrenMode != IncludeChildrenMode.IncludeAllChildren || includeChildrenMode != OriginalIncludeChildrenMode) && !_MyOfficialObject_Accessed)
                {
                    var deserialized = MyOfficialObject;
                }
                WriteChild(ref writer, ref _MyOfficialObject, includeChildrenMode, _MyOfficialObject_Accessed, () => GetChildSlice(LazinatorMemoryStorage, _MyOfficialObject_ByteIndex, _MyOfficialObject_ByteLength, false, false, null), verifyCleanness, updateStoredBuffer, false, false, this);
            }
            
            if (updateStoredBuffer)
            {
                _MyOfficialObject_ByteIndex = startOfObjectPosition - startPosition;
            }
            startOfObjectPosition = writer.Position;
            if (includeChildrenMode != IncludeChildrenMode.ExcludeAllChildren && includeChildrenMode != IncludeChildrenMode.IncludeOnlyIncludableChildren)
            {
                if ((includeChildrenMode != IncludeChildrenMode.IncludeAllChildren || includeChildrenMode != OriginalIncludeChildrenMode) && !_MyUnofficialObject_Accessed)
                {
                    var deserialized = MyUnofficialObject;
                }
                WriteChild(ref writer, ref _MyUnofficialObject, includeChildrenMode, _MyUnofficialObject_Accessed, () => GetChildSlice(LazinatorMemoryStorage, _MyUnofficialObject_ByteIndex, _MyUnofficialObject_ByteLength, false, false, null), verifyCleanness, updateStoredBuffer, false, false, this);
            }
            
            if (updateStoredBuffer)
            {
                _MyUnofficialObject_ByteIndex = startOfObjectPosition - startPosition;
            }
            if (updateStoredBuffer)
            {
                _UnofficialInterfaceIncorporator_EndByteIndex = writer.Position - startPosition;
            }
        }
        
    }
}
