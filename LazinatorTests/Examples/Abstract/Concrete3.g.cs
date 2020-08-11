//3d6ff8f3-3fa4-9774-d05b-69c24310ef13
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by the Lazinator tool, version 0.1.0.380
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

#nullable disable
namespace LazinatorTests.Examples.Abstract
{
    using Lazinator.Attributes;
    using Lazinator.Buffers;
    using Lazinator.Core;
    using Lazinator.Exceptions;
    using Lazinator.Support;
    using LazinatorTests.Examples;
    using System;
    using System.Buffers;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Runtime.InteropServices;
    using static Lazinator.Core.LazinatorUtilities;
    
    [Autogenerated]
    public partial class Concrete3 : ILazinator
    {
        /* Property definitions */
        
        protected int _IntList3_ByteIndex;
        protected override int _Example2_ByteLength => _Example3_ByteIndex - _Example2_ByteIndex;
        protected override int _Example3_ByteLength => _IntList1_ByteIndex - _Example3_ByteIndex;
        protected override int _IntList1_ByteLength => _IntList2_ByteIndex - _IntList1_ByteIndex;
        protected override int _IntList2_ByteLength => _IntList3_ByteIndex - _IntList2_ByteIndex;
        private int _Concrete3_EndByteIndex;
        protected virtual int _IntList3_ByteLength => _Concrete3_EndByteIndex - _IntList3_ByteIndex;
        
        
        protected string _String1;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public override string String1
        {
            get
            {
                return _String1;
            }
            set
            {
                IsDirty = true;
                _String1 = value;
            }
        }
        
        protected string _String2;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public override string String2
        {
            get
            {
                return _String2;
            }
            set
            {
                IsDirty = true;
                _String2 = value;
            }
        }
        
        protected string _String3;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public string String3
        {
            get
            {
                return _String3;
            }
            set
            {
                IsDirty = true;
                _String3 = value;
            }
        }
        
