//30c0cae5-ee16-c100-c98c-b4fe66e0fb27
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by the Lazinator tool, version 0.1.0.329
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace LazinatorTests.Examples.Abstract
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
    public partial class Concrete5 : Abstract4, ILazinator
    {
        /* Property definitions */
        
        protected int _IntList5_ByteIndex;
        protected override int _IntList4_ByteLength => _IntList5_ByteIndex - _IntList4_ByteIndex;
        private int _Concrete5_EndByteIndex;
        protected virtual int _IntList5_ByteLength => _Concrete5_EndByteIndex - _IntList5_ByteIndex;
        
        
        protected string _String4;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public override string String4
        {
            get
            {
                return _String4;
            }
            set
            {
                IsDirty = true;
                _String4 = value;
            }
        }
        
        protected string _String5;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public string String5
        {
            get
            {
                return _String5;
            }
            set
            {
                IsDirty = true;
                _String5 = value;
            }
        }
        
        protected List<int> _IntList4;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public override List<int> IntList4
        {
            get
            {
                if (!_IntList4_Accessed)
                {
                    if (LazinatorObjectBytes.Length == 0)
                    {
                        _IntList4 = default(List<int>);
                    }
                    else
                    {
                        LazinatorMemory childData = GetChildSlice(LazinatorMemoryStorage, _IntList4_ByteIndex, _IntList4_ByteLength, false, false, null);
                        _IntList4 = ConvertFromBytes_List_Gint_g(childData);
                    }
                    _IntList4_Accessed = true;
                }
                IsDirty = true; 
                return _IntList4;
            }
            set
            {
                IsDirty = true;
                DescendantIsDirty = true;
                _IntList4 = value;
                _IntList4_Accessed = true;
            }
        }
        
        protected List<int> _IntList5;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public List<int> IntList5
        {
            get
            {
                if (!_IntList5_Accessed)
                {
                    if (LazinatorObjectBytes.Length == 0)
                    {
                        _IntList5 = default(List<int>);
                    }
                    else
                    {
                        LazinatorMemory childData = GetChildSlice(LazinatorMemoryStorage, _IntList5_ByteIndex, _IntList5_ByteLength, false, false, null);
                        _IntList5 = ConvertFromBytes_List_Gint_g(childData);
                    }
                    _IntList5_Accessed = true;
                }
                IsDirty = true; 
                return _IntList5;
            }
            set
            {
                IsDirty = true;
                DescendantIsDirty = true;
                _IntList5 = value;
                _IntList5_Accessed = true;
            }
        }
        protected bool _IntList5_Accessed;
        /* Clone overrides */
        
        public Concrete5() : base()
        {
        }
        
        public override ILazinator CloneLazinator(IncludeChildrenMode includeChildrenMode = IncludeChildrenMode.IncludeAllChildren, CloneBufferOptions cloneBufferOptions = CloneBufferOptions.IndependentBuffers)
        {
            var clone = new Concrete5()
            {
                OriginalIncludeChildrenMode = includeChildrenMode
            };
            clone = CompleteClone(this, clone, includeChildrenMode, cloneBufferOptions);
            return clone;
        }
        
        public override ILazinator AssignCloneProperties(ILazinator clone, IncludeChildrenMode includeChildrenMode)
        {
            base.AssignCloneProperties(clone, includeChildrenMode);
            Concrete5 typedClone = (Concrete5) clone;
            typedClone.String4 = String4;
            typedClone.String5 = String5;
            typedClone.IntList4 = CloneOrChange_List_Gint_g(IntList4, l => l?.CloneLazinator(includeChildrenMode, CloneBufferOptions.NoBuffer), false);
            typedClone.IntList5 = CloneOrChange_List_Gint_g(IntList5, l => l?.CloneLazinator(includeChildrenMode, CloneBufferOptions.NoBuffer), false);
            
            return typedClone;
        }
        
        /* Properties */
        
        public override IEnumerable<(string propertyName, ILazinator descendant)> EnumerateLazinatorDescendants(Func<ILazinator, bool> matchCriterion, bool stopExploringBelowMatch, Func<ILazinator, bool> exploreCriterion, bool exploreOnlyDeserializedChildren, bool enumerateNulls)
        {
            foreach (var inheritedYield in base.EnumerateLazinatorDescendants(matchCriterion, stopExploringBelowMatch, exploreCriterion, exploreOnlyDeserializedChildren, enumerateNulls))
            {
                yield return inheritedYield;
            }
            yield break;
        }
        
        
        public override IEnumerable<(string propertyName, object descendant)> EnumerateNonLazinatorProperties()
        {
            foreach (var inheritedYield in base.EnumerateNonLazinatorProperties())
            {
                yield return inheritedYield;
            }
            yield return ("String4", (object)String4);
            yield return ("String5", (object)String5);
            yield return ("IntList4", (object)IntList4);
            yield return ("IntList5", (object)IntList5);
            yield break;
        }
        
        public override ILazinator ForEachLazinator(Func<ILazinator, ILazinator> changeFunc, bool exploreOnlyDeserializedChildren)
        {
            base.ForEachLazinator(changeFunc, exploreOnlyDeserializedChildren);
            if ((!exploreOnlyDeserializedChildren && IntList4 != null) || (_IntList4_Accessed && _IntList4 != null))
            {
                _IntList4 = (List<int>) CloneOrChange_List_Gint_g(_IntList4, l => l?.ForEachLazinator(changeFunc, exploreOnlyDeserializedChildren), true);
            }
            if ((!exploreOnlyDeserializedChildren && IntList5 != null) || (_IntList5_Accessed && _IntList5 != null))
            {
                _IntList5 = (List<int>) CloneOrChange_List_Gint_g(_IntList5, l => l?.ForEachLazinator(changeFunc, exploreOnlyDeserializedChildren), true);
            }
            return changeFunc(this);
        }
        
        public override void FreeInMemoryObjects()
        {
            base.FreeInMemoryObjects();
            _IntList4 = default;
            _IntList5 = default;
            _IntList4_Accessed = _IntList5_Accessed = false;
            IsDirty = false;
            DescendantIsDirty = false;
            HasChanged = false;
            DescendantHasChanged = false;
        }
        
        /* Conversion */
        
        public override int LazinatorUniqueID => 239;
        
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
            _String4 = span.ToString_VarIntLengthUtf8(ref bytesSoFar);
            _String5 = span.ToString_VarIntLengthUtf8(ref bytesSoFar);
            _IntList4_ByteIndex = bytesSoFar;
            bytesSoFar = span.ToInt32(ref bytesSoFar) + bytesSoFar;
            _IntList5_ByteIndex = bytesSoFar;
            bytesSoFar = span.ToInt32(ref bytesSoFar) + bytesSoFar;
            _Concrete5_EndByteIndex = bytesSoFar;
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
                    if (_Example2_Accessed && _Example2 != null)
                    {
                        _Example2.UpdateStoredBuffer(ref writer, startPosition + _Example2_ByteIndex + sizeof(int), _Example2_ByteLength - sizeof(int), IncludeChildrenMode.IncludeAllChildren, true);
                    }
                    if (_Example3_Accessed && _Example3 != null)
                    {
                        _Example3.UpdateStoredBuffer(ref writer, startPosition + _Example3_ByteIndex + sizeof(int), _Example3_ByteLength - sizeof(int), IncludeChildrenMode.IncludeAllChildren, true);
                    }
                    if (_IntList4_Accessed && _IntList4 != null)
                    {
                        _IntList4 = (List<int>) CloneOrChange_List_Gint_g(_IntList4, l => l.RemoveBufferInHierarchy(), true);
                    }
                    if (_IntList5_Accessed && _IntList5 != null)
                    {
                        _IntList5 = (List<int>) CloneOrChange_List_Gint_g(_IntList5, l => l.RemoveBufferInHierarchy(), true);
                    }
                }
                
            }
            else
            {
                throw new LazinatorSerializationException("Cannot update stored buffer when serializing only some children.");
            }
            
            var newBuffer = writer.Slice(startPosition, length);
            LazinatorMemoryStorage = newBuffer;
        }
        
        protected override void WritePropertiesIntoBuffer(ref BinaryBufferWriter writer, IncludeChildrenMode includeChildrenMode, bool verifyCleanness, bool updateStoredBuffer, bool includeUniqueID)
        {
            int startPosition = writer.Position;
            int startOfObjectPosition = 0;
            base.WritePropertiesIntoBuffer(ref writer, includeChildrenMode, verifyCleanness, updateStoredBuffer, includeUniqueID);
            // write properties
            EncodeCharAndString.WriteStringUtf8WithVarIntPrefix(ref writer, _String4);
            EncodeCharAndString.WriteStringUtf8WithVarIntPrefix(ref writer, _String5);
            startOfObjectPosition = writer.Position;
            if (includeChildrenMode != IncludeChildrenMode.IncludeAllChildren || includeChildrenMode != OriginalIncludeChildrenMode && !_IntList4_Accessed)
            {
                var deserialized = IntList4;
            }
            WriteNonLazinatorObject(
            nonLazinatorObject: _IntList4, isBelievedDirty: _IntList4_Accessed || (includeChildrenMode != OriginalIncludeChildrenMode),
            isAccessed: _IntList4_Accessed, writer: ref writer,
            getChildSliceForFieldFn: () => GetChildSlice(LazinatorMemoryStorage, _IntList4_ByteIndex, _IntList4_ByteLength, false, false, null),
            verifyCleanness: false,
            binaryWriterAction: (ref BinaryBufferWriter w, bool v) =>
            ConvertToBytes_List_Gint_g(ref w, _IntList4,
            includeChildrenMode, v, updateStoredBuffer));
            if (updateStoredBuffer)
            {
                _IntList4_ByteIndex = startOfObjectPosition - startPosition;
            }
            startOfObjectPosition = writer.Position;
            if (includeChildrenMode != IncludeChildrenMode.IncludeAllChildren || includeChildrenMode != OriginalIncludeChildrenMode && !_IntList5_Accessed)
            {
                var deserialized = IntList5;
            }
            WriteNonLazinatorObject(
            nonLazinatorObject: _IntList5, isBelievedDirty: _IntList5_Accessed || (includeChildrenMode != OriginalIncludeChildrenMode),
            isAccessed: _IntList5_Accessed, writer: ref writer,
            getChildSliceForFieldFn: () => GetChildSlice(LazinatorMemoryStorage, _IntList5_ByteIndex, _IntList5_ByteLength, false, false, null),
            verifyCleanness: false,
            binaryWriterAction: (ref BinaryBufferWriter w, bool v) =>
            ConvertToBytes_List_Gint_g(ref w, _IntList5,
            includeChildrenMode, v, updateStoredBuffer));
            if (updateStoredBuffer)
            {
                _IntList5_ByteIndex = startOfObjectPosition - startPosition;
            }
            if (updateStoredBuffer)
            {
                _Concrete5_EndByteIndex = writer.Position - startPosition;
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
