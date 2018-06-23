//b577bb12-253a-02c4-c3da-e457c2172549
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by the Lazinator tool, version 0.1.0.146
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
        /* Serialization, deserialization, and object relationships */
        
        ILazinator _LazinatorParentClass;
        public AvlNode() : base()
        {
        }
        
        public ILazinator LazinatorParentClass 
        { 
            get => _LazinatorParentClass;
            set
            {
                _LazinatorParentClass = value;
                if (value != null && (IsDirty || DescendantIsDirty))
                {
                    value.DescendantIsDirty = true;
                }
            }
        }
        
        IncludeChildrenMode OriginalIncludeChildrenMode;
        
        public int Deserialize()
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
        
        public MemoryInBuffer SerializeNewBuffer(IncludeChildrenMode includeChildrenMode, bool verifyCleanness)
        {
            return EncodeOrRecycleToNewBuffer(includeChildrenMode, OriginalIncludeChildrenMode, verifyCleanness, IsDirty, DescendantIsDirty, false, LazinatorObjectBytes, (StreamManuallyDelegate) EncodeToNewBuffer);
        }
        
        MemoryInBuffer EncodeToNewBuffer(IncludeChildrenMode includeChildrenMode, bool verifyCleanness) => LazinatorUtilities.EncodeToNewBinaryBufferWriter(this, includeChildrenMode, verifyCleanness);
        
        public ILazinator CloneLazinator()
        {
            return CloneLazinator(OriginalIncludeChildrenMode);
        }
        
        public ILazinator CloneLazinator(IncludeChildrenMode includeChildrenMode)
        {
            MemoryInBuffer bytes = EncodeOrRecycleToNewBuffer(includeChildrenMode, OriginalIncludeChildrenMode, false, IsDirty, DescendantIsDirty, false, LazinatorObjectBytes, (StreamManuallyDelegate)EncodeToNewBuffer);
            var clone = new AvlNode<TKey, TValue>()
            {
                LazinatorParentClass = LazinatorParentClass,
                OriginalIncludeChildrenMode = includeChildrenMode,
                HierarchyBytes = bytes,
            };
            clone.LazinatorParentClass = null;
            return clone;
        }
        
        public bool HasChanged { get; set; }
        
        bool _IsDirty;
        public bool IsDirty
        {
            [DebuggerStepThrough]
            get => _IsDirty;
            [DebuggerStepThrough]
            set
            {
                if (_IsDirty != value)
                {
                    _IsDirty = value;
                    if (_IsDirty)
                    {
                        InformParentOfDirtiness();
                    }
                }
                if (_IsDirty)
                {
                    HasChanged = true;
                }
            }
        }
        
        public InformParentOfDirtinessDelegate InformParentOfDirtinessDelegate { get; set; }
        public void InformParentOfDirtiness()
        {
            if (InformParentOfDirtinessDelegate == null)
            {
                if (LazinatorParentClass != null)
                {
                    LazinatorParentClass.DescendantIsDirty = true;
                }
            }
            else
            {
                InformParentOfDirtinessDelegate();
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
                        _DescendantHasChanged = true;
                        if (LazinatorParentClass != null)
                        {
                            LazinatorParentClass.DescendantIsDirty = true;
                        }
                    }
                }
                if (_DescendantIsDirty)
                {
                    _DescendantHasChanged = true;
                }
            }
        }
        
        private MemoryInBuffer _HierarchyBytes;
        public MemoryInBuffer HierarchyBytes
        {
            set
            {
                _HierarchyBytes = value;
                LazinatorObjectBytes = value.FilledMemory;
            }
        }
        
        ReadOnlyMemory<byte> _LazinatorObjectBytes;
        public ReadOnlyMemory<byte> LazinatorObjectBytes
        {
            get => _LazinatorObjectBytes;
            set
            {
                _LazinatorObjectBytes = value;
                int length = Deserialize();
                _LazinatorObjectBytes = _LazinatorObjectBytes.Slice(0, length);
            }
        }
        
        public void LazinatorConvertToBytes()
        {
            if (!IsDirty && !DescendantIsDirty && _LazinatorObjectBytes.Length > 0)
            {
                return;
            }
            MemoryInBuffer bytes = EncodeOrRecycleToNewBuffer(IncludeChildrenMode.IncludeAllChildren, OriginalIncludeChildrenMode, false, IsDirty, DescendantIsDirty, false, LazinatorObjectBytes, (StreamManuallyDelegate)EncodeToNewBuffer);
            _LazinatorObjectBytes = bytes.FilledMemory;
        }
        
        public int GetByteLength()
        {
            LazinatorConvertToBytes();
            return _LazinatorObjectBytes.Length;
        }
        
        public uint GetBinaryHashCode32()
        {
            LazinatorConvertToBytes();
            return FarmhashByteSpans.Hash32(LazinatorObjectBytes.Span);
        }
        
        public ulong GetBinaryHashCode64()
        {
            LazinatorConvertToBytes();
            return FarmhashByteSpans.Hash64(LazinatorObjectBytes.Span);
        }
        
        public Guid GetBinaryHashCode128()
        {
            LazinatorConvertToBytes();
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
        
        private int _Balance;
        public int Balance
        {
            get
            {
                return _Balance;
            }
            set
            {
                IsDirty = true;
                _Balance = value;
            }
        }
        private int _Count;
        public int Count
        {
            get
            {
                return _Count;
            }
            set
            {
                IsDirty = true;
                _Count = value;
            }
        }
        private TKey _Key;
        public TKey Key
        {
            get
            {
                if (!_Key_Accessed)
                {
                    if (LazinatorObjectBytes.Length == 0)
                    {
                        _Key = default(TKey);
                    }
                    else
                    {
                        ReadOnlyMemory<byte> childData = GetChildSlice(LazinatorObjectBytes, _Key_ByteIndex, _Key_ByteLength, false, false, null);
                        
                        _Key = DeserializationFactory.Instance.CreateBasedOnType<TKey>(childData, this); 
                    }
                    _Key_Accessed = true;
                } 
                return _Key;
            }
            set
            {
                if (!System.Collections.Generic.EqualityComparer<TKey>.Default.Equals(value, default(TKey)))
                {
                    if (value.LazinatorParentClass != null)
                    {
                        throw new MovedLazinatorException($"The property Key cannot be set to a Lazinator object with a defined LazinatorParentClass, because AutoChangeParent is set to false in the configuration file and no attribute providing an exception is present.");
                    }
                    value.LazinatorParentClass = this;
                    value.IsDirty = true;
                }
                IsDirty = true;
                _Key = value;
                _Key_Accessed = true;
            }
        }
        bool _Key_Accessed;
        private AvlNode<TKey, TValue> _Left;
        public AvlNode<TKey, TValue> Left
        {
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
                        ReadOnlyMemory<byte> childData = GetChildSlice(LazinatorObjectBytes, _Left_ByteIndex, _Left_ByteLength, false, false, null);
                        if (childData.Length == 0)
                        {
                            _Left = default;
                        }
                        else _Left = new AvlNode<TKey, TValue>()
                        {
                            LazinatorParentClass = this,
                            LazinatorObjectBytes = childData,
                        };
                    }
                    _Left_Accessed = true;
                } 
                return _Left;
            }
            set
            {
                if (value != null)
                {
                    value.LazinatorParentClass = this;
                    value.IsDirty = true;
                }
                IsDirty = true;
                _Left = value;
                _Left_Accessed = true;
            }
        }
        bool _Left_Accessed;
        private AvlNode<TKey, TValue> _Right;
        public AvlNode<TKey, TValue> Right
        {
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
                        ReadOnlyMemory<byte> childData = GetChildSlice(LazinatorObjectBytes, _Right_ByteIndex, _Right_ByteLength, false, false, null);
                        if (childData.Length == 0)
                        {
                            _Right = default;
                        }
                        else _Right = new AvlNode<TKey, TValue>()
                        {
                            LazinatorParentClass = this,
                            LazinatorObjectBytes = childData,
                        };
                    }
                    _Right_Accessed = true;
                } 
                return _Right;
            }
            set
            {
                if (value != null)
                {
                    value.LazinatorParentClass = this;
                    value.IsDirty = true;
                }
                IsDirty = true;
                _Right = value;
                _Right_Accessed = true;
            }
        }
        bool _Right_Accessed;
        private TValue _Value;
        public TValue Value
        {
            get
            {
                if (!_Value_Accessed)
                {
                    if (LazinatorObjectBytes.Length == 0)
                    {
                        _Value = default(TValue);
                    }
                    else
                    {
                        ReadOnlyMemory<byte> childData = GetChildSlice(LazinatorObjectBytes, _Value_ByteIndex, _Value_ByteLength, false, false, null);
                        
                        _Value = DeserializationFactory.Instance.CreateBasedOnType<TValue>(childData, this); 
                    }
                    _Value_Accessed = true;
                } 
                return _Value;
            }
            set
            {
                if (!System.Collections.Generic.EqualityComparer<TValue>.Default.Equals(value, default(TValue)))
                {
                    if (value.LazinatorParentClass != null)
                    {
                        throw new MovedLazinatorException($"The property Value cannot be set to a Lazinator object with a defined LazinatorParentClass, because AutoChangeParent is set to false in the configuration file and no attribute providing an exception is present.");
                    }
                    value.LazinatorParentClass = this;
                    value.IsDirty = true;
                }
                IsDirty = true;
                _Value = value;
                _Value_Accessed = true;
            }
        }
        bool _Value_Accessed;
        
        public IEnumerable<ILazinator> EnumerateLazinatorNodes(Func<ILazinator, bool> matchCriterion, bool stopExploringBelowMatch, Func<ILazinator, bool> exploreCriterion, bool exploreOnlyDeserializedChildren)
        {
            bool match = (matchCriterion == null) ? true : matchCriterion(this);
            bool explore = (!match || !stopExploringBelowMatch) && ((exploreCriterion == null) ? true : exploreCriterion(this));
            if (match)
            {
                yield return this;
            }
            if (explore)
            {
                foreach (ILazinator dirty in EnumerateLazinatorNodes_Helper(matchCriterion, stopExploringBelowMatch, exploreCriterion, exploreOnlyDeserializedChildren))
                {
                    yield return dirty;
                }
            }
        }
        
        IEnumerable<ILazinator> EnumerateLazinatorNodes_Helper(Func<ILazinator, bool> matchCriterion, bool stopExploringBelowMatch, Func<ILazinator, bool> exploreCriterion, bool exploreOnlyDeserializedChildren)
        {
            if ((!exploreOnlyDeserializedChildren && !System.Collections.Generic.EqualityComparer<TKey>.Default.Equals(Key, default(TKey))) || (_Key_Accessed && !System.Collections.Generic.EqualityComparer<TKey>.Default.Equals(_Key, default(TKey))))
            {
                foreach (ILazinator toYield in Key.EnumerateLazinatorNodes(matchCriterion, stopExploringBelowMatch, exploreCriterion, exploreOnlyDeserializedChildren))
                {
                    yield return toYield;
                }
            }
            if ((!exploreOnlyDeserializedChildren && Left != null) || (_Left_Accessed && _Left != null))
            {
                foreach (ILazinator toYield in Left.EnumerateLazinatorNodes(matchCriterion, stopExploringBelowMatch, exploreCriterion, exploreOnlyDeserializedChildren))
                {
                    yield return toYield;
                }
            }
            if ((!exploreOnlyDeserializedChildren && Right != null) || (_Right_Accessed && _Right != null))
            {
                foreach (ILazinator toYield in Right.EnumerateLazinatorNodes(matchCriterion, stopExploringBelowMatch, exploreCriterion, exploreOnlyDeserializedChildren))
                {
                    yield return toYield;
                }
            }
            if ((!exploreOnlyDeserializedChildren && !System.Collections.Generic.EqualityComparer<TValue>.Default.Equals(Value, default(TValue))) || (_Value_Accessed && !System.Collections.Generic.EqualityComparer<TValue>.Default.Equals(_Value, default(TValue))))
            {
                foreach (ILazinator toYield in Value.EnumerateLazinatorNodes(matchCriterion, stopExploringBelowMatch, exploreCriterion, exploreOnlyDeserializedChildren))
                {
                    yield return toYield;
                }
            }
            yield break;
        }
        
        void ResetAccessedProperties()
        {
            _Left_Accessed = _Right_Accessed = false;
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
        
        public void SerializeExistingBuffer(BinaryBufferWriter writer, IncludeChildrenMode includeChildrenMode, bool verifyCleanness, bool updateStoredBuffer)
        {
            TabbedText.WriteLine($"Initiating serialization of Lazinator.Collections.Avl.AvlNode<TKey, TValue> ");
            if (includeChildrenMode != IncludeChildrenMode.IncludeAllChildren)
            {
                updateStoredBuffer = false;
            }
            int startPosition = writer.Position;
            WritePropertiesIntoBuffer(writer, includeChildrenMode, verifyCleanness, updateStoredBuffer, true);
            if (updateStoredBuffer)
            {
                
                _IsDirty = false;
                _DescendantIsDirty = includeChildrenMode != IncludeChildrenMode.IncludeAllChildren && ((_Key_Accessed && !System.Collections.Generic.EqualityComparer<TKey>.Default.Equals(_Key, default(TKey)) && (Key.IsDirty || Key.DescendantIsDirty)) || (_Left_Accessed && _Left != null && (Left.IsDirty || Left.DescendantIsDirty)) || (_Right_Accessed && _Right != null && (Right.IsDirty || Right.DescendantIsDirty)) || (_Value_Accessed && !System.Collections.Generic.EqualityComparer<TValue>.Default.Equals(_Value, default(TValue)) && (Value.IsDirty || Value.DescendantIsDirty)));
                
                _LazinatorObjectBytes = writer.Slice(startPosition);
            }
        }
        void WritePropertiesIntoBuffer(BinaryBufferWriter writer, IncludeChildrenMode includeChildrenMode, bool verifyCleanness, bool updateStoredBuffer, bool includeUniqueID)
        {
            int startPosition = writer.Position;
            int startOfObjectPosition = 0;
            // header information
            TabbedText.WriteLine($"Writing properties for Lazinator.Collections.Avl.AvlNode<TKey, TValue> starting at {writer.Position}.");
            TabbedText.WriteLine($"Includes? uniqueID {(LazinatorGenericID.IsEmpty ? LazinatorUniqueID.ToString() : String.Join("","",LazinatorGenericID.TypeAndInnerTypeIDs.ToArray()))} {includeUniqueID}, Lazinator version {Lazinator.Support.LazinatorVersionInfo.LazinatorIntVersion} True, Object version {LazinatorObjectVersion} True, IncludeChildrenMode {includeChildrenMode} True");
            TabbedText.WriteLine($"IsDirty {IsDirty} DescendantIsDirty {DescendantIsDirty} HasParentClass {LazinatorParentClass != null}");
            if (includeUniqueID)
            {
                if (LazinatorGenericID.IsEmpty)
                {
                    CompressedIntegralTypes.WriteCompressedInt(writer, LazinatorUniqueID);
                }
                else
                {
                    WriteLazinatorGenericID(writer, LazinatorGenericID);
                }
            }
            CompressedIntegralTypes.WriteCompressedInt(writer, Lazinator.Support.LazinatorVersionInfo.LazinatorIntVersion);
            CompressedIntegralTypes.WriteCompressedInt(writer, LazinatorObjectVersion);
            writer.Write((byte)includeChildrenMode);
            // write properties
            TabbedText.WriteLine($"Byte {writer.Position}, Balance value {_Balance}");
            TabbedText.Tabs++;
            CompressedIntegralTypes.WriteCompressedInt(writer, _Balance);
            TabbedText.Tabs--;
            TabbedText.WriteLine($"Byte {writer.Position}, Count value {_Count}");
            TabbedText.Tabs++;
            CompressedIntegralTypes.WriteCompressedInt(writer, _Count);
            TabbedText.Tabs--;
            TabbedText.WriteLine($"Byte {writer.Position}, Key value {_Key}");
            TabbedText.Tabs++;
            startOfObjectPosition = writer.Position;
            if (includeChildrenMode != IncludeChildrenMode.ExcludeAllChildren && includeChildrenMode != IncludeChildrenMode.IncludeOnlyIncludableChildren) 
            {
                WriteChild(writer, _Key, includeChildrenMode, _Key_Accessed, () => GetChildSlice(LazinatorObjectBytes, _Key_ByteIndex, _Key_ByteLength, false, false, null), verifyCleanness, updateStoredBuffer, false, false, this);
            }
            if (updateStoredBuffer)
            {
                _Key_ByteIndex = startOfObjectPosition - startPosition;
            }
            TabbedText.Tabs--;
            TabbedText.WriteLine($"Byte {writer.Position}, Left (accessed? {_Left_Accessed}) (backing var null? {_Left == null}) ");
            TabbedText.Tabs++;
            startOfObjectPosition = writer.Position;
            if (includeChildrenMode != IncludeChildrenMode.ExcludeAllChildren && includeChildrenMode != IncludeChildrenMode.IncludeOnlyIncludableChildren) 
            {
                WriteChild(writer, _Left, includeChildrenMode, _Left_Accessed, () => GetChildSlice(LazinatorObjectBytes, _Left_ByteIndex, _Left_ByteLength, false, false, null), verifyCleanness, updateStoredBuffer, false, false, this);
            }
            if (updateStoredBuffer)
            {
                _Left_ByteIndex = startOfObjectPosition - startPosition;
            }
            TabbedText.Tabs--;
            TabbedText.WriteLine($"Byte {writer.Position}, Right (accessed? {_Right_Accessed}) (backing var null? {_Right == null}) ");
            TabbedText.Tabs++;
            startOfObjectPosition = writer.Position;
            if (includeChildrenMode != IncludeChildrenMode.ExcludeAllChildren && includeChildrenMode != IncludeChildrenMode.IncludeOnlyIncludableChildren) 
            {
                WriteChild(writer, _Right, includeChildrenMode, _Right_Accessed, () => GetChildSlice(LazinatorObjectBytes, _Right_ByteIndex, _Right_ByteLength, false, false, null), verifyCleanness, updateStoredBuffer, false, false, this);
            }
            if (updateStoredBuffer)
            {
                _Right_ByteIndex = startOfObjectPosition - startPosition;
            }
            TabbedText.Tabs--;
            TabbedText.WriteLine($"Byte {writer.Position}, Value value {_Value}");
            TabbedText.Tabs++;
            startOfObjectPosition = writer.Position;
            if (includeChildrenMode != IncludeChildrenMode.ExcludeAllChildren && includeChildrenMode != IncludeChildrenMode.IncludeOnlyIncludableChildren) 
            {
                WriteChild(writer, _Value, includeChildrenMode, _Value_Accessed, () => GetChildSlice(LazinatorObjectBytes, _Value_ByteIndex, _Value_ByteLength, false, false, null), verifyCleanness, updateStoredBuffer, false, false, this);
            }
            if (updateStoredBuffer)
            {
                _Value_ByteIndex = startOfObjectPosition - startPosition;
            }
            TabbedText.Tabs--;
            if (updateStoredBuffer)
            {
                _AvlNode_TKey_TValue_EndByteIndex = writer.Position - startPosition;
            }
            TabbedText.WriteLine($"Byte {writer.Position} (end of AvlNode<TKey, TValue>) ");
        }
        
    }
}