        protected Example _Example2;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public override Example Example2
        {
            get
            {
                if (!_Example2_Accessed)
                {
                    Lazinate_Example2();
                } 
                return _Example2;
            }
            set
            {
                if (_Example2 != null)
                {
                    _Example2.LazinatorParents = _Example2.LazinatorParents.WithRemoved(this);
                }
                if (value != null)
                {
                    value.LazinatorParents = value.LazinatorParents.WithAdded(this);
                }
                
                IsDirty = true;
                DescendantIsDirty = true;
                _Example2 = value;
                _Example2_Accessed = true;
            }
        }
        private void Lazinate_Example2()
        {
            if (LazinatorObjectBytes.Length == 0)
            {
                _Example2 = null;
            }
            else
            {
                LazinatorMemory childData = GetChildSlice(LazinatorMemoryStorage, _Example2_ByteIndex, _Example2_ByteLength, false, false, null);
                
                _Example2 = DeserializationFactory.Instance.CreateBaseOrDerivedType(1012, () => new Example(LazinatorConstructorEnum.LazinatorConstructor), childData, this); 
            }
            _Example2_Accessed = true;
        }
        
        
        protected Example _Example3;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public override Example Example3
        {
            get
            {
                if (!_Example3_Accessed)
                {
                    Lazinate_Example3();
                } 
                return _Example3;
            }
            set
            {
                if (_Example3 != null)
                {
                    _Example3.LazinatorParents = _Example3.LazinatorParents.WithRemoved(this);
                }
                if (value != null)
                {
                    value.LazinatorParents = value.LazinatorParents.WithAdded(this);
                }
                
                IsDirty = true;
                DescendantIsDirty = true;
                _Example3 = value;
                _Example3_Accessed = true;
            }
        }
        private void Lazinate_Example3()
        {
            if (LazinatorObjectBytes.Length == 0)
            {
                _Example3 = null;
            }
            else
            {
                LazinatorMemory childData = GetChildSlice(LazinatorMemoryStorage, _Example3_ByteIndex, _Example3_ByteLength, false, false, null);
                
                _Example3 = DeserializationFactory.Instance.CreateBaseOrDerivedType(1012, () => new Example(LazinatorConstructorEnum.LazinatorConstructor), childData, this); 
            }
            _Example3_Accessed = true;
        }
        
        
        protected List<int> _IntList1;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public override List<int> IntList1
        {
            get
            {
                if (!_IntList1_Accessed)
                {
                    Lazinate_IntList1();
                }
                IsDirty = true; 
                return _IntList1;
            }
            set
            {
                IsDirty = true;
                DescendantIsDirty = true;
                _IntList1 = value;
                _IntList1_Accessed = true;
            }
        }
        private void Lazinate_IntList1()
        {
            if (LazinatorObjectBytes.Length == 0)
            {
                _IntList1 = default(List<int>);
            }
            else
            {
                LazinatorMemory childData = GetChildSlice(LazinatorMemoryStorage, _IntList1_ByteIndex, _IntList1_ByteLength, false, false, null);
                _IntList1 = ConvertFromBytes_List_Gint_g(childData);
            }
            _IntList1_Accessed = true;
        }
        
        
        protected List<int> _IntList2;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public override List<int> IntList2
        {
            get
            {
                if (!_IntList2_Accessed)
                {
                    Lazinate_IntList2();
                }
                IsDirty = true; 
                return _IntList2;
            }
            set
            {
                IsDirty = true;
                DescendantIsDirty = true;
                _IntList2 = value;
                _IntList2_Accessed = true;
            }
        }
        private void Lazinate_IntList2()
        {
            if (LazinatorObjectBytes.Length == 0)
            {
                _IntList2 = default(List<int>);
            }
            else
            {
                LazinatorMemory childData = GetChildSlice(LazinatorMemoryStorage, _IntList2_ByteIndex, _IntList2_ByteLength, false, false, null);
                _IntList2 = ConvertFromBytes_List_Gint_g(childData);
            }
            _IntList2_Accessed = true;
        }
        
        
        protected List<int> _IntList3;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public List<int> IntList3
        {
            get
            {
                if (!_IntList3_Accessed)
                {
                    Lazinate_IntList3();
                }
                IsDirty = true; 
                return _IntList3;
            }
            set
            {
                IsDirty = true;
                DescendantIsDirty = true;
                _IntList3 = value;
                _IntList3_Accessed = true;
            }
        }
        protected bool _IntList3_Accessed;
        private void Lazinate_IntList3()
        {
            if (LazinatorObjectBytes.Length == 0)
            {
                _IntList3 = default(List<int>);
            }
            else
            {
                LazinatorMemory childData = GetChildSlice(LazinatorMemoryStorage, _IntList3_ByteIndex, _IntList3_ByteLength, false, false, null);
                _IntList3 = ConvertFromBytes_List_Gint_g(childData);
            }
            _IntList3_Accessed = true;
        }
        
        
        /* Serialization, deserialization, and object relationships */
        
        public Concrete3(LazinatorConstructorEnum constructorEnum)
        {
        }
        
        public override LazinatorParentsCollection LazinatorParents { get; set; }
        
        public override IncludeChildrenMode OriginalIncludeChildrenMode { get; set; }
        
        public override int Deserialize()
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
        
        public override LazinatorMemory SerializeLazinator(IncludeChildrenMode includeChildrenMode, bool verifyCleanness, bool updateStoredBuffer) 
        {
            if (LazinatorMemoryStorage.IsEmpty || includeChildrenMode != OriginalIncludeChildrenMode || (verifyCleanness || IsDirty || (includeChildrenMode != IncludeChildrenMode.ExcludeAllChildren && DescendantIsDirty)))
            {
                return EncodeToNewBuffer(includeChildrenMode, verifyCleanness, updateStoredBuffer);
            }
            BinaryBufferWriter writer = new BinaryBufferWriter(LazinatorMemoryStorage.Length);
            writer.Write(LazinatorMemoryStorage.Span);
            return writer.LazinatorMemory;
        }
        
        protected override LazinatorMemory EncodeToNewBuffer(IncludeChildrenMode includeChildrenMode, bool verifyCleanness, bool updateStoredBuffer) 
        {
            int bufferSize = LazinatorMemoryStorage.Length == 0 ? ExpandableBytes.DefaultMinBufferSize : LazinatorMemoryStorage.Length;
            BinaryBufferWriter writer = new BinaryBufferWriter(bufferSize);
            SerializeExistingBuffer(ref writer, includeChildrenMode, verifyCleanness, updateStoredBuffer);
            return writer.LazinatorMemory;
        }
        
