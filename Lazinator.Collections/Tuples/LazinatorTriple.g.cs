//fa62e7a5-ba29-1be0-f4a9-8e4392722d6e
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by the Lazinator tool, version 0.1.0.432, on 2024/03/23 04:24:52.215 PM.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

#nullable disable
namespace Lazinator.Collections.Tuples
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
    public partial class LazinatorTriple<T, U, V> : ILazinator
    {
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public bool IsStruct => false;
        
        /* Property definitions */
        
        protected int _Item1_ByteIndex;
        protected int _Item2_ByteIndex;
        protected int _Item3_ByteIndex;
        protected virtual int _Item1_ByteLength => _Item2_ByteIndex - _Item1_ByteIndex;
        protected virtual int _Item2_ByteLength => _Item3_ByteIndex - _Item2_ByteIndex;
        private int _LazinatorTriple_T_U_V_EndByteIndex = 0;
        protected virtual  int _Item3_ByteLength => _LazinatorTriple_T_U_V_EndByteIndex - _Item3_ByteIndex;
        protected virtual int _OverallEndByteIndex => _LazinatorTriple_T_U_V_EndByteIndex;
        
        
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        protected T _Item1;
        public virtual T Item1
        {
            [DebuggerStepThrough]
            get
            {
                if (!_Item1_Accessed)
                {
                    LazinateItem1();
                } 
                return _Item1;
            }
            [DebuggerStepThrough]
            set
            {
                if (value != null && value.IsStruct)
                {
                    value.LazinatorParents = new LazinatorParentsCollection(this, null);
                }
                else
                {
                    if (_Item1 != null)
                    {
                        _Item1.LazinatorParents = _Item1.LazinatorParents.WithRemoved(this);
                    }
                    if (value != null)
                    {
                        value.LazinatorParents = value.LazinatorParents.WithAdded(this);
                    }
                }
                
                IsDirty = true;
                DescendantIsDirty = true;
                _Item1 = value;
                _Item1_Accessed = true;
            }
        }
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        protected bool _Item1_Accessed;
        private void LazinateItem1()
        {
            if (LazinatorMemoryStorage.Length == 0)
            {
                _Item1 = default(T);
                if (_Item1 != null)
                { // Item1 is a struct
                    _Item1.LazinatorParents = new LazinatorParentsCollection(this, null);
                }
            }
            else
            {
                LazinatorMemory childData = GetChildSlice(LazinatorMemoryStorage, _Item1_ByteIndex, _Item1_ByteLength, null);
                _Item1 = DeserializationFactory.Instance.CreateBasedOnType<T>(childData, this); 
            }
            _Item1_Accessed = true;
        }
        
        
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        protected U _Item2;
        public virtual U Item2
        {
            [DebuggerStepThrough]
            get
            {
                if (!_Item2_Accessed)
                {
                    LazinateItem2();
                } 
                return _Item2;
            }
            [DebuggerStepThrough]
            set
            {
                if (value != null && value.IsStruct)
                {
                    value.LazinatorParents = new LazinatorParentsCollection(this, null);
                }
                else
                {
                    if (_Item2 != null)
                    {
                        _Item2.LazinatorParents = _Item2.LazinatorParents.WithRemoved(this);
                    }
                    if (value != null)
                    {
                        value.LazinatorParents = value.LazinatorParents.WithAdded(this);
                    }
                }
                
                IsDirty = true;
                DescendantIsDirty = true;
                _Item2 = value;
                _Item2_Accessed = true;
            }
        }
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        protected bool _Item2_Accessed;
        private void LazinateItem2()
        {
            if (LazinatorMemoryStorage.Length == 0)
            {
                _Item2 = default(U);
                if (_Item2 != null)
                { // Item2 is a struct
                    _Item2.LazinatorParents = new LazinatorParentsCollection(this, null);
                }
            }
            else
            {
                LazinatorMemory childData = GetChildSlice(LazinatorMemoryStorage, _Item2_ByteIndex, _Item2_ByteLength, null);
                _Item2 = DeserializationFactory.Instance.CreateBasedOnType<U>(childData, this); 
            }
            _Item2_Accessed = true;
        }
        
        
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        protected V _Item3;
        public virtual V Item3
        {
            [DebuggerStepThrough]
            get
            {
                if (!_Item3_Accessed)
                {
                    LazinateItem3();
                } 
                return _Item3;
            }
            [DebuggerStepThrough]
            set
            {
                if (value != null && value.IsStruct)
                {
                    value.LazinatorParents = new LazinatorParentsCollection(this, null);
                }
                else
                {
                    if (_Item3 != null)
                    {
                        _Item3.LazinatorParents = _Item3.LazinatorParents.WithRemoved(this);
                    }
                    if (value != null)
                    {
                        value.LazinatorParents = value.LazinatorParents.WithAdded(this);
                    }
                }
                
                IsDirty = true;
                DescendantIsDirty = true;
                _Item3 = value;
                _Item3_Accessed = true;
            }
        }
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        protected bool _Item3_Accessed;
        private void LazinateItem3()
        {
            if (LazinatorMemoryStorage.Length == 0)
            {
                _Item3 = default(V);
                if (_Item3 != null)
                { // Item3 is a struct
                    _Item3.LazinatorParents = new LazinatorParentsCollection(this, null);
                }
            }
            else
            {
                LazinatorMemory childData = GetChildSlice(LazinatorMemoryStorage, _Item3_ByteIndex, _Item3_ByteLength, null);
                _Item3 = DeserializationFactory.Instance.CreateBasedOnType<V>(childData, this); 
            }
            _Item3_Accessed = true;
        }
        
        
        /* Serialization, deserialization, and object relationships */
        
        public LazinatorTriple(IncludeChildrenMode originalIncludeChildrenMode = IncludeChildrenMode.IncludeAllChildren)
        {
            OriginalIncludeChildrenMode = originalIncludeChildrenMode;
        }
        
        public LazinatorTriple(LazinatorMemory serializedBytes, ILazinator parent = null, IncludeChildrenMode originalIncludeChildrenMode = IncludeChildrenMode.IncludeAllChildren, int? lazinatorObjectVersion = null)
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
        
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public virtual LazinatorParentsCollection LazinatorParents { get; set; }
        
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public virtual LazinatorMemory LazinatorMemoryStorage
        {
            get;
            set;
        }
        
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public virtual IncludeChildrenMode OriginalIncludeChildrenMode { get; set; }
        
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public virtual bool HasChanged { get; set; }
        
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        protected bool _IsDirty;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
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
        
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        protected bool _DescendantHasChanged;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
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
        
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        protected bool _DescendantIsDirty;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
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
            LazinatorTriple<T, U, V> clone;
            if (cloneBufferOptions == CloneBufferOptions.NoBuffer)
            {
                clone = new LazinatorTriple<T, U, V>(includeChildrenMode);
                clone.LazinatorObjectVersion = LazinatorObjectVersion;
                clone = (LazinatorTriple<T, U, V>)AssignCloneProperties(clone, includeChildrenMode);
            }
            else
            {
                LazinatorMemory bytes = EncodeOrRecycleToNewBuffer(includeChildrenMode, OriginalIncludeChildrenMode, IsDirty, DescendantIsDirty, false, LazinatorMemoryStorage, this);
                clone = new LazinatorTriple<T, U, V>(bytes);
            }
            return clone;
        }
        
        protected virtual ILazinator AssignCloneProperties(ILazinator clone, IncludeChildrenMode includeChildrenMode)
        {
            clone.FreeInMemoryObjects();
            LazinatorTriple<T, U, V> typedClone = (LazinatorTriple<T, U, V>) clone;
            if (includeChildrenMode != IncludeChildrenMode.ExcludeAllChildren && includeChildrenMode != IncludeChildrenMode.IncludeOnlyIncludableChildren)
            {
                if (Item1 == null)
                {
                    typedClone.Item1 = default(T);
                }
                else
                {
                    typedClone.Item1 = (T) Item1.CloneLazinator(includeChildrenMode, CloneBufferOptions.NoBuffer);
                }
            }
            if (includeChildrenMode != IncludeChildrenMode.ExcludeAllChildren && includeChildrenMode != IncludeChildrenMode.IncludeOnlyIncludableChildren)
            {
                if (Item2 == null)
                {
                    typedClone.Item2 = default(U);
                }
                else
                {
                    typedClone.Item2 = (U) Item2.CloneLazinator(includeChildrenMode, CloneBufferOptions.NoBuffer);
                }
            }
            if (includeChildrenMode != IncludeChildrenMode.ExcludeAllChildren && includeChildrenMode != IncludeChildrenMode.IncludeOnlyIncludableChildren)
            {
                if (Item3 == null)
                {
                    typedClone.Item3 = default(V);
                }
                else
                {
                    typedClone.Item3 = (V) Item3.CloneLazinator(includeChildrenMode, CloneBufferOptions.NoBuffer);
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
            if (enumerateNulls && (!exploreOnlyDeserializedChildren || _Item1_Accessed) && Item1 == null)
            {
                yield return ("Item1", default);
            }
            else
            {
                if ((!exploreOnlyDeserializedChildren && Item1 != null) || (_Item1_Accessed && _Item1 != null))
                {
                    bool isMatch_Item1 = matchCriterion == null || matchCriterion(Item1);
                    bool shouldExplore_Item1 = exploreCriterion == null || exploreCriterion(Item1);
                    if (isMatch_Item1)
                    {
                        yield return ("Item1", Item1);
                    }
                    if ((!stopExploringBelowMatch || !isMatch_Item1) && shouldExplore_Item1)
                    {
                        foreach (var toYield in Item1.EnumerateLazinatorDescendants(matchCriterion, stopExploringBelowMatch, exploreCriterion, exploreOnlyDeserializedChildren, enumerateNulls))
                        {
                            yield return ("Item1" + "." + toYield.propertyName, toYield.descendant);
                        }
                    }
                }
            }
            if (enumerateNulls && (!exploreOnlyDeserializedChildren || _Item2_Accessed) && Item2 == null)
            {
                yield return ("Item2", default);
            }
            else
            {
                if ((!exploreOnlyDeserializedChildren && Item2 != null) || (_Item2_Accessed && _Item2 != null))
                {
                    bool isMatch_Item2 = matchCriterion == null || matchCriterion(Item2);
                    bool shouldExplore_Item2 = exploreCriterion == null || exploreCriterion(Item2);
                    if (isMatch_Item2)
                    {
                        yield return ("Item2", Item2);
                    }
                    if ((!stopExploringBelowMatch || !isMatch_Item2) && shouldExplore_Item2)
                    {
                        foreach (var toYield in Item2.EnumerateLazinatorDescendants(matchCriterion, stopExploringBelowMatch, exploreCriterion, exploreOnlyDeserializedChildren, enumerateNulls))
                        {
                            yield return ("Item2" + "." + toYield.propertyName, toYield.descendant);
                        }
                    }
                }
            }
            if (enumerateNulls && (!exploreOnlyDeserializedChildren || _Item3_Accessed) && Item3 == null)
            {
                yield return ("Item3", default);
            }
            else
            {
                if ((!exploreOnlyDeserializedChildren && Item3 != null) || (_Item3_Accessed && _Item3 != null))
                {
                    bool isMatch_Item3 = matchCriterion == null || matchCriterion(Item3);
                    bool shouldExplore_Item3 = exploreCriterion == null || exploreCriterion(Item3);
                    if (isMatch_Item3)
                    {
                        yield return ("Item3", Item3);
                    }
                    if ((!stopExploringBelowMatch || !isMatch_Item3) && shouldExplore_Item3)
                    {
                        foreach (var toYield in Item3.EnumerateLazinatorDescendants(matchCriterion, stopExploringBelowMatch, exploreCriterion, exploreOnlyDeserializedChildren, enumerateNulls))
                        {
                            yield return ("Item3" + "." + toYield.propertyName, toYield.descendant);
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
            if ((!exploreOnlyDeserializedChildren && Item1 != null) || (_Item1_Accessed && _Item1 != null))
            {
                _Item1 = (T) _Item1.ForEachLazinator(changeFunc, exploreOnlyDeserializedChildren, true);
            }
            if ((!exploreOnlyDeserializedChildren && Item2 != null) || (_Item2_Accessed && _Item2 != null))
            {
                _Item2 = (U) _Item2.ForEachLazinator(changeFunc, exploreOnlyDeserializedChildren, true);
            }
            if ((!exploreOnlyDeserializedChildren && Item3 != null) || (_Item3_Accessed && _Item3 != null))
            {
                _Item3 = (V) _Item3.ForEachLazinator(changeFunc, exploreOnlyDeserializedChildren, true);
            }
            if (changeThisLevel && changeFunc != null)
            {
                return changeFunc(this);
            }
            return this;
        }
        
        public virtual void FreeInMemoryObjects()
        {
            _Item1 = default;
            _Item2 = default;
            _Item3 = default;
            _Item1_Accessed = _Item2_Accessed = _Item3_Accessed = false;
            IsDirty = false;
            DescendantIsDirty = false;
            HasChanged = false;
            DescendantHasChanged = false;
        }
        
        /* Conversion */
        
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public virtual int LazinatorUniqueID => 206;
        
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        protected virtual bool ContainsOpenGenericParameters => true;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public virtual LazinatorGenericIDType LazinatorGenericID => LazinatorGenericIDType.GetCachedForType<LazinatorTriple<T, U, V>>(() => DeserializationFactory.Instance.GetUniqueIDListForGenericType(206, new Type[] { typeof(T), typeof(U), typeof(V) }));
        
        
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public virtual int LazinatorObjectVersion { get; set; } = 0;
        
        
        protected virtual int ConvertFromBytesAfterHeader(IncludeChildrenMode includeChildrenMode, int serializedVersionNumber, ref int bytesSoFar)
        {
            ReadOnlySpan<byte> span = LazinatorMemoryStorage.InitialReadOnlyMemory.Span;
            ConvertFromBytesForPrimitiveProperties(span, includeChildrenMode, serializedVersionNumber, ref bytesSoFar);
            int lengthForLengths = 0;
            if (includeChildrenMode != IncludeChildrenMode.ExcludeAllChildren && includeChildrenMode != IncludeChildrenMode.IncludeOnlyIncludableChildren)
            {
                lengthForLengths += 12;
            }
            int totalChildrenSize = ConvertFromBytesForChildLengths(span, includeChildrenMode, serializedVersionNumber, bytesSoFar + lengthForLengths, ref bytesSoFar);
            return bytesSoFar + totalChildrenSize;
        }
        
        protected virtual void ConvertFromBytesForPrimitiveProperties(ReadOnlySpan<byte> span, IncludeChildrenMode includeChildrenMode, int serializedVersionNumber, ref int bytesSoFar)
        {
        }
        
        protected virtual int ConvertFromBytesForChildLengths(ReadOnlySpan<byte> span, IncludeChildrenMode includeChildrenMode, int serializedVersionNumber, int indexOfFirstChild, ref int bytesSoFar)
        {
            int totalChildrenBytes = 0;
            _Item1_ByteIndex = indexOfFirstChild + totalChildrenBytes;
            if (includeChildrenMode != IncludeChildrenMode.ExcludeAllChildren && includeChildrenMode != IncludeChildrenMode.IncludeOnlyIncludableChildren)
            {
                totalChildrenBytes += span.ToInt32(ref bytesSoFar);
            }
            _Item2_ByteIndex = indexOfFirstChild + totalChildrenBytes;
            if (includeChildrenMode != IncludeChildrenMode.ExcludeAllChildren && includeChildrenMode != IncludeChildrenMode.IncludeOnlyIncludableChildren)
            {
                totalChildrenBytes += span.ToInt32(ref bytesSoFar);
            }
            _Item3_ByteIndex = indexOfFirstChild + totalChildrenBytes;
            if (includeChildrenMode != IncludeChildrenMode.ExcludeAllChildren && includeChildrenMode != IncludeChildrenMode.IncludeOnlyIncludableChildren)
            {
                totalChildrenBytes += span.ToInt32(ref bytesSoFar);
            }
            _LazinatorTriple_T_U_V_EndByteIndex = indexOfFirstChild + totalChildrenBytes;
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
                
                if (_Item1_Accessed && _Item1 != null && _Item1.IsStruct && (_Item1.IsDirty || _Item1.DescendantIsDirty))
                {
                    _Item1_Accessed = false;
                }
                if (_Item2_Accessed && _Item2 != null && _Item2.IsStruct && (_Item2.IsDirty || _Item2.DescendantIsDirty))
                {
                    _Item2_Accessed = false;
                }
                if (_Item3_Accessed && _Item3 != null && _Item3.IsStruct && (_Item3.IsDirty || _Item3.DescendantIsDirty))
                {
                    _Item3_Accessed = false;
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
            if (_Item1_Accessed && _Item1 != null)
            {
                Item1.UpdateStoredBuffer(ref writer, startPosition + _Item1_ByteIndex, _Item1_ByteLength, IncludeChildrenMode.IncludeAllChildren, true);
            }
            if (_Item2_Accessed && _Item2 != null)
            {
                Item2.UpdateStoredBuffer(ref writer, startPosition + _Item2_ByteIndex, _Item2_ByteLength, IncludeChildrenMode.IncludeAllChildren, true);
            }
            if (_Item3_Accessed && _Item3 != null)
            {
                Item3.UpdateStoredBuffer(ref writer, startPosition + _Item3_ByteIndex, _Item3_ByteLength, IncludeChildrenMode.IncludeAllChildren, true);
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
            
            
            int lengthForLengths = 0;
            if (options.IncludeChildrenMode != IncludeChildrenMode.ExcludeAllChildren && options.IncludeChildrenMode != IncludeChildrenMode.IncludeOnlyIncludableChildren)
            {
                lengthForLengths += 12;
            }
            
            var previousLengthsPosition = writer.SetLengthsPosition(lengthForLengths);
            WriteChildrenPropertiesIntoBuffer(ref writer, options, includeUniqueID, startPosition);
            writer.ResetLengthsPosition(previousLengthsPosition);
            
        }
        
        protected virtual void WritePrimitivePropertiesIntoBuffer(ref BufferWriter writer, in LazinatorSerializationOptions options, bool includeUniqueID)
        {
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
                if ((options.IncludeChildrenMode != IncludeChildrenMode.IncludeAllChildren || options.IncludeChildrenMode != OriginalIncludeChildrenMode) && !_Item1_Accessed)
                {
                    var deserialized = Item1;
                }
                WriteChild(ref writer, ref _Item1, options, _Item1_Accessed, () => GetChildSlice(LazinatorMemoryStorage, _Item1_ByteIndex, _Item1_ByteLength, null), this);
                lengthValue = writer.ActiveMemoryPosition - startOfChildPosition;
                if (lengthValue > int.MaxValue)
                {
                    ThrowHelper.ThrowTooLargeException(int.MaxValue);
                }
                writer.RecordLength((int) lengthValue);
            }
            if (options.UpdateStoredBuffer)
            {
                _Item1_ByteIndex = startOfChildPosition - startOfObjectPosition;
                
            }
            startOfChildPosition = writer.ActiveMemoryPosition;
            if (options.IncludeChildrenMode != IncludeChildrenMode.ExcludeAllChildren && options.IncludeChildrenMode != IncludeChildrenMode.IncludeOnlyIncludableChildren)
            {
                if ((options.IncludeChildrenMode != IncludeChildrenMode.IncludeAllChildren || options.IncludeChildrenMode != OriginalIncludeChildrenMode) && !_Item2_Accessed)
                {
                    var deserialized = Item2;
                }
                WriteChild(ref writer, ref _Item2, options, _Item2_Accessed, () => GetChildSlice(LazinatorMemoryStorage, _Item2_ByteIndex, _Item2_ByteLength, null), this);
                lengthValue = writer.ActiveMemoryPosition - startOfChildPosition;
                if (lengthValue > int.MaxValue)
                {
                    ThrowHelper.ThrowTooLargeException(int.MaxValue);
                }
                writer.RecordLength((int) lengthValue);
            }
            if (options.UpdateStoredBuffer)
            {
                _Item2_ByteIndex = startOfChildPosition - startOfObjectPosition;
                
            }
            startOfChildPosition = writer.ActiveMemoryPosition;
            if (options.IncludeChildrenMode != IncludeChildrenMode.ExcludeAllChildren && options.IncludeChildrenMode != IncludeChildrenMode.IncludeOnlyIncludableChildren)
            {
                if ((options.IncludeChildrenMode != IncludeChildrenMode.IncludeAllChildren || options.IncludeChildrenMode != OriginalIncludeChildrenMode) && !_Item3_Accessed)
                {
                    var deserialized = Item3;
                }
                WriteChild(ref writer, ref _Item3, options, _Item3_Accessed, () => GetChildSlice(LazinatorMemoryStorage, _Item3_ByteIndex, _Item3_ByteLength, null), this);
                lengthValue = writer.ActiveMemoryPosition - startOfChildPosition;
                if (lengthValue > int.MaxValue)
                {
                    ThrowHelper.ThrowTooLargeException(int.MaxValue);
                }
                writer.RecordLength((int) lengthValue);
            }
            if (options.UpdateStoredBuffer)
            {
                _Item3_ByteIndex = startOfChildPosition - startOfObjectPosition;
                
            }
            if (options.UpdateStoredBuffer)
            {
                _LazinatorTriple_T_U_V_EndByteIndex = writer.ActiveMemoryPosition - startOfObjectPosition;
            }
            
        }
    }
}
#nullable restore
