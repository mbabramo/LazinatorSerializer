using Lazinator.Attributes;
using Lazinator.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lazinator.Collections
{
    [NonexclusiveLazinator((int)LazinatorCollectionUniqueIDs.ILazinatorKeyableMultivalueDictionary)]
    public interface ILazinatorKeyableMultivalueDictionary<TKey, TValue> : ILazinatorKeyableMultivalue<TKey, TValue>, IDictionary<TKey, TValue> where TKey : ILazinator where TValue : ILazinator
    {
    }
}