        public override ILazinator CloneLazinator(IncludeChildrenMode includeChildrenMode = IncludeChildrenMode.IncludeAllChildren, CloneBufferOptions cloneBufferOptions = CloneBufferOptions.IndependentBuffers)
        {
            var clone = new Concrete3(LazinatorConstructorEnum.LazinatorConstructor)
            {
                OriginalIncludeChildrenMode = includeChildrenMode
            };
            clone = CompleteClone(this, clone, includeChildrenMode, cloneBufferOptions);
            return clone;
        }
        
        public override ILazinator AssignCloneProperties(ILazinator clone, IncludeChildrenMode includeChildrenMode)
        {
            clone.FreeInMemoryObjects();
            Concrete3 typedClone = (Concrete3) clone;
            typedClone.String1 = String1;
            typedClone.String2 = String2;
            typedClone.String3 = String3;
            if (includeChildrenMode != IncludeChildrenMode.ExcludeAllChildren && includeChildrenMode != IncludeChildrenMode.IncludeOnlyIncludableChildren)
            {
                if (Example2 == null)
                {
                    typedClone.Example2 = null;
                }
                else
                {
                    typedClone.Example2 = (Example) Example2.CloneLazinator(includeChildrenMode, CloneBufferOptions.NoBuffer);
                }
                
            }
            
            if (includeChildrenMode != IncludeChildrenMode.ExcludeAllChildren && includeChildrenMode != IncludeChildrenMode.IncludeOnlyIncludableChildren)
            {
                if (Example3 == null)
                {
                    typedClone.Example3 = null;
                }
                else
                {
                    typedClone.Example3 = (Example) Example3.CloneLazinator(includeChildrenMode, CloneBufferOptions.NoBuffer);
                }
                
            }
            
            typedClone.IntList1 = CloneOrChange_List_Gint_g(IntList1, l => l?.CloneLazinator(includeChildrenMode, CloneBufferOptions.NoBuffer), false);
            typedClone.IntList2 = CloneOrChange_List_Gint_g(IntList2, l => l?.CloneLazinator(includeChildrenMode, CloneBufferOptions.NoBuffer), false);
            typedClone.IntList3 = CloneOrChange_List_Gint_g(IntList3, l => l?.CloneLazinator(includeChildrenMode, CloneBufferOptions.NoBuffer), false);
            
            return typedClone;
        }
        
        public override bool HasChanged { get; set; }
        
        protected bool _IsDirty;
        public override bool IsDirty
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
        public override bool DescendantHasChanged
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
        public override bool DescendantIsDirty
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
        
        public override void DeserializeLazinator(LazinatorMemory serializedBytes)
        {
            LazinatorMemoryStorage = serializedBytes;
            int length = Deserialize();
            if (length != LazinatorMemoryStorage.Length)
            {
                LazinatorMemoryStorage = LazinatorMemoryStorage.Slice(0, length);
            }
        }
        
        public override LazinatorMemory LazinatorMemoryStorage
        {
            get;
            set;
        }
        protected override ReadOnlyMemory<byte> LazinatorObjectBytes => LazinatorMemoryStorage.IsEmpty ? LazinatorMemory.EmptyReadOnlyMemory : LazinatorMemoryStorage.Memory;
        
        public override void UpdateStoredBuffer()
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
        
        public override int GetByteLength()
        {
            UpdateStoredBuffer();
            return LazinatorObjectBytes.Length;
        }
        
        public override bool NonBinaryHash32 => false;
        
        
        public override IEnumerable<ILazinator> EnumerateLazinatorNodes(Func<ILazinator, bool> matchCriterion, bool stopExploringBelowMatch, Func<ILazinator, bool> exploreCriterion, bool exploreOnlyDeserializedChildren, bool enumerateNulls)
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
        
