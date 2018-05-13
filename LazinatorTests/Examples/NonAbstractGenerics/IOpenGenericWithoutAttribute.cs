using System;
using System.Collections.Generic;
using System.Text;
using Lazinator.Support;
using Lazinator.Core;

namespace LazinatorTests.Examples
{
    // Does not implement Lazinator, since there is no corresponding class. For an example of an open generic with a corresponding abstract class, see AbstractGeneric1. 
    public interface IOpenGenericWithoutAttribute<T, U> where U : ILazinator
    {
        T ItemT { get; set; }
        U ItemU { get; set; }
    }
}
