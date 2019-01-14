using Lazinator.Collections.Avl;
using Lazinator.Collections.Interfaces;
using Lazinator.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lazinator.Collections.Factories
{
    public partial class AvlSortedKeyMultivalueTreeFactory<TKey, TValue> : IAvlSortedKeyMultivalueTreeFactory<TKey, TValue>, ISortedKeyMultivalueContainerFactory<TKey, TValue> where TKey : ILazinator, IComparable<TKey> where TValue : ILazinator
    {
        public ISortedKeyMultivalueContainer<TKey, TValue> Create()
        {
            return null; // DEBUG
        }
    }
}
