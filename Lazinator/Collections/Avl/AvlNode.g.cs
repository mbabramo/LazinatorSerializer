//ab814f04-6382-3413-2eca-027e70db9d6d
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by the Lazinator tool, version 0.1.0.232
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Lazinator.Collections.Avl
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
    public sealed partial class AvlNode<TKey, TValue> : ILazinator
    {
        public bool IsStruct => false;
        
        /* Serialization, deserialization, and object relationships */
        
        public AvlNode() : base()
        {
        }
        
        public LazinatorParentsCollection LazinatorParents { get; set; }
        
        public IncludeChildrenMode OriginalIncludeChildrenMode { get; set; }
        
        public int Deserialize()
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
        
        public LazinatorMemory SerializeLazinator(IncludeChildrenMode includeChildrenMode, bool verifyCleanness, bool updateStoredBuffer)
        {
            return EncodeOrRecycleToNewBuffer(includeChildrenMode, OriginalIncludeChildrenMode, verifyCleanness, IsDirty, DescendantIsDirty, false, LazinatorMemoryStorage, (EncodeManuallyDelegate) EncodeToNewBuffer, updateStoredBuffer);
        }
        
        LazinatorMemory EncodeToNewBuffer(IncludeChildrenMode includeChildrenMode, bool verifyCleanness, bool updateStoredBuffer) => LazinatorUtilities.EncodeToNewBinaryBufferWriter(this, includeChildrenMode, verifyCleanness, updateStoredBuffer);
        
        public ILazinator CloneLazinator(IncludeChildrenMode includeChildrenMode = IncludeChildrenMode.IncludeAllChildren, CloneBufferOptions cloneBufferOptions = CloneBufferOptions.LinkedBuffer)
        {
            var clone = new AvlNode<TKey, TValue>()
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
        
        void AssignCloneProperties(ILazinator clone, IncludeChildrenMode includeChildrenMode)
        {
            clone.FreeInMemoryObjects();
            AvlNode<TKey, TValue> typedClone = (AvlNode<TKey, TValue>) clone;
            typedClone.Balance = Balance;
            typedClone.Count = Count;
            if (includeChildrenMode != IncludeChildrenMode.ExcludeAllChildren && includeChildrenMode != IncludeChildrenMode.IncludeOnlyIncludableChildren) 
            {
                typedClone.Key = (System.Collections.Generic.EqualityComparer<TKey>.Default.Equals(Key, default(TKey))) ? default(TKey) : (TKey) Key.CloneLazinator(includeChildrenMode, CloneBufferOptions.NoBuffer);
            }
            if (includeChildrenMode != IncludeChildrenMode.ExcludeAllChildren && includeChildrenMode != IncludeChildrenMode.IncludeOnlyIncludableChildren) 
            {
                typedClone.Left = (Left == null) ? default(AvlNode<TKey, TValue>) : (AvlNode<TKey, TValue>) Left.CloneLazinator(includeChildrenMode, CloneBufferOptions.NoBuffer);
            }
            if (includeChildrenMode != IncludeChildrenMode.ExcludeAllChildren && includeChildrenMode != IncludeChildrenMode.IncludeOnlyIncludableChildren) 
            {
                typedClone.Right = (Right == null) ? default(AvlNode<TKey, TValue>) : (AvlNode<TKey, TValue>) Right.CloneLazinator(includeChildrenMode, CloneBufferOptions.NoBuffer);
            }
            if (includeChildrenMode != IncludeChildrenMode.ExcludeAllChildren && includeChildrenMode != IncludeChildrenMode.IncludeOnlyIncludableChildren) 
            {
                typedClone.Value = (System.Collections.Generic.EqualityComparer<TValue>.Default.Equals(Value, default(TValue))) ? default(TValue) : (TValue) Value.CloneLazinator(includeChildrenMode, CloneBufferOptions.NoBuffer);
            }
        }
        
        public bool HasChanged { get; set; }
        
        bool _IsDirty;
        public bool IsDirty
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
        
        bool _DescendantHasChanged;
        public bool DescendantHasChanged
        {
            [DebuggerStepThrough]
            get => _DescendantHasChanged;
            [DebuggerStepThrough]
            set
            {
                _DescendantHasChanged = value;
            }
        }
        
        bool _DescendantIsDirty;
        public bool DescendantIsDirty
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
        
        public void DeserializeLazinator(LazinatorMemory serializedBytes)
        {
            LazinatorMemoryStorage = serializedBytes;
        }
        
        LazinatorMemory _LazinatorMemoryStorage;
        public LazinatorMemory LazinatorMemoryStorage
        {
            get => _LazinatorMemoryStorage;
            set
            {
                _LazinatorMemoryStorage = value;
                int length = Deserialize();
                if (length != _LazinatorMemoryStorage.Length)
                {
                    _LazinatorMemoryStorage = _LazinatorMemoryStorage.Slice(0, length);
                }
            }
        }
        ReadOnlyMemory<byte> LazinatorObjectBytes => LazinatorMemoryStorage?.Memory ?? LazinatorUtilities.EmptyReadOnlyMemory;
        
        public void EnsureLazinatorMemoryUpToDate()
        {
            if (!IsDirty && !DescendantIsDirty && LazinatorObjectBytes.Length > 0 && OriginalIncludeChildrenMode == IncludeChildrenMode.IncludeAllChildren)
            {
                return;
            }
            EncodeOrRecycleToNewBuffer(IncludeChildrenMode.IncludeAllChildren, OriginalIncludeChildrenMode, false, IsDirty, DescendantIsDirty, false, LazinatorMemoryStorage, (EncodeManuallyDelegate)EncodeToNewBuffer, true);
            OriginalIncludeChildrenMode = IncludeChildrenMode.IncludeAllChildren;
        }
        
        public int GetByteLength()
        {
            EnsureLazinatorMemoryUpToDate();
            return LazinatorObjectBytes.Length;
        }
        
        public uint GetBinaryHashCode32()
        {
            EnsureLazinatorMemoryUpToDate();
            return FarmhashByteSpans.Hash32(LazinatorObjectBytes.Span);
        }
        
        public ulong GetBinaryHashCode64()
        {
            EnsureLazinatorMemoryUpToDate();
            return FarmhashByteSpans.Hash64(LazinatorObjectBytes.Span);
        }
        
        public Guid GetBinaryHashCode128()
        {
            EnsureLazinatorMemoryUpToDate();
            return FarmhashByteSpans.Hash128(LazinatorObjectBytes.Span);
        }
        
        /* Property definitions */
        
        int _Key_ByteIndex;
        int _Left_ByteIndex;
        int _Right_ByteIndex;
        int _Value_ByteIndex;
        int _Key_ByteLength => _Left_ByteIndex - _Key_ByteIndex;
        int _Left_ByteLength => _Right_ByteIndex - _Left_ByteIndex;
        int _Right_ByteLength => _Value_ByteIndex - _Right_ByteIndex;
        private int _AvlNode_TKey_TValue_EndByteIndex = 0;
        int _Value_ByteLength => _AvlNode_TKey_TValue_EndByteIndex - _Value_ByteIndex;
        
        int _Balance;
        public int Balance
        {
            [DebuggerStepThrough]
            get
            {
                return _Balance;
            }
            [DebuggerStepThrough]
            set
            {
                IsDirty = true;
                _Balance = value;
            }
        }
        int _Count;
        public int Count
        {
            [DebuggerStepThrough]
            get
            {
                return _Count;
            }
            [DebuggerStepThrough]
            set
            {
                IsDirty = true;
                _Count = value;
            }
        }
        TKey _Key;
        public TKey Key
        {
            [DebuggerStepThrough]
            get
            {
                if (!_Key_Accessed)
                {
                    if (LazinatorObjectBytes.Length == 0)
                    {
                        _Key = default(TKey);
                        if (_Key != null)
                        { // Key is a struct
                            _Key.LazinatorParents = new LazinatorParentsCollection(this);
                        }
                    }
                    else
                    {
                        LazinatorMemory childData = GetChildSlice(LazinatorMemoryStorage, _Key_ByteIndex, _Key_ByteLength, false, false, null);
                        
                        _Key = DeserializationFactory.Instance.CreateBasedOnType<TKey>(childData, this); 
                    }
                    _Key_Accessed = true;
                } 
                return _Key;
            }
            [DebuggerStepThrough]
            set
            {
                if (value != null && value.IsStruct)
                {
                    value.LazinatorParents = new LazinatorParentsCollection(this);
                }
                else
                {
                    if (_Key != null)
                    {
                        _Key.LazinatorParents = _Key.LazinatorParents.WithRemoved(this);
                    }
                    if (value != null)
                    {
                        value.LazinatorParents = value.LazinatorParents.WithAdded(this);
                    }
                }
                
                IsDirty = true;
                DescendantIsDirty = true;
                _Key = value;
                _Key_Accessed = true;
            }
        }
        bool _Key_Accessed;
        AvlNode<TKey, TValue> _Left;
        public AvlNode<TKey, TValue> Left
        {
            [DebuggerStepThrough]
            get
            {
                if (!_Left_Accessed)
                {
                    if (LazinatorObjectBytes.Length == 0)
                    {
                        _Left = default(AvlNode<TKey, TValue>);
                    }
                    else
                    {
                        LazinatorMemory childData = GetChildSlice(LazinatorMemoryStorage, _Left_ByteIndex, _Left_ByteLength, false, false, null);
                        if (childData.Length == 0)
                        {
                            _Left = default;
                        }
                        else _Left = new AvlNode<TKey, TValue>()
                        {
                            LazinatorParents = new LazinatorParentsCollection(this),
                            LazinatorMemoryStorage = childData,
                        };
                    }
                    _Left_Accessed = true;
                } 
                return _Left;
            }
            [DebuggerStepThrough]
            set
            {
                if (_Left != null)
                {
                    _Left.LazinatorParents = _Left.LazinatorParents.WithRemoved(this);
                }
                if (value != null)
                {
                    value.LazinatorParents = value.LazinatorParents.WithAdded(this);
                }
                
                IsDirty = true;
                DescendantIsDirty = true;
                _Left = value;
                _Left_Accessed = true;
            }
        }
        bool _Left_Accessed;
        AvlNode<TKey, TValue> _Right;
        public AvlNode<TKey, TValue> Right
        {
            [DebuggerStepThrough]
            get
            {
                if (!_Right_Accessed)
                {
                    if (LazinatorObjectBytes.Length == 0)
                    {
                        _Right = default(AvlNode<TKey, TValue>);
                    }
                    else
                    {
                        LazinatorMemory childData = GetChildSlice(LazinatorMemoryStorage, _Right_ByteIndex, _Right_ByteLength, false, false, null);
                        if (childData.Length == 0)
                        {
                            _Right = default;
                        }
                        else _Right = new AvlNode<TKey, TValue>()
                        {
                            LazinatorParents = new LazinatorParentsCollection(this),
                            LazinatorMemoryStorage = childData,
                        };
                    }
                    _Right_Accessed = true;
                } 
                return _Right;
            }
            [DebuggerStepThrough]
            set
            {
                if (_Right != null)
                {
                    _Right.LazinatorParents = _Right.LazinatorParents.WithRemoved(this);
                }
                if (value != null)
                {
                    value.LazinatorParents = value.LazinatorParents.WithAdded(this);
                }
                
                IsDirty = true;
                DescendantIsDirty = true;
                _Right = value;
                _Right_Accessed = true;
            }
        }
        bool _Right_Accessed;
        TValue _Value;
        public TValue Value
        {
            [DebuggerStepThrough]
            get
            {
                if (!_Value_Accessed)
                {
                    if (LazinatorObjectBytes.Length == 0)
                    {
                        _Value = default(TValue);
                        if (_Value != null)
                        { // Value is a struct
                            _Value.LazinatorParents = new LazinatorParentsCollection(this);
                        }
                    }
                    else
                    {
                        LazinatorMemory childData = GetChildSlice(LazinatorMemoryStorage, _Value_ByteIndex, _Value_ByteLength, false, false, null);
                        
                        _Value = DeserializationFactory.Instance.CreateBasedOnType<TValue>(childData, this); 
                    }
                    _Value_Accessed = true;
                } 
                return _Value;
            }
            [DebuggerStepThrough]
            set
            {
                if (value != null && value.IsStruct)
                {
                    value.LazinatorParents = new LazinatorParentsCollection(this);
                }
                else
                {
                    if (_Value != null)
                    {
                        _Value.LazinatorParents = _Value.LazinatorParents.WithRemoved(this);
                    }
                    if (value != null)
                    {
                        value.LazinatorParents = value.LazinatorParents.WithAdded(this);
                    }
                }
                
                IsDirty = true;
                DescendantIsDirty = true;
                _Value = value;
                _Value_Accessed = true;
            }
        }
        bool _Value_Accessed;
        
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
        
        public IEnumerable<(string propertyName, ILazinator descendant)> EnumerateLazinatorDescendants(Func<ILazinator, bool> matchCriterion, bool stopExploringBelowMatch, Func<ILazinator, bool> exploreCriterion, bool exploreOnlyDeserializedChildren, bool enumerateNulls)
        {
            if (enumerateNulls && (!exploreOnlyDeserializedChildren || _Key_Accessed) && (System.Collections.Generic.EqualityComparer<TKey>.Default.Equals(Key, default(TKey))))
            {
                yield return ("Key", default);
            }
            else if ((!exploreOnlyDeserializedChildren && !System.Collections.Generic.EqualityComparer<TKey>.Default.Equals(Key, default(TKey))) || (_Key_Accessed && !System.Collections.Generic.EqualityComparer<TKey>.Default.Equals(_Key, default(TKey))))
            {
                bool isMatch = matchCriterion == null || matchCriterion(Key);
                bool shouldExplore = exploreCriterion == null || exploreCriterion(Key);
                if (isMatch)
                {
                    yield return ("Key", Key);
                }
                if ((!stopExploringBelowMatch || !isMatch) && shouldExplore)
                {
                    foreach (var toYield in Key.EnumerateLazinatorDescendants(matchCriterion, stopExploringBelowMatch, exploreCriterion, exploreOnlyDeserializedChildren, enumerateNulls))
                    {
                        yield return ("Key" + "." + toYield.propertyName, toYield.descendant);
                    }
                }
            }
            if (enumerateNulls && (!exploreOnlyDeserializedChildren || _Left_Accessed) && (Left == null))
            {
                yield return ("Left", default);
            }
            else if ((!exploreOnlyDeserializedChildren && Left != null) || (_Left_Accessed && _Left != null))
            {
                bool isMatch = matchCriterion == null || matchCriterion(Left);
                bool shouldExplore = exploreCriterion == null || exploreCriterion(Left);
                if (isMatch)
                {
                    yield return ("Left", Left);
                }
                if ((!stopExploringBelowMatch || !isMatch) && shouldExplore)
                {
                    foreach (var toYield in Left.EnumerateLazinatorDescendants(matchCriterion, stopExploringBelowMatch, exploreCriterion, exploreOnlyDeserializedChildren, enumerateNulls))
                    {
                        yield return ("Left" + "." + toYield.propertyName, toYield.descendant);
                    }
                }
            }
            if (enumerateNulls && (!exploreOnlyDeserializedChildren || _Right_Accessed) && (Right == null))
            {
                yield return ("Right", default);
            }
            else if ((!exploreOnlyDeserializedChildren && Right != null) || (_Right_Accessed && _Right != null))
            {
                bool isMatch = matchCriterion == null || matchCriterion(Right);
                bool shouldExplore = exploreCriterion == null || exploreCriterion(Right);
                if (isMatch)
                {
                    yield return ("Right", Right);
                }
                if ((!stopExploringBelowMatch || !isMatch) && shouldExplore)
                {
                    foreach (var toYield in Right.EnumerateLazinatorDescendants(matchCriterion, stopExploringBelowMatch, exploreCriterion, exploreOnlyDeserializedChildren, enumerateNulls))
                    {
                        yield return ("Right" + "." + toYield.propertyName, toYield.descendant);
                    }
                }
            }
            if (enumerateNulls && (!exploreOnlyDeserializedChildren || _Value_Accessed) && (System.Collections.Generic.EqualityComparer<TValue>.Default.Equals(Value, default(TValue))))
            {
                yield return ("Value", default);
            }
            else if ((!exploreOnlyDeserializedChildren && !System.Collections.Generic.EqualityComparer<TValue>.Default.Equals(Value, default(TValue))) || (_Value_Accessed && !System.Collections.Generic.EqualityComparer<TValue>.Default.Equals(_Value, default(TValue))))
            {
                bool isMatch = matchCriterion == null || matchCriterion(Value);
                bool shouldExplore = exploreCriterion == null || exploreCriterion(Value);
                if (isMatch)
                {
                    yield return ("Value", Value);
                }
                if ((!stopExploringBelowMatch || !isMatch) && shouldExplore)
                {
                    foreach (var toYield in Value.EnumerateLazinatorDescendants(matchCriterion, stopExploringBelowMatch, exploreCriterion, exploreOnlyDeserializedChildren, enumerateNulls))
                    {
                        yield return ("Value" + "." + toYield.propertyName, toYield.descendant);
                    }
                }
            }
            yield break;
        }
        
        
        public IEnumerable<(string propertyName, object descendant)> EnumerateNonLazinatorProperties()
        {
            yield return ("Balance", (object)Balance);
            yield return ("Count", (object)Count);
            yield break;
        }
        
        public void FreeInMemoryObjects()
        {
            _Left = default;
            _Right = default;
            _Left_Accessed = _Right_Accessed = false;
            IsDirty = false;
            DescendantIsDirty = false;
            HasChanged = false;
            DescendantHasChanged = false;
        }
        
        /* Conversion */
        
        public int LazinatorUniqueID => 93;
        
        bool ContainsOpenGenericParameters => true;
        LazinatorGenericIDType _LazinatorGenericID { get; set; }
        public LazinatorGenericIDType LazinatorGenericID
        {
            get
            {
                if (_LazinatorGenericID.IsEmpty)
                {
                    _LazinatorGenericID = DeserializationFactory.Instance.GetUniqueIDListForGenericType(93, new Type[] { typeof(TKey), typeof(TValue) });
                }
                return _LazinatorGenericID;
            }
            set
            {
                _LazinatorGenericID = value;
            }
        }
        
        public int LazinatorObjectVersion { get; set; } = 0;
        
        
        public void ConvertFromBytesAfterHeader(IncludeChildrenMode includeChildrenMode, int serializedVersionNumber, ref int bytesSoFar)
        {
            ReadOnlySpan<byte> span = LazinatorObjectBytes.Span;
            _Balance = span.ToDecompressedInt(ref bytesSoFar);
            _Count = span.ToDecompressedInt(ref bytesSoFar);
            _Key_ByteIndex = bytesSoFar;
            if (includeChildrenMode != IncludeChildrenMode.ExcludeAllChildren && includeChildrenMode != IncludeChildrenMode.IncludeOnlyIncludableChildren) 
            {
                bytesSoFar = span.ToInt32(ref bytesSoFar) + bytesSoFar;
            }
            _Left_ByteIndex = bytesSoFar;
            if (includeChildrenMode != IncludeChildrenMode.ExcludeAllChildren && includeChildrenMode != IncludeChildrenMode.IncludeOnlyIncludableChildren) 
            {
                bytesSoFar = span.ToInt32(ref bytesSoFar) + bytesSoFar;
            }
            _Right_ByteIndex = bytesSoFar;
            if (includeChildrenMode != IncludeChildrenMode.ExcludeAllChildren && includeChildrenMode != IncludeChildrenMode.IncludeOnlyIncludableChildren) 
            {
                bytesSoFar = span.ToInt32(ref bytesSoFar) + bytesSoFar;
            }
            _Value_ByteIndex = bytesSoFar;
            if (includeChildrenMode != IncludeChildrenMode.ExcludeAllChildren && includeChildrenMode != IncludeChildrenMode.IncludeOnlyIncludableChildren) 
            {
                bytesSoFar = span.ToInt32(ref bytesSoFar) + bytesSoFar;
            }
            _AvlNode_TKey_TValue_EndByteIndex = bytesSoFar;
        }
        
        public void SerializeExistingBuffer(ref BinaryBufferWriter writer, IncludeChildrenMode includeChildrenMode, bool verifyCleanness, bool updateStoredBuffer)
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
                    if (_Key_Accessed && _Key != null && _Key.IsStruct && (_Key.IsDirty || _Key.DescendantIsDirty))
                    {
                        _Key_Accessed = false;
                    }
                    if (_Value_Accessed && _Value != null && _Value.IsStruct && (_Value.IsDirty || _Value.DescendantIsDirty))
                    {
                        _Value_Accessed = false;
                    }
                }
                else
                {
                    throw new Exception("Cannot update stored buffer when serializing only some children.");
                }
                
                var newBuffer = writer.Slice(startPosition);
                if (_LazinatorMemoryStorage != null)
                {
                    _LazinatorMemoryStorage.ReplaceWithNewBuffer(newBuffer);
                }
                _LazinatorMemoryStorage = newBuffer;
            }
        }
        void WritePropertiesIntoBuffer(ref BinaryBufferWriter writer, IncludeChildrenMode includeChildrenMode, bool verifyCleanness, bool updateStoredBuffer, bool includeUniqueID)
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
            CompressedIntegralTypes.WriteCompressedInt(ref writer, _Balance);
            CompressedIntegralTypes.WriteCompressedInt(ref writer, _Count);
            startOfObjectPosition = writer.Position;
            if (includeChildrenMode != IncludeChildrenMode.ExcludeAllChildren && includeChildrenMode != IncludeChildrenMode.IncludeOnlyIncludableChildren) 
            {
                if (includeChildrenMode != IncludeChildrenMode.IncludeAllChildren || includeChildrenMode != OriginalIncludeChildrenMode && !_Key_Accessed)
                {
                    var deserialized = Key;
                }
                WriteChild(ref writer, _Key, includeChildrenMode, _Key_Accessed, () => GetChildSlice(LazinatorMemoryStorage, _Key_ByteIndex, _Key_ByteLength, false, false, null), verifyCleanness, updateStoredBuffer, false, false, this);
            }
            if (updateStoredBuffer)
            {
                _Key_ByteIndex = startOfObjectPosition - startPosition;
            }
            startOfObjectPosition = writer.Position;
            if (includeChildrenMode != IncludeChildrenMode.ExcludeAllChildren && includeChildrenMode != IncludeChildrenMode.IncludeOnlyIncludableChildren) 
            {
                if (includeChildrenMode != IncludeChildrenMode.IncludeAllChildren || includeChildrenMode != OriginalIncludeChildrenMode && !_Left_Accessed)
                {
                    var deserialized = Left;
                }
                WriteChild(ref writer, _Left, includeChildrenMode, _Left_Accessed, () => GetChildSlice(LazinatorMemoryStorage, _Left_ByteIndex, _Left_ByteLength, false, false, null), verifyCleanness, updateStoredBuffer, false, false, this);
            }
            if (updateStoredBuffer)
            {
                _Left_ByteIndex = startOfObjectPosition - startPosition;
            }
            startOfObjectPosition = writer.Position;
            if (includeChildrenMode != IncludeChildrenMode.ExcludeAllChildren && includeChildrenMode != IncludeChildrenMode.IncludeOnlyIncludableChildren) 
            {
                if (includeChildrenMode != IncludeChildrenMode.IncludeAllChildren || includeChildrenMode != OriginalIncludeChildrenMode && !_Right_Accessed)
                {
                    var deserialized = Right;
                }
                WriteChild(ref writer, _Right, includeChildrenMode, _Right_Accessed, () => GetChildSlice(LazinatorMemoryStorage, _Right_ByteIndex, _Right_ByteLength, false, false, null), verifyCleanness, updateStoredBuffer, false, false, this);
            }
            if (updateStoredBuffer)
            {
                _Right_ByteIndex = startOfObjectPosition - startPosition;
            }
            startOfObjectPosition = writer.Position;
            if (includeChildrenMode != IncludeChildrenMode.ExcludeAllChildren && includeChildrenMode != IncludeChildrenMode.IncludeOnlyIncludableChildren) 
            {
                if (includeChildrenMode != IncludeChildrenMode.IncludeAllChildren || includeChildrenMode != OriginalIncludeChildrenMode && !_Value_Accessed)
                {
                    var deserialized = Value;
                }
                WriteChild(ref writer, _Value, includeChildrenMode, _Value_Accessed, () => GetChildSlice(LazinatorMemoryStorage, _Value_ByteIndex, _Value_ByteLength, false, false, null), verifyCleanness, updateStoredBuffer, false, false, this);
            }
            if (updateStoredBuffer)
            {
                _Value_ByteIndex = startOfObjectPosition - startPosition;
            }
            if (updateStoredBuffer)
            {
                _AvlNode_TKey_TValue_EndByteIndex = writer.Position - startPosition;
            }
        }
        
    }
}
