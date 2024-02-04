using Lazinator.Buffers;
using LazinatorAvlCollections.Avl;
using LazinatorAvlCollections.Avl.KeyValueTree;
using LazinatorAvlCollections.Avl.ListTree;
using LazinatorAvlCollections.Avl.ValueTree;
using Lazinator.Collections.Dictionary;
using Lazinator.Collections.Interfaces;
using LazinatorAvlCollections.Avl.BinaryTree;
using Lazinator.Collections.Tuples;
using Lazinator.Core;
using Lazinator.Wrappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lazinator.Collections;
using System.Diagnostics;

namespace LazinatorAvlCollections.Factories
{
    /// <summary>
    /// A factory for creating a container by specifying information about the outermost level and another factory containing information
    /// on inner levels (or by using default choices for inner levels).
    /// </summary>
    public partial class ContainerFactory : IContainerFactory
    {
        #region Construction and initialization

        public ContainerFactory()
        {
        }

        public ContainerFactory(ContainerLevel thisLevel)
        {
            InitializeLevels(new List<ContainerLevel>() { thisLevel });
        }

        public ContainerFactory(ContainerLevel thisLevel, ContainerFactory innerContainerFactory)
        {
            ThisLevel = thisLevel;
            InnerFactory = innerContainerFactory;
        }

        public ContainerFactory(IEnumerable<ContainerLevel> levels)
        {
            InitializeLevels(levels);
        }

        private void InitializeLevels(IEnumerable<ContainerLevel> levels)
        {
            ThisLevel = levels.First();
            var remaining = levels.Skip(1);
            var next = remaining.FirstOrDefault();
            if (next == null)
            {
                next = GetDefaultInnerFactory();
                if (next != null)
                    remaining = new List<ContainerLevel>() { next };
            }
            VerifyNextLevel(next);
            if (remaining.Any())
                InnerFactory = new ContainerFactory(remaining);
        }

        #endregion


        #region Static creation methods

        public static ILazinatorDictionaryable<K, V> CreateUnsortedAvlDictionary<K, V>(DictionaryTypes dictionaryToUse, int? splitThresholdForLeafLayer = null, List<int> splitThresholdsForInnerLayers = null) where K : ILazinator where V : ILazinator
        {
            var levels = GetContainerLevels(dictionaryToUse);
            if (splitThresholdForLeafLayer != null)
                levels = ModifyLevelsForSplitThresholds(levels, (int)splitThresholdForLeafLayer, splitThresholdsForInnerLayers);
            var factory = GetDictionaryFactory(levels);
            return factory.CreateUnsortedAvlDictionary<K, V>();
        }

        public static ILazinatorDictionaryable<K, V> CreateDictionaryOfType<K, V>(DictionaryTypes dictionaryToUse, int? splitThresholdForLeafLayer = null, List<int> splitThresholdsForInnerLayers = null) where K : ILazinator, IComparable<K> where V : ILazinator
        {
            if (dictionaryToUse == DictionaryTypes.NonAvl)
                return new LazinatorDictionary<K, V>();
            var levels = GetContainerLevels(dictionaryToUse);
            if (splitThresholdForLeafLayer != null)
                levels = ModifyLevelsForSplitThresholds(levels, (int) splitThresholdForLeafLayer, splitThresholdsForInnerLayers);
            var factory = GetDictionaryFactory(levels);
            return factory.CreateDictionaryOfType<K, V>();
        }

        public static ILazinatorListable<T> CreateListOfType<T>(ListTypes listType, int? splitThresholdForLeafLayer = null, List<int> splitThresholdsForInnerLayers = null) where T : ILazinator
        {
            var levels = GetContainerLevels(listType);
            if (splitThresholdForLeafLayer != null && !(listType is ListTypes.LazinatorList or ListTypes.LazinatorLinkedList or ListTypes.LazinatorSortedList or ListTypes.LazinatorSortedListWithDuplicates or ListTypes.LazinatorSortedLinkedList or ListTypes.LazinatorSortedLinkedListWithDuplicates))
                levels = ModifyLevelsForSplitThresholds(levels, (int)splitThresholdForLeafLayer, splitThresholdsForInnerLayers);
            var factory = GetListFactory(levels);
            return factory.CreateLazinatorListable<T>();
        }

