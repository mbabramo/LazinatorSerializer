//8696a7f8-021b-18d6-c6e4-7ab3706cf1e1
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by the Lazinator tool, version 0.1.0.223
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
        /* Clone overrides */
        
        public DerivedLazinatorList() : base()
        {
        }
        
        public override ILazinator CloneLazinator(IncludeChildrenMode includeChildrenMode = IncludeChildrenMode.IncludeAllChildren, CloneBufferOptions cloneBufferOptions = CloneBufferOptions.LinkedBuffer)
        {
            var clone = new DerivedLazinatorList<T>()
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
            DerivedLazinatorList<T> typedClone = (DerivedLazinatorList<T>) clone;
            typedClone.MyListName = MyListName;
        }
        
        /* Properties */
        
        protected string _MyListName;
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
        
        
        public override IEnumerable<(string propertyName, object descendant)> EnumerateNonLazinatorProperties()
        {
            foreach (var inheritedYield in base.EnumerateNonLazinatorProperties())
            {
                yield return inheritedYield;
            }
            yield return ("MyListName", (object)MyListName);
            yield break;
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
                
                _IsDirty = false;
                if (includeChildrenMode == IncludeChildrenMode.IncludeAllChildren)
                {
                    _DescendantIsDirty = false;
                }
                else
                {
                    throw new Exception("Cannot update stored buffer when serializing only some children.");
                }
                
                var newBuffer = writer.Slice(startPosition);
                if (_LazinatorMemoryStorage != null)
                {
                    _LazinatorMemoryStorage.DisposeWithThis(newBuffer);
                }
                _LazinatorMemoryStorage = newBuffer;
            }
        }
        
        protected override void WritePropertiesIntoBuffer(ref BinaryBufferWriter writer, IncludeChildrenMode includeChildrenMode, bool verifyCleanness, bool updateStoredBuffer, bool includeUniqueID)
        {
            base.WritePropertiesIntoBuffer(ref writer, includeChildrenMode, verifyCleanness, updateStoredBuffer, includeUniqueID);
            // write properties
            EncodeCharAndString.WriteStringUtf8WithVarIntPrefix(ref writer, _MyListName);
        }
        
    }
}
