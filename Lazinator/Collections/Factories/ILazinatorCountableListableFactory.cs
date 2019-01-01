using Lazinator.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lazinator.Collections.Factories
{
    public interface ILazinatorCountableListableFactory<T> where T : ILazinator
    {
        ILazinatorCountableListable<T> GetCountableListable();
    }
}
