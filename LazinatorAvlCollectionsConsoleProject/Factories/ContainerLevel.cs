using System;
using System.Collections.Generic;
using System.Text;

namespace LazinatorAvlCollections.Factories
{
    /// <summary>
    /// Information regarding some level of a container, where each level may contain some other container type.
    /// </summary>
    public partial class ContainerLevel : IContainerLevel
    { 
        public ContainerLevel(ContainerType valueContainerType)
        {
            ContainerType = valueContainerType;
            SplitThreshold = valueContainerType is ContainerType.LazinatorList or ContainerType.LazinatorLinkedList or ContainerType.LazinatorSortedList or ContainerType.LazinatorSortedLinkedList ? 100 : int.MaxValue;
            Unbalanced = false;
            AllowDuplicates = false;
        }

        public ContainerLevel(ContainerType valueContainerType, bool allowDuplicates)
        {
            ContainerType = valueContainerType;
            AllowDuplicates = allowDuplicates;
            SplitThreshold = valueContainerType is ContainerType.LazinatorList or ContainerType.LazinatorLinkedList or ContainerType.LazinatorSortedList or ContainerType.LazinatorSortedLinkedList ? 100 : int.MaxValue;
            Unbalanced = false;
            CacheEnds = true;
        }

        public ContainerLevel(ContainerType valueContainerType, bool allowDuplicates, int splitThreshold, bool unbalanced = false, bool cacheEnds = true)
        {
            ContainerType = valueContainerType;
            AllowDuplicates = allowDuplicates;
            SplitThreshold = splitThreshold;
            Unbalanced = unbalanced;
            CacheEnds = cacheEnds;
        }

        public override string ToString()
        {
            return $"{ContainerType}; split at {SplitThreshold}; duplicates {AllowDuplicates}; unbalanced {Unbalanced}; cache ends {CacheEnds}";
        }
    }
}
