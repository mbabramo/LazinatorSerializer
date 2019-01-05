using Lazinator.Core;
using Lazinator.Attributes;
using System;

namespace Lazinator.Collections.Factories
{
    [Lazinator((int)LazinatorCollectionUniqueIDs.ILazinatorLinkedListFactory)]
    public interface ILazinatorLinkedListFactory<T> where T : ILazinator
    {
    }
}