using Lazinator.Attributes;
using Lazinator.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace LazinatorCollections.Interfaces
{
    [NonexclusiveLazinator((int)LazinatorCollectionUniqueIDs.IAggregatedMultivalueContainer)]
    public interface IAggregatedMultivalueContainer<T> : IAggregatedValueContainer<T>, IIndexableMultivalueContainer<T>, ICountableContainer, ILazinator where T : ILazinator
    {
    }
}
