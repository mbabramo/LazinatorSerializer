/*Location5053*//*Location5038*///fc192821-5f69-d84c-fa81-b2a6e39c12c9
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by the Lazinator tool, version 0.1.0.380
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

#nullable disable
namespace LazinatorTests.Examples.NonAbstractGenerics
{/*Location5039*/
    using Lazinator.Attributes;/*Location5040*/
    using Lazinator.Buffers;/*Location5041*/
    using Lazinator.Core;/*Location5042*/
    using Lazinator.Exceptions;/*Location5043*/
    using Lazinator.Support;/*Location5044*/
    using LazinatorTests.Examples;/*Location5045*/
    using System;/*Location5046*/
    using System.Buffers;/*Location5047*/
    using System.Collections.Generic;/*Location5048*/
    using System.Diagnostics;/*Location5049*/
    using System.IO;/*Location5050*/
    using System.Linq;/*Location5051*/
    using System.Runtime.InteropServices;/*Location5052*/
    using static Lazinator.Core.LazinatorUtilities;
    
    [Autogenerated]
    public partial class InheritingClosedGeneric : ClosedGeneric, ILazinator
    {
        /*Location5054*//* Property definitions */
        
        
        /*Location5055*/
        protected int _YetAnotherInt;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public int YetAnotherInt
        {
            get
            {
                return _YetAnotherInt;
            }
            set
            {
                IsDirty = true;
                _YetAnotherInt = value;
            }
        }
        /*Location5057*/        /* Clone overrides */
        
        public InheritingClosedGeneric(IncludeChildrenMode originalIncludeChildrenMode) : base(originalIncludeChildrenMode)
        {
        }
        
        public InheritingClosedGeneric(LazinatorMemory serializedBytes, ILazinator parent = null) : base(serializedBytes, parent)
        {
        }
        
        public override ILazinator CloneLazinator(IncludeChildrenMode includeChildrenMode = IncludeChildrenMode.IncludeAllChildren, CloneBufferOptions cloneBufferOptions = CloneBufferOptions.IndependentBuffers)
        {
            InheritingClosedGeneric clone;
            if (cloneBufferOptions == CloneBufferOptions.NoBuffer)
            {
                clone = new InheritingClosedGeneric(includeChildrenMode);
                clone.LazinatorObjectVersion = LazinatorObjectVersion;
                clone = (InheritingClosedGeneric)AssignCloneProperties(clone, includeChildrenMode);
            }
            else
            {
                LazinatorMemory bytes = EncodeOrRecycleToNewBuffer(includeChildrenMode, OriginalIncludeChildrenMode, false, IsDirty, DescendantIsDirty, false, LazinatorMemoryStorage, false, this);
                clone = new InheritingClosedGeneric(bytes);
            }
            return clone;
        }
        
        public override ILazinator AssignCloneProperties(ILazinator clone, IncludeChildrenMode includeChildrenMode)
        {
            base.AssignCloneProperties(clone, includeChildrenMode);
            InheritingClosedGeneric typedClone = (InheritingClosedGeneric) clone;
            /*Location5056*/typedClone.YetAnotherInt = YetAnotherInt;
            
            return typedClone;
        }
        
        /* Properties */
        /*Location5058*/
        public override IEnumerable<(string propertyName, ILazinator descendant)> EnumerateLazinatorDescendants(Func<ILazinator, bool> matchCriterion, bool stopExploringBelowMatch, Func<ILazinator, bool> exploreCriterion, bool exploreOnlyDeserializedChildren, bool enumerateNulls)
        {
            foreach (var inheritedYield in base.EnumerateLazinatorDescendants(matchCriterion, stopExploringBelowMatch, exploreCriterion, exploreOnlyDeserializedChildren, enumerateNulls))
            {
                yield return inheritedYield;
            }
            /*Location5059*/yield break;
        }
        /*Location5060*/
        
