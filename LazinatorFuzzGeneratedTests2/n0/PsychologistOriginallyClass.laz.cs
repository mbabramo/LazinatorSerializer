/*Location644*//*Location629*///3f3cc510-1c0f-2199-ddb6-46d7aeaa2c32
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by the Lazinator tool, version 0.1.0.432, on 2024/01/01 12:00:00.000 AM.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

#nullable enable
namespace FuzzTests.n0
{
    #pragma warning disable 8019//Location630
    using Lazinator.Attributes;/*Location631*/
    using Lazinator.Buffers;/*Location632*/
    using Lazinator.Core;/*Location633*/
    using Lazinator.Exceptions;/*Location634*/
    using Lazinator.Support;/*Location635*/
    using static Lazinator.Buffers.WriteUncompressedPrimitives;/*Location636*/
    using System;/*Location637*/
    using System.Buffers;/*Location638*/
    using System.Collections.Generic;/*Location639*/
    using System.Diagnostics;/*Location640*/
    using System.IO;/*Location641*/
    using System.Linq;/*Location642*/
    using System.Runtime.InteropServices;/*Location643*/
    using static Lazinator.Core.LazinatorUtilities;
    #pragma warning restore 8019
    
    [Autogenerated]
    public partial class PsychologistOriginallyClass : RefugeeSmartClass, ILazinator
    {
        /*Location645*//* Property definitions */
        
        /*Location646*/        protected int _AssociateDistinguish_ByteIndex;
        /*Location647*/        protected int _ConfirmOperation_ByteIndex;
        /*Location648*/        protected int _LiteraryWeek_ByteIndex;
        /*Location649*/        protected int _SkyPersonal_ByteIndex;
        /*Location650*/protected virtual int _AssociateDistinguish_ByteLength => _ConfirmOperation_ByteIndex - _AssociateDistinguish_ByteIndex;
        /*Location651*/protected virtual int _ConfirmOperation_ByteLength => _LiteraryWeek_ByteIndex - _ConfirmOperation_ByteIndex;
        /*Location652*/protected virtual int _LiteraryWeek_ByteLength => _SkyPersonal_ByteIndex - _LiteraryWeek_ByteIndex;
        /*Location653*/private int _PsychologistOriginallyClass_EndByteIndex;
        /*Location654*/protected virtual  int _SkyPersonal_ByteLength => _PsychologistOriginallyClass_EndByteIndex - _SkyPersonal_ByteIndex;
        /*Location655*/protected virtual int _OverallEndByteIndex => _PsychologistOriginallyClass_EndByteIndex;
        
        /*Location659*/
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        protected RemainingSubjectStruct _AssociateDistinguish;
        public RemainingSubjectStruct AssociateDistinguish
        {
            [DebuggerStepThrough]
            get
            {
                if (!_AssociateDistinguish_Accessed)
                {
                    LazinateAssociateDistinguish();
                } 
                return _AssociateDistinguish;
            }
            [DebuggerStepThrough]
            set
            {
                value.LazinatorParents = new LazinatorParentsCollection(this);/*Location658*/
                
                IsDirty = true;
                DescendantIsDirty = true;
                _AssociateDistinguish = value;
                _AssociateDistinguish_Accessed = true;
            }
        }
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        protected bool _AssociateDistinguish_Accessed;
        private void LazinateAssociateDistinguish()
        {
            if (LazinatorMemoryStorage.Length == 0)
            {
                _AssociateDistinguish = default(RemainingSubjectStruct);
                _AssociateDistinguish.LazinatorParents = new LazinatorParentsCollection(this, null);/*Location656*/
            }
            else
            {
                LazinatorMemory childData = GetChildSlice(LazinatorMemoryStorage, _AssociateDistinguish_ByteIndex, _AssociateDistinguish_ByteLength, null);_AssociateDistinguish = new RemainingSubjectStruct(childData)
                {
                    LazinatorParents = new LazinatorParentsCollection(this, null)/*Location657*/
                };
                
            }
            _AssociateDistinguish_Accessed = true;
        }
        
