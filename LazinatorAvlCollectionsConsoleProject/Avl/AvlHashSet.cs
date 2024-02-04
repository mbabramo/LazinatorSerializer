using LazinatorAvlCollections.Factories;
using Lazinator.Core;
using Lazinator.Wrappers;
using System;
using System.Collections.Generic;
using System.Text;

namespace LazinatorAvlCollections.Avl
{
    /// <summary>
    /// A hashset, organized with an underlying Avl tree ordered by item hashes
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public partial class AvlHashSet<T> : IAvlHashSet<T> where T : ILazinator
    {
        public AvlHashSet()
        {
        }

        public AvlHashSet(ContainerFactory innerFactory)
        {
            UnderlyingDictionary = new AvlDictionary<T, Placeholder>(false, innerFactory);
        }

        public bool Contains(T value)
        {
            return UnderlyingDictionary.ContainsKey(value);
        }

        public void Add(T value)
        {
            UnderlyingDictionary[value] = new Placeholder();
        }

        public bool Removes(T value)
        {
            return UnderlyingDictionary.Remove(value);
        }
    }
}
