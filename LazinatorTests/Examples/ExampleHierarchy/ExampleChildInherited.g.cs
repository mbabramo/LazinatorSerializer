//0e98f8d7-2052-f666-645d-d25adaa4ef87
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by the Lazinator tool, version 0.1.0.288
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

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
        /* Clone overrides */
        
        public ExampleChildInherited() : base()
        {
        }
        
        public override ILazinator CloneLazinator(IncludeChildrenMode includeChildrenMode = IncludeChildrenMode.IncludeAllChildren, CloneBufferOptions cloneBufferOptions = CloneBufferOptions.LinkedBuffer)
        {
            var clone = new ExampleChildInherited()
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
        
        protected override void AssignCloneProperties(ILazinator clone, IncludeChildrenMode includeChildrenMode)
        {
            base.AssignCloneProperties(clone, includeChildrenMode);
            ExampleChildInherited typedClone = (ExampleChildInherited) clone;
            typedClone.MyInt = MyInt;
            if (includeChildrenMode != IncludeChildrenMode.ExcludeAllChildren && includeChildrenMode != IncludeChildrenMode.IncludeOnlyIncludableChildren) 
            {
                typedClone.MyGrandchildInInherited = (MyGrandchildInInherited == null) ? default(ExampleGrandchild) : (ExampleGrandchild) MyGrandchildInInherited.CloneLazinator(includeChildrenMode, CloneBufferOptions.NoBuffer);
            }
        }
        
        /* Properties */
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
                    if (LazinatorObjectBytes.Length == 0)
                    {
                        _MyGrandchildInInherited = default(ExampleGrandchild);
                    }
                    else
                    {
                        LazinatorMemory childData = GetChildSlice(LazinatorMemoryStorage, _MyGrandchildInInherited_ByteIndex, _MyGrandchildInInherited_ByteLength, false, false, null);
                        
                        _MyGrandchildInInherited = DeserializationFactory.Instance.CreateBaseOrDerivedType(279, () => new ExampleGrandchild(), childData, this); 
                    }
                    _MyGrandchildInInherited_Accessed = true;
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
        
        public override IEnumerable<(string propertyName, ILazinator descendant)> EnumerateLazinatorDescendants(Func<ILazinator, bool> matchCriterion, bool stopExploringBelowMatch, Func<ILazinator, bool> exploreCriterion, bool exploreOnlyDeserializedChildren, bool enumerateNulls)
        {
            foreach (var inheritedYield in base.EnumerateLazinatorDescendants(matchCriterion, stopExploringBelowMatch, exploreCriterion, exploreOnlyDeserializedChildren, enumerateNulls))
            {
                yield return inheritedYield;
            }
            if (enumerateNulls && (!exploreOnlyDeserializedChildren || _MyGrandchildInInherited_Accessed) && (MyGrandchildInInherited == null))
            {
                yield return ("MyGrandchildInInherited", default);
            }
            else if ((!exploreOnlyDeserializedChildren && MyGrandchildInInherited != null) || (_MyGrandchildInInherited_Accessed && _MyGrandchildInInherited != null))
            {
                bool isMatch = matchCriterion == null || matchCriterion(MyGrandchildInInherited);
                bool shouldExplore = exploreCriterion == null || exploreCriterion(MyGrandchildInInherited);
                if (isMatch)
                {
                    yield return ("MyGrandchildInInherited", MyGrandchildInInherited);
                }
                if ((!stopExploringBelowMatch || !isMatch) && shouldExplore)
                {
                    foreach (var toYield in MyGrandchildInInherited.EnumerateLazinatorDescendants(matchCriterion, stopExploringBelowMatch, exploreCriterion, exploreOnlyDeserializedChildren, enumerateNulls))
                    {
                        yield return ("MyGrandchildInInherited" + "." + toYield.propertyName, toYield.descendant);
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
        
        public override ILazinator ForEachLazinator(Func<ILazinator, ILazinator> changeFunc, bool exploreOnlyDeserializedChildren)
        {
            base.ForEachLazinator(changeFunc, exploreOnlyDeserializedChildren);
            if ((!exploreOnlyDeserializedChildren && MyGrandchildInInherited != null) || (_MyGrandchildInInherited_Accessed && _MyGrandchildInInherited != null))
            {
                _MyGrandchildInInherited = (ExampleGrandchild) _MyGrandchildInInherited.ForEachLazinator(changeFunc, exploreOnlyDeserializedChildren);
            }
            return changeFunc(this);
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
        
        public override int LazinatorUniqueID => 214;
        
        protected override bool ContainsOpenGenericParameters => false;
        public override LazinatorGenericIDType LazinatorGenericID
        {
            get => default;
            set { }
        }
        
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
                    if (_MyExampleGrandchild_Accessed && _MyExampleGrandchild != null)
                    {
                        _MyExampleGrandchild.UpdateStoredBuffer(ref writer, startPosition + _MyExampleGrandchild_ByteIndex + sizeof(int), _MyExampleGrandchild_ByteLength - sizeof(int), IncludeChildrenMode.IncludeAllChildren, true);
                    }
                    if (_MyWrapperContainer_Accessed && _MyWrapperContainer != null)
                    {
                        _MyWrapperContainer.UpdateStoredBuffer(ref writer, startPosition + _MyWrapperContainer_ByteIndex + sizeof(int), _MyWrapperContainer_ByteLength - sizeof(int), IncludeChildrenMode.IncludeAllChildren, true);
                    }
                    if (_MyGrandchildInInherited_Accessed && _MyGrandchildInInherited != null)
                    {
                        _MyGrandchildInInherited.UpdateStoredBuffer(ref writer, startPosition + _MyGrandchildInInherited_ByteIndex + sizeof(int), _MyGrandchildInInherited_ByteLength - sizeof(int), IncludeChildrenMode.IncludeAllChildren, true);
                    }
                }
                
            }
            else
            {
                throw new LazinatorSerializationException("Cannot update stored buffer when serializing only some children.");
            }
            
            var newBuffer = writer.Slice(startPosition, length);
            LazinatorMemoryStorage = ReplaceBuffer(LazinatorMemoryStorage, newBuffer, LazinatorParents, startPosition == 0, IsStruct);
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
                if (includeChildrenMode != IncludeChildrenMode.IncludeAllChildren || includeChildrenMode != OriginalIncludeChildrenMode && !_MyGrandchildInInherited_Accessed)
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
