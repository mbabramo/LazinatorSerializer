using Lazinator.Collections.Interfaces;
using Lazinator.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lazinator.Collections.Factories
{
    public interface ISortedKeyMultivalueContainerFactory<TKey, TValue> where TKey : ILazinator, IComparable<TKey> where TValue : ILazinator
    {
        ISortedKeyMultivalueContainer<TKey, TValue> Create();
    }
}
