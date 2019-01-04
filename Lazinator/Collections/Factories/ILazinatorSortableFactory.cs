using Lazinator.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lazinator.Collections.Factories
{
    public interface ILazinatorSortableFactory<T>  where T : ILazinator, IComparable<T>
    {
        ILazinatorSortable<T> CreateSortable();
    }
}