        public override IEnumerable<(string propertyName, ILazinator descendant)> EnumerateLazinatorDescendants(Func<ILazinator, bool> matchCriterion, bool stopExploringBelowMatch, Func<ILazinator, bool> exploreCriterion, bool exploreOnlyDeserializedChildren, bool enumerateNulls)
        {
            if (enumerateNulls && (!exploreOnlyDeserializedChildren || _Example2_Accessed) && Example2 == null)
            {
                yield return ("Example2", default);
            }
            else
            {
                if ((!exploreOnlyDeserializedChildren && Example2 != null) || (_Example2_Accessed && _Example2 != null))
                {
                    bool isMatch_Example2 = matchCriterion == null || matchCriterion(Example2);
                    bool shouldExplore_Example2 = exploreCriterion == null || exploreCriterion(Example2);
                    if (isMatch_Example2)
                    {
                        yield return ("Example2", Example2);
                    }
                    if ((!stopExploringBelowMatch || !isMatch_Example2) && shouldExplore_Example2)
                    {
                        foreach (var toYield in Example2.EnumerateLazinatorDescendants(matchCriterion, stopExploringBelowMatch, exploreCriterion, exploreOnlyDeserializedChildren, enumerateNulls))
                        {
                            yield return ("Example2" + "." + toYield.propertyName, toYield.descendant);
                        }
                    }
                }
                
            }
            
            if (enumerateNulls && (!exploreOnlyDeserializedChildren || _Example3_Accessed) && Example3 == null)
            {
                yield return ("Example3", default);
            }
            else
            {
                if ((!exploreOnlyDeserializedChildren && Example3 != null) || (_Example3_Accessed && _Example3 != null))
                {
                    bool isMatch_Example3 = matchCriterion == null || matchCriterion(Example3);
                    bool shouldExplore_Example3 = exploreCriterion == null || exploreCriterion(Example3);
                    if (isMatch_Example3)
                    {
                        yield return ("Example3", Example3);
                    }
                    if ((!stopExploringBelowMatch || !isMatch_Example3) && shouldExplore_Example3)
                    {
                        foreach (var toYield in Example3.EnumerateLazinatorDescendants(matchCriterion, stopExploringBelowMatch, exploreCriterion, exploreOnlyDeserializedChildren, enumerateNulls))
                        {
                            yield return ("Example3" + "." + toYield.propertyName, toYield.descendant);
                        }
                    }
                }
                
            }
            
            yield break;
        }
        
        
        public override IEnumerable<(string propertyName, object descendant)> EnumerateNonLazinatorProperties()
        {
            yield return ("String1", (object)String1);
            yield return ("String2", (object)String2);
            yield return ("String3", (object)String3);
            yield return ("IntList1", (object)IntList1);
            yield return ("IntList2", (object)IntList2);
            yield return ("IntList3", (object)IntList3);
            yield break;
        }
        
        public override ILazinator ForEachLazinator(Func<ILazinator, ILazinator> changeFunc, bool exploreOnlyDeserializedChildren, bool changeThisLevel)
        {
            if ((!exploreOnlyDeserializedChildren && Example2 != null) || (_Example2_Accessed && _Example2 != null))
            {
                _Example2 = (Example) _Example2.ForEachLazinator(changeFunc, exploreOnlyDeserializedChildren, true);
            }
            if ((!exploreOnlyDeserializedChildren && Example3 != null) || (_Example3_Accessed && _Example3 != null))
            {
                _Example3 = (Example) _Example3.ForEachLazinator(changeFunc, exploreOnlyDeserializedChildren, true);
            }
            if ((!exploreOnlyDeserializedChildren && IntList1 != null) || (_IntList1_Accessed && _IntList1 != null))
            {
                _IntList1 = (List<int>) CloneOrChange_List_Gint_g(_IntList1, l => l?.ForEachLazinator(changeFunc, exploreOnlyDeserializedChildren, true), true);
            }
            if ((!exploreOnlyDeserializedChildren && IntList2 != null) || (_IntList2_Accessed && _IntList2 != null))
            {
                _IntList2 = (List<int>) CloneOrChange_List_Gint_g(_IntList2, l => l?.ForEachLazinator(changeFunc, exploreOnlyDeserializedChildren, true), true);
            }
            if ((!exploreOnlyDeserializedChildren && IntList3 != null) || (_IntList3_Accessed && _IntList3 != null))
            {
                _IntList3 = (List<int>) CloneOrChange_List_Gint_g(_IntList3, l => l?.ForEachLazinator(changeFunc, exploreOnlyDeserializedChildren, true), true);
            }
            if (changeThisLevel && changeFunc != null)
            {
                return changeFunc(this);
            }
            return this;
        }
        
