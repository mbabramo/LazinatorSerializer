/*Location4170*//*Location4155*///8f96b9a7-fb48-261e-fddf-455dbd843151
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
{/*Location4156*/
    using Lazinator.Attributes;/*Location4157*/
    using Lazinator.Buffers;/*Location4158*/
    using Lazinator.Core;/*Location4159*/
    using Lazinator.Exceptions;/*Location4160*/
    using Lazinator.Support;/*Location4161*/
    using Lazinator.Wrappers;/*Location4162*/
    using System;/*Location4163*/
    using System.Buffers;/*Location4164*/
    using System.Collections.Generic;/*Location4165*/
    using System.Diagnostics;/*Location4166*/
    using System.IO;/*Location4167*/
    using System.Linq;/*Location4168*/
    using System.Runtime.InteropServices;/*Location4169*/
    using static Lazinator.Core.LazinatorUtilities;
    
    [Autogenerated]
    public partial class ConcreteFromGenericFromBase : GenericFromBase<WNullableDecimal>, ILazinator
    {
        /*Location4171*//* Property definitions */
        
        
        /*Location4172*/        /* Clone overrides */
        
        public ConcreteFromGenericFromBase(IncludeChildrenMode originalIncludeChildrenMode) : base(originalIncludeChildrenMode)
        {
        }
        
        public ConcreteFromGenericFromBase(LazinatorMemory serializedBytes, ILazinator parent = null) : base(serializedBytes, parent)
        {
        }
        
        public override ILazinator CloneLazinator(IncludeChildrenMode includeChildrenMode = IncludeChildrenMode.IncludeAllChildren, CloneBufferOptions cloneBufferOptions = CloneBufferOptions.IndependentBuffers)
        {
            ConcreteFromGenericFromBase clone;
            if (cloneBufferOptions == CloneBufferOptions.NoBuffer)
            {
                clone = new ConcreteFromGenericFromBase(includeChildrenMode);
                clone.LazinatorObjectVersion = LazinatorObjectVersion;
                clone = (ConcreteFromGenericFromBase)AssignCloneProperties(clone, includeChildrenMode);
            }
            else
            {
                LazinatorMemory bytes = EncodeOrRecycleToNewBuffer(includeChildrenMode, OriginalIncludeChildrenMode, false, IsDirty, DescendantIsDirty, false, LazinatorMemoryStorage, false, this);
                clone = new ConcreteFromGenericFromBase(bytes);
            }
            return clone;
        }
        
        public override ILazinator AssignCloneProperties(ILazinator clone, IncludeChildrenMode includeChildrenMode)
        {
            base.AssignCloneProperties(clone, includeChildrenMode);
            ConcreteFromGenericFromBase typedClone = (ConcreteFromGenericFromBase) clone;
            
            return typedClone;
        }
        
        /* Properties */
        /*Location4173*/
        public override IEnumerable<(string propertyName, ILazinator descendant)> EnumerateLazinatorDescendants(Func<ILazinator, bool> matchCriterion, bool stopExploringBelowMatch, Func<ILazinator, bool> exploreCriterion, bool exploreOnlyDeserializedChildren, bool enumerateNulls)
        {
            foreach (var inheritedYield in base.EnumerateLazinatorDescendants(matchCriterion, stopExploringBelowMatch, exploreCriterion, exploreOnlyDeserializedChildren, enumerateNulls))
            {
                yield return inheritedYield;
            }
            /*Location4174*/yield break;
        }
        /*Location4175*/
        
        public override IEnumerable<(string propertyName, object descendant)> EnumerateNonLazinatorProperties()
        {
            foreach (var inheritedYield in base.EnumerateNonLazinatorProperties())
            {
                yield return inheritedYield;
            }
            /*Location4176*/yield break;
        }
        /*Location4177*/
        public override ILazinator ForEachLazinator(Func<ILazinator, ILazinator> changeFunc, bool exploreOnlyDeserializedChildren, bool changeThisLevel)
        {
            base.ForEachLazinator(changeFunc, exploreOnlyDeserializedChildren, false);
            /*Location4178*/if (changeThisLevel && changeFunc != null)
            {
                return changeFunc(this);
            }
            return this;
        }
        /*Location4179*/
        public override void FreeInMemoryObjects()
        {
            base.FreeInMemoryObjects();
            
            IsDirty = false;
            DescendantIsDirty = false;
            HasChanged = false;
            DescendantHasChanged = false;
        }
        /*Location4180*/
        /* Conversion */
        
        public override int LazinatorUniqueID => 1072;
        
        protected override bool ContainsOpenGenericParameters => false;
        public override LazinatorGenericIDType LazinatorGenericID => default;
        
        
        public override int LazinatorObjectVersion { get; set; } = 0;
        
        
        /*Location4181*/public override void ConvertFromBytesAfterHeader(IncludeChildrenMode includeChildrenMode, int serializedVersionNumber, ref int bytesSoFar)
        {
            base.ConvertFromBytesAfterHeader(OriginalIncludeChildrenMode, serializedVersionNumber, ref bytesSoFar);
            ReadOnlySpan<byte> span = LazinatorObjectBytes.Span;
            /*Location4182*/        }
            
            /*Location4183*/public override void SerializeExistingBuffer(ref BinaryBufferWriter writer, IncludeChildrenMode includeChildrenMode, bool verifyCleanness, bool updateStoredBuffer)
            {
                /*Location4184*/if (includeChildrenMode != IncludeChildrenMode.IncludeAllChildren)
                {
                    updateStoredBuffer = false;
                }
                /*Location4185*/int startPosition = writer.Position;
                WritePropertiesIntoBuffer(ref writer, includeChildrenMode, verifyCleanness, updateStoredBuffer, true);
                /*Location4186*/if (updateStoredBuffer)
                {
                    UpdateStoredBuffer(ref writer, startPosition, writer.Position - startPosition, includeChildrenMode, false);
                    /*Location4187*/}
                    /*Location4188*/}
                    /*Location4189*/
                    public override void UpdateStoredBuffer(ref BinaryBufferWriter writer, int startPosition, int length, IncludeChildrenMode includeChildrenMode, bool updateDeserializedChildren)
                    {
                        /*Location4190*/_IsDirty = false;
                        if (includeChildrenMode == IncludeChildrenMode.IncludeAllChildren)
                        {
                            _DescendantIsDirty = false;/*Location4191*/
                            if (updateDeserializedChildren)
                            {
                                UpdateDeserializedChildren(ref writer, startPosition);
                                /*Location4192*/}
                                /*Location4193*/
                                _MyT_Accessed = false;/*Location4194*/
                            }
                            else
                            {
                                ThrowHelper.ThrowCannotUpdateStoredBuffer();
                            }
                            /*Location4195*/
                            var newBuffer = writer.Slice(startPosition, length);
                            LazinatorMemoryStorage = newBuffer;
                            /*Location4196*/}
                            /*Location4197*/
                            protected override void UpdateDeserializedChildren(ref BinaryBufferWriter writer, int startPosition)
                            {
                                base.UpdateDeserializedChildren(ref writer, startPosition);
                                /*Location4198*/}
                                
                                /*Location4199*/
                                protected override void WritePropertiesIntoBuffer(ref BinaryBufferWriter writer, IncludeChildrenMode includeChildrenMode, bool verifyCleanness, bool updateStoredBuffer, bool includeUniqueID)
                                {
                                    base.WritePropertiesIntoBuffer(ref writer, includeChildrenMode, verifyCleanness, updateStoredBuffer, includeUniqueID);
                                    /*Location4200*/// write properties
                                    /*Location4201*/}
                                    /*Location4202*/
                                }
                            }