        /*Location660*/public RemainingSubjectStruct AssociateDistinguish_Copy
        {
            [DebuggerStepThrough]
            get
            {
                if (!_AssociateDistinguish_Accessed)
                {
                    if (LazinatorMemoryStorage.Length == 0)
                    {
                        return default(RemainingSubjectStruct);
                    }
                    else
                    {
                        LazinatorMemory childData = GetChildSlice(LazinatorMemoryStorage, _AssociateDistinguish_ByteIndex, _AssociateDistinguish_ByteLength, null);
                        var toReturn = new RemainingSubjectStruct(childData);
                        toReturn.IsDirty = false;
                        return toReturn;
                    }
                }
                var cleanCopy = _AssociateDistinguish;
                cleanCopy.IsDirty = false;
                cleanCopy.DescendantIsDirty = false;
                return cleanCopy;
            }
        }
        /*Location661*/
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        protected WorryAllianceClass? _ConfirmOperation;
        public WorryAllianceClass? ConfirmOperation
        {
            [DebuggerStepThrough]
            get
            {
                if (!_ConfirmOperation_Accessed)
                {
                    LazinateConfirmOperation();
                } 
                return _ConfirmOperation;
            }
            [DebuggerStepThrough]
            set
            {
                if (_ConfirmOperation != null)
                {
                    _ConfirmOperation.LazinatorParents = _ConfirmOperation.LazinatorParents.WithRemoved(this);
                }
                if (value != null)
                {
                    value.LazinatorParents = value.LazinatorParents.WithAdded(this);
                }
                
                IsDirty = true;
                DescendantIsDirty = true;
                _ConfirmOperation = value;
                _ConfirmOperation_Accessed = true;
            }
        }
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        protected bool _ConfirmOperation_Accessed;
        private void LazinateConfirmOperation()
        {
            if (LazinatorMemoryStorage.Length == 0)
            {
                _ConfirmOperation = null;
            }
            else
            {
                LazinatorMemory childData = GetChildSlice(LazinatorMemoryStorage, _ConfirmOperation_ByteIndex, _ConfirmOperation_ByteLength, null);
                _ConfirmOperation = DeserializationFactory.Instance.CreateBaseOrDerivedType(10001, (c, p) => new WorryAllianceClass(c, p), childData, this); 
            }
            _ConfirmOperation_Accessed = true;
        }
        
        /*Location662*/
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        protected MealExpensiveClass? _LiteraryWeek;
        public MealExpensiveClass? LiteraryWeek
        {
            [DebuggerStepThrough]
            get
            {
                if (!_LiteraryWeek_Accessed)
                {
                    LazinateLiteraryWeek();
                } 
                return _LiteraryWeek;
            }
            [DebuggerStepThrough]
            set
            {
                if (_LiteraryWeek != null)
                {
                    _LiteraryWeek.LazinatorParents = _LiteraryWeek.LazinatorParents.WithRemoved(this);
                }
                if (value != null)
                {
                    value.LazinatorParents = value.LazinatorParents.WithAdded(this);
                }
                
                IsDirty = true;
                DescendantIsDirty = true;
                _LiteraryWeek = value;
                _LiteraryWeek_Accessed = true;
            }
        }
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        protected bool _LiteraryWeek_Accessed;
        private void LazinateLiteraryWeek()
        {
            if (LazinatorMemoryStorage.Length == 0)
            {
                _LiteraryWeek = null;
            }
            else
            {
                LazinatorMemory childData = GetChildSlice(LazinatorMemoryStorage, _LiteraryWeek_ByteIndex, _LiteraryWeek_ByteLength, null);
                _LiteraryWeek = DeserializationFactory.Instance.CreateBaseOrDerivedType(10005, (c, p) => new MealExpensiveClass(c, p), childData, this); 
            }
            _LiteraryWeek_Accessed = true;
        }
        
        /*Location663*/
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        protected WorryAllianceClass _SkyPersonal;
        public WorryAllianceClass SkyPersonal
        {
            [DebuggerStepThrough]
            get
            {
                
                return _SkyPersonal!;
            }
            [DebuggerStepThrough]
            set
            {
                _ = value ?? throw new ArgumentNullException(nameof(value));
                if (_SkyPersonal != null)
                {
                    _SkyPersonal.LazinatorParents = _SkyPersonal.LazinatorParents.WithRemoved(this);
                }
                value.LazinatorParents = value.LazinatorParents.WithAdded(this);
                
                IsDirty = true;
                DescendantIsDirty = true;
                _SkyPersonal = value;
            }
        }
        
        /*Location664*/// DEBUG5
        /*Location677*/        /* Clone overrides */
        