        public override void FreeInMemoryObjects()
        {
            _Example2 = default;
            _Example3 = default;
            _IntList1 = default;
            _IntList2 = default;
            _IntList3 = default;
            _Example2_Accessed = _Example3_Accessed = _IntList1_Accessed = _IntList2_Accessed = _IntList3_Accessed = false;
            IsDirty = false;
            DescendantIsDirty = false;
            HasChanged = false;
            DescendantHasChanged = false;
        }
        
        /* Conversion */
        
        public override int LazinatorUniqueID => 1037;
        
        protected override bool ContainsOpenGenericParameters => false;
        public override LazinatorGenericIDType LazinatorGenericID => default;
        
        
        public override int LazinatorObjectVersion { get; set; } = 0;
        
        
        public override void ConvertFromBytesAfterHeader(IncludeChildrenMode includeChildrenMode, int serializedVersionNumber, ref int bytesSoFar)
        {
            ReadOnlySpan<byte> span = LazinatorObjectBytes.Span;
            _String1 = span.ToString_VarIntLengthUtf8(ref bytesSoFar);
            _String2 = span.ToString_VarIntLengthUtf8(ref bytesSoFar);
            _String3 = span.ToString_VarIntLengthUtf8(ref bytesSoFar);
            _Example2_ByteIndex = bytesSoFar;
            if (includeChildrenMode != IncludeChildrenMode.ExcludeAllChildren && includeChildrenMode != IncludeChildrenMode.IncludeOnlyIncludableChildren)
            {
                bytesSoFar = span.ToInt32(ref bytesSoFar) + bytesSoFar;
            }
            
            _Example3_ByteIndex = bytesSoFar;
            if (includeChildrenMode != IncludeChildrenMode.ExcludeAllChildren && includeChildrenMode != IncludeChildrenMode.IncludeOnlyIncludableChildren)
            {
                bytesSoFar = span.ToInt32(ref bytesSoFar) + bytesSoFar;
            }
            
            _IntList1_ByteIndex = bytesSoFar;
            bytesSoFar = span.ToInt32(ref bytesSoFar) + bytesSoFar;
            _IntList2_ByteIndex = bytesSoFar;
            bytesSoFar = span.ToInt32(ref bytesSoFar) + bytesSoFar;
            _IntList3_ByteIndex = bytesSoFar;
            bytesSoFar = span.ToInt32(ref bytesSoFar) + bytesSoFar;
            _Concrete3_EndByteIndex = bytesSoFar;
        }
        
        public override void SerializeExistingBuffer(ref BinaryBufferWriter writer, IncludeChildrenMode includeChildrenMode, bool verifyCleanness, bool updateStoredBuffer)
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
        
        public override void UpdateStoredBuffer(ref BinaryBufferWriter writer, int startPosition, int length, IncludeChildrenMode includeChildrenMode, bool updateDeserializedChildren)
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
        
