﻿namespace LazinatorCollections
{
    /// <summary>
    /// Unique IDs for Lazinator collections. These must be unique across all Lazinator objects. They are encoded so that the object type can be recognized during deserialization.
    /// </summary>
    public enum LazinatorCollectionUniqueIDs
    { 
        ILazinatorOffsetList = 200,
        ILazinatorList,
        ILazinatorFastReadList,
        ILazinatorBitArray,
        ILazinatorBitArrayUnofficial,
        ILazinatorTuple,
        ILazinatorTriple,
        ILazinatorQueue,
        ILazinatorStack,
        ILazinatorArray,
        IDictionaryBucket,
        IDictionary,
        IDictionaryUnofficial,
        ILazinatorFastReadListInt32,
        ILazinatorFastReadListInt16,
        ILazinatorListUnofficial,
        ILazinatorGeneralTree,
        ILazinatorGeneralTreeUnofficial,
        ILazinatorLocationAwareTree,
        ILazinatorSortedList,
        ILazinatorKeyValue,
        ICountableContainer,
        ILazinatorSorted,
        ILazinatorListable,
        ILazinatorLinkedListNode,
        ILazinatorLinkedList,
        ILazinatorSortedLinkedList,
        IIndexableValueContainer,
        IIndexableKeyValueTree,
        IKeyValueContainer,
        ISortedKeyValueContainer,
        ISortedMultivalueContainer,
        ISortedIndexableContainer,
        ISortedIndexableKeyValueContainer,
        ILazinatorDictionaryable,
        ILazinatorComparableKeyValue,
        IValueContainer,
        IMultivalueContainer,
        ISortedIndexableMultivalueContainer,
        IIndexableMultivalueContainer,
        IKeyMultivalueContainer,
        ISortedKeyMultivalueContainer,
        ISortedIndexableKeyMultivalueContainer,
        IIndexableKeyMultivalueContainer,
        ISortedValueContainer,
        ILazinatorMultivalueDictionaryable,
        ILazinatorSortable,
        IKeyAndValueEnumerators,
        IAggregatedMultivalueContainer,
        IAggregatedValueContainer,
        IByteSpanUnofficial,
        IByteSpan,
        IMultilevelReporter,
        IMultilevelReportReceiver,
        IRemote,
    }
}
