﻿using System;
using System.Collections.Generic;
using System.Text;

namespace LazinatorAvlCollections.Factories
{
    public partial class ContainerLevel : IContainerLevel
    { 
        public ContainerLevel(ContainerType valueContainerType)
        {
            ContainerType = valueContainerType;
            SplitThreshold = long.MaxValue;
            Unbalanced = false;
            AllowDuplicates = false;
        }

        public ContainerLevel(ContainerType valueContainerType, bool allowDuplicates)
        {
            ContainerType = valueContainerType;
            AllowDuplicates = allowDuplicates;
            SplitThreshold = long.MaxValue;
            Unbalanced = false;
            CacheEnds = true;
        }

        public ContainerLevel(ContainerType valueContainerType, bool allowDuplicates, long splitThreshold, bool unbalanced = false, bool cacheEnds = true)
        {
            ContainerType = valueContainerType;
            AllowDuplicates = allowDuplicates;
            SplitThreshold = splitThreshold;
            Unbalanced = unbalanced;
            CacheEnds = cacheEnds;
        }
    }
}
