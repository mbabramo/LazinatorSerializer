//6e1d5abe-344b-e8d7-9986-4cb026124dd4
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by the Lazinator tool, version 0.1.0.153
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Lazinator.Collections
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
    public partial class LazinatorTriple<T, U, V> : ILazinator
    {
        /* Serialization, deserialization, and object relationships */
        
        protected ILazinator _LazinatorParentClass;
        public virtual ILazinator LazinatorParentClass 
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
        
        protected IncludeChildrenMode OriginalIncludeChildrenMode;
        
        public virtual int Deserialize()
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
        
        public virtual MemoryInBuffer SerializeNewBuffer(IncludeChildrenMode includeChildrenMode, bool verifyCleanness)
        {
            return EncodeOrRecycleToNewBuffer(includeChildrenMode, OriginalIncludeChildrenMode, verifyCleanness, IsDirty, DescendantIsDirty, false, LazinatorObjectBytes, (StreamManuallyDelegate) EncodeToNewBuffer);
        }
        
        protected virtual MemoryInBuffer EncodeToNewBuffer(IncludeChildrenMode includeChildrenMode, bool verifyCleanness) => LazinatorUtilities.EncodeToNewBinaryBufferWriter(this, includeChildrenMode, verifyCleanness);
        
        public virtual ILazinator CloneLazinator()
        {
            return CloneLazinator(OriginalIncludeChildrenMode);
        }
        
        public virtual ILazinator CloneLazinator(IncludeChildrenMode includeChildrenMode)
        {
            MemoryInBuffer bytes = EncodeOrRecycleToNewBuffer(includeChildrenMode, OriginalIncludeChildrenMode, false, IsDirty, DescendantIsDirty, false, LazinatorObjectBytes, (StreamManuallyDelegate)EncodeToNewBuffer);
            var clone = new LazinatorTriple<T, U, V>()
            {
                LazinatorParentClass = LazinatorParentClass,
                OriginalIncludeChildrenMode = includeChildrenMode,
                HierarchyBytes = bytes,
            };
            clone.LazinatorParentClass = null;
            return clone;
        }
        
        public virtual bool HasChanged { get; set; }
        
        protected bool _IsDirty;
        public virtual bool IsDirty
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
        
        public virtual InformParentOfDirtinessDelegate InformParentOfDirtinessDelegate { get; set; }
        public virtual void InformParentOfDirtiness()
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
        public virtual MemoryInBuffer HierarchyBytes
        {
            set
            {
                _HierarchyBytes = value;
                LazinatorObjectBytes = value.FilledMemory;
            }
        }
        
        protected ReadOnlyMemory<byte> _LazinatorObjectBytes;
        public virtual ReadOnlyMemory<byte> LazinatorObjectBytes
        {
            get => _LazinatorObjectBytes;
            set
            {
                _LazinatorObjectBytes = value;
                int length = Deserialize();
                _LazinatorObjectBytes = _LazinatorObjectBytes.Slice(0, length);
            }
        }
        
        public virtual void LazinatorConvertToBytes()
        {
            if (!IsDirty && !DescendantIsDirty && _LazinatorObjectBytes.Length > 0)
            {
                return;
            }
            MemoryInBuffer bytes = EncodeOrRecycleToNewBuffer(IncludeChildrenMode.IncludeAllChildren, OriginalIncludeChildrenMode, false, IsDirty, DescendantIsDirty, false, LazinatorObjectBytes, (StreamManuallyDelegate)EncodeToNewBuffer);
            _LazinatorObjectBytes = bytes.FilledMemory;
        }
        
        public virtual int GetByteLength()
        {
            LazinatorConvertToBytes();
            return _LazinatorObjectBytes.Length;
        }
        
        public virtual uint GetBinaryHashCode32()
        {
            LazinatorConvertToBytes();
            return FarmhashByteSpans.Hash32(LazinatorObjectBytes.Span);
        }
        
        public virtual ulong GetBinaryHashCode64()
        {
            LazinatorConvertToBytes();
            return FarmhashByteSpans.Hash64(LazinatorObjectBytes.Span);
        }
        
        public virtual Guid GetBinaryHashCode128()
        {
            LazinatorConvertToBytes();
            return FarmhashByteSpans.Hash128(LazinatorObjectBytes.Span);
        }
        
        /* Property definitions */
        
        protected int _Item1_ByteIndex;
        protected int _Item2_ByteIndex;
        protected int _Item3_ByteIndex;
        protected virtual int _Item1_ByteLength => _Item2_ByteIndex - _Item1_ByteIndex;
        protected virtual int _Item2_ByteLength => _Item3_ByteIndex - _Item2_ByteIndex;
        private int _LazinatorTriple_T_U_V_EndByteIndex = 0;
        protected virtual int _Item3_ByteLength => _LazinatorTriple_T_U_V_EndByteIndex - _Item3_ByteIndex;
        
        private T _Item1;
        public virtual T Item1
        {
            get
            {
                if (!_Item1_Accessed)
                {
                    if (LazinatorObjectBytes.Length == 0)
                    {
                        _Item1 = default(T);
                    }
                    else
                    {
                        ReadOnlyMemory<byte> childData = GetChildSlice(LazinatorObjectBytes, _Item1_ByteIndex, _Item1_ByteLength, false, false, null);
                        
                        _Item1 = DeserializationFactory.Instance.CreateBasedOnType<T>(childData, this); 
                    }
                    _Item1_Accessed = true;
                } 
                return _Item1;
            }
            set
            {
                if (!System.Collections.Generic.EqualityComparer<T>.Default.Equals(value, default(T)))
                {
                    value.LazinatorParentClass = this;
                    value.IsDirty = true;
                }
                IsDirty = true;
                _Item1 = value;
                _Item1_Accessed = true;
            }
        }
        protected bool _Item1_Accessed;
        private U _Item2;
        public virtual U Item2
        {
            get
            {
                if (!_Item2_Accessed)
                {
                    if (LazinatorObjectBytes.Length == 0)
                    {
                        _Item2 = default(U);
                    }
                    else
                    {
                        ReadOnlyMemory<byte> childData = GetChildSlice(LazinatorObjectBytes, _Item2_ByteIndex, _Item2_ByteLength, false, false, null);
                        
                        _Item2 = DeserializationFactory.Instance.CreateBasedOnType<U>(childData, this); 
                    }
                    _Item2_Accessed = true;
                } 
                return _Item2;
            }
            set
            {
                if (!System.Collections.Generic.EqualityComparer<U>.Default.Equals(value, default(U)))
                {
                    value.LazinatorParentClass = this;
                    value.IsDirty = true;
                }
                IsDirty = true;
                _Item2 = value;
                _Item2_Accessed = true;
            }
        }
        protected bool _Item2_Accessed;
        private V _Item3;
        public virtual V Item3
        {
            get
            {
                if (!_Item3_Accessed)
                {
                    if (LazinatorObjectBytes.Length == 0)
                    {
                        _Item3 = default(V);
                    }
                    else
                    {
                        ReadOnlyMemory<byte> childData = GetChildSlice(LazinatorObjectBytes, _Item3_ByteIndex, _Item3_ByteLength, false, false, null);
                        
                        _Item3 = DeserializationFactory.Instance.CreateBasedOnType<V>(childData, this); 
                    }
                    _Item3_Accessed = true;
                } 
                return _Item3;
            }
            set
            {
                if (!System.Collections.Generic.EqualityComparer<V>.Default.Equals(value, default(V)))
                {
                    value.LazinatorParentClass = this;
                    value.IsDirty = true;
                }
                IsDirty = true;
                _Item3 = value;
                _Item3_Accessed = true;
            }
        }
        protected bool _Item3_Accessed;
        
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
            if (enumerateNulls && (System.Collections.Generic.EqualityComparer<T>.Default.Equals(Item1, default(T))))
            {
                yield return ("Item1", default);
            }
            else if ((!exploreOnlyDeserializedChildren && !System.Collections.Generic.EqualityComparer<T>.Default.Equals(Item1, default(T))) || (_Item1_Accessed && !System.Collections.Generic.EqualityComparer<T>.Default.Equals(_Item1, default(T))))
            {
                foreach (ILazinator toYield in Item1.EnumerateLazinatorNodes(matchCriterion, stopExploringBelowMatch, exploreCriterion, exploreOnlyDeserializedChildren, enumerateNulls))
                {
                    yield return ("Item1", toYield);
                }
            }
            if (enumerateNulls && (System.Collections.Generic.EqualityComparer<U>.Default.Equals(Item2, default(U))))
            {
                yield return ("Item2", default);
            }
            else if ((!exploreOnlyDeserializedChildren && !System.Collections.Generic.EqualityComparer<U>.Default.Equals(Item2, default(U))) || (_Item2_Accessed && !System.Collections.Generic.EqualityComparer<U>.Default.Equals(_Item2, default(U))))
            {
                foreach (ILazinator toYield in Item2.EnumerateLazinatorNodes(matchCriterion, stopExploringBelowMatch, exploreCriterion, exploreOnlyDeserializedChildren, enumerateNulls))
                {
                    yield return ("Item2", toYield);
                }
            }
            if (enumerateNulls && (System.Collections.Generic.EqualityComparer<V>.Default.Equals(Item3, default(V))))
            {
                yield return ("Item3", default);
            }
            else if ((!exploreOnlyDeserializedChildren && !System.Collections.Generic.EqualityComparer<V>.Default.Equals(Item3, default(V))) || (_Item3_Accessed && !System.Collections.Generic.EqualityComparer<V>.Default.Equals(_Item3, default(V))))
            {
                foreach (ILazinator toYield in Item3.EnumerateLazinatorNodes(matchCriterion, stopExploringBelowMatch, exploreCriterion, exploreOnlyDeserializedChildren, enumerateNulls))
                {
                    yield return ("Item3", toYield);
                }
            }
            yield break;
        }
        
        
        public virtual IEnumerable<(string propertyName, object descendant)> EnumerateNonLazinatorProperties()
        {
            yield break;
        }
        
        protected virtual void ResetAccessedProperties()
        {
            
        }
        
        /* Conversion */
        
        public virtual int LazinatorUniqueID => 106;
        
        protected virtual bool ContainsOpenGenericParameters => true;
        protected virtual LazinatorGenericIDType _LazinatorGenericID { get; set; }
        public virtual LazinatorGenericIDType LazinatorGenericID
        {
            get
            {
                if (_LazinatorGenericID.IsEmpty)
                {
                    _LazinatorGenericID = DeserializationFactory.Instance.GetUniqueIDListForGenericType(106, new Type[] { typeof(T), typeof(U), typeof(V) });
                }
                return _LazinatorGenericID;
            }
            set
            {
                _LazinatorGenericID = value;
            }
        }
        
        public virtual int LazinatorObjectVersion { get; set; } = 0;
        
        
        public virtual void ConvertFromBytesAfterHeader(IncludeChildrenMode includeChildrenMode, int serializedVersionNumber, ref int bytesSoFar)
        {
            ReadOnlySpan<byte> span = LazinatorObjectBytes.Span;
            _Item1_ByteIndex = bytesSoFar;
            if (includeChildrenMode != IncludeChildrenMode.ExcludeAllChildren && includeChildrenMode != IncludeChildrenMode.IncludeOnlyIncludableChildren) 
            {
                bytesSoFar = span.ToInt32(ref bytesSoFar) + bytesSoFar;
            }
            _Item2_ByteIndex = bytesSoFar;
            if (includeChildrenMode != IncludeChildrenMode.ExcludeAllChildren && includeChildrenMode != IncludeChildrenMode.IncludeOnlyIncludableChildren) 
            {
                bytesSoFar = span.ToInt32(ref bytesSoFar) + bytesSoFar;
            }
            _Item3_ByteIndex = bytesSoFar;
            if (includeChildrenMode != IncludeChildrenMode.ExcludeAllChildren && includeChildrenMode != IncludeChildrenMode.IncludeOnlyIncludableChildren) 
            {
                bytesSoFar = span.ToInt32(ref bytesSoFar) + bytesSoFar;
            }
            _LazinatorTriple_T_U_V_EndByteIndex = bytesSoFar;
        }
        
        public virtual void SerializeExistingBuffer(BinaryBufferWriter writer, IncludeChildrenMode includeChildrenMode, bool verifyCleanness, bool updateStoredBuffer)
        {
            TabbedText.WriteLine($"Initiating serialization of Lazinator.Collections.LazinatorTriple<T, U, V> ");
            if (includeChildrenMode != IncludeChildrenMode.IncludeAllChildren)
            {
                updateStoredBuffer = false;
            }
            int startPosition = writer.Position;
            WritePropertiesIntoBuffer(writer, includeChildrenMode, verifyCleanness, updateStoredBuffer, true);
            if (updateStoredBuffer)
            {
                
                _IsDirty = false;
                _DescendantIsDirty = includeChildrenMode != IncludeChildrenMode.IncludeAllChildren && ((_Item1_Accessed && !System.Collections.Generic.EqualityComparer<T>.Default.Equals(_Item1, default(T)) && (Item1.IsDirty || Item1.DescendantIsDirty)) || (_Item2_Accessed && !System.Collections.Generic.EqualityComparer<U>.Default.Equals(_Item2, default(U)) && (Item2.IsDirty || Item2.DescendantIsDirty)) || (_Item3_Accessed && !System.Collections.Generic.EqualityComparer<V>.Default.Equals(_Item3, default(V)) && (Item3.IsDirty || Item3.DescendantIsDirty)));
                
                _LazinatorObjectBytes = writer.Slice(startPosition);
            }
        }
        protected virtual void WritePropertiesIntoBuffer(BinaryBufferWriter writer, IncludeChildrenMode includeChildrenMode, bool verifyCleanness, bool updateStoredBuffer, bool includeUniqueID)
        {
            int startPosition = writer.Position;
            int startOfObjectPosition = 0;
            // header information
            TabbedText.WriteLine($"Writing properties for Lazinator.Collections.LazinatorTriple<T, U, V> starting at {writer.Position}.");
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
            TabbedText.WriteLine($"Byte {writer.Position}, Item1 value {_Item1}");
            TabbedText.Tabs++;
            startOfObjectPosition = writer.Position;
            if (includeChildrenMode != IncludeChildrenMode.ExcludeAllChildren && includeChildrenMode != IncludeChildrenMode.IncludeOnlyIncludableChildren) 
            {
                WriteChild(writer, _Item1, includeChildrenMode, _Item1_Accessed, () => GetChildSlice(LazinatorObjectBytes, _Item1_ByteIndex, _Item1_ByteLength, false, false, null), verifyCleanness, updateStoredBuffer, false, false, this);
            }
            if (updateStoredBuffer)
            {
                _Item1_ByteIndex = startOfObjectPosition - startPosition;
            }
            TabbedText.Tabs--;
            TabbedText.WriteLine($"Byte {writer.Position}, Item2 value {_Item2}");
            TabbedText.Tabs++;
            startOfObjectPosition = writer.Position;
            if (includeChildrenMode != IncludeChildrenMode.ExcludeAllChildren && includeChildrenMode != IncludeChildrenMode.IncludeOnlyIncludableChildren) 
            {
                WriteChild(writer, _Item2, includeChildrenMode, _Item2_Accessed, () => GetChildSlice(LazinatorObjectBytes, _Item2_ByteIndex, _Item2_ByteLength, false, false, null), verifyCleanness, updateStoredBuffer, false, false, this);
            }
            if (updateStoredBuffer)
            {
                _Item2_ByteIndex = startOfObjectPosition - startPosition;
            }
            TabbedText.Tabs--;
            TabbedText.WriteLine($"Byte {writer.Position}, Item3 value {_Item3}");
            TabbedText.Tabs++;
            startOfObjectPosition = writer.Position;
            if (includeChildrenMode != IncludeChildrenMode.ExcludeAllChildren && includeChildrenMode != IncludeChildrenMode.IncludeOnlyIncludableChildren) 
            {
                WriteChild(writer, _Item3, includeChildrenMode, _Item3_Accessed, () => GetChildSlice(LazinatorObjectBytes, _Item3_ByteIndex, _Item3_ByteLength, false, false, null), verifyCleanness, updateStoredBuffer, false, false, this);
            }
            if (updateStoredBuffer)
            {
                _Item3_ByteIndex = startOfObjectPosition - startPosition;
            }
            TabbedText.Tabs--;
            if (updateStoredBuffer)
            {
                _LazinatorTriple_T_U_V_EndByteIndex = writer.Position - startPosition;
            }
            TabbedText.WriteLine($"Byte {writer.Position} (end of LazinatorTriple<T, U, V>) ");
        }
        
    }
}