        public PsychologistOriginallyClass(WorryAllianceClass skyPersonal, IncludeChildrenMode originalIncludeChildrenMode = IncludeChildrenMode.IncludeAllChildren) : base(originalIncludeChildrenMode)
        {
            _SkyPersonal = skyPersonal;
            
            if (skyPersonal == null)
            {
                throw new ArgumentNullException("skyPersonal");
            }
        }
        
        public PsychologistOriginallyClass(LazinatorMemory serializedBytes, ILazinator? parent = null, IncludeChildrenMode originalIncludeChildrenMode = IncludeChildrenMode.IncludeAllChildren, int? lazinatorObjectVersion = null) : base(serializedBytes, parent, originalIncludeChildrenMode, lazinatorObjectVersion)
        {LazinatorMemory childData;
            
            childData = GetChildSlice(LazinatorMemoryStorage, _SkyPersonal_ByteIndex, _SkyPersonal_ByteLength, null);
            _SkyPersonal = DeserializationFactory.Instance.CreateBaseOrDerivedType(10001, (c, p) => new WorryAllianceClass(c, p), childData, this); 
        }
        
        public override ILazinator? CloneLazinator(IncludeChildrenMode includeChildrenMode = IncludeChildrenMode.IncludeAllChildren, CloneBufferOptions cloneBufferOptions = CloneBufferOptions.IndependentBuffers)
        {
            PsychologistOriginallyClass clone;
            if (cloneBufferOptions == CloneBufferOptions.NoBuffer)
            {
                clone = new PsychologistOriginallyClass(SkyPersonal, includeChildrenMode);
                clone.LazinatorObjectVersion = LazinatorObjectVersion;
                clone = (PsychologistOriginallyClass)AssignCloneProperties(clone, includeChildrenMode)!;
            }
            else
            {
                LazinatorMemory bytes = EncodeOrRecycleToNewBuffer(includeChildrenMode, OriginalIncludeChildrenMode, IsDirty, DescendantIsDirty, false, LazinatorMemoryStorage, this);
                clone = new PsychologistOriginallyClass(bytes);
            }
            return clone;
        }
        
        protected override ILazinator? AssignCloneProperties(ILazinator clone, IncludeChildrenMode includeChildrenMode)
        {
            base.AssignCloneProperties(clone, includeChildrenMode);
            PsychologistOriginallyClass typedClone = (PsychologistOriginallyClass) clone;
            /*Location666*/if (includeChildrenMode != IncludeChildrenMode.ExcludeAllChildren && includeChildrenMode != IncludeChildrenMode.IncludeOnlyIncludableChildren)
            {
                typedClone.AssociateDistinguish = (RemainingSubjectStruct) AssociateDistinguish.CloneLazinator(includeChildrenMode, CloneBufferOptions.NoBuffer)!;/*Location665*/
            }
            /*Location670*/if (includeChildrenMode != IncludeChildrenMode.ExcludeAllChildren && includeChildrenMode != IncludeChildrenMode.IncludeOnlyIncludableChildren)
            {
                if (ConfirmOperation == null)
                {
                    typedClone.ConfirmOperation = null;/*Location668*//*Location669*/
                }
                else
                {
                    typedClone.ConfirmOperation = (WorryAllianceClass?) ConfirmOperation.CloneLazinator(includeChildrenMode, CloneBufferOptions.NoBuffer)!;/*Location667*/
                }
            }
            /*Location674*/if (includeChildrenMode != IncludeChildrenMode.ExcludeAllChildren && includeChildrenMode != IncludeChildrenMode.IncludeOnlyIncludableChildren)
            {
                if (LiteraryWeek == null)
                {
                    typedClone.LiteraryWeek = null;/*Location672*//*Location673*/
                }
                else
                {
                    typedClone.LiteraryWeek = (MealExpensiveClass?) LiteraryWeek.CloneLazinator(includeChildrenMode, CloneBufferOptions.NoBuffer)!;/*Location671*/
                }
            }
            /*Location676*/typedClone.SkyPersonal = (WorryAllianceClass) SkyPersonal.CloneLazinator(includeChildrenMode, CloneBufferOptions.NoBuffer)!;/*Location675*/
            
            return typedClone;
        }
        
