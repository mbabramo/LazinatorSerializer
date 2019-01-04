using Lazinator.Attributes;
using Lazinator.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lazinator.Collections
{
    [NonexclusiveLazinator((int)LazinatorCollectionUniqueIDs.ILazinatorKeyable)]
    public interface ILazinatorKeyable<TKey, TValue> : ILazinator where TKey : ILazinator where TValue : ILazinator
    {
    }
}
