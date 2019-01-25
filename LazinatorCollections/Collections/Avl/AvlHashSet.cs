using Lazinator.Collections.Factories;
using Lazinator.Core;
using Lazinator.Wrappers;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lazinator.Collections.Avl
{
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