        /* Properties */
        /*Location678*/
        public override IEnumerable<(string propertyName, ILazinator? descendant)> EnumerateLazinatorDescendants(Func<ILazinator?, bool>? matchCriterion, bool stopExploringBelowMatch, Func<ILazinator?, bool>? exploreCriterion, bool exploreOnlyDeserializedChildren, bool enumerateNulls)
        {
            foreach (var inheritedYield in base.EnumerateLazinatorDescendants(matchCriterion, stopExploringBelowMatch, exploreCriterion, exploreOnlyDeserializedChildren, enumerateNulls))
            {
                yield return inheritedYield;
            }
            /*Location679*/bool isMatch_AssociateDistinguish = matchCriterion == null || matchCriterion(AssociateDistinguish);
            bool shouldExplore_AssociateDistinguish = exploreCriterion == null || exploreCriterion(AssociateDistinguish);
            if (isMatch_AssociateDistinguish)
            {
                yield return ("AssociateDistinguish", AssociateDistinguish);
            }
            if ((!stopExploringBelowMatch || !isMatch_AssociateDistinguish) && shouldExplore_AssociateDistinguish)
            {
                foreach (var toYield in AssociateDistinguish!.EnumerateLazinatorDescendants(matchCriterion, stopExploringBelowMatch, exploreCriterion, exploreOnlyDeserializedChildren, enumerateNulls))
                {
                    yield return ("AssociateDistinguish" + "." + toYield.propertyName, toYield.descendant);
                }
            }
            /*Location680*/if (enumerateNulls && (!exploreOnlyDeserializedChildren || _ConfirmOperation_Accessed) && ConfirmOperation == null)
            {
                yield return ("ConfirmOperation", default);
            }
            else
            {
                if ((!exploreOnlyDeserializedChildren && ConfirmOperation != null) || (_ConfirmOperation_Accessed && _ConfirmOperation != null))
                {
                    bool isMatch_ConfirmOperation = matchCriterion == null || matchCriterion(ConfirmOperation);
                    bool shouldExplore_ConfirmOperation = exploreCriterion == null || exploreCriterion(ConfirmOperation);
                    if (isMatch_ConfirmOperation)
                    {
                        yield return ("ConfirmOperation", ConfirmOperation);
                    }
                    if ((!stopExploringBelowMatch || !isMatch_ConfirmOperation) && shouldExplore_ConfirmOperation)
                    {
                        foreach (var toYield in ConfirmOperation!.EnumerateLazinatorDescendants(matchCriterion, stopExploringBelowMatch, exploreCriterion, exploreOnlyDeserializedChildren, enumerateNulls))
                        {
                            yield return ("ConfirmOperation" + "." + toYield.propertyName, toYield.descendant);
                        }
                    }
                }
            }
            /*Location681*/if (enumerateNulls && (!exploreOnlyDeserializedChildren || _LiteraryWeek_Accessed) && LiteraryWeek == null)
            {
                yield return ("LiteraryWeek", default);
            }
            else
            {
                if ((!exploreOnlyDeserializedChildren && LiteraryWeek != null) || (_LiteraryWeek_Accessed && _LiteraryWeek != null))
                {
                    bool isMatch_LiteraryWeek = matchCriterion == null || matchCriterion(LiteraryWeek);
                    bool shouldExplore_LiteraryWeek = exploreCriterion == null || exploreCriterion(LiteraryWeek);
                    if (isMatch_LiteraryWeek)
                    {
                        yield return ("LiteraryWeek", LiteraryWeek);
                    }
                    if ((!stopExploringBelowMatch || !isMatch_LiteraryWeek) && shouldExplore_LiteraryWeek)
                    {
                        foreach (var toYield in LiteraryWeek!.EnumerateLazinatorDescendants(matchCriterion, stopExploringBelowMatch, exploreCriterion, exploreOnlyDeserializedChildren, enumerateNulls))
                        {
                            yield return ("LiteraryWeek" + "." + toYield.propertyName, toYield.descendant);
                        }
                    }
                }
            }
            /*Location682*/if (enumerateNulls && SkyPersonal == null)
            {
                yield return ("SkyPersonal", default);
            }
            else
            {
                bool isMatch_SkyPersonal = matchCriterion == null || matchCriterion(SkyPersonal);
                bool shouldExplore_SkyPersonal = exploreCriterion == null || exploreCriterion(SkyPersonal);
                if (isMatch_SkyPersonal)
                {
                    yield return ("SkyPersonal", SkyPersonal);
                }
                if ((!stopExploringBelowMatch || !isMatch_SkyPersonal) && shouldExplore_SkyPersonal)
                {
                    foreach (var toYield in SkyPersonal!.EnumerateLazinatorDescendants(matchCriterion, stopExploringBelowMatch, exploreCriterion, exploreOnlyDeserializedChildren, enumerateNulls))
                    {
                        yield return ("SkyPersonal" + "." + toYield.propertyName, toYield.descendant);
                    }
                }
            }
            /*Location683*/yield break;
        }
        /*Location684*/
        