        public static ILazinatorSorted<T> CreateSortedListOfType<T>(ListTypes listType, int? splitThresholdForLeafLayer = null, List<int> splitThresholdsForInnerLayers = null) where T : ILazinator, IComparable<T>
        {
            var levels = GetContainerLevels(listType);
            if (splitThresholdForLeafLayer != null && !(listType is ListTypes.LazinatorList or ListTypes.LazinatorLinkedList or ListTypes.LazinatorSortedList or ListTypes.LazinatorSortedListWithDuplicates or ListTypes.LazinatorSortedLinkedList or ListTypes.LazinatorSortedLinkedListWithDuplicates))
                levels = ModifyLevelsForSplitThresholds(levels, (int)splitThresholdForLeafLayer, splitThresholdsForInnerLayers);
            var factory = GetListFactory(levels);
            return factory.CreateLazinatorSorted<T>();
        }

        private static ContainerFactory GetDictionaryFactory(List<ContainerLevel> levels)
        {
            if (levels.Count == 1 && levels[0].ContainerType == ContainerType.LazinatorDictionary)
                return new ContainerFactory(new ContainerLevel(ContainerType.LazinatorDictionary));
            return new ContainerFactory(levels);
        }

        private static ContainerFactory GetListFactory(List<ContainerLevel> levels)
        {
            return new ContainerFactory(levels);
        }

        private static List<ContainerLevel> GetContainerLevels(ListTypes l)
        {
            switch (l)
            {
                case ListTypes.LazinatorList:
                    return new List<ContainerLevel>()
                    {
                        new ContainerLevel(ContainerType.LazinatorList)
                    };
                case ListTypes.LazinatorLinkedList:
                    return new List<ContainerLevel>()
                    {
                        new ContainerLevel(ContainerType.LazinatorLinkedList)
                    };
                case ListTypes.LazinatorSortedList:
                    return new List<ContainerLevel>()
                    {
                        new ContainerLevel(ContainerType.LazinatorSortedList)
                    };
                case ListTypes.LazinatorSortedListWithDuplicates:
                    return new List<ContainerLevel>()
                    {
                        new ContainerLevel(ContainerType.LazinatorSortedList, true)
                    };
                case ListTypes.LazinatorSortedLinkedList:
                    return new List<ContainerLevel>()
                    {
                        new ContainerLevel(ContainerType.LazinatorSortedLinkedList)
                    };
                case ListTypes.LazinatorSortedLinkedListWithDuplicates:
                    return new List<ContainerLevel>()
                    {
                        new ContainerLevel(ContainerType.LazinatorSortedLinkedList, true)
                    };
                case ListTypes.UnbalancedAvlList:
                    return new List<ContainerLevel>()
                    {
                        new ContainerLevel(ContainerType.AvlList, false, int.MaxValue, true),
                        new ContainerLevel(ContainerType.AvlIndexableTree, false, int.MaxValue, true)
                    };
                case ListTypes.AvlList:
                    return new List<ContainerLevel>()
                    {
                        new ContainerLevel(ContainerType.AvlList),
                        new ContainerLevel(ContainerType.AvlIndexableTree)
                    };
                case ListTypes.AvlListMultilayer:
                    return new List<ContainerLevel>()
                    {
                        new ContainerLevel(ContainerType.AvlList),
                        new ContainerLevel(ContainerType.AvlIndexableListTree),
                        new ContainerLevel(ContainerType.LazinatorList, false, 5)
                    };
                case ListTypes.UnbalancedAvlSortedList:
                    return new List<ContainerLevel>()
                    {
                        new ContainerLevel(ContainerType.AvlSortedList, false, int.MaxValue, true),
                        new ContainerLevel(ContainerType.AvlSortedIndexableTree, false, int.MaxValue, true)
                    };
                case ListTypes.AvlSortedList:
                    return new List<ContainerLevel>()
                    {
                        new ContainerLevel(ContainerType.AvlSortedList),
                        new ContainerLevel(ContainerType.AvlSortedIndexableTree)
                    };
                case ListTypes.AvlSortedListWithDuplicates:
                    return new List<ContainerLevel>()
                    {
                        new ContainerLevel(ContainerType.AvlSortedList, true),
                        new ContainerLevel(ContainerType.AvlSortedIndexableTree, true)
                    };
                case ListTypes.AvlSortedListMultilayer:
                    return new List<ContainerLevel>()
                    {
                        new ContainerLevel(ContainerType.AvlSortedList),
                        new ContainerLevel(ContainerType.AvlSortedIndexableListTree),
                        new ContainerLevel(ContainerType.LazinatorSortedList, false, 5)
                    };
                case ListTypes.AvlSortedListMultilayerWithDuplicates:
                    return new List<ContainerLevel>()
                    {
                        new ContainerLevel(ContainerType.AvlSortedList, true),
                        new ContainerLevel(ContainerType.AvlSortedIndexableListTree, true),
                        new ContainerLevel(ContainerType.LazinatorSortedList, true, 5)
                    };
                default:
                    throw new NotImplementedException();
            }
        }

