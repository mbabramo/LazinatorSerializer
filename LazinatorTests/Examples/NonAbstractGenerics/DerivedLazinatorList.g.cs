//bbb28db6-77cf-6064-7eb5-55c069c80cb9
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by the Lazinator tool, version 0.1.0.326
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace LazinatorTests.Examples
{
    using Lazinator.Attributes;
    using Lazinator.Buffers;
    using Lazinator.Collections;
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
    public partial class DerivedLazinatorList<T> : Lazinator.Collections.LazinatorList<T>, ILazinator
    {
        /* Property definitions */
        
        
        
        protected string _MyListName;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public string MyListName
        {
            get
            {
                return _MyListName;
            }
            set
            {
                IsDirty = true;
                _MyListName = value;
            }
        }
        /* Clone overrides */
        
        public DerivedLazinatorList() : base()
        {
        }
        
        public override ILazinator CloneLazinator(IncludeChildrenMode includeChildrenMode = IncludeChildrenMode.IncludeAllChildren, CloneBufferOptions cloneBufferOptions = CloneBufferOptions.IndependentBuffers)
        {
            var clone = new DerivedLazinatorList<T>()
            {
                OriginalIncludeChildrenMode = includeChildrenMode
            };
            clone = CompleteClone(this, clone, includeChildrenMode, cloneBufferOptions);
            return clone;
        }
        
        /* Properties */
        
        
        public override IEnumerable<(string propertyName, object descendant)> EnumerateNonLazinatorProperties()
        {
            foreach (var inheritedYield in base.EnumerateNonLazinatorProperties())
            {
                yield return inheritedYield;
            }
            yield return ("MyListName", (object)MyListName);
            yield break;
        }
        
        public override ILazinator ForEachLazinator(Func<ILazinator, ILazinator> changeFunc, bool exploreOnlyDeserializedChildren)
        {
            base.ForEachLazinator(changeFunc, exploreOnlyDeserializedChildren);
            return changeFunc(this);
        }
        
        public override void FreeInMemoryObjects()
        {
            base.FreeInMemoryObjects();
            
            IsDirty = false;
            DescendantIsDirty = false;
            HasChanged = false;
            DescendantHasChanged = false;
            OnFreeInMemoryObjects();
        }
        
        /* Conversion */
        
        public override int LazinatorUniqueID => 203;
        
        protected override bool ContainsOpenGenericParameters => true;
        public override LazinatorGenericIDType LazinatorGenericID
        {
            get
            {
                if (_LazinatorGenericID.IsEmpty)
                {
                    _LazinatorGenericID = DeserializationFactory.Instance.GetUniqueIDListForGenericType(203, new Type[] { typeof(T) });
                }
                return _LazinatorGenericID;
            }
            set
            {
                _LazinatorGenericID = value;
            }
        }
        
        public override int LazinatorObjectVersion { get; set; } = 0;
        
        
        public override void ConvertFromBytesAfterHeader(IncludeChildrenMode includeChildrenMode, int serializedVersionNumber, ref int bytesSoFar)
        {
            base.ConvertFromBytesAfterHeader(OriginalIncludeChildrenMode, serializedVersionNumber, ref bytesSoFar);
            ReadOnlySpan<byte> span = LazinatorObjectBytes.Span;
            _MyListName = span.ToString_VarIntLengthUtf8(ref bytesSoFar);
        }
        
        public override void SerializeExistingBuffer(ref BinaryBufferWriter writer, IncludeChildrenMode includeChildrenMode, bool verifyCleanness, bool updateStoredBuffer)
        {
            if (includeChildrenMode != IncludeChildrenMode.IncludeAllChildren)
            {
                updateStoredBuffer = false;
            }
            PreSerialization(verifyCleanness, updateStoredBuffer);
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
                    if (_Offsets_Accessed && _Offsets != null)
                    {
                        _Offsets.UpdateStoredBuffer(ref writer, startPosition + _Offsets_ByteIndex + sizeof(int), _Offsets_ByteLength - sizeof(int), IncludeChildrenMode.IncludeAllChildren, true);
                    }
                    OnUpdateDeserializedChildren(ref writer, startPosition);
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
            base.WritePropertiesIntoBuffer(ref writer, includeChildrenMode, verifyCleanness, updateStoredBuffer, includeUniqueID);
            // write properties
            EncodeCharAndString.WriteStringUtf8WithVarIntPrefix(ref writer, _MyListName);
            OnPropertiesWritten(updateStoredBuffer);
        }
        
    }
}