        public override IEnumerable<(string propertyName, object? descendant)> EnumerateNonLazinatorProperties()
        {
            foreach (var inheritedYield in base.EnumerateNonLazinatorProperties())
            {
                yield return inheritedYield;
            }
            /*Location685*/yield break;
        }
        /*Location686*/
        public override ILazinator? ForEachLazinator(Func<ILazinator?, ILazinator?>? changeFunc, bool exploreOnlyDeserializedChildren, bool changeThisLevel)
        {
            base.ForEachLazinator(changeFunc, exploreOnlyDeserializedChildren, false);
            /*Location687*/var deserialized_AssociateDistinguish = AssociateDistinguish;
            _AssociateDistinguish = (RemainingSubjectStruct) _AssociateDistinguish!.ForEachLazinator(changeFunc, exploreOnlyDeserializedChildren, true)!;
            /*Location688*/if ((!exploreOnlyDeserializedChildren && ConfirmOperation != null) || (_ConfirmOperation_Accessed && _ConfirmOperation != null))
            {
                _ConfirmOperation = (WorryAllianceClass?) _ConfirmOperation!.ForEachLazinator(changeFunc, exploreOnlyDeserializedChildren, true)!;
            }
            /*Location689*/if ((!exploreOnlyDeserializedChildren && LiteraryWeek != null) || (_LiteraryWeek_Accessed && _LiteraryWeek != null))
            {
                _LiteraryWeek = (MealExpensiveClass?) _LiteraryWeek!.ForEachLazinator(changeFunc, exploreOnlyDeserializedChildren, true)!;
            }
            /*Location690*/_SkyPersonal = (WorryAllianceClass) _SkyPersonal!.ForEachLazinator(changeFunc, exploreOnlyDeserializedChildren, true)!;
            /*Location691*/if (changeThisLevel && changeFunc != null)
            {
                return changeFunc(this);
            }
            return this;
        }
        /*Location692*/
        public override void FreeInMemoryObjects()
        {
            base.FreeInMemoryObjects();
            _AssociateDistinguish = default;
            _ConfirmOperation = default;
            _LiteraryWeek = default;
            _AssociateDistinguish_Accessed = _ConfirmOperation_Accessed = _LiteraryWeek_Accessed = false;
            IsDirty = false;
            DescendantIsDirty = false;
            HasChanged = false;
            DescendantHasChanged = false;
        }
        /*Location693*/
        /* Conversion */
        
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public override int LazinatorUniqueID => 10008;
        
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        protected override bool ContainsOpenGenericParameters => false;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public override LazinatorGenericIDType LazinatorGenericID => default;
        
        
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public override int LazinatorObjectVersion { get; set; } = 0;
        
        
        /*Location694*/protected override int ConvertFromBytesAfterHeader(IncludeChildrenMode includeChildrenMode, int serializedVersionNumber, ref int bytesSoFar)
        {
            ReadOnlySpan<byte> span = LazinatorMemoryStorage.InitialReadOnlyMemory.Span;
            ConvertFromBytesForPrimitiveProperties(span, includeChildrenMode, serializedVersionNumber, ref bytesSoFar);
            int lengthForLengths = 4;
            if (includeChildrenMode != IncludeChildrenMode.ExcludeAllChildren && includeChildrenMode != IncludeChildrenMode.IncludeOnlyIncludableChildren)
            {
                lengthForLengths += 12;
            }
            int totalChildrenSize = ConvertFromBytesForChildLengths(span, includeChildrenMode, serializedVersionNumber, bytesSoFar + lengthForLengths, ref bytesSoFar);
            return bytesSoFar + totalChildrenSize;
        }
        