        private static List<ContainerLevel> GetContainerLevels(DictionaryTypes dictionaryToUse)
        {
            switch (dictionaryToUse)
            {
                case DictionaryTypes.Avl:
                    return new List<ContainerLevel>()
                    {
                        new ContainerLevel(ContainerType.AvlDictionary, false),
                        new ContainerLevel(ContainerType.AvlSortedKeyValueTree, true),
                        new ContainerLevel(ContainerType.AvlIndexableTree, true),
                        new ContainerLevel(ContainerType.LazinatorList, true)
                    };
                case DictionaryTypes.AvlIndexable: // for now, same as AvlDictionary, because that is already based on indexable treee
                    return new List<ContainerLevel>()
                    {
                        new ContainerLevel(ContainerType.AvlDictionary, false),
                        new ContainerLevel(ContainerType.AvlSortedKeyValueTree, true),
                        new ContainerLevel(ContainerType.AvlIndexableTree, true),
                        new ContainerLevel(ContainerType.LazinatorList, true)
                    };
                case DictionaryTypes.AvlMultiValue:
                    return new List<ContainerLevel>()
                    {
                        new ContainerLevel(ContainerType.AvlDictionary, true),
                        new ContainerLevel(ContainerType.AvlSortedKeyValueTree, true),
                        new ContainerLevel(ContainerType.AvlIndexableTree, true),
                        new ContainerLevel(ContainerType.LazinatorList, true)
                    };
                case DictionaryTypes.AvlSorted:
                    return new List<ContainerLevel>()
                    {
                        new ContainerLevel(ContainerType.AvlSortedDictionary, false),
                        new ContainerLevel(ContainerType.AvlSortedKeyValueTree, false),
                        new ContainerLevel(ContainerType.AvlIndexableTree, false),
                        new ContainerLevel(ContainerType.LazinatorList, false)
                    };
                case DictionaryTypes.AvlSortedIndexable:
                    return new List<ContainerLevel>()
                    {
                        new ContainerLevel(ContainerType.AvlSortedIndexableDictionary, false),
                        new ContainerLevel(ContainerType.AvlSortedIndexableKeyValueTree, false),
                        new ContainerLevel(ContainerType.AvlIndexableTree, false),
                        new ContainerLevel(ContainerType.LazinatorList, false)
                    };
                case DictionaryTypes.AvlSortedMultiValue:
                    return new List<ContainerLevel>()
                    {
                        new ContainerLevel(ContainerType.AvlSortedDictionary, true),
                        new ContainerLevel(ContainerType.AvlSortedKeyValueTree, true),
                        new ContainerLevel(ContainerType.AvlIndexableTree, true),
                        new ContainerLevel(ContainerType.LazinatorList, true)
                    };
                case DictionaryTypes.AvlMultilayer:
                    return new List<ContainerLevel>()
                    {
                        new ContainerLevel(ContainerType.AvlDictionary, false),
                        new ContainerLevel(ContainerType.AvlSortedKeyValueTree, true),
                        new ContainerLevel(ContainerType.AvlIndexableListTree, true),
                        new ContainerLevel(ContainerType.LazinatorList, true)
                    };
                case DictionaryTypes.AvlIndexableMultilayer:
                    return new List<ContainerLevel>()
                    {
                        new ContainerLevel(ContainerType.AvlDictionary, false),
                        new ContainerLevel(ContainerType.AvlSortedKeyValueTree, true),
                        new ContainerLevel(ContainerType.AvlIndexableListTree, true),
                        new ContainerLevel(ContainerType.LazinatorList, true)
                    };
                case DictionaryTypes.AvlMultiValueMultilayer:
                    return new List<ContainerLevel>()
                    {
                        new ContainerLevel(ContainerType.AvlDictionary, true),
                        new ContainerLevel(ContainerType.AvlSortedKeyValueTree, true),
                        new ContainerLevel(ContainerType.AvlIndexableListTree, true),
                        new ContainerLevel(ContainerType.LazinatorList, true)
                    };
                case DictionaryTypes.AvlSortedMultilayer:
                    return new List<ContainerLevel>()
                    {
                        new ContainerLevel(ContainerType.AvlSortedDictionary, false),
                        new ContainerLevel(ContainerType.AvlSortedKeyValueTree, false),
                        new ContainerLevel(ContainerType.AvlIndexableListTree, false),
                        new ContainerLevel(ContainerType.LazinatorList, false)
                    };
                case DictionaryTypes.AvlSortedIndexableMultilayer:
                    return new List<ContainerLevel>()
                    {
                        new ContainerLevel(ContainerType.AvlSortedIndexableDictionary, false),
                        new ContainerLevel(ContainerType.AvlSortedIndexableKeyValueTree, false),
                        new ContainerLevel(ContainerType.AvlIndexableListTree, false),
                        new ContainerLevel(ContainerType.LazinatorList, false)
                    };
                case DictionaryTypes.AvlSortedMultiValueMultilayer:
                    return new List<ContainerLevel>()
                    {
                        new ContainerLevel(ContainerType.AvlSortedDictionary, true),
                        new ContainerLevel(ContainerType.AvlSortedKeyValueTree, true),
                        new ContainerLevel(ContainerType.AvlIndexableListTree, true),
                        new ContainerLevel(ContainerType.LazinatorList, true)
                    };

                default:
                    throw new NotSupportedException();
            }
        }

