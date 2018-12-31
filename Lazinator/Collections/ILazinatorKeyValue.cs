using Lazinator.Attributes;
using Lazinator.Core;
using System;

namespace Lazinator.Collections
{
    [Lazinator((int)LazinatorCollectionUniqueIDs.LazinatorKeyValue)]
    interface ILazinatorKeyValue<T, U> : ILazinator where T : ILazinator, IComparable<T> where U : ILazinator
    {
        T Key { get; set; }
        U Value { get; set; }
    }
}