        protected override void UpdateDeserializedChildren(ref BinaryBufferWriter writer, int startPosition)
        {
            if (_Example2_Accessed && _Example2 != null)
            {
                Example2.UpdateStoredBuffer(ref writer, startPosition + _Example2_ByteIndex + sizeof(int), _Example2_ByteLength - sizeof(int), IncludeChildrenMode.IncludeAllChildren, true);
            }
            
            if (_Example3_Accessed && _Example3 != null)
            {
                Example3.UpdateStoredBuffer(ref writer, startPosition + _Example3_ByteIndex + sizeof(int), _Example3_ByteLength - sizeof(int), IncludeChildrenMode.IncludeAllChildren, true);
            }
            
            if (_IntList1_Accessed && _IntList1 != null)
            {
                _IntList1 = (List<int>) CloneOrChange_List_Gint_g(_IntList1, l => l.RemoveBufferInHierarchy(), true);
            }
            if (_IntList2_Accessed && _IntList2 != null)
            {
                _IntList2 = (List<int>) CloneOrChange_List_Gint_g(_IntList2, l => l.RemoveBufferInHierarchy(), true);
            }
            if (_IntList3_Accessed && _IntList3 != null)
            {
                _IntList3 = (List<int>) CloneOrChange_List_Gint_g(_IntList3, l => l.RemoveBufferInHierarchy(), true);
            }
        }
        
        
        protected override void WritePropertiesIntoBuffer(ref BinaryBufferWriter writer, IncludeChildrenMode includeChildrenMode, bool verifyCleanness, bool updateStoredBuffer, bool includeUniqueID)
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
            EncodeCharAndString.WriteStringUtf8WithVarIntPrefix(ref writer, _String1);
            EncodeCharAndString.WriteStringUtf8WithVarIntPrefix(ref writer, _String2);
            EncodeCharAndString.WriteStringUtf8WithVarIntPrefix(ref writer, _String3);
            startOfObjectPosition = writer.Position;
            if (includeChildrenMode != IncludeChildrenMode.ExcludeAllChildren && includeChildrenMode != IncludeChildrenMode.IncludeOnlyIncludableChildren)
            {
                if ((includeChildrenMode != IncludeChildrenMode.IncludeAllChildren || includeChildrenMode != OriginalIncludeChildrenMode) && !_Example2_Accessed)
                {
                    var deserialized = Example2;
                }
                WriteChild(ref writer, ref _Example2, includeChildrenMode, _Example2_Accessed, () => GetChildSlice(LazinatorMemoryStorage, _Example2_ByteIndex, _Example2_ByteLength, false, false, null), verifyCleanness, updateStoredBuffer, false, false, this);
            }
            
            if (updateStoredBuffer)
            {
                _Example2_ByteIndex = startOfObjectPosition - startPosition;
            }
            startOfObjectPosition = writer.Position;
            if (includeChildrenMode != IncludeChildrenMode.ExcludeAllChildren && includeChildrenMode != IncludeChildrenMode.IncludeOnlyIncludableChildren)
            {
                if ((includeChildrenMode != IncludeChildrenMode.IncludeAllChildren || includeChildrenMode != OriginalIncludeChildrenMode) && !_Example3_Accessed)
                {
                    var deserialized = Example3;
                }
                WriteChild(ref writer, ref _Example3, includeChildrenMode, _Example3_Accessed, () => GetChildSlice(LazinatorMemoryStorage, _Example3_ByteIndex, _Example3_ByteLength, false, false, null), verifyCleanness, updateStoredBuffer, false, false, this);
            }
            