        #endregion

        #region Creation based on information from this level

        public virtual IValueContainer<T> CreateValueContainer<T>() where T : ILazinator
        {
            switch (ThisLevel.ContainerType)
            {
                case ContainerType.LazinatorList:
                    return new LazinatorList<T>(ThisLevel.AllowDuplicates);
                case ContainerType.LazinatorLinkedList:
                    return new LazinatorLinkedList<T>(ThisLevel.AllowDuplicates);
                case ContainerType.AvlTree:
                    return new AvlTree<T>(ThisLevel.AllowDuplicates, ThisLevel.Unbalanced, ThisLevel.CacheEnds);
                case ContainerType.AvlIndexableTree:
                    return new AvlIndexableTree<T>(ThisLevel.AllowDuplicates, ThisLevel.Unbalanced, ThisLevel.CacheEnds);
                case ContainerType.AvlIndexableListTree:
                    return new AvlIndexableListTree<T>(ThisLevel.AllowDuplicates, ThisLevel.Unbalanced, InnerFactory);
                case ContainerType.AvlListTree:
                    return new AvlListTree<T>(ThisLevel.AllowDuplicates, ThisLevel.Unbalanced, InnerFactory);
                default:
                    throw new NotImplementedException();
            }
        }

        public IAggregatedMultivalueContainer<IIndexableMultivalueContainer<T>> CreateAggregatedValueContainer<T>() where T : ILazinator
        {
            switch (ThisLevel.ContainerType)
            {
                case ContainerType.AvlAggregatedTree:
                    return new AvlAggregatedTree<IIndexableMultivalueContainer<T>>(ThisLevel.AllowDuplicates, ThisLevel.Unbalanced);
                default:
                    throw new NotImplementedException();
            }
        }

        public ISortedValueContainer<T> CreateSortedValueContainer<T>() where T : ILazinator, IComparable<T>
        {
            switch (ThisLevel.ContainerType)
            {
                case ContainerType.LazinatorSortedList:
                    return new LazinatorSortedList<T>(ThisLevel.AllowDuplicates);
                case ContainerType.LazinatorSortedLinkedList:
                    return new LazinatorSortedLinkedList<T>(ThisLevel.AllowDuplicates);
                case ContainerType.AvlSortedTree:
                    return new AvlSortedTree<T>(ThisLevel.AllowDuplicates, ThisLevel.Unbalanced, ThisLevel.CacheEnds);
                case ContainerType.AvlSortedIndexableTree:
                    return new AvlSortedIndexableTree<T>(ThisLevel.AllowDuplicates, ThisLevel.Unbalanced, ThisLevel.CacheEnds);
                case ContainerType.AvlSortedIndexableListTree:
                    return new AvlSortedIndexableListTree<T>(ThisLevel.AllowDuplicates, ThisLevel.Unbalanced, InnerFactory);
                case ContainerType.AvlSortedListTree:
                    return new AvlSortedListTree<T>(ThisLevel.AllowDuplicates, ThisLevel.Unbalanced, InnerFactory);
                default:
                    throw new NotImplementedException();
            }
        }