        /*Location695*/protected override void ConvertFromBytesForPrimitiveProperties(ReadOnlySpan<byte> span, IncludeChildrenMode includeChildrenMode, int serializedVersionNumber, ref int bytesSoFar)
        {
            base.ConvertFromBytesForPrimitiveProperties(span, OriginalIncludeChildrenMode, serializedVersionNumber, ref bytesSoFar);
            /*Location696*/        }
            
            /*Location697*/protected override int ConvertFromBytesForChildLengths(ReadOnlySpan<byte> span, IncludeChildrenMode includeChildrenMode, int serializedVersionNumber, int indexOfFirstChild, ref int bytesSoFar)
            {
                int totalChildrenBytes = 0;
                totalChildrenBytes = base.ConvertFromBytesForChildLengths(span, OriginalIncludeChildrenMode, serializedVersionNumber, indexOfFirstChild, ref bytesSoFar);
                /*Location698*/_AssociateDistinguish_ByteIndex = indexOfFirstChild + totalChildrenBytes;
                /*Location699*/if (includeChildrenMode != IncludeChildrenMode.ExcludeAllChildren && includeChildrenMode != IncludeChildrenMode.IncludeOnlyIncludableChildren)
                {
                    totalChildrenBytes += span.ToInt32(ref bytesSoFar);
                }
                /*Location700*/_ConfirmOperation_ByteIndex = indexOfFirstChild + totalChildrenBytes;
                /*Location701*/if (includeChildrenMode != IncludeChildrenMode.ExcludeAllChildren && includeChildrenMode != IncludeChildrenMode.IncludeOnlyIncludableChildren)
                {
                    totalChildrenBytes += span.ToInt32(ref bytesSoFar);
                }
                /*Location702*/_LiteraryWeek_ByteIndex = indexOfFirstChild + totalChildrenBytes;
                /*Location703*/if (includeChildrenMode != IncludeChildrenMode.ExcludeAllChildren && includeChildrenMode != IncludeChildrenMode.IncludeOnlyIncludableChildren)
                {
                    totalChildrenBytes += span.ToInt32(ref bytesSoFar);
                }
                /*Location704*/_SkyPersonal_ByteIndex = indexOfFirstChild + totalChildrenBytes;
                /*Location705*/totalChildrenBytes += span.ToInt32(ref bytesSoFar);
                /*Location706*/_PsychologistOriginallyClass_EndByteIndex = indexOfFirstChild + totalChildrenBytes;
                /*Location707*/return totalChildrenBytes;
            }
            
            /*Location708*/public override void SerializeToExistingBuffer(ref BufferWriter writer, in LazinatorSerializationOptions options)
            {
                /*Location709*/int startPosition = writer.ActiveMemoryPosition;
                WritePropertiesIntoBuffer(ref writer, options, true);
                /*Location710*/if (options.UpdateStoredBuffer)
                {
                    UpdateStoredBuffer(ref writer, startPosition, writer.ActiveMemoryPosition - startPosition, options.IncludeChildrenMode, false);
                    /*Location711*/}
                    /*Location712*/}
                    /*Location713*/
                    public override void UpdateStoredBuffer(ref BufferWriter writer, long startPosition, long length, IncludeChildrenMode includeChildrenMode, bool updateDeserializedChildren)
                    {
                        /*Location714*/_IsDirty = false;
                        if (includeChildrenMode == IncludeChildrenMode.IncludeAllChildren)
                        {
                            _DescendantIsDirty = false;/*Location715*/
                            if (updateDeserializedChildren)
                            {
                                UpdateDeserializedChildren(ref writer, startPosition);
                                /*Location716*/}
                                /*Location717*/
                                _AssociateDistinguish_Accessed = false;/*Location718*/
                            }
                            else
                            {
                                ThrowHelper.ThrowCannotUpdateStoredBuffer();
                            }
                            /*Location719*/
                            var newBuffer = writer.Slice(startPosition, length);
                            LazinatorMemoryStorage = newBuffer;
                            /*Location720*/}
                            /*Location721*/
                            protected override void UpdateDeserializedChildren(ref BufferWriter writer, long startPosition)
                            {
                                base.UpdateDeserializedChildren(ref writer, startPosition);
                                /*Location722*/AssociateDistinguish!.UpdateStoredBuffer(ref writer, startPosition + _AssociateDistinguish_ByteIndex, _AssociateDistinguish_ByteLength, IncludeChildrenMode.IncludeAllChildren, true);
                                /*Location723*/if (_ConfirmOperation_Accessed && _ConfirmOperation != null)
                                {
                                    ConfirmOperation!.UpdateStoredBuffer(ref writer, startPosition + _ConfirmOperation_ByteIndex, _ConfirmOperation_ByteLength, IncludeChildrenMode.IncludeAllChildren, true);
                                }
                                /*Location724*/if (_LiteraryWeek_Accessed && _LiteraryWeek != null)
                                {
                                    LiteraryWeek!.UpdateStoredBuffer(ref writer, startPosition + _LiteraryWeek_ByteIndex, _LiteraryWeek_ByteLength, IncludeChildrenMode.IncludeAllChildren, true);
                                }
                                /*Location725*/SkyPersonal!.UpdateStoredBuffer(ref writer, startPosition + _SkyPersonal_ByteIndex, _SkyPersonal_ByteLength, IncludeChildrenMode.IncludeAllChildren, true);
                                /*Location726*/
                            }
                            
