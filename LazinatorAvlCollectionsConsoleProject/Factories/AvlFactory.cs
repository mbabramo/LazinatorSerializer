using Lazinator.Core;
using LazinatorAvlCollections.Avl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LazinatorAvlCollections.Factories
{
    public static class AvlFactory
    {
        public static int DefaultSplitThresholdForLeafLayer = 10;
        public static int DefaultSplitThresholdForInternalLayers = 10; 
        public static int DefaultNumInternalLayers = 10;
        private static List<int> InnerLayersSplits => Enumerable.Repeat(DefaultSplitThresholdForInternalLayers, DefaultNumInternalLayers).ToList(); 
        public static AvlList<T> CreateUnbalancedAvlList<T>() where T : ILazinator => (AvlList<T>)ContainerFactory.CreateListOfType<T>(ListTypes.UnbalancedAvlList);
        public static AvlList<T> CreateAvlList<T>() where T : ILazinator => (AvlList<T>)ContainerFactory.CreateListOfType<T>(ListTypes.AvlList);
        public static AvlSortedList<T> CreateAvlSortedList<T>() where T : ILazinator, IComparable<T> => (AvlSortedList<T>)ContainerFactory.CreateSortedListOfType<T>(ListTypes.AvlSortedList);
        public static AvlSortedList<T> CreateUnbalancedAvlSortedList<T>() where T : ILazinator, IComparable<T> => (AvlSortedList<T>)ContainerFactory.CreateSortedListOfType<T>(ListTypes.UnbalancedAvlSortedList);
        public static AvlSortedList<T> CreateAvlSortedListWithDuplicates<T>() where T : ILazinator, IComparable<T> => (AvlSortedList<T>)ContainerFactory.CreateSortedListOfType<T>(ListTypes.AvlSortedListWithDuplicates);
        public static AvlList<T> CreateAvlListMultilayer<T>() where T : ILazinator => (AvlList<T>)ContainerFactory.CreateListOfType<T>(ListTypes.AvlListMultilayer, DefaultSplitThresholdForLeafLayer, InnerLayersSplits);
        public static AvlSortedList<T> CreateAvlSortedListMultilayer<T>() where T : ILazinator, IComparable<T> => (AvlSortedList<T>)ContainerFactory.CreateSortedListOfType<T>(ListTypes.AvlSortedListMultilayer, DefaultSplitThresholdForLeafLayer, InnerLayersSplits);
        public static AvlSortedList<T> CreateAvlSortedListMultilayerWithDuplicates<T>() where T : ILazinator, IComparable<T> => (AvlSortedList<T>)ContainerFactory.CreateSortedListOfType<T>(ListTypes.AvlSortedListMultilayerWithDuplicates, DefaultSplitThresholdForLeafLayer, InnerLayersSplits);
        public static AvlList<T> CreateAvlListMultilayer<T>(int splitThresholdForLeafLayer, int splitThresholdForInternalLayers, int numInternalLayers) where T : ILazinator => (AvlList<T>)ContainerFactory.CreateListOfType<T>(ListTypes.AvlListMultilayer, splitThresholdForLeafLayer, Enumerable.Repeat(splitThresholdForInternalLayers, numInternalLayers).ToList());
        public static AvlSortedList<T> CreateAvlSortedListMultilayer<T>(int splitThresholdForLeafLayer, int splitThresholdForInternalLayers, int numInternalLayers) where T : ILazinator, IComparable<T> => (AvlSortedList<T>)ContainerFactory.CreateSortedListOfType<T>(ListTypes.AvlSortedListMultilayer, splitThresholdForLeafLayer, Enumerable.Repeat(splitThresholdForInternalLayers, numInternalLayers).ToList());
        public static AvlSortedList<T> CreateAvlSortedListMultilayerWithDuplicates<T>(int splitThresholdForLeafLayer, int splitThresholdForInternalLayers, int numInternalLayers) where T : ILazinator, IComparable<T> => (AvlSortedList<T>)ContainerFactory.CreateSortedListOfType<T>(ListTypes.AvlSortedListMultilayerWithDuplicates, splitThresholdForLeafLayer, Enumerable.Repeat(splitThresholdForInternalLayers, numInternalLayers).ToList());

        public static AvlDictionary<K, V> CreateAvlDictionary<K, V>() where K : ILazinator where V : ILazinator => (AvlDictionary<K, V>)ContainerFactory.CreateUnsortedAvlDictionary<K, V>(DictionaryTypes.Avl);

        public static AvlDictionary<K, V> CreateAvlMultiValueDictionary<K, V>() where K : ILazinator where V : ILazinator => (AvlDictionary<K, V>)ContainerFactory.CreateUnsortedAvlDictionary<K, V>(DictionaryTypes.AvlMultiValue);
            
        public static AvlSortedDictionary<K, V> CreateAvlSortedDictionary<K, V>() where K : ILazinator, IComparable<K> where V : ILazinator => (AvlSortedDictionary<K, V>)ContainerFactory.CreateDictionaryOfType<K, V>(DictionaryTypes.AvlSorted); 
        public static AvlSortedIndexableDictionary<K, V> CreateAvlSortedIndexableDictionary<K, V>() where K : ILazinator, IComparable<K> where V : ILazinator => (AvlSortedIndexableDictionary<K, V>)ContainerFactory.CreateDictionaryOfType<K, V>(DictionaryTypes.AvlSortedIndexable);
        public static AvlSortedDictionary<K, V> CreateAvlSortedMultivalueDictionary<K, V>() where K : ILazinator, IComparable<K> where V : ILazinator => (AvlSortedDictionary<K, V>)ContainerFactory.CreateDictionaryOfType<K, V>(DictionaryTypes.AvlSortedMultiValue);

        public static AvlDictionary<K, V> CreateAvlDictionaryMultilayer<K, V>() where K : ILazinator where V : ILazinator => (AvlDictionary<K, V>)ContainerFactory.CreateUnsortedAvlDictionary<K, V>(DictionaryTypes.AvlMultilayer, DefaultSplitThresholdForLeafLayer, InnerLayersSplits);

        public static AvlDictionary<K, V> CreateAvlMultiValueDictionaryMultilayer<K, V>() where K : ILazinator where V : ILazinator => (AvlDictionary<K, V>)ContainerFactory.CreateUnsortedAvlDictionary<K, V>(DictionaryTypes.AvlMultiValueMultilayer, DefaultSplitThresholdForLeafLayer, InnerLayersSplits);

        public static AvlSortedDictionary<K, V> CreateAvlSortedDictionaryMultilayer<K, V>() where K : ILazinator, IComparable<K> where V : ILazinator => (AvlSortedDictionary<K, V>)ContainerFactory.CreateDictionaryOfType<K, V>(DictionaryTypes.AvlSortedMultilayer, DefaultSplitThresholdForLeafLayer, InnerLayersSplits);
        public static AvlSortedIndexableDictionary<K, V> CreateAvlSortedIndexableDictionaryMultilayer<K, V>() where K : ILazinator, IComparable<K> where V : ILazinator => (AvlSortedIndexableDictionary<K, V>)ContainerFactory.CreateDictionaryOfType<K, V>(DictionaryTypes.AvlSortedIndexableMultilayer, DefaultSplitThresholdForLeafLayer, InnerLayersSplits);
        public static AvlSortedDictionary<K, V> CreateAvlSortedMultivalueDictionaryMultilayer<K, V>() where K : ILazinator, IComparable<K> where V : ILazinator => (AvlSortedDictionary<K, V>)ContainerFactory.CreateDictionaryOfType<K, V>(DictionaryTypes.AvlSortedMultiValueMultilayer, DefaultSplitThresholdForLeafLayer, InnerLayersSplits);
        public static AvlDictionary<K, V> CreateAvlDictionaryMultilayer<K, V>(int splitThresholdForLeafLayer, int splitThresholdForInternalLayers, int numInternalLayers) where K : ILazinator where V : ILazinator => (AvlDictionary<K, V>)ContainerFactory.CreateUnsortedAvlDictionary<K, V>(DictionaryTypes.AvlMultilayer, splitThresholdForLeafLayer, Enumerable.Repeat(splitThresholdForInternalLayers, numInternalLayers).ToList());

        public static AvlDictionary<K, V> CreateAvlMultiValueDictionaryMultilayer<K, V>(int splitThresholdForLeafLayer, int splitThresholdForInternalLayers, int numInternalLayers) where K : ILazinator where V : ILazinator => (AvlDictionary<K, V>)ContainerFactory.CreateUnsortedAvlDictionary<K, V>(DictionaryTypes.AvlMultiValueMultilayer, splitThresholdForLeafLayer, Enumerable.Repeat(splitThresholdForInternalLayers, numInternalLayers).ToList());

        public static AvlSortedDictionary<K, V> CreateAvlSortedDictionaryMultilayer<K, V>(int splitThresholdForLeafLayer, int splitThresholdForInternalLayers, int numInternalLayers) where K : ILazinator, IComparable<K> where V : ILazinator => (AvlSortedDictionary<K, V>)ContainerFactory.CreateDictionaryOfType<K, V>(DictionaryTypes.AvlSortedMultilayer, splitThresholdForLeafLayer, Enumerable.Repeat(splitThresholdForInternalLayers, numInternalLayers).ToList());
        public static AvlSortedIndexableDictionary<K, V> CreateAvlIndexableSortedDictionaryMultilayer<K, V>(int splitThresholdForLeafLayer, int splitThresholdForInternalLayers, int numInternalLayers) where K : ILazinator, IComparable<K> where V : ILazinator => (AvlSortedIndexableDictionary<K, V>)ContainerFactory.CreateDictionaryOfType<K, V>(DictionaryTypes.AvlSortedIndexableMultilayer, splitThresholdForLeafLayer, Enumerable.Repeat(splitThresholdForInternalLayers, numInternalLayers).ToList());
        public static AvlSortedDictionary<K, V> CreateAvlSortedMultivalueDictionaryMultilayer<K, V>(int splitThresholdForLeafLayer, int splitThresholdForInternalLayers, int numInternalLayers) where K : ILazinator, IComparable<K> where V : ILazinator => (AvlSortedDictionary<K, V>)ContainerFactory.CreateDictionaryOfType<K, V>(DictionaryTypes.AvlSortedMultiValueMultilayer, splitThresholdForLeafLayer, Enumerable.Repeat(splitThresholdForInternalLayers, numInternalLayers).ToList());
    }
}