        public ILazinatorListable<T> CreateLazinatorListable<T>() where T : ILazinator
        {
            switch (ThisLevel.ContainerType)
            {
                case ContainerType.LazinatorList:
                    return new LazinatorList<T>(ThisLevel.AllowDuplicates);
                case ContainerType.LazinatorLinkedList:
                    return new LazinatorLinkedList<T>(ThisLevel.AllowDuplicates);
                case ContainerType.AvlList:
                    return new AvlList<T>(InnerFactory);
                default:
                    throw new NotImplementedException();
            }
        }

        public ILazinatorSorted<T> CreateLazinatorSorted<T>() where T : ILazinator, IComparable<T>
        {
            switch (ThisLevel.ContainerType)
            {
                case ContainerType.LazinatorSortedList:
                    return new LazinatorSortedList<T>(ThisLevel.AllowDuplicates);
                case ContainerType.LazinatorSortedLinkedList:
                    return new LazinatorSortedLinkedList<T>(ThisLevel.AllowDuplicates);
                case ContainerType.AvlSortedList:
                    return new AvlSortedList<T>(ThisLevel.AllowDuplicates, InnerFactory);
                default:
                    throw new NotImplementedException();
            }
        }

        public IKeyValueContainer<K, V> CreateKeyValueContainer<K, V>() where K : ILazinator where V : ILazinator
        {
            switch (ThisLevel.ContainerType)
            {
                case ContainerType.AvlKeyValueTree:
                    return new AvlKeyValueTree<K, V>(InnerFactory, ThisLevel.AllowDuplicates, ThisLevel.Unbalanced);
                case ContainerType.AvlIndexableKeyValueTree:
                    return new AvlIndexableKeyValueTree<K, V>(InnerFactory, ThisLevel.AllowDuplicates, ThisLevel.Unbalanced);
                default:
                    throw new NotImplementedException();
            }
        }

        public IKeyValueContainer<K, V> CreateSortedKeyValueContainer<K, V>() where K : ILazinator, IComparable<K> where V : ILazinator
        {
            switch (ThisLevel.ContainerType)
            {
                case ContainerType.AvlSortedKeyValueTree:
                    return new AvlSortedKeyValueTree<K, V>(InnerFactory, ThisLevel.AllowDuplicates, ThisLevel.Unbalanced);
                case ContainerType.AvlSortedIndexableKeyValueTree:
                    return new AvlSortedIndexableKeyValueTree<K, V>(InnerFactory, ThisLevel.AllowDuplicates, ThisLevel.Unbalanced);
                default:
                    return CreateKeyValueContainer<K, V>();
            }
        }

        public ILazinatorDictionaryable<K, V> CreateDictionaryOfType<K, V>() where K : ILazinator, IComparable<K> where V : ILazinator
        { 
            switch (ThisLevel.ContainerType)
            {
                case ContainerType.AvlSortedDictionary:
                    return new AvlSortedDictionary<K, V>(ThisLevel.AllowDuplicates, InnerFactory);
                case ContainerType.AvlSortedIndexableDictionary:
                    return new AvlSortedIndexableDictionary<K, V>(ThisLevel.AllowDuplicates, InnerFactory);
                case ContainerType.AvlDictionary:
                    return new AvlDictionary<K, V>(ThisLevel.AllowDuplicates, InnerFactory);
                case ContainerType.LazinatorDictionary:
                    return new LazinatorDictionary<K, V>();
                default:
                    throw new NotImplementedException();
            }
        }

        public ILazinatorDictionaryable<K, V> CreateUnsortedAvlDictionary<K, V>() where K : ILazinator where V : ILazinator
        {
            return new AvlDictionary<K, V>(ThisLevel.AllowDuplicates, InnerFactory);
        }

