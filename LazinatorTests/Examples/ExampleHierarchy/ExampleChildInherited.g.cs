//b4791ed4-fa6b-d849-39fd-bf152c441156
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by the Lazinator tool, version 0.1.0.383
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
    using System;
    using System.Buffers;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Runtime.InteropServices;
    using static Lazinator.Core.LazinatorUtilities;
    
    [Autogenerated]
    public partial class ExampleChildInherited : ExampleChild, ILazinator
    {
        /* Property definitions */
        
        protected int _MyGrandchildInInherited_ByteIndex;
        private int _ExampleChildInherited_EndByteIndex;
        protected virtual int _MyGrandchildInInherited_ByteLength => _ExampleChildInherited_EndByteIndex - _MyGrandchildInInherited_ByteIndex;
        
        
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
        
        protected ExampleGrandchild _MyGrandchildInInherited;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public ExampleGrandchild MyGrandchildInInherited
        {
            get
            {
                if (!_MyGrandchildInInherited_Accessed)
                {
                    Lazinate_MyGrandchildInInherited();
                } 
                return _MyGrandchildInInherited;
            }
            set
            {
                if (_MyGrandchildInInherited != null)
                {
                    _MyGrandchildInInherited.LazinatorParents = _MyGrandchildInInherited.LazinatorParents.WithRemoved(this);
                }
                if (value != null)
                {
                    value.LazinatorParents = value.LazinatorParents.WithAdded(this);
                }
                
                IsDirty = true;
                DescendantIsDirty = true;
                _MyGrandchildInInherited = value;
                _MyGrandchildInInherited_Accessed = true;
            }
        }
        protected bool _MyGrandchildInInherited_Accessed;
        private void Lazinate_MyGrandchildInInherited()
        {
            if (LazinatorObjectBytes.Length == 0)
            {
                _MyGrandchildInInherited = null;
            }
            else
            {
                LazinatorMemory childData = GetChildSlice(LazinatorMemoryStorage, _MyGrandchildInInherited_ByteIndex, _MyGrandchildInInherited_ByteLength, false, false, null);
                
                _MyGrandchildInInherited = DeserializationFactory.Instance.CreateBaseOrDerivedType(1079, (c, p) => new ExampleGrandchild(c, p), childData, this); 
            }
            
            _MyGrandchildInInherited_Accessed = true;
        }
        
        /* Clone overrides */
        
        public ExampleChildInherited(IncludeChildrenMode originalIncludeChildrenMode) : base(originalIncludeChildrenMode)
        {
        }
        
        public ExampleChildInherited(LazinatorMemory serializedBytes, ILazinator parent = null) : base(serializedBytes, parent)
        {
        }
        
        public override ILazinator CloneLazinator(IncludeChildrenMode includeChildrenMode = IncludeChildrenMode.IncludeAllChildren, CloneBufferOptions cloneBufferOptions = CloneBufferOptions.IndependentBuffers)
        {
            ExampleChildInherited clone;
            if (cloneBufferOptions == CloneBufferOptions.NoBuffer)
            {
                clone = new ExampleChildInherited(includeChildrenMode);
                clone.LazinatorObjectVersion = LazinatorObjectVersion;
                clone = (ExampleChildInherited)AssignCloneProperties(clone, includeChildrenMode);
            }
            else
            {
                LazinatorMemory bytes = EncodeOrRecycleToNewBuffer(includeChildrenMode, OriginalIncludeChildrenMode, false, IsDirty, DescendantIsDirty, false, LazinatorMemoryStorage, false, this);
                clone = new ExampleChildInherited(bytes);
            }
            return clone;
        }
        
        public override ILazinator AssignCloneProperties(ILazinator clone, IncludeChildrenMode includeChildrenMode)
        {
            base.AssignCloneProperties(clone, includeChildrenMode);
            ExampleChildInherited typedClone = (ExampleChildInherited) clone;
            typedClone.MyInt = MyInt;
            if (includeChildrenMode != IncludeChildrenMode.ExcludeAllChildren && includeChildrenMode != IncludeChildrenMode.IncludeOnlyIncludableChildren)
            {
                if (MyGrandchildInInherited == null)
                {
                    typedClone.MyGrandchildInInherited = null;
                }
                else
                {
                    typedClone.MyGrandchildInInherited = (ExampleGrandchild) MyGrandchildInInherited.CloneLazinator(includeChildrenMode, CloneBufferOptions.NoBuffer);
                }
                
            }
            
            
            return typedClone;
        }
        
        /* Properties */
        
        public override IEnumerable<(string propertyName, ILazinator descendant)> EnumerateLazinatorDescendants(Func<ILazinator, bool> matchCriterion, bool stopExploringBelowMatch, Func<ILazinator, bool> exploreCriterion, bool exploreOnlyDeserializedChildren, bool enumerateNulls)
        {
            foreach (var inheritedYield in base.EnumerateLazinatorDescendants(matchCriterion, stopExploringBelowMatch, exploreCriterion, exploreOnlyDeserializedChildren, enumerateNulls))
            {
                yield return inheritedYield;
            }
            if (enumerateNulls && (!exploreOnlyDeserializedChildren || _MyGrandchildInInherited_Accessed) && MyGrandchildInInherited == null)
            {
                yield return ("MyGrandchildInInherited", default);
            }
            else
            {
                if ((!exploreOnlyDeserializedChildren && MyGrandchildInInherited != null) || (_MyGrandchildInInherited_Accessed && _MyGrandchildInInherited != null))
                {
                    bool isMatch_MyGrandchildInInherited = matchCriterion == null || matchCriterion(MyGrandchildInInherited);
                    bool shouldExplore_MyGrandchildInInherited = exploreCriterion == null || exploreCriterion(MyGrandchildInInherited);
                    if (isMatch_MyGrandchildInInherited)
                    {
                        yield return ("MyGrandchildInInherited", MyGrandchildInInherited);
                    }
                    if ((!stopExploringBelowMatch || !isMatch_MyGrandchildInInherited) && shouldExplore_MyGrandchildInInherited)
                    {
                        foreach (var toYield in MyGrandchildInInherited.EnumerateLazinatorDescendants(matchCriterion, stopExploringBelowMatch, exploreCriterion, exploreOnlyDeserializedChildren, enumerateNulls))
                        {
                            yield return ("MyGrandchildInInherited" + "." + toYield.propertyName, toYield.descendant);
                        }
                    }
                }
                
            }
            
            yield break;
        }
        
        
        public override IEnumerable<(string propertyName, object descendant)> EnumerateNonLazinatorProperties()
        {
            foreach (var inheritedYield in base.EnumerateNonLazinatorProperties())
            {
                yield return inheritedYield;
            }
            yield return ("MyInt", (object)MyInt);
            yield break;
        }
        
        public override ILazinator ForEachLazinator(Func<ILazinator, ILazinator> changeFunc, bool exploreOnlyDeserializedChildren, bool changeThisLevel)
        {
            base.ForEachLazinator(changeFunc, exploreOnlyDeserializedChildren, false);
            if ((!exploreOnlyDeserializedChildren && MyGrandchildInInherited != null) || (_MyGrandchildInInherited_Accessed && _MyGrandchildInInherited != null))
            {
                _MyGrandchildInInherited = (ExampleGrandchild) _MyGrandchildInInherited.ForEachLazinator(changeFunc, exploreOnlyDeserializedChildren, true);
            }
            if (changeThisLevel && changeFunc != null)
            {
                return changeFunc(this);
            }
            return this;
        }
        
        public override void FreeInMemoryObjects()
        {
            base.FreeInMemoryObjects();
            _MyGrandchildInInherited = default;
            _MyGrandchildInInherited_Accessed = false;
            IsDirty = false;
            DescendantIsDirty = false;
            HasChanged = false;
            DescendantHasChanged = false;
        }
        
        /* Conversion */
        
        public override int LazinatorUniqueID => 1014;
        
        protected override bool ContainsOpenGenericParameters => false;
        public override LazinatorGenericIDType LazinatorGenericID => default;
        
        
        public override int LazinatorObjectVersion { get; set; } = 0;
        
        
        public override void ConvertFromBytesAfterHeader(IncludeChildrenMode includeChildrenMode, int serializedVersionNumber, ref int bytesSoFar)
        {
            base.ConvertFromBytesAfterHeader(OriginalIncludeChildrenMode, serializedVersionNumber, ref bytesSoFar);
            ReadOnlySpan<byte> span = LazinatorObjectBytes.Span;
            _MyInt = span.ToDecompressedInt(ref bytesSoFar);
            _MyGrandchildInInherited_ByteIndex = bytesSoFar;
            if (includeChildrenMode != IncludeChildrenMode.ExcludeAllChildren && includeChildrenMode != IncludeChildrenMode.IncludeOnlyIncludableChildren)
            {
                bytesSoFar = span.ToInt32(ref bytesSoFar) + bytesSoFar;
            }
            
            _ExampleChildInherited_EndByteIndex = bytesSoFar;
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
            base.UpdateDeserializedChildren(ref writer, startPosition);
            if (_MyGrandchildInInherited_Accessed && _MyGrandchildInInherited != null)
            {
                MyGrandchildInInherited.UpdateStoredBuffer(ref writer, startPosition + _MyGrandchildInInherited_ByteIndex + sizeof(int), _MyGrandchildInInherited_ByteLength - sizeof(int), IncludeChildrenMode.IncludeAllChildren, true);
            }
            
        }
        
        
        protected override void WritePropertiesIntoBuffer(ref BinaryBufferWriter writer, IncludeChildrenMode includeChildrenMode, bool verifyCleanness, bool updateStoredBuffer, bool includeUniqueID)
        {
            int startPosition = writer.Position;
            int startOfObjectPosition = 0;
            base.WritePropertiesIntoBuffer(ref writer, includeChildrenMode, verifyCleanness, updateStoredBuffer, includeUniqueID);
            // write properties
            CompressedIntegralTypes.WriteCompressedInt(ref writer, _MyInt);
            startOfObjectPosition = writer.Position;
            if (includeChildrenMode != IncludeChildrenMode.ExcludeAllChildren && includeChildrenMode != IncludeChildrenMode.IncludeOnlyIncludableChildren)
            {
                if ((includeChildrenMode != IncludeChildrenMode.IncludeAllChildren || includeChildrenMode != OriginalIncludeChildrenMode) && !_MyGrandchildInInherited_Accessed)
                {
                    var deserialized = MyGrandchildInInherited;
                }
                WriteChild(ref writer, ref _MyGrandchildInInherited, includeChildrenMode, _MyGrandchildInInherited_Accessed, () => GetChildSlice(LazinatorMemoryStorage, _MyGrandchildInInherited_ByteIndex, _MyGrandchildInInherited_ByteLength, false, false, null), verifyCleanness, updateStoredBuffer, false, false, this);
            }
            
            if (updateStoredBuffer)
            {
                _MyGrandchildInInherited_ByteIndex = startOfObjectPosition - startPosition;
            }
            if (updateStoredBuffer)
            {
                _ExampleChildInherited_EndByteIndex = writer.Position - startPosition;
            }
        }
        
    }
}
