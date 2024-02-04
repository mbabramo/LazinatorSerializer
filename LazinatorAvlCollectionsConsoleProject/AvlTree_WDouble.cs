using Lazinator.Core;
using Lazinator.Collections.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using LazinatorAvlCollections.Avl.ValueTree;
using Lazinator.Wrappers;

namespace PerformanceProfiling
{ 
    /// <summary>
    /// A basic AvlValueTree. 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public partial class AvlTree_WDouble : AvlTreeWithNodeType<WDouble, AvlNode_WDouble>, IAvlTree_WDouble
    {
        public AvlTree_WDouble(bool allowDuplicates, bool unbalanced, bool cacheEnds) : base(allowDuplicates, unbalanced, cacheEnds)
        {

        }

        public override IValueContainer<WDouble> CreateNewWithSameSettings()
        {
            return new AvlTree_WDouble(AllowDuplicates, Unbalanced, CacheEnds);
        }
    }
}
