using Lazinator.Attributes;
using Lazinator.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lazinator.Collections
{
    [NonexclusiveLazinator((int)LazinatorCollectionUniqueIDs.ILazinatorCountableListable)]
    public interface ILazinatorCountableListable<T> : ILazinatorCountable, ILazinatorListable<T>, IList<T> where T : ILazinator
    {
    }
}
