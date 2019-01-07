using Lazinator.Collections.Avl;
using Lazinator.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lazinator.Collections.Factories
{
    public partial class AvlKeyValueTreeFactory<TKey, TValue> : IAvlKeyValueTreeFactory<TKey, TValue> where TKey : ILazinator, IComparable<TKey> where TValue : ILazinator
    {
        public AvlKeyValueTreeFactory<TKey, TValue> Create()
        {
            return null; // DEBUG
        }
    }
}
