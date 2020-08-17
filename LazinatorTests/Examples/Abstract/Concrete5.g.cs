/*Location3825*//*Location3811*///092d2e87-9e02-743c-ceee-b679d1a98dc7
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
{/*Location3812*/
    using Lazinator.Attributes;/*Location3813*/
    using Lazinator.Buffers;/*Location3814*/
    using Lazinator.Core;/*Location3815*/
    using Lazinator.Exceptions;/*Location3816*/
    using Lazinator.Support;/*Location3817*/
    using System;/*Location3818*/
    using System.Buffers;/*Location3819*/
    using System.Collections.Generic;/*Location3820*/
    using System.Diagnostics;/*Location3821*/
    using System.IO;/*Location3822*/
    using System.Linq;/*Location3823*/
    using System.Runtime.InteropServices;/*Location3824*/
    using static Lazinator.Core.LazinatorUtilities;
    
    [Autogenerated]
    public partial class Concrete5 : Abstract4, ILazinator
    {
        /*Location3826*//* Property definitions */
        
        /*Location3827*/        protected int _IntList5_ByteIndex;
        /*Location3828*/protected override int _IntList4_ByteLength => _IntList5_ByteIndex - _IntList4_ByteIndex;
        /*Location3829*/private int _Concrete5_EndByteIndex;
        /*Location3830*/protected virtual int _IntList5_ByteLength => _Concrete5_EndByteIndex - _IntList5_ByteIndex;
        
        /*Location3831*/
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
        /*Location3832*/
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
        /*Location3833*/
        protected List<int> _IntList4;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public override List<int> IntList4
        {
            get
            {
                if (!_IntList4_Accessed)
                {
                    Lazinate_IntList4();
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
        private void Lazinate_IntList4()
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
        
        /*Location3834*/
        protected List<int> _IntList5;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public List<int> IntList5
        {
            get
            {
                if (!_IntList5_Accessed)
                {
                    Lazinate_IntList5();
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
        private void Lazinate_IntList5()
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
        
        /*Location3839*/        /* Clone overrides */
        
        public Concrete5(IncludeChildrenMode originalIncludeChildrenMode)
        {
            OriginalIncludeChildrenMode = originalIncludeChildrenMode;
        }
        
        public Concrete5(LazinatorMemory serializedBytes, ILazinator parent = null)
        {
            LazinatorParents = new LazinatorParentsCollection(parent);
            DeserializeLazinator(serializedBytes);
            HasChanged = false;
            DescendantHasChanged = false;
        }
        
        public override ILazinator CloneLazinator(IncludeChildrenMode includeChildrenMode = IncludeChildrenMode.IncludeAllChildren, CloneBufferOptions cloneBufferOptions = CloneBufferOptions.IndependentBuffers)
        {
            Concrete5 clone;
            if (cloneBufferOptions == CloneBufferOptions.NoBuffer)
            {
                clone = new Concrete5(includeChildrenMode);
                clone.LazinatorObjectVersion = LazinatorObjectVersion;
                clone = (Concrete5)AssignCloneProperties(clone, includeChildrenMode);
            }
            else
            {
                LazinatorMemory bytes = EncodeOrRecycleToNewBuffer(includeChildrenMode, OriginalIncludeChildrenMode, false, IsDirty, DescendantIsDirty, false, LazinatorMemoryStorage, false, this);
                clone = new Concrete5(bytes);
            }
            return clone;
        }
        
        public override ILazinator AssignCloneProperties(ILazinator clone, IncludeChildrenMode includeChildrenMode)
        {
            base.AssignCloneProperties(clone, includeChildrenMode);
            Concrete5 typedClone = (Concrete5) clone;
            /*Location3835*/typedClone.String4 = String4;
            /*Location3836*/typedClone.String5 = String5;
            /*Location3837*/typedClone.IntList4 = CloneOrChange_List_Gint_g(IntList4, l => l?.CloneLazinator(includeChildrenMode, CloneBufferOptions.NoBuffer), false);
            /*Location3838*/typedClone.IntList5 = CloneOrChange_List_Gint_g(IntList5, l => l?.CloneLazinator(includeChildrenMode, CloneBufferOptions.NoBuffer), false);
            
            return typedClone;
        }
        
        /* Properties */
        /*Location3840*/
        public override IEnumerable<(string propertyName, ILazinator descendant)> EnumerateLazinatorDescendants(Func<ILazinator, bool> matchCriterion, bool stopExploringBelowMatch, Func<ILazinator, bool> exploreCriterion, bool exploreOnlyDeserializedChildren, bool enumerateNulls)
        {
            foreach (var inheritedYield in base.EnumerateLazinatorDescendants(matchCriterion, stopExploringBelowMatch, exploreCriterion, exploreOnlyDeserializedChildren, enumerateNulls))
            {
                yield return inheritedYield;
            }
            /*Location3841*/yield break;
        }
        /*Location3842*/
        
        public override IEnumerable<(string propertyName, object descendant)> EnumerateNonLazinatorProperties()
        {
            foreach (var inheritedYield in base.EnumerateNonLazinatorProperties())
            {
                yield return inheritedYield;
            }
            /*Location3843*/yield return ("String4", (object)String4);
            /*Location3844*/yield return ("String5", (object)String5);
            /*Location3845*/yield return ("IntList4", (object)IntList4);
            /*Location3846*/yield return ("IntList5", (object)IntList5);
            /*Location3847*/yield break;
        }
        /*Location3848*/
        public override ILazinator ForEachLazinator(Func<ILazinator, ILazinator> changeFunc, bool exploreOnlyDeserializedChildren, bool changeThisLevel)
        {
            base.ForEachLazinator(changeFunc, exploreOnlyDeserializedChildren, false);
            /*Location3849*/if ((!exploreOnlyDeserializedChildren && IntList4 != null) || (_IntList4_Accessed && _IntList4 != null))
            {
                _IntList4 = (List<int>) CloneOrChange_List_Gint_g(_IntList4, l => l?.ForEachLazinator(changeFunc, exploreOnlyDeserializedChildren, true), true);
            }
            /*Location3850*/if ((!exploreOnlyDeserializedChildren && IntList5 != null) || (_IntList5_Accessed && _IntList5 != null))
            {
                _IntList5 = (List<int>) CloneOrChange_List_Gint_g(_IntList5, l => l?.ForEachLazinator(changeFunc, exploreOnlyDeserializedChildren, true), true);
            }
            /*Location3851*/if (changeThisLevel && changeFunc != null)
            {
                return changeFunc(this);
            }
            return this;
        }
        /*Location3852*/
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
        /*Location3853*/
        /* Conversion */
        
        public override int LazinatorUniqueID => 1039;
        
        protected override bool ContainsOpenGenericParameters => false;
        public override LazinatorGenericIDType LazinatorGenericID => default;
        
        
        public override int LazinatorObjectVersion { get; set; } = 0;
        
        
        /*Location3854*/public override void ConvertFromBytesAfterHeader(IncludeChildrenMode includeChildrenMode, int serializedVersionNumber, ref int bytesSoFar)
        {
            base.ConvertFromBytesAfterHeader(OriginalIncludeChildrenMode, serializedVersionNumber, ref bytesSoFar);
            ReadOnlySpan<byte> span = LazinatorObjectBytes.Span;
            /*Location3855*/_String4 = span.ToString_VarIntLengthUtf8(ref bytesSoFar);
            /*Location3856*/_String5 = span.ToString_VarIntLengthUtf8(ref bytesSoFar);
            /*Location3857*/_IntList4_ByteIndex = bytesSoFar;
            bytesSoFar = span.ToInt32(ref bytesSoFar) + bytesSoFar;
            /*Location3858*/_IntList5_ByteIndex = bytesSoFar;
            bytesSoFar = span.ToInt32(ref bytesSoFar) + bytesSoFar;
            /*Location3859*/_Concrete5_EndByteIndex = bytesSoFar;
            /*Location3860*/        }
            
            /*Location3861*/public override void SerializeExistingBuffer(ref BinaryBufferWriter writer, IncludeChildrenMode includeChildrenMode, bool verifyCleanness, bool updateStoredBuffer)
            {
                /*Location3862*/if (includeChildrenMode != IncludeChildrenMode.IncludeAllChildren)
                {
                    updateStoredBuffer = false;
                }
                /*Location3863*/int startPosition = writer.Position;
                WritePropertiesIntoBuffer(ref writer, includeChildrenMode, verifyCleanness, updateStoredBuffer, true);
                /*Location3864*/if (updateStoredBuffer)
                {
                    UpdateStoredBuffer(ref writer, startPosition, writer.Position - startPosition, includeChildrenMode, false);
                    /*Location3865*/}
                    /*Location3866*/}
                    /*Location3867*/
                    public override void UpdateStoredBuffer(ref BinaryBufferWriter writer, int startPosition, int length, IncludeChildrenMode includeChildrenMode, bool updateDeserializedChildren)
                    {
                        /*Location3868*/_IsDirty = false;
                        if (includeChildrenMode == IncludeChildrenMode.IncludeAllChildren)
                        {
                            _DescendantIsDirty = false;/*Location3869*/
                            if (updateDeserializedChildren)
                            {
                                UpdateDeserializedChildren(ref writer, startPosition);
                                /*Location3870*/}
                                /*Location3871*//*Location3872*/
                            }
                            else
                            {
                                ThrowHelper.ThrowCannotUpdateStoredBuffer();
                            }
                            /*Location3873*/
                            var newBuffer = writer.Slice(startPosition, length);
                            LazinatorMemoryStorage = newBuffer;
                            /*Location3874*/}
                            /*Location3875*/
                            protected override void UpdateDeserializedChildren(ref BinaryBufferWriter writer, int startPosition)
                            {
                                base.UpdateDeserializedChildren(ref writer, startPosition);
                                /*Location3876*/if (_IntList4_Accessed && _IntList4 != null)
                                {
                                    _IntList4 = (List<int>) CloneOrChange_List_Gint_g(_IntList4, l => l.RemoveBufferInHierarchy(), true);
                                }
                                /*Location3877*/if (_IntList5_Accessed && _IntList5 != null)
                                {
                                    _IntList5 = (List<int>) CloneOrChange_List_Gint_g(_IntList5, l => l.RemoveBufferInHierarchy(), true);
                                }
                                /*Location3878*/}
                                
                                /*Location3879*/
                                protected override void WritePropertiesIntoBuffer(ref BinaryBufferWriter writer, IncludeChildrenMode includeChildrenMode, bool verifyCleanness, bool updateStoredBuffer, bool includeUniqueID)
                                {
                                    int startPosition = writer.Position;
                                    int startOfObjectPosition = 0;
                                    base.WritePropertiesIntoBuffer(ref writer, includeChildrenMode, verifyCleanness, updateStoredBuffer, includeUniqueID);
                                    /*Location3880*/// write properties
                                    /*Location3881*/EncodeCharAndString.WriteStringUtf8WithVarIntPrefix(ref writer, _String4);
                                    /*Location3882*/EncodeCharAndString.WriteStringUtf8WithVarIntPrefix(ref writer, _String5);
                                    /*Location3883*/startOfObjectPosition = writer.Position;
                                    /*Location3884*/if ((includeChildrenMode != IncludeChildrenMode.IncludeAllChildren || includeChildrenMode != OriginalIncludeChildrenMode) && !_IntList4_Accessed)
                                    {
                                        var deserialized = IntList4;
                                    }
                                    /*Location3885*/WriteNonLazinatorObject(
                                    nonLazinatorObject: _IntList4, isBelievedDirty: _IntList4_Accessed || (includeChildrenMode != OriginalIncludeChildrenMode),
                                    isAccessed: _IntList4_Accessed, writer: ref writer,
                                    getChildSliceForFieldFn: () => GetChildSlice(LazinatorMemoryStorage, _IntList4_ByteIndex, _IntList4_ByteLength, false, false, null),
                                    verifyCleanness: false,
                                    binaryWriterAction: (ref BinaryBufferWriter w, bool v) =>
                                    ConvertToBytes_List_Gint_g(ref w, _IntList4,
                                    includeChildrenMode, v, updateStoredBuffer));
                                    /*Location3886*/if (updateStoredBuffer)
                                    {
                                        _IntList4_ByteIndex = startOfObjectPosition - startPosition;
                                    }
                                    /*Location3887*/startOfObjectPosition = writer.Position;
                                    /*Location3888*/if ((includeChildrenMode != IncludeChildrenMode.IncludeAllChildren || includeChildrenMode != OriginalIncludeChildrenMode) && !_IntList5_Accessed)
                                    {
                                        var deserialized = IntList5;
                                    }
                                    /*Location3889*/WriteNonLazinatorObject(
                                    nonLazinatorObject: _IntList5, isBelievedDirty: _IntList5_Accessed || (includeChildrenMode != OriginalIncludeChildrenMode),
                                    isAccessed: _IntList5_Accessed, writer: ref writer,
                                    getChildSliceForFieldFn: () => GetChildSlice(LazinatorMemoryStorage, _IntList5_ByteIndex, _IntList5_ByteLength, false, false, null),
                                    verifyCleanness: false,
                                    binaryWriterAction: (ref BinaryBufferWriter w, bool v) =>
                                    ConvertToBytes_List_Gint_g(ref w, _IntList5,
                                    includeChildrenMode, v, updateStoredBuffer));
                                    /*Location3890*/if (updateStoredBuffer)
                                    {
                                        _IntList5_ByteIndex = startOfObjectPosition - startPosition;
                                    }
                                    /*Location3891*/if (updateStoredBuffer)
                                    {
                                        /*Location3892*/_Concrete5_EndByteIndex = writer.Position - startPosition;
                                        /*Location3893*/}
                                        /*Location3894*/}
                                        /*Location3895*/
                                        /* Conversion of supported collections and tuples */
                                        /*Location3896*/
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
                                        }/*Location3897*/
                                        
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
                                        /*Location3898*/
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
                                        /*Location3899*/
                                    }
                                }
