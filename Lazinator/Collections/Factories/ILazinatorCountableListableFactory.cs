using Lazinator.Core;
using Lazinator.Attributes;
using System;

namespace Lazinator.Collections.Factories
{
    [NonexclusiveLazinator((int)LazinatorCollectionUniqueIDs.ILazinatorCountableListableFactory)]
    public interface ILazinatorCountableListableFactory<T> where T : ILazinator
    {
        ILazinatorCountableListable<T> CreateCountableListable();
    }
}
