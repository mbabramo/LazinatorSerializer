using Lazinator.Core;
using Lazinator.Attributes;
using System;

namespace Lazinator.Collections.Factories
{
    [Lazinator((int)LazinatorCollectionUniqueIDs.ILazinatorListFactory)]
    public interface ILazinatorListFactory<T> where T : ILazinator
    {
    }
}