        public IValueContainer<LazinatorKeyValue<K, V>> CreateContainerOfKeyValues<K, V>() where K : ILazinator where V : ILazinator
        {
            switch (ThisLevel.ContainerType)
            {
                case ContainerType.LazinatorList:
                    return new LazinatorList<LazinatorKeyValue<K, V>>(ThisLevel.AllowDuplicates);
                case ContainerType.LazinatorLinkedList:
                    return new LazinatorLinkedList<LazinatorKeyValue<K, V>>(ThisLevel.AllowDuplicates);
                case ContainerType.AvlTree:
                    return new AvlTree<LazinatorKeyValue<K, V>>(ThisLevel.AllowDuplicates, ThisLevel.Unbalanced, ThisLevel.CacheEnds);
                case ContainerType.AvlIndexableTree:
                    return new AvlIndexableTree<LazinatorKeyValue<K, V>>(ThisLevel.AllowDuplicates, ThisLevel.Unbalanced, ThisLevel.CacheEnds);
                case ContainerType.AvlListTree:
                    return new AvlListTree<LazinatorKeyValue<K, V>>(ThisLevel.AllowDuplicates, ThisLevel.Unbalanced, InnerFactory);
                case ContainerType.AvlIndexableListTree:
                    return new AvlIndexableListTree<LazinatorKeyValue<K, V>>(ThisLevel.AllowDuplicates, ThisLevel.Unbalanced, InnerFactory);
                default:
                    throw new NotImplementedException();
            }
        }

        public IKeyValueContainer<WUInt32, LazinatorKeyValue<K, V>> GetHashableKeyValueContainer<K, V>() where K : ILazinator where V : ILazinator
        {
            switch (ThisLevel.ContainerType)
            {
                case ContainerType.AvlSortedKeyValueTree:
                    return new AvlSortedKeyValueTree<WUInt32, LazinatorKeyValue<K, V>>(InnerFactory, true, ThisLevel.Unbalanced);
                case ContainerType.AvlSortedIndexableKeyValueTree:
                    return new AvlSortedIndexableKeyValueTree<WUInt32, LazinatorKeyValue<K, V>>(InnerFactory, true, ThisLevel.Unbalanced);
                default:
                    throw new NotImplementedException();
            }
        }

        #endregion

        #region Utility methods

        public bool ShouldSplit<T>(IValueContainer<T> container) where T : ILazinator
        {
            return container.ShouldSplit(ThisLevel.SplitThreshold);
        }

        public virtual ContainerLevel GetDefaultInnerFactory()
        {
            switch (ThisLevel.ContainerType)
            {
                case ContainerType.LazinatorList:
                    break;
                case ContainerType.LazinatorSortedList:
                    break;
                case ContainerType.LazinatorLinkedList:
                    break;
                case ContainerType.LazinatorSortedLinkedList:
                    break;
                case ContainerType.AvlTree:
                    break;
                case ContainerType.AvlSortedTree:
                    break;
                case ContainerType.AvlIndexableTree:
                    break;
                case ContainerType.AvlSortedIndexableTree:
                    break;
                case ContainerType.AvlKeyValueTree:
                case ContainerType.AvlSortedKeyValueTree:
                    return new ContainerLevel(ContainerType.AvlTree, ThisLevel.AllowDuplicates, int.MaxValue, ThisLevel.Unbalanced);
                case ContainerType.AvlIndexableKeyValueTree:
                case ContainerType.AvlSortedIndexableKeyValueTree:
                    return new ContainerLevel(ContainerType.AvlIndexableTree, ThisLevel.AllowDuplicates, int.MaxValue, ThisLevel.Unbalanced);
                case ContainerType.AvlListTree:
                case ContainerType.AvlIndexableListTree:
                    return new ContainerLevel(ContainerType.LazinatorList, ThisLevel.AllowDuplicates, 10, ThisLevel.Unbalanced);
                case ContainerType.AvlSortedListTree:
                case ContainerType.AvlSortedIndexableListTree:
                    return new ContainerLevel(ContainerType.LazinatorSortedList, ThisLevel.AllowDuplicates, 10, ThisLevel.Unbalanced);
                case ContainerType.AvlList:
                    return new ContainerLevel(ContainerType.AvlTree, ThisLevel.AllowDuplicates, int.MaxValue, ThisLevel.Unbalanced);
                case ContainerType.AvlSortedList:
                    return new ContainerLevel(ContainerType.AvlSortedTree, ThisLevel.AllowDuplicates, int.MaxValue, ThisLevel.Unbalanced);
                case ContainerType.AvlDictionary:
                    return new ContainerLevel(ContainerType.AvlSortedKeyValueTree, ThisLevel.AllowDuplicates, int.MaxValue, ThisLevel.Unbalanced);
                case ContainerType.AvlSortedDictionary:
                    return new ContainerLevel(ContainerType.AvlSortedKeyValueTree, ThisLevel.AllowDuplicates, int.MaxValue, ThisLevel.Unbalanced);
                case ContainerType.AvlSortedIndexableDictionary:
                    return new ContainerLevel(ContainerType.AvlSortedIndexableKeyValueTree, ThisLevel.AllowDuplicates, int.MaxValue, ThisLevel.Unbalanced);
                case ContainerType.LazinatorDictionary:
                    break;
                default:
                    throw new NotImplementedException();
            }
            return null;
        }