            if (updateStoredBuffer)
            {
                _Example3_ByteIndex = startOfObjectPosition - startPosition;
            }
            startOfObjectPosition = writer.Position;
            if ((includeChildrenMode != IncludeChildrenMode.IncludeAllChildren || includeChildrenMode != OriginalIncludeChildrenMode) && !_IntList1_Accessed)
            {
                var deserialized = IntList1;
            }
            WriteNonLazinatorObject(
            nonLazinatorObject: _IntList1, isBelievedDirty: _IntList1_Accessed || (includeChildrenMode != OriginalIncludeChildrenMode),
            isAccessed: _IntList1_Accessed, writer: ref writer,
            getChildSliceForFieldFn: () => GetChildSlice(LazinatorMemoryStorage, _IntList1_ByteIndex, _IntList1_ByteLength, false, false, null),
            verifyCleanness: false,
            binaryWriterAction: (ref BinaryBufferWriter w, bool v) =>
            ConvertToBytes_List_Gint_g(ref w, _IntList1,
            includeChildrenMode, v, updateStoredBuffer));
            if (updateStoredBuffer)
            {
                _IntList1_ByteIndex = startOfObjectPosition - startPosition;
            }
            startOfObjectPosition = writer.Position;
            if ((includeChildrenMode != IncludeChildrenMode.IncludeAllChildren || includeChildrenMode != OriginalIncludeChildrenMode) && !_IntList2_Accessed)
            {
                var deserialized = IntList2;
            }
            WriteNonLazinatorObject(
            nonLazinatorObject: _IntList2, isBelievedDirty: _IntList2_Accessed || (includeChildrenMode != OriginalIncludeChildrenMode),
            isAccessed: _IntList2_Accessed, writer: ref writer,
            getChildSliceForFieldFn: () => GetChildSlice(LazinatorMemoryStorage, _IntList2_ByteIndex, _IntList2_ByteLength, false, false, null),
            verifyCleanness: false,
            binaryWriterAction: (ref BinaryBufferWriter w, bool v) =>
            ConvertToBytes_List_Gint_g(ref w, _IntList2,
            includeChildrenMode, v, updateStoredBuffer));
            if (updateStoredBuffer)
            {
                _IntList2_ByteIndex = startOfObjectPosition - startPosition;
            }
            startOfObjectPosition = writer.Position;
            if ((includeChildrenMode != IncludeChildrenMode.IncludeAllChildren || includeChildrenMode != OriginalIncludeChildrenMode) && !_IntList3_Accessed)
            {
                var deserialized = IntList3;
            }
            WriteNonLazinatorObject(
            nonLazinatorObject: _IntList3, isBelievedDirty: _IntList3_Accessed || (includeChildrenMode != OriginalIncludeChildrenMode),
            isAccessed: _IntList3_Accessed, writer: ref writer,
            getChildSliceForFieldFn: () => GetChildSlice(LazinatorMemoryStorage, _IntList3_ByteIndex, _IntList3_ByteLength, false, false, null),
            verifyCleanness: false,
            binaryWriterAction: (ref BinaryBufferWriter w, bool v) =>
            ConvertToBytes_List_Gint_g(ref w, _IntList3,
            includeChildrenMode, v, updateStoredBuffer));
            if (updateStoredBuffer)
            {
                _IntList3_ByteIndex = startOfObjectPosition - startPosition;
            }
            if (updateStoredBuffer)
            {
                _Concrete3_EndByteIndex = writer.Position - startPosition;
            }
        }
        
        /* Conversion of supported collections and tuples */
        
        private static List<int> ConvertFromBytes_List_Gint_g(LazinatorMemory storage)
        {
            if (storage.Length == 0)
            {
                return default(List<int>);
            }
            ReadOnlySpan<byte> span = storage.Span;
            
            int bytesSoFar = 0;
            int collectionLength = span.ToDecompressedInt(ref bytesSoFar);
            
            List<int> collection = new List<int>(collectionLength);
            for (int itemIndex = 0; itemIndex < collectionLength; itemIndex++)
            {
                int item = span.ToDecompressedInt(ref bytesSoFar);
                collection.Add(item);
            }
            
            return collection;
        }
        
        private static void ConvertToBytes_List_Gint_g(ref BinaryBufferWriter writer, List<int> itemToConvert, IncludeChildrenMode includeChildrenMode, bool verifyCleanness, bool updateStoredBuffer)
        {
            if (itemToConvert == default(List<int>))
            {
                return;
            }
            CompressedIntegralTypes.WriteCompressedInt(ref writer, itemToConvert.Count);
            int itemToConvertCount = itemToConvert.Count;
            for (int itemIndex = 0; itemIndex < itemToConvertCount; itemIndex++)
            {
                CompressedIntegralTypes.WriteCompressedInt(ref writer, itemToConvert[itemIndex]);
            }
        }
        
        private static List<int> CloneOrChange_List_Gint_g(List<int> itemToClone, Func<ILazinator, ILazinator> cloneOrChangeFunc, bool avoidCloningIfPossible)
        {
            if (itemToClone == null)
            {
                return default;
            }
            
            int collectionLength = itemToClone.Count;
            List<int> collection = new List<int>(collectionLength);
            int itemToCloneCount = itemToClone.Count;
            for (int itemIndex = 0; itemIndex < itemToCloneCount; itemIndex++)
            {
                var itemCopied = (int) itemToClone[itemIndex];
                collection.Add(itemCopied);
            }
            return collection;
        }
        
    }
}