        public override IEnumerable<(string propertyName, object descendant)> EnumerateNonLazinatorProperties()
        {
            foreach (var inheritedYield in base.EnumerateNonLazinatorProperties())
            {
                yield return inheritedYield;
            }
            /*Location5061*/yield return ("YetAnotherInt", (object)YetAnotherInt);
            /*Location5062*/yield break;
        }
        /*Location5063*/
        public override ILazinator ForEachLazinator(Func<ILazinator, ILazinator> changeFunc, bool exploreOnlyDeserializedChildren, bool changeThisLevel)
        {
            base.ForEachLazinator(changeFunc, exploreOnlyDeserializedChildren, false);
            /*Location5064*/if (changeThisLevel && changeFunc != null)
            {
                return changeFunc(this);
            }
            return this;
        }
        /*Location5065*/
        public override void FreeInMemoryObjects()
        {
            base.FreeInMemoryObjects();
            
            IsDirty = false;
            DescendantIsDirty = false;
            HasChanged = false;
            DescendantHasChanged = false;
        }
        /*Location5066*/
        /* Conversion */
        
        public override int LazinatorUniqueID => 1051;
        
        protected override bool ContainsOpenGenericParameters => false;
        public override LazinatorGenericIDType LazinatorGenericID => default;
        
        
        public override int LazinatorObjectVersion { get; set; } = 0;
        
        
        /*Location5067*/public override void ConvertFromBytesAfterHeader(IncludeChildrenMode includeChildrenMode, int serializedVersionNumber, ref int bytesSoFar)
        {
            base.ConvertFromBytesAfterHeader(OriginalIncludeChildrenMode, serializedVersionNumber, ref bytesSoFar);
            ReadOnlySpan<byte> span = LazinatorObjectBytes.Span;
            /*Location5068*/_YetAnotherInt = span.ToDecompressedInt(ref bytesSoFar);
            /*Location5069*/        }
            
            /*Location5070*/public override void SerializeExistingBuffer(ref BinaryBufferWriter writer, IncludeChildrenMode includeChildrenMode, bool verifyCleanness, bool updateStoredBuffer)
            {
                /*Location5071*/if (includeChildrenMode != IncludeChildrenMode.IncludeAllChildren)
                {
                    updateStoredBuffer = false;
                }
                /*Location5072*/int startPosition = writer.Position;
                WritePropertiesIntoBuffer(ref writer, includeChildrenMode, verifyCleanness, updateStoredBuffer, true);
                /*Location5073*/if (updateStoredBuffer)
                {
                    UpdateStoredBuffer(ref writer, startPosition, writer.Position - startPosition, includeChildrenMode, false);
                    /*Location5074*/}
                    /*Location5075*/}
                    /*Location5076*/
                    public override void UpdateStoredBuffer(ref BinaryBufferWriter writer, int startPosition, int length, IncludeChildrenMode includeChildrenMode, bool updateDeserializedChildren)
                    {
                        /*Location5077*/_IsDirty = false;
                        if (includeChildrenMode == IncludeChildrenMode.IncludeAllChildren)
                        {
                            _DescendantIsDirty = false;/*Location5078*/
                            if (updateDeserializedChildren)
                            {
                                UpdateDeserializedChildren(ref writer, startPosition);
                                /*Location5079*/}
                                /*Location5080*//*Location5081*/
                            }
                            else
                            {
                                ThrowHelper.ThrowCannotUpdateStoredBuffer();
                            }
                            /*Location5082*/
                            var newBuffer = writer.Slice(startPosition, length);
                            LazinatorMemoryStorage = newBuffer;
                            /*Location5083*/}
                            /*Location5084*/
                            protected override void UpdateDeserializedChildren(ref BinaryBufferWriter writer, int startPosition)
                            {
                                base.UpdateDeserializedChildren(ref writer, startPosition);
                                /*Location5085*/}
                                
                                /*Location5086*/
                                protected override void WritePropertiesIntoBuffer(ref BinaryBufferWriter writer, IncludeChildrenMode includeChildrenMode, bool verifyCleanness, bool updateStoredBuffer, bool includeUniqueID)
                                {
                                    base.WritePropertiesIntoBuffer(ref writer, includeChildrenMode, verifyCleanness, updateStoredBuffer, includeUniqueID);
                                    /*Location5087*/// write properties
                                    /*Location5088*/CompressedIntegralTypes.WriteCompressedInt(ref writer, _YetAnotherInt);
                                    /*Location5089*/}
                                    /*Location5090*/
                                }
                            }