                            /*Location727*/
                            protected override void WritePropertiesIntoBuffer(ref BufferWriter writer, in LazinatorSerializationOptions options, bool includeUniqueID)
                            {
                                int startPosition = writer.ActiveMemoryPosition;
                                /*Location728*/if (includeUniqueID)
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
                                /*Location729*/CompressedIntegralTypes.WriteCompressedInt(ref writer, Lazinator.Support.LazinatorVersionInfo.LazinatorIntVersion);
                                CompressedIntegralTypes.WriteCompressedInt(ref writer, LazinatorObjectVersion);
                                writer.Write((byte)options.IncludeChildrenMode);
                                /*Location730*/// write properties
                                /*Location731*/
                                WritePrimitivePropertiesIntoBuffer(ref writer, options, includeUniqueID);
                                int lengthForLengths = 4;
                                if (options.IncludeChildrenMode != IncludeChildrenMode.ExcludeAllChildren && options.IncludeChildrenMode != IncludeChildrenMode.IncludeOnlyIncludableChildren)
                                {
                                    lengthForLengths += 12;
                                }
                                
                                var previousLengthsPosition = writer.SetLengthsPosition(lengthForLengths);
                                WriteChildrenPropertiesIntoBuffer(ref writer, options, includeUniqueID, startPosition);
                                writer.ResetLengthsPosition(previousLengthsPosition);
                                /*Location732*//*Location733*/
                            }
                            /*Location734*/
                            protected override void WritePrimitivePropertiesIntoBuffer(ref BufferWriter writer, in LazinatorSerializationOptions options, bool includeUniqueID)
                            {
                                base.WritePrimitivePropertiesIntoBuffer(ref writer, options, includeUniqueID);
                                /*Location735*/}/*Location736*//*Location737*/
                                protected override void WriteChildrenPropertiesIntoBuffer(ref BufferWriter writer, LazinatorSerializationOptions options, bool includeUniqueID, int startOfObjectPosition)
                                {
                                    base.WriteChildrenPropertiesIntoBuffer(ref writer, options, includeUniqueID, startOfObjectPosition);
                                    /*Location738*/if (options.SplittingPossible)
                                    {
                                        options = options.WithoutSplittingPossible();
                                    }
                                    /*Location739*/int startOfChildPosition = 0;
                                    /*Location740*/int lengthValue = 0;
                                    /*Location741*/startOfChildPosition = writer.ActiveMemoryPosition;
                                    /*Location742*/if (options.IncludeChildrenMode != IncludeChildrenMode.ExcludeAllChildren && options.IncludeChildrenMode != IncludeChildrenMode.IncludeOnlyIncludableChildren)
                                    {
                                        if ((options.IncludeChildrenMode != IncludeChildrenMode.IncludeAllChildren || options.IncludeChildrenMode != OriginalIncludeChildrenMode) && !_AssociateDistinguish_Accessed)
                                        {
                                            var deserialized = AssociateDistinguish;
                                        }
                                        WriteChild(ref writer, ref _AssociateDistinguish!, options, _AssociateDistinguish_Accessed, () => GetChildSlice(LazinatorMemoryStorage, _AssociateDistinguish_ByteIndex, _AssociateDistinguish_ByteLength, null), this);
                                        lengthValue = writer.ActiveMemoryPosition - startOfChildPosition;
                                        if (lengthValue > int.MaxValue)
                                        {
                                            ThrowHelper.ThrowTooLargeException(int.MaxValue);
                                        }
                                        writer.RecordLength((int) lengthValue);
                                    }
                                    /*Location743*/if (options.UpdateStoredBuffer)
                                    {
                                        _AssociateDistinguish_ByteIndex = startOfChildPosition - startOfObjectPosition;
                                        
                                    }
                                    /*Location744*/startOfChildPosition = writer.ActiveMemoryPosition;
                                    /*Location745*/if (options.IncludeChildrenMode != IncludeChildrenMode.ExcludeAllChildren && options.IncludeChildrenMode != IncludeChildrenMode.IncludeOnlyIncludableChildren)
                                    {
                                        if ((options.IncludeChildrenMode != IncludeChildrenMode.IncludeAllChildren || options.IncludeChildrenMode != OriginalIncludeChildrenMode) && !_ConfirmOperation_Accessed)
                                        {
                                            var deserialized = ConfirmOperation;
                                        }
                                        WriteChild(ref writer, ref _ConfirmOperation, options, _ConfirmOperation_Accessed, () => GetChildSlice(LazinatorMemoryStorage, _ConfirmOperation_ByteIndex, _ConfirmOperation_ByteLength, null), this);
                                        lengthValue = writer.ActiveMemoryPosition - startOfChildPosition;
                                        if (lengthValue > int.MaxValue)
                                        {
                                            ThrowHelper.ThrowTooLargeException(int.MaxValue);
                                        }
                                        writer.RecordLength((int) lengthValue);
                                    }
                                    /*Location746*/if (options.UpdateStoredBuffer)
                                    {
                                        _ConfirmOperation_ByteIndex = startOfChildPosition - startOfObjectPosition;
                                        
                                    }
                                    /*Location747*/startOfChildPosition = writer.ActiveMemoryPosition;
                                    /*Location748*/if (options.IncludeChildrenMode != IncludeChildrenMode.ExcludeAllChildren && options.IncludeChildrenMode != IncludeChildrenMode.IncludeOnlyIncludableChildren)
                                    {
                                        if ((options.IncludeChildrenMode != IncludeChildrenMode.IncludeAllChildren || options.IncludeChildrenMode != OriginalIncludeChildrenMode) && !_LiteraryWeek_Accessed)
                                        {
                                            var deserialized = LiteraryWeek;
                                        }
                                        WriteChild(ref writer, ref _LiteraryWeek, options, _LiteraryWeek_Accessed, () => GetChildSlice(LazinatorMemoryStorage, _LiteraryWeek_ByteIndex, _LiteraryWeek_ByteLength, null), this);
                                        lengthValue = writer.ActiveMemoryPosition - startOfChildPosition;
                                        if (lengthValue > int.MaxValue)
                                        {
                                            ThrowHelper.ThrowTooLargeException(int.MaxValue);
                                        }
                                        writer.RecordLength((int) lengthValue);
                                    }
                                    /*Location749*/if (options.UpdateStoredBuffer)
                                    {
                                        _LiteraryWeek_ByteIndex = startOfChildPosition - startOfObjectPosition;
                                        
                                    }
                                    /*Location750*/startOfChildPosition = writer.ActiveMemoryPosition;
                                    /*Location751*/
                                    WriteChild(ref writer, ref _SkyPersonal!, options, true, () => GetChildSlice(LazinatorMemoryStorage, _SkyPersonal_ByteIndex, _SkyPersonal_ByteLength, null), this);
                                    lengthValue = writer.ActiveMemoryPosition - startOfChildPosition;
                                    if (lengthValue > int.MaxValue)
                                    {
                                        ThrowHelper.ThrowTooLargeException(int.MaxValue);
                                    }
                                    writer.RecordLength((int) lengthValue);
                                    /*Location752*/if (options.UpdateStoredBuffer)
                                    {
                                        _SkyPersonal_ByteIndex = startOfChildPosition - startOfObjectPosition;
                                        
                                    }
                                    /*Location753*/if (options.UpdateStoredBuffer)
                                    {
                                        /*Location754*/_PsychologistOriginallyClass_EndByteIndex = writer.ActiveMemoryPosition - startOfObjectPosition;
                                        /*Location755*/}
                                        /*Location756*/
                                        /*Location757*/}/*Location758*//*Location759*/
                                    }
                                }
                                #nullable restore