        public virtual void VerifyNextLevel(ContainerLevel nextLevel)
        {
            switch (ThisLevel.ContainerType)
            {
                case ContainerType.AvlKeyValueTree:
                case ContainerType.AvlSortedKeyValueTree:
                case ContainerType.AvlIndexableKeyValueTree:
                case ContainerType.AvlSortedIndexableKeyValueTree:
                case ContainerType.AvlListTree:
                case ContainerType.AvlSortedListTree:
                case ContainerType.AvlIndexableListTree:
                case ContainerType.AvlSortedIndexableListTree:
                case ContainerType.AvlList:
                case ContainerType.AvlSortedList:
                case ContainerType.AvlDictionary:
                case ContainerType.AvlSortedDictionary:
                case ContainerType.AvlSortedIndexableDictionary:
                    if (nextLevel == null)
                        throw new Exception($"{ThisLevel.ContainerType} requires a next container level.");
                    break;
                default:
                    break;
            }

            switch (ThisLevel.ContainerType)
            {
                case ContainerType.AvlKeyValueTree:
                case ContainerType.AvlSortedKeyValueTree:
                case ContainerType.AvlIndexableKeyValueTree:
                case ContainerType.AvlSortedIndexableKeyValueTree:
                case ContainerType.AvlListTree:
                case ContainerType.AvlSortedListTree:
                case ContainerType.AvlIndexableListTree:
                case ContainerType.AvlSortedIndexableListTree:
                case ContainerType.AvlSortedIndexableDictionary:
                case ContainerType.AvlList:
                case ContainerType.AvlSortedList:
                case ContainerType.AvlSortedDictionary:
                    if (nextLevel.AllowDuplicates != ThisLevel.AllowDuplicates)
                        throw new Exception($"{ThisLevel.ContainerType} requires a next level with the same setting for AllowDuplicates.");
                    break;
                default:
                    break;
            }

            switch (ThisLevel.ContainerType)
            {
                case ContainerType.AvlKeyValueTree:
                case ContainerType.AvlSortedKeyValueTree:
                case ContainerType.AvlIndexableKeyValueTree:
                case ContainerType.AvlSortedIndexableKeyValueTree:
                case ContainerType.AvlSortedDictionary:
                case ContainerType.AvlSortedIndexableDictionary:
                    if (nextLevel.Unbalanced != ThisLevel.Unbalanced)
                        throw new Exception($"{ThisLevel.ContainerType} requires a next level with the same setting for Unbalanced.");
                    break;
                default:
                    break;
            }

            if (ThisLevel.ContainerType == ContainerType.AvlDictionary)
            {
                if (nextLevel.AllowDuplicates == false)
                    throw new Exception($"AvlDictionary requires a next container level that allows duplicates, even if the dictionary itself does not.");
            }
        }

        private static List<ContainerLevel> ModifyLevelsForSplitThresholds(List<ContainerLevel> levels, int splitThresholdForLeafLayer, List<int> splitThresholdsForInternalLayers)
        {
            var allButLast2 = levels.Take(levels.Count - 2).ToList();
            var secondToLast = levels.Take(levels.Count - 1).Last();
            var last = levels.Last();
            last.SplitThreshold = splitThresholdForLeafLayer;

            var revisedLevels = allButLast2; // start with those
            secondToLast.SplitThreshold = int.MaxValue; // no threshold on the highest tree -- e.g., with one AvlTree, splitting happens based on the bottom layer
            revisedLevels.Add(secondToLast);
            foreach (int splitThreshold in splitThresholdsForInternalLayers ?? new List<int>())
            {
                var clone = secondToLast.CloneLazinatorTyped();
                clone.SplitThreshold = splitThreshold;
                revisedLevels.Add(clone);
            }
            return revisedLevels;
        }

        #endregion

    }
}